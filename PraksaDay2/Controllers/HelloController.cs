using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;
using CityServiceCommon;
using CityModelCommon;
using CityModel;
using CityService;
using System.Threading.Tasks;
using AutoMapper;

namespace PraksaDay2.Controllers
{
    [RoutePrefix("api/Hello")]
    public class HelloController : ApiController
    {
        protected IService service;

        public HelloController(IService serv)
        {
            this.service = serv;
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<HttpResponseMessage> Get(int id)
        {
            ICity result;
            GetViewModel view;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<ICity, GetViewModel>());
            var mapper = new Mapper(config);

            result = await service.GetCity(id);

            view = mapper.Map<ICity, GetViewModel>(result);

            if (result != null)
            {
                view.Name = result.Name;
                return Request.CreateResponse(HttpStatusCode.OK, view);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "No content");
            }

        }

        [HttpGet]
        [Route("Get/All")]
        public async Task<HttpResponseMessage> GetAll([FromUri] int size, [FromUri] int page, [FromUri] string filter, [FromUri] string sort)
        {
            List<ICity> results;
            List<GetViewModel> view = new List<GetViewModel>();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<ICity, GetViewModel>());
            var mapper = new Mapper(config);

            results = await service.GetAllCity(size, page, filter, sort);

            if (results != null)
            {
                foreach(City rez in results)
                {
                    view.Add(mapper.Map<ICity, GetViewModel>(rez));
                }
                return Request.CreateResponse(HttpStatusCode.OK, view);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "No content");
            }

        }

        [HttpDelete]
        [Route("Delete/Resident/{id}")]
        public async Task<HttpResponseMessage> Delete(int id)
        {
            if (await service.DeleteResident(id) == true)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "OK");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Not Found");
            }
        }

        [HttpPost]
        [Route("Post/Resident/{id}")]
        public async Task<HttpResponseMessage> Post([FromBody] PostViewModel res, int id)
        {
            IResidents resident;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<PostViewModel, IResidents>());
            var mapper = new Mapper(config);
            resident = mapper.Map<PostViewModel, IResidents>(res);

            //if (res.Name != null) { resident.Name = res.Name; } else { resident.Name = ""; }
            //if (res.Surname != null) { resident.Surname = res.Surname; } else { resident.Surname = ""; }
            //if (res.Pbr != null) { resident.Pbr = Int32.Parse(res.Pbr); } else { resident.Pbr = 0; }
            //if (res.Gender != null) { resident.Gender = res.Gender; } else { resident.Gender = ""; }

            if (await service.PostResident(resident, id) == true)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "OK");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
            }
        }
    }
}
