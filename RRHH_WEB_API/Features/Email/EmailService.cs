using Aspose.Cells;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using RRHH_WEB_API._Common;
using RRHH_WEB_API._Entidades;
using RRHH_WEB_API._Infraestructura;
using RRHH_WEB_API.Features.Email.Dto;
using RRHH_WEB_API.Features.Maestros.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using NsExcel = Microsoft.Office.Interop.Excel;

namespace RRHH_WEB_API.Features.Email
{
    public class EmailService
    {
        private readonly RRHH_DBContext _rrhh_Web_DBContext;
        private readonly ACS_DBContext _acs_DBContext;
        private readonly DbSet<HoraEmpleadoTrabajada> _horaEmpleadoTrabajadasInstance;
        private readonly DbSet<Employee> _employeeInstance;
        private IConfiguration _configuration;


        private EmailConfiguration _emailConfiguration = new EmailConfiguration();
        private EmailSendParams _emailSendParams = new EmailSendParams();

        public EmailService(RRHH_DBContext rrhh_Web_DBContext,IConfiguration configuration,ACS_DBContext acs_DBContext)
        {
            _rrhh_Web_DBContext = rrhh_Web_DBContext;
            _acs_DBContext = acs_DBContext;
            _horaEmpleadoTrabajadasInstance = _acs_DBContext.HoraEmpleadoTrabajada;
            _employeeInstance = rrhh_Web_DBContext.Employee;
            _configuration = configuration;

            _emailConfiguration.Port = this._configuration.GetValue<int>("Smtp:Port");
            _emailConfiguration.SmtpServer = this._configuration.GetValue<string>("Smtp:Server");
            _emailConfiguration.UserName = this._configuration.GetValue<string>("Smtp:UserName");
            _emailConfiguration.Password = this._configuration.GetValue<string>("Smtp:Password");
            _emailConfiguration.From = this._configuration.GetValue<string>("Smtp:FromAddress");
            _emailConfiguration.DisplayName = this._configuration.GetValue<string>("Smtp:DisplayName");
            _emailConfiguration.RRHHEmail = this._configuration.GetValue<string>("Smtp:RRHH_Mail");

        }


        public Response<bool> EnviarDetalleHorasPorEmpleado(EnviarDetalleHorasParamsDto  enviarDetalleHorasParamsDto)
        {
            try
            {

                var culture = new System.Globalization.CultureInfo("es-ES");


                //var test = _horaEmpleadoTrabajadasInstance.AsQueryable().AsNoTracking().Include(r=> r.Employee)
                //            .Where(h => h.EmployeeId == enviarDetalleHorasParamsDto.EmployeeId && h.FechaI >= Convert.ToDateTime(enviarDetalleHorasParamsDto.FechaInicio).Date).ToList();


                var empleado = _employeeInstance.FirstOrDefault(d => d.Id == enviarDetalleHorasParamsDto.EmployeeId);

                var detalleHoras = _horaEmpleadoTrabajadasInstance.AsQueryable()
                                  .AsNoTracking().Include(r=> r.Employee).Where(h => h.EmployeeId == enviarDetalleHorasParamsDto.EmployeeId
                                  && h.FechaI >= Convert.ToDateTime(enviarDetalleHorasParamsDto.FechaInicio).Date
                                  && h.FechaI <= Convert.ToDateTime(enviarDetalleHorasParamsDto.FechaFin).Date
                                  && h.Enable == true
                                  )
                   .Select(r => new HoraEmpleadoCorreoDto
                   {
                       Serial = r.Id.ToString(),
                       //Code = _employeeInstance.AsQueryable().AsNoTracking().FirstOrDefault(r => r.Id == enviarDetalleHorasParamsDto.EmployeeId).BarCode, //Codigo Anterior
                       Code = empleado.BarCode,
                       EmpleadoId = enviarDetalleHorasParamsDto.EmployeeId,
                       //EmployeeName = _employeeInstance.AsQueryable().AsNoTracking().FirstOrDefault(r=> r.Id==enviarDetalleHorasParamsDto.EmployeeId).Name,//Codigo Anterior
                       EmployeeName = empleado.Name,
                       NormalHour = r.Cantidad,
                       ExtraHours = r.CantidadDe,
                       FechaI = (DateTime)r.FechaI,
                       FechaF = (DateTime)r.FechaF.GetValueOrDefault(),
                       Fecha = culture.DateTimeFormat.GetDayName(r.Fecha.DayOfWeek) + ", " + r.Fecha.ToString("dd/MM/yyyy"),
                       Semana = (int)r.Week
                   }).ToList();

                 
                var data = ToDataTable(detalleHoras);

                var mail = _employeeInstance.FirstOrDefault(r => r.Id == enviarDetalleHorasParamsDto.EmployeeId).WorkEmail;
                var name = _employeeInstance.FirstOrDefault(r => r.Id == enviarDetalleHorasParamsDto.EmployeeId).Name;

                //EnviarCorreo(data,mail,name);
                if (string.IsNullOrEmpty(mail))
                {
                    return Response<bool>.Validation("No tiene configurado un correo");
                }
                
                EnviarCorreo(data, mail, name);

                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Response<bool>.Excepcion(ex.Message);
            }
        }


        private void EnviarCorreo(DataTable dt, string email, string nombre)
        {
            string file = "C:\\temp\\DetalleDeHoras.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                wb.SaveAs(file);

                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("standrrhh@aquafeedapp.com", "Aquafeed Apps");
                message.To.Add(email);
                //message.To.Add("reuceda05@hotmail.com");
                message.Subject = "Detalle de horas trabajas";
                message.Body = "<p>Estimado(a) " + nombre + ", reciba un cordial saludo,</p> <p>Se adjunta el archivo de horas trabajas que usted solicitó</p> ";
                message.IsBodyHtml = true;
                message.Attachments.Add(new Attachment(file));

                smtp.EnableSsl = true;
                smtp.Port = 578;
                smtp.Host = "outlook.office365.com";
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential("apps@aquafeedhn.net", "$Applications1620&$");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtp.Send(message);
            }
        }


