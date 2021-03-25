using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class Propietario
    {
        public int Id { get; set; }
        public Persona Persona { get; set; }
        public bool Activo { get; set; }
    }
}
