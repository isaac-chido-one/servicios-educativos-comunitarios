using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEducativosComunitarios.Model
{
    public class LocalityModel
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public int Municipio { get; set; }
        public string? Comunidad { get; set; }
        public int Ambito { get; set; }
        public string? Latitud { get; set; }
        public string? Longitud { get; set; }
        public int Poblacion { get; set; }
    }
}