        private byte[] ExporttoExcel<T>(List<T> table, string filename)
        {
            using ExcelPackage pack = new ExcelPackage();
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(filename);
            ws.Cells["A1"].LoadFromCollection(table, true, TableStyles.Light1);
            return pack.GetAsByteArray();
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public void EnviarCorreo_General(EmailSendParams emailSendParams)
        {

            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(_emailConfiguration.From, _emailConfiguration.DisplayName);

            foreach (var item in emailSendParams.Destinatarios)
            {
                message.To.Add(item);
            }
            //message.To.Add("reuceda05@hotmail.com");

            message.Subject = emailSendParams.Subject;
            message.Body = emailSendParams.Body;
            message.IsBodyHtml = true;

            smtp.EnableSsl = true;
            smtp.Port = _emailConfiguration.Port;
            smtp.Host = _emailConfiguration.SmtpServer;
            //smtp.UseDefaultCredentials = true;
            smtp.Credentials = new NetworkCredential(_emailConfiguration.UserName, _emailConfiguration.Password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtp.Send(message);

        }


        public bool EnviarCorreo_QuejasSugerencias()
        {

            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            bool enviado = false;

            try
            {
                string body = "<html>  \r\n<body style='font-family: Arial, sans-serif; margin:0; padding:0; background:#f4f6f8;'>    \r\n\t<table width='100%' cellpadding='0' cellspacing='0' role='presentation'>     \r\n\t\t<tr>        \r\n\t\t<td align='center' style='padding:20px 10px;'>        \r\n\t\t\t<table width='600' cellpadding='0' cellspacing='0' role='presentation' style='background:#ffffff; border-radius:8px; overflow:hidden; box-shadow:0 2px 6px rgba(0,0,0,0.08);'>\r\n\t\t\t\t<!-- Encabezado -->            \r\n\t\t\t\t<tr>              \r\n\t\t\t\t\t<td style='background:#004b8d; color:#ffffff; padding:20px; text-align:left;'>\r\n\t\t\t\t\t<h1 style='margin:0; font-size:20px;'>Notificación: Nuevo Registro</h1>\r\n\t\t\t\t\t</td>            \r\n\t\t\t\t</tr>            \r\n\t\t\t\t<!-- Cuerpo -->            \r\n\t\t\t\t<tr>\r\n\t\t\t\t\t<td style='padding:20px; color:#333333;'>                \r\n\t\t\t\t\t\t<p style='margin:0 0 12px 0;'>Estimado equipo de <strong>Recursos Humanos</strong>,</p>\r\n\t\t\t\t\t\t<p style='margin:0 0 12px 0; line-height:1.6;'>Les informamos que se ha recibido un nuevo registro en el segmento de <strong>Quejas/Denuncias/Sugerencias</strong> en el Stand RRHH.</p>\r\n\t\t\t\t\t\t<p style='margin:0 0 12px 0; line-height:1.6;'>Les solicitamos revisar la información y dar el seguimiento correspondiente.</p>\r\n\t\t\t\t\t\t<table cellpadding='0' cellspacing='0' role='presentation' style='margin:18px 0;'>\r\n\t\t\t\t\t\t<tr>                    \r\n\t\t\t\t\t\t<td>                      \r\n\t\t\t\t\t\t<a href='http://10.50.11.26:82/#/pages/quejas-sugerencias-denuncias-admin' target='_blank' style='display:inline-block; text-decoration:none; padding:12px 18px; border-radius:6px; background:#004b8d; color:#ffffff; font-weight:600;'>Dar Seguimiento</a>\r\n\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t</table>                \r\n\t\t\t\t\t\t<p style='margin:0 0 6px 0; font-size:13px; color:#555555;'>En caso de requerir información adicional, pueden comunicarse con el área de soporte o administración del sistema.</p>\r\n\t\t\t\t\t</td>\r\n\t\t\t\t</tr>\r\n\t\t\t\t<!-- Pie -->\r\n\t\t\t\t<tr>\r\n\t\t\t\t\t<td style='background:#f1f3f5; padding:12px 20px; font-size:12px; color:#666666;'>\r\n\t\t\t\t\t<p style='margin:0;'>Este es un mensaje automático. Por favor, no responda directamente a este correo.</p>\r\n\t\t\t\t\t</td>\r\n\t\t\t\t</tr>\r\n\t\t\t</table>\r\n\t\t</td>\r\n\t\t</tr>\r\n\t</table>\r\n</body>\r\n</html>";
                message.From = new MailAddress(_emailConfiguration.From, _emailConfiguration.DisplayName);
                //message.To.Add("reuceda05@hotmail.com");
                message.To.Add(_emailConfiguration.RRHHEmail);

                message.Subject = "Notificación: Nuevo Registro Quejas/Denuncias/Sugerencias";
                message.Body = body;
                message.IsBodyHtml = true;

                smtp.EnableSsl = true;
                smtp.Port = _emailConfiguration.Port;
                smtp.Host = _emailConfiguration.SmtpServer;
                //smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(_emailConfiguration.UserName, _emailConfiguration.Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;


                smtp.Send(message);

                enviado = true;

                return enviado;
            }
            catch (Exception ex)
            {

                return false;
            }

        }
    }
}
