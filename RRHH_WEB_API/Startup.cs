using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RRHH_WEB_API._Infraestructura;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RRHH_WEB_API.Features.Maestros;
using RRHH_WEB_API.Features.Solicitud;
using RRHH_WEB_API.Features.Login;
using RRHH_WEB_API.Features.Upload;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using RRHH_WEB_API._Entidades;
using RRHH_WEB_API.Features.GestionesVarias;
using RRHH_WEB_API.Features.Encuestas;
using RRHH_WEB_API._Common;
using RRHH_WEB_API.Features.Email;
using Nest;
using Microsoft.Owin.Hosting;

namespace RRHH_WEB_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

           services.AddCors();
            services.AddControllers();

            services.AddDbContext<RRHH_DBContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("cnxRRHH")));

            services.AddDbContext<ACS_DBContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("cnxACS")));

            //var emailConfig = Configuration
            //        .GetSection("EmailConfiguration")
            //        .Get<EmailConfiguration>();
            //            services.AddSingleton(emailConfig);

            services.AddTransient<LoginService>();
            services.AddTransient<MaestroService>();
            services.AddTransient<SolicitudService>();
            services.AddTransient<SolicitudVacacionService>();
            services.AddTransient<GestionesVariasService>();
            services.AddTransient<UploadService>();
            services.AddTransient<EncuestaService>();
            services.AddTransient<EmailService>();
            services.AddTransient<EmployeePictureService>();
            services.AddSingleton<Seguridad>();


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Token:iss"],
                        ValidAudience = Configuration["Token:aud"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                     Encoding.UTF8.GetBytes(Configuration["LlaveSecreta"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RRHH_Web", Version = "v1" });

                // Define el esquema de seguridad JWT
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseCors(option => option
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                app.UseCors(builder =>
                {
                    builder.WithOrigins("http://localhost:4200", "http://localhost:82", "http://10.50.11.32:82", "http://10.50.11.26:82")
                            .AllowAnyHeader()
                            .WithMethods("GET", "POST")
                            .AllowCredentials();
                });
            }



            app.UseHsts();

            app.UseStaticFiles();


            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RRHH API V1");

            });

            app.UseHttpsRedirection();
        

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
