using Microsoft.EntityFrameworkCore;

namespace Locations.APP.Domain;

public class LocationsDb : DbContext
{
 public DbSet<City> Cities { get; set; }
 public DbSet<Country> Countries { get; set; }
 public LocationsDb(DbContextOptions options) : base(options)
 {
 }

 protected override void OnModelCreating(ModelBuilder modelBuilder)
 {
  modelBuilder.Entity<Country>().HasIndex(countryEntity => countryEntity.CountryName).IsUnique();

  modelBuilder.Entity<City>().HasIndex(cityEntity => cityEntity.CityName).IsUnique();
  
  
  modelBuilder.Entity<City>()
   .HasOne(cityEntity => cityEntity.Country) // each City entity has one related Country entity
   .WithMany(countryEntity => countryEntity.Cities) // each Country entity has many related City entities
   .HasForeignKey(cityEntity => cityEntity.CountryId) // the foreign key property in the City entity that
   .OnDelete(DeleteBehavior.NoAction); // prevents deletion of a Country entity if there are related City entities

  
 }
 
}