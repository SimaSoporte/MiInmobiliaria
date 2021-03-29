using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class Inquilino
    {
        [Key]
        public int Id { get; set; }
        public Persona Persona { get; set; }
        public bool Activo { get; set; }
    }
}
