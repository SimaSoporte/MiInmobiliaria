using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class Persona
    {
        public int Id { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNac { get; set; }
        public string Dni { get; set; }
        public TipoPersona Tipo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Foto { get; set; }
        public string Formato { get; set; }
    }
}
