using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_1.Models
{
    public class Pagamenti
    {
        public int Id { get; set; }
        public int DipendenteId { get; set; }
        public DateTime PeriodoPagamento { get; set; }
        public decimal Ammontare { get; set; }
        public bool Stipendio { get; set; }
    }
}