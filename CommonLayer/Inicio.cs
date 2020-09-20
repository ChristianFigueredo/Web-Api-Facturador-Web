using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer
{
    public class Inicio
    {
        public string Api { get; set; }
        public string Estatus { get; set; }

        public Inicio(string a, string s)
        {
            Api = a;
            Estatus = s;
            return;
        }
    }
}
