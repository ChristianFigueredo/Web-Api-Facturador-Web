using System;
using System.Collections.Generic;

namespace DataLayer.Models.DB
{
    public partial class Factura
    {
        public Factura()
        {
            Producto = new HashSet<Producto>();
        }

        public int Id { get; set; }
        public string Numero { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorSubtotal { get; set; }
        public decimal ValorIva { get; set; }
        public int? IdCliente { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual ICollection<Producto> Producto { get; set; }
    }
}
