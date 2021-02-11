using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CityServiceCommon;
using CityModelCommon;
using CityRepositoryCommon;
using CityModel;
using CityRepository;

namespace CityService
{
    public class Service : IService
    {
        protected IRepository repository;

        public Service(IRepository repo)
        {
            this.repository = repo;
        }

        public async Task<ICity> GetCity(int id)
        {
            return await repository.GetCityRep(id);
        }

        public async Task<List<ICity>> GetAllCity(int size, int page, string filter, string sort)
        {
            return await repository.GetAllCityRep(size, page, filter, sort);
        }

        public async Task<bool> DeleteResident(int id)
        {
            return await repository.DeleteresidentRep(id);
        }

        public async Task<bool> PostResident(IResidents res, int id)
        {
            return await repository.PostResidentRep(res, id);
        }
    }
}
