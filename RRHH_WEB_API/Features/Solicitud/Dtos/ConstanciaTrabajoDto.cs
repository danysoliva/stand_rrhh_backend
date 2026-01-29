using System;
using System.Collections.Generic;

namespace RRHH_WEB_API.Features.Solicitud.Dtos
{
    public class ConstanciaTrabajoDto
    {
        public int TipoConstanciaId { get; set; }
        public string Moneda { get; set; }
        public string Employee { get; set; }
        public string IdentificationId { get; set; }
        public string Department { get; set; }
        public string Job { get; set; }
        public DateTime FechaIngreso { get; set; }
        public List<ConstanciaTrabajoIngresoDeduccionDto> Deducciones { get; set; }
        public List<ConstanciaTrabajoIngresoDeduccionDto> Ingresos { get; set; }
        public decimal TotalDeducciones { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal IngresosNetos { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int DiaIngreso { get; set; }
        public string MesIngreso { get; set; }
        public int AnioIngreso { get; set; }
        public int DiaActual { get; set; }
        public string MesActual { get; set; }
        public int AnioActual { get; set; }
    }


    public class ConstanciaTrabajoIngresoDeduccionDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Monto { get; set; }
    }


    //public class ConstanciaTrabajoIngresosDto
    //{
    //    public decimal SalarioOrdinario { get; set; }
    //    public decimal SalarioOtrosIngresos { get; set; }
    //}

    //public class ConstanciaTrabajoDeduccionesDto
    //{
    //    public decimal ISR { get; set; }
    //    public decimal Beneficios { get; set; }
    //    public decimal PlanMedico { get; set; }
    //    public decimal Embargos { get; set; }
    //    public decimal OtrosEmbargos { get; set; }
    //    public decimal Coopertiva { get; set; }
    //    public decimal PrestamoAtlantida { get; set; }
    //    public decimal Otros { get; set; }
    //}
}
