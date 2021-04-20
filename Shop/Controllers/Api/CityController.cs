using Newtonsoft.Json.Linq;
using Shop.Models;
using Shop.Models.ApiEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Shop.Controllers.Api
{
    [RoutePrefix("api")]
    public class CityController : ApiController
    {
        Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();
        
        [Route("City/StatePage")]
        public IHttpActionResult Get()
        {
            List<StateEntity> state = db.States.Select(a => new StateEntity
            {
                stateID=a.stateID,
                stateName=a.stateName
            }
            ).ToList();
            return Ok(state);
        }

        [Route("City/Statepage/{id}")]
        public IHttpActionResult Get(int id)
        {
            List<CityEntity> cities = db.Cities.Where(a => a.stateID == id)
                .Select(a => new CityEntity()
                {
                    cityID=a.cityID,
                    cityName=a.cityName,
                    stateID=a.stateID
                }).ToList();
            return Ok(cities);
        }
    }
}
