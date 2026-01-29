using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RRHH_WEB_API._Common;
using RRHH_WEB_API._Entidades;
using RRHH_WEB_API._Infraestructura;
using RRHH_WEB_API.Features.Login.Dtos;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RRHH_WEB_API.Features.Login
{
    public class LoginService
    {
        
        private readonly IConfiguration _configuration;
        private readonly DbSet<Employee> _employeeInstance;
        //private readonly Seguridad _seguridad;

        public LoginService(RRHH_Web_DBContext rrhh_DBContext, IConfiguration configuration 
            //,Seguridad seguridad
            )
        {
            _configuration = configuration;
            _employeeInstance = rrhh_DBContext.Employee;
            //_seguridad = seguridad;
        }

        public Response<LoginDto> Acceder(CredencialUsuarioDto credencial)
        {
            LoginDto loginDto = new LoginDto();
            try
            {
                //string barcode;
                //string pin; //byte[] pin;

                if (credencial.Barcode == null || credencial.Pin == null)
                {
                    return Response<LoginDto>.Validation("Usuario o contraseña incorrecta");
                }
                //pin = _seguridad.GenerarSHA1(credencialUsuarioDTO.Pin);

                Employee employee = _employeeInstance.AsQueryable().AsNoTracking().Include(x=>x.UserDelegation).Where(x => x.BarCode == credencial.Barcode && x.Pin == credencial.Pin && x.Resource.Active).FirstOrDefault();
                if (employee == null)
                {
                    return Response<LoginDto>.Validation("Usuario o contraseña incorrecta");
                }


                loginDto.EmpleadoId = employee.Id;
                loginDto.Barcode = employee.BarCode;
                loginDto.Name = employee.Name;
                loginDto.HasStaffInCharge = _employeeInstance.AsQueryable().AsNoTracking().Any(x => x.ParentId == employee.Id);
                loginDto.UserLevelId = (employee.UserDelegation.UserLevelId > 0) ? employee.UserDelegation.UserLevelId : (int)UserLevelEnum.Usuario;
                loginDto.Token = GenerarToken(credencial, employee.Id);

                return Response<LoginDto>.Success(loginDto);
            }
            catch (Exception ex)
            {
                return Response<LoginDto>.Excepcion("Ocurrió un error " + ex.Message);
            }
        }

        public string GenerarToken(CredencialUsuarioDto credencial, object id)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, id.ToString()),
                new Claim("EmpleadoId", id.ToString()),
                new Claim("Barcode", credencial.Barcode),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["LlaveSecreta"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddDays(10);
            JwtSecurityToken token = new JwtSecurityToken(
               issuer: _configuration["Token:iss"],
               audience: _configuration["Token:iss"],
               claims: claims,
               expires: expiracion,
               signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
