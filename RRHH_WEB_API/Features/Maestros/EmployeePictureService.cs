using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RRHH_WEB_API._Common;
using RRHH_WEB_API._Entidades;
using RRHH_WEB_API._Infraestructura;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentFTP;

namespace RRHH_WEB_API.Features.Maestros
{
    public class EmployeePictureService
    {
        private readonly RRHH_DBContext _rrhh_DBContext;
        private readonly IConfiguration _configuration;

        public EmployeePictureService(RRHH_DBContext rrhh_DBContext, IConfiguration configuration)
        {
            _rrhh_DBContext = rrhh_DBContext;
            _configuration = configuration;
        }

        public async Task<byte[]> GetEmployeePicture(int idEmployee)
        {
            try
            {
                var picture = await _rrhh_DBContext.EmployeePicture
                    .AsNoTracking()
                    .Where(x => x.IdEmployee == idEmployee && x.Active)
                    .FirstOrDefaultAsync();

                if (picture == null || string.IsNullOrEmpty(picture.Path))
                {
                    return null;
                }

                // Example path: ftp://10.50.11.32/RRHH/Empleados/FjU2lkcWYAgNG6d.jpg
                if (picture.Path.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase))
                {
                    return await GetImageFromFtp(picture.Path);
                }
                
                // Fallback for local paths if any
                if (File.Exists(picture.Path))
                {
                    return await File.ReadAllBytesAsync(picture.Path);
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<byte[]> GetImageFromFtp(string ftpPath)
        {
            try
            {
                Uri ftpUri = new Uri(ftpPath);
                string host = ftpUri.Host;
                string path = ftpUri.AbsolutePath;

                string user = _configuration["FtpConfiguration:Username"];
                string pass = _configuration["FtpConfiguration:Password"];

                using (var client = new AsyncFtpClient(host, user, pass))
                {
                    await client.Connect();

                    using (var ms = new MemoryStream())
                    {
                        bool success = await client.DownloadStream(ms, path);
                        if (success)
                        {
                            return ms.ToArray();
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Log error
            }
            return null;
        }
    }
}
