using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erronka.Models
{
    public class Erabiltzailea
    {
        public int id { get; set; }
        public string Izena { get; set; } = string.Empty;
        public string Pasahitza { get; set; } = string.Empty;
        public string Rola { get; set; } = "admin";
    }
}
