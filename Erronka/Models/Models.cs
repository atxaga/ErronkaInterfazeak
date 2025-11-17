using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Erronka.Models
{
    [Table("erreserbak")]
    public class Erreserba
    {
        [Key]
        public int id { get; set; }

        public int? mahi_id { get; set; }  
        public int? erabiltzaile_id { get; set; } 
        public DateTime data { get; set; }  
        [MaxLength(10)]
        public string mota { get; set; }  
        public string izena { get; set; }  
    }

    [Table("produktuak")]
    public class Produktua
    {
        [Key]
        public int id { get; set; }
        [MaxLength(100)]
        public string izena { get; set; }
        public decimal prezioa { get; set; }
        public int stock { get; set; }
        [MaxLength(50)]
        public string kategoria { get; set; }
    }

    [Table("Erabiltzaileak")]
    public class Erabiltzailea
    {
        [Key]
        public int id { get; set; }
        [MaxLength(50)]
        public string Izena { get; set; }
        [MaxLength(255)]
        public string Pasahitza { get; set; }
        [MaxLength(20)]
        public string Rola { get; set; }
    }

    public class Ticket
    {
        public int id { get; set; }
        public string Izena { get; set; }
        public decimal Prezioa { get; set; }
        public int Kantitatea { get; set; }
        public decimal Totala => Prezioa * Kantitatea;
    }
}