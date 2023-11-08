using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace ProjektWEBapi.Model
{
    public class Trailer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int WeightKG { get; set; }
        public string Dimensions { get; set; }

        public Trailer(int id, string name, int weight, string dimensions)
        {
            Id = id;
            Name = name;
            WeightKG = weight;
            Dimensions = dimensions;
        }
        [JsonConstructor]
        public Trailer() { }
    }
    public class TrailerContext : DbContext // TODO: Lav ny class DbContext
    {
        public TrailerContext(DbContextOptions<TrailerContext> options) : base(options) { }

        public DbSet<Trailer> Trailers { get; set; }
    }
}
