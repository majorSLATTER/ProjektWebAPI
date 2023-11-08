using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace ProjektWEBapi.Model
{


	public class Booking
	{
		public int Id { get; set; }
		public int TrailerID { get; set; }
		public string BookerFullName { get; set; }
		public string BookerLicencePlate { get; set; }

        public Booking(int id, int trailerid, string Booker, string Licence)
        {
            Id = id;
			TrailerID = trailerid; 
			BookerFullName = Booker; 
			BookerLicencePlate = Licence;
        }
        [JsonConstructor]
        public Booking() { }
    }
	public class BookingContext : DbContext // TODO: Lav ny class DbContext
	{
		public BookingContext(DbContextOptions<BookingContext> options) : base(options) { }

		public DbSet<Booking> Bookings { get; set; }
	}
    
}
