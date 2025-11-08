using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEducativosComunitarios.Model
{
    public interface IServiceRepository
    {
        public void Add(ServiceModel serviceModel);
        public void Delete(ServiceModel serviceModel);
        public IEnumerable<ServiceModel> GetAll();
        public void Update(ServiceModel serviceModel);
    }
}
