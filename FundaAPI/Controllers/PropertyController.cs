using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FundaAPI.BAL;
using FundaAPI.Repositories;

namespace FundaAPI.Controllers
{
    public class PropertyController : ApiController
    {
        [Route("api/funda/GetTop10/{type}/{city}/{extra?}")]
        [HttpGet]
        public HttpResponseMessage GetTop10Listing(string type, string city, string extra = "")
        {
            PropertyBAL propBAL = new PropertyBAL(new PropertyRepository());
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(propBAL.GetProperties(type, city, extra, 1)), System.Text.Encoding.UTF8, "text/html")
            };
        }

    }
}
