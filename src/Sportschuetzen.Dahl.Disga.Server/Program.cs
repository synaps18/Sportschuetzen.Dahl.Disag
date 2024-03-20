using Sportschuetzen.Dahl.Disag.Rm3;

namespace Sportschuetzen.Dahl.Disag.Server
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			switch (builder.Environment.EnvironmentName)
			{
				case "Development.Simulation":
				{
					builder.Services.AddSingleton<IDisagRm3, DisagRm3Simulation>();
					break;
				}
				default:
				{
					builder.Services.AddSingleton<IDisagRm3, DisagRm3>();
					break;
				}
			}

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

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
