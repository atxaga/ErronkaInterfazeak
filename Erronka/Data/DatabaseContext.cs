using Microsoft.EntityFrameworkCore;
using Erronka.Models;

namespace Erronka.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Erabiltzailea> Erabiltzaileak { get; set; }
        public DbSet<Erreserba> Erreserbak { get; set; }       
        public DbSet<Produktua> Produktuak { get; set; }     

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=tpv_sistema;Username=tpv;Password=tpv");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Erabiltzailea>().HasData(
                new Erabiltzailea { id = 1, Izena = "admin", Pasahitza = "1234", Rola = "admin" },
                new Erabiltzailea { id = 2, Izena = "usuario", Pasahitza = "1234", Rola = "user" }
            );

      
            modelBuilder.Entity<Erreserba>()
                        .Property(r => r.mota)
                        .HasMaxLength(10);

            modelBuilder.Entity<Erreserba>()
                        .Property(r => r.data)
                        .HasColumnType("date");

            modelBuilder.Entity<Produktua>()
                        .Property(p => p.izena)
                        .HasMaxLength(100);
        }
    }
}


