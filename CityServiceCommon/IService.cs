using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CityModelCommon;
using CityRepositoryCommon;
using CityModel;

namespace CityServiceCommon
{
    public interface IService
    {
        Task<ICity> GetCity(int id);

        Task<List<ICity>> GetAllCity(int size, int page, string filter, string sort);

        Task<bool> DeleteResident(int id);

        Task<bool> PostResident(IResidents res, int id);
    }

}
