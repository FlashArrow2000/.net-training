using API_Negocio_Libros.Contexts;
using API_Negocio_Libros.Entities;
using API_Negocio_Libros.Helpers;
using API_Negocio_Libros.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Negocio_Libros
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
            //Configuracion de AutoMapper (una libreria que se instala con este condigo en la consola de VS: Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection )
            services.AddAutoMapper( configuration =>
                {
                    configuration.CreateMap<Autor, AutorDTO>();
                    configuration.CreateMap<AutorCreacion_DTO, Autor>().ReverseMap(); //El ReverseMap() sirve para hacer el mismo mapeo pero de manera inversa.
                                                                                      //Seria algo asi: CreateMap<Autor, AutorCreacion_DTO>
                    configuration.CreateMap<Libro, LibroDTO>();
                },  typeof(Startup));
            //config de mi filtro
            services.AddScoped<MiFiltroDeAccion>();
            services.AddResponseCaching();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //Esto es para evitar una referencia ciclica entre clases
            services.AddControllers(options =>
            {
                options.Filters.Add(new MiFiltroDeExcepcion());
                // Si hubiese Inyección de dependencias en el filtro
                //options.Filters.Add(typeof(MiFiltroDeExcepcion)); 
            }).AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseResponseCaching();
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
