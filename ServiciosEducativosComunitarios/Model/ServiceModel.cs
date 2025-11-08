using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEducativosComunitarios.Model
{
    public class ServiceModel
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public int LocalityId { get; set; }
        public string? Locality { get; set; }
        public int Period { get; set; }
        public int Program { get; set; }
        public int Status { get; set; }
    }
}
