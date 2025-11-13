using Microsoft.EntityFrameworkCore;
using Erronka.Models;

namespace Erronka.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Erabiltzailea> Erabiltzaileak { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Conexión al PostgreSQL
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=tpv_sistema;Username=tpv;Password=tpv");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Datos iniciales
            modelBuilder.Entity<Erabiltzailea>().HasData(
                new Erabiltzailea { id = 1, Izena = "admin", Pasahitza = "1234", Rola = "admin" },
                new Erabiltzailea { id = 2, Izena = "usuario", Pasahitza = "1234", Rola = "user" }
            );
        }
    }
}

