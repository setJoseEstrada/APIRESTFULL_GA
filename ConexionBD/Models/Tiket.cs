using System;
using System.Collections.Generic;

namespace ConexionBD.Models
{
    public partial class Tiket
    {
        public string? IdTienda { get; set; }
        public string? IdRegistradora { get; set; }
        public DateTime? FechaHora { get; set; }
        public int? Ticket { get; set; }
        public decimal? Impuesto { get; set; }
        public decimal? Total { get; set; }
        public DateTime? FechaHoraCreacion { get; set; }
    }
}
