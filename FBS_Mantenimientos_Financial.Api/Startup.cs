using AutoMapper;
using FBS_CRUD_Generico.Contexts;
using FBS_CRUD_Generico.Extensions;
using FBS_Mantenimientos_Financial.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Text;
using Financial2_5.Encriptacion;
using FBS_Mantenimientos_Financial.Domain.DbContext;

namespace FBS_Mantenimientos_Financial.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            //services.AddDbContext<HistoryDbContext>(options =>
            //      options.UseSqlServer(
            //          Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("FBS_Mantenimientos_Financial.Domain")));
            services.AddDbContext<HistoryDbContext>(options =>
                   options.UseMySql(connectionString,
                       ServerVersion.AutoDetect(connectionString),
                       mySqlOption => mySqlOption.EnableRetryOnFailure().SchemaBehavior(MySqlSchemaBehavior.Translate,
                           (schema, entity) => $"{schema ?? "dbo"}.{entity}")).UseLazyLoadingProxies()
                      .ConfigureWarnings(warning =>
                      {
                          warning.Ignore(CoreEventId.DetachedLazyLoadingWarning);
                      }));

            //services.AddDbContext<MantenimientoDbContext>(options =>
            //       options.UseSqlServer(
            //           Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies()
            //            .ConfigureWarnings(warning =>
            //            {
            //                warning.Ignore(CoreEventId.DetachedLazyLoadingWarning);
            //            }));
            services.AddDbContext<MantenimientoDbContext>(options =>
                    options.UseMySql(connectionString,
                        ServerVersion.AutoDetect(connectionString),
                        mySqlOption => mySqlOption.EnableRetryOnFailure().SchemaBehavior(MySqlSchemaBehavior.Translate,
                            (schema, entity) => $"{schema ?? "dbo"}.{entity}")).UseLazyLoadingProxies()
                       .ConfigureWarnings(warning =>
                       {
                           warning.Ignore(CoreEventId.DetachedLazyLoadingWarning);
                       }));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            }).AddCookie();

            services.AddScoped<ApplicationDbContext, MantenimientoDbContext>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.ConfigurarControllerGenerico(typeof(MantenimientoDbContext).Assembly);
            services.AddAutoMapper(typeof(MantenimientoDbContext).Assembly);
            services.AddCors();
            services.AddAuthorizationCore();
            services.AddFBSEncriptacion(Convert.ToInt32(Configuration["IteracionHash"]));

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseRouting();
            app.UseAuthentication(); //first than useauthorization
            app.UseAuthorization(); //for distinct roles

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}