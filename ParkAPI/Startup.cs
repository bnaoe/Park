using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ParkAPI.Data;
using ParkAPI.Mapper;
using ParkAPI.Repository;
using ParkAPI.Repository.IRepository;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ParkAPI
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
            services.AddDbContext<ApplicationDBContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<ITrailRepository, TrailRepository>();

            services.AddAutoMapper(typeof(ParkMappings));

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
            services.AddTransient <IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();

                //services.AddSwaggerGen(options =>
                //{
                //    options.SwaggerDoc("ParkOpenAPISpec",
                //        new Microsoft.OpenApi.Models.OpenApiInfo()
                //        {
                //            Title = "Park API",
                //            Version = "1",
                //            Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                //            {
                //                Email = "brian.naoe01@gmail.com",
                //                Name = "Brian Naoe"

                //            }
                //        });


                //    var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //    var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                //    options.IncludeXmlComments(cmlCommentsFullPath);
                //});
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json",
                        desc.GroupName.ToUpper());
                    options.RoutePrefix = "";
                }
                    
                
            });

            //app.UseSwaggerUI(options =>
            //{
            //    options.SwaggerEndpoint("/swagger/ParkOpenAPISpec/swagger.json","Park API");
            //    //options.SwaggerEndpoint("/swagger/ParkOpenAPISpecTrails/swagger.json", "Park API Trails");

            //    options.RoutePrefix = "";
            //});

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
