using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEducativosComunitarios.Model
{
    public interface ILocalityRepository
    {
        public void Add(LocalityModel localityModel);
        public void Delete(LocalityModel localityModel);
        public IEnumerable<LocalityModel> GetAll();
        public void Update(LocalityModel localityModel);
        public bool CodeExists(LocalityModel localityModel);
        public bool HasServices(LocalityModel localityModel);
    }
}
