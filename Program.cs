
using Glammor.Db;
using Glammor.Models;
using Microsoft.EntityFrameworkCore;

namespace Glammor
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddAuthorization();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddDbContext<EarringsDbContext>(options =>
			options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			// ReadAll
			app.MapGet("/Earrings", async (EarringsDbContext dbContext) =>
			{
				var earrings = await dbContext._Earrings.ToListAsync();
				return Results.Ok(earrings);
			});

			// ReadById
			app.MapGet("/Earring/{id}", async (int id, EarringsDbContext dbContext) =>
			{
				var earring = await dbContext._Earrings.FindAsync(id);

				if (earring == null)
				{
					return Results.NotFound("Sorry, earring not found!");
				}

				return Results.Ok(earring);

			});

			// Create new 
			app.MapPost("/Earring", async (EarringsDbContext dbContext, Earrings earring) =>
			{
				dbContext._Earrings.Add(earring);
				await dbContext.SaveChangesAsync();
				return Results.Ok("Added");
			});

			// Update
			app.MapPut("/Earring/{id}", async (int id, Earrings earring, EarringsDbContext dbContext) =>
			{
				var earringToUodate = await dbContext._Earrings.FindAsync(id);

				if (earringToUodate == null)
				{
					return Results.NotFound("Sorry, earring not found");
				}

				earringToUodate.Name = earring.Name;
				earringToUodate.Description = earring.Description;
				earringToUodate.Image = earring.Image;
				earringToUodate.Price = earring.Price;
				earringToUodate.Quantity = earring.Quantity;

				await dbContext.SaveChangesAsync();
				return Results.Ok("Successgully updated");
			});

			
			//Delete

			app.MapDelete("/Earring/{id}", async (int id, EarringsDbContext dbContext) =>
			{
				var earringToDelete = await dbContext._Earrings.FindAsync(id);

				if (earringToDelete == null)
				{
					return Results.NotFound("Sorry, earring not found!");
				}

				dbContext._Earrings.Remove(earringToDelete);
				await dbContext.SaveChangesAsync();

				return Results.Ok("Successfully deleted");

			});

			app.Run();
		}
	}
}
