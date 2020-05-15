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
        public Guid? Numero { get; set; }
        public decimal? ValorTotal { get; set; }
        public decimal? ValorSubtotal { get; set; }
        public decimal? ValorIva { get; set; }
        public decimal? ValorDescuento { get; set; }
        public DateTime? FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }
        public int? IdEstado { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdCliente { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual EstadosFactura IdEstadoNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<Producto> Producto { get; set; }
    }
}
