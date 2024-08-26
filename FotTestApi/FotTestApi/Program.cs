
using FotTestApi.Data;

using FotTestApi.Service;
using Microsoft.EntityFrameworkCore;

namespace FotTestApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddScoped<IAgenService,AgentService>();
			builder.Services.AddScoped<ITargetService,TargetService>();
			builder.Services.AddScoped<IMissionsService, MissionService>();


			builder.Services.AddDbContext<ApplicationBDContext>((sp, options) =>
			{
				var configuration = sp.GetRequiredService<IConfiguration>();
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
			}, ServiceLifetime.Scoped); 
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
			
		}
	}
}
