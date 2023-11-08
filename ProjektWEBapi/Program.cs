using ProjektWEBapi.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ProjektWEBapi
{

	public class Program
	{
		private static string _sqlServerIP = "172.17.0.2";
		
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
		
			builder.Services.AddDbContext<TrailerContext>(options =>
			options.UseSqlServer(connectionString));
            builder.Services.AddDbContext<BookingContext>(options =>
            options.UseSqlServer(connectionString));

            builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
	

			var app = builder.Build();
			app.UseSwaggerUI();
	

			app.MapGet("/", () => "Hello World! => "+ _sqlServerIP);

			app.MapPost("/createBooking",  (Booking booking, BookingContext dbContext) =>
			{
				var accessToken = "bOuse96dkqXnLVlygLSBGefT1CxWw2X9iUnQrQrdSFDRuql6YRO1nD7WeM7e7W5W";
				using (var httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
					var apiURL = $"https://api.nrpla.de/{booking.BookerLicencePlate}";
					var respons = httpClient.GetAsync(apiURL).Result;

					if (respons.IsSuccessStatusCode)
					{
                        dbContext.Bookings.Add(booking);
                        dbContext.SaveChanges();

                        return Results.Created($"Booking id: {booking.Id}", booking);
                    }
					else
					{
						return Results.UnprocessableEntity("Fejl i nummerplade. Nummerplade er ikke registreret.");
					}
				}
				

			});
            app.MapPost("/createTrailer", (Trailer trailer, TrailerContext dbContext) =>
            {

                dbContext.Trailers.Add(trailer);
                dbContext.SaveChanges();

                return Results.Created($"Trailer id: {trailer.Id}", trailer);

            });
            app.MapGet("/bookings", (BookingContext bookingContext) =>
			{ 
				try
				{
					
                    var allBookings = bookingContext.Bookings.ToList(); 
                    return Results.Ok(allBookings); 
                }
				catch (Exception ex)
				{
					return Results.BadRequest("Fejl");
				}
			});
            app.MapGet("/trailers", (TrailerContext trailerContext) =>
            {
                try
                {
					
                    var allTrailers = trailerContext.Trailers.ToList(); // Get all Bookings from the database
                    return Results.Ok(allTrailers); // Return them with a 200 OK status
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Fejl {ex}");
                }
                
            });
			app.MapPut("/updateTrailer/{id}", (int id, Trailer trailer, TrailerContext dbContext) =>
			{
				var existingId = dbContext.Trailers.Find(id);
				if (existingId == null)
				{
					return Results.NotFound();
				}
				existingId.WeightKG = trailer.WeightKG;
				existingId.Dimensions = trailer.Dimensions;
				existingId.Name = trailer.Name;
				dbContext.SaveChanges();
				return Results.NoContent();
			});
			app.MapDelete("/deleteTrailer/{id}", (int id, TrailerContext DbContext) =>
			{
				var removeInstance = DbContext.Trailers.Find(id);
				if (removeInstance == null)
				{
					return Results.NotFound();
				}
				DbContext.Trailers.Remove(removeInstance);
				DbContext.SaveChanges();
				return Results.NoContent();
			});
			app.MapDelete("/deleteALLTrailers", (TrailerContext dbContext) =>
			{
				var allRemove = dbContext.Trailers.ToList();
				dbContext.Trailers.RemoveRange(allRemove);
				dbContext.SaveChanges();
				return Results.NoContent();
			});

            app.UseSwagger(x => x.SerializeAsV2 = true);

			app.Run();


		}
	}
}