using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ParkyAPI.Data;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;
using AutoMapper;
using ParkyAPI.ParkyMapper;
using System.IO;
using System.Reflection;

namespace ParkyAPI
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
			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			services.AddScoped<INationalParkRepository, NationalParkRepository>();
			services.AddScoped<ITrailRepository, TrailRepository>();
			services.AddAutoMapper(typeof(ParkyMappings));
			services.AddSwaggerGen(options=> {
				options.SwaggerDoc("ParkyOpenAPISpecNP",
					new Microsoft.OpenApi.Models.OpenApiInfo()
					{

						Title = "Parky API(National Park)",
						Version = "1",
						Description = "Abayomi's Parky API NP",
						Contact = new Microsoft.OpenApi.Models.OpenApiContact()
						{
							Email = "igwubor@gmail.com",
							Name = "Abayomi Igwubor",
							Url=  new Uri("https://abayomiigwubor.netlify.com/")
						},
						License = new Microsoft.OpenApi.Models.OpenApiLicense()
						{
							Name = "MIT License",
							Url= new Uri("https://en.wikipedia.org/wiki/MIT_License")
						
						}

					});
				options.SwaggerDoc("ParkyOpenAPISpecTrails",
	new Microsoft.OpenApi.Models.OpenApiInfo()
	{

		Title = "Parky API Trails",
		Version = "1",
		Description = "Abayomi's Parky API Trails",
		Contact = new Microsoft.OpenApi.Models.OpenApiContact()
		{
			Email = "igwubor@gmail.com",
			Name = "Abayomi Igwubor",
			Url = new Uri("https://abayomiigwubor.netlify.com/")
		},
		License = new Microsoft.OpenApi.Models.OpenApiLicense()
		{
			Name = "MIT License",
			Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")

		}

	});
				var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name }.xml";
				var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
				options.IncludeXmlComments(cmlCommentsFullPath);

			});
			services.AddControllers();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();
			app.UseSwagger();
			app.UseSwaggerUI(options => {
				options.SwaggerEndpoint("/swagger/ParkyOpenAPISpecNP/swagger.json", "Parky API NP");
				options.SwaggerEndpoint("/swagger/ParkyOpenAPISpecTrails/swagger.json", "Parky API Trails");
				options.RoutePrefix = "";
			
			});
			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
