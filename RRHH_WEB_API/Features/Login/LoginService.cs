using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RRHH_WEB_API._Common;
using RRHH_WEB_API._Entidades;
using RRHH_WEB_API._Infraestructura;
using RRHH_WEB_API.Features.Login.Dtos;
using RRHH_WEB_API.Features.Maestros.Dtos;
using System;
using System.Collections.Generic;
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
        private readonly DbSet<UserDelegation> _userDelagationInstance;


        private readonly RRHH_DBContext _context;
        private readonly ACS_DBContext _acs_DBcontext;
        private readonly Seguridad _seguridad;

        public LoginService(RRHH_DBContext rrhh_DBContext, ACS_DBContext acs_DBContext, IConfiguration configuration, Seguridad seguridad)
        {
            _configuration = configuration;
            _employeeInstance = rrhh_DBContext.Employee;
            _userDelagationInstance = acs_DBContext.UserDelegation;

            _seguridad = seguridad;
            _context = rrhh_DBContext;
            _acs_DBcontext = acs_DBContext;

        }

        public Response<LoginDto> Acceder(CredencialUsuarioDto credencial)
        {
            LoginDto loginDto = new LoginDto();
            try
            {
                if (credencial.Barcode == null || credencial.Pin == null)
                {
                    return Response<LoginDto>.Validation("Usuario o contraseña incorrecta");
                }


                // Ahora buscamos solo por Barcode ya que no podemos comparar el Hash directamente en el WHERE
                Employee employee = _employeeInstance.AsQueryable()
                    .AsNoTracking()
                    //.Include(x => x.UserDelegation)
                    .Where(x => x.BarCode == credencial.Barcode && x.Active==true)
                    .FirstOrDefault();

                //var delegation = _userDelagationInstance
                //    .FirstOrDefault(x => x.EmployeeId == employee.Id);

                //// Busca la delegación
                //var delegation = _userDelagationInstance
                //    .Where(x => x.EmployeeId == employee.Id);



                    employee.UserDelegation = _userDelagationInstance.FirstOrDefault(x => x.EmployeeId == employee.Id);


                if (employee.UserDelegation == null)
                {
                    // Si ocurre cualquier error técnico (ej. _userDelagationInstance es null)
                    var userDelegation = new UserDelegation
                    {
                        //Employee = employee,
                        EmployeeId = employee.Id,
                        UserLevelId = (int)UserLevelEnum.Usuario, // Asignamos el nivel de usuario normal
                        Enable = true
                    };

                    _userDelagationInstance.Add(userDelegation);
                    _acs_DBcontext.SaveChanges();

                    employee.UserDelegation = userDelegation;
                }


                // Verificamos si existe el empleado y si el hash del PIN coincide
                if (employee == null || !_seguridad.VerificarHash(credencial.Pin, employee.Pin))
                {
                    return Response<LoginDto>.Validation("Usuario o contraseña incorrecta");
                }

                loginDto.EmpleadoId = employee.Id;
                loginDto.Barcode = employee.BarCode;
                loginDto.Name = employee.Name;
                loginDto.HasStaffInCharge = _employeeInstance.AsQueryable().AsNoTracking().Any(x => x.ParentId == employee.Id);
                loginDto.UserLevelId = (employee.UserDelegation.UserLevelId > 0) ? employee.UserDelegation.UserLevelId : (int)UserLevelEnum.Usuario;
                loginDto.Token = GenerarToken(credencial.Barcode, employee.Id);
                loginDto.RefreshToken = GenerarRefreshToken(employee.Id);

                return Response<LoginDto>.Success(loginDto);
            }
            catch (Exception ex)
            {
                return Response<LoginDto>.Excepcion("Ocurrió un error " + ex.Message);
            }
        }

        public string GenerarToken(string barcode, object id)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, id.ToString()),
                new Claim("EmpleadoId", id.ToString()),
                new Claim("Barcode", barcode),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["LlaveSecreta"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddHours(10);
            JwtSecurityToken token = new JwtSecurityToken(
               issuer: _configuration["Token:iss"],
               audience: _configuration["Token:iss"],
               claims: claims,
               expires: expiracion,
               signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerarRefreshToken(object id)
        {
            var randomNumber = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                string token = Convert.ToBase64String(randomNumber);

                var refreshToken = new UserRefreshToken
                {
                    EmployeeId = (int)id,
                    Token = token,
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow
                };

                _context.UserRefreshTokens.Add(refreshToken);
                _context.SaveChanges();

                return token;
            }
        }

        public Response<LoginDto> RefrescarToken(RefreshTokenRequestDto request)
        {
            try
            {
                var principal = GetPrincipalFromExpiredToken(request.Token);
                if (principal == null) return Response<LoginDto>.Validation("Token inválido");

                var employeeIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "EmpleadoId")?.Value;
                if (string.IsNullOrEmpty(employeeIdClaim)) return Response<LoginDto>.Validation("Token inválido");

                int employeeId = int.Parse(employeeIdClaim);

                var savedRefreshToken = _context.UserRefreshTokens
                    .Where(x => x.EmployeeId == employeeId && x.Token == request.RefreshToken && x.Revoked == null && x.Expires > DateTime.UtcNow)
                    .OrderByDescending(x => x.Created)
                    .FirstOrDefault();

                if (savedRefreshToken == null) return Response<LoginDto>.Validation("Refresh Token inválido o expirado");

                // Revocar el token viejo (rotación)
                savedRefreshToken.Revoked = DateTime.UtcNow;
                _context.UserRefreshTokens.Update(savedRefreshToken);

                var employee = _employeeInstance.AsNoTracking().FirstOrDefault(x => x.Id == employeeId);
                
                var loginDto = new LoginDto
                {
                    EmpleadoId = employeeId,
                    Barcode = employee.BarCode,
                    Name = employee.Name,
                    Token = GenerarToken(employee.BarCode, employeeId),
                    RefreshToken = GenerarRefreshToken(employeeId)
                };

                return Response<LoginDto>.Success(loginDto);
            }
            catch (Exception ex)
            {
                return Response<LoginDto>.Excepcion("Error al refrescar token: " + ex.Message);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["LlaveSecreta"])),
                ValidateLifetime = false // Importante: Aquí no validamos el tiempo porque justamente el token ya expiró
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public Response<bool> BulkActualizarPIN(List<BulkCambiarPinDto> listaPines)
        {
            try
            {
                if (listaPines == null || !listaPines.Any())
                    return Response<bool>.Validation("La lista de pines está vacía");

                var employeeIds = listaPines.Select(x => x.EmployeeId).ToList();
                var employees = _employeeInstance.Where(x => employeeIds.Contains(x.Id)).ToList();



                foreach (var item in listaPines)
                {
                    var emp = employees.FirstOrDefault(x => x.Id == item.EmployeeId);
                    if (emp != null)
                    {
                        emp.Pin = _seguridad.GenerarHash(item.NuevoPin);

                      
                        //UserDelegation userDelegation = new UserDelegation
                        //{
                        //    EmployeeId = emp.Id,
                        //    UserLevelId=2,
                        //    Enable= true
                        //};
                    }
                }

                _context.SaveChanges();
                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Response<bool>.Validation(ex.Message);
            }
        }
    }
}
