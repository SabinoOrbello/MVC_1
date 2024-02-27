using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_1.Models
{
    public class DettagliDipendente
    {
        public Dipendenti Dipendente { get; set; }
        public List<Pagamenti> Pagamenti { get; set; }

        public DettagliDipendente()
        {
            Pagamenti = new List<Pagamenti>();
        }
    }
}