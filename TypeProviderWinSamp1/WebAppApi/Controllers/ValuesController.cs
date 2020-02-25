using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using Microsoft.AspNet.Mvc;
using Newtonsoft.Json.Linq;

namespace WebAppApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : ApiController
    {
        private JObject CreateData(string key1, string key2, int? key3, double? key4)
        {
            JObject obj = new JObject();
            obj["name"] = key1;
            obj["nickname"] = key2;
            obj["age"] = key3;
            obj["score"] = key4;
            return obj;
        }

        [HttpGet]
        public JArray Get()
        {
            JArray output = new JArray();
            output.Add(CreateData("Key One", "Nickname Two", 33, 81.4));
            output.Add(CreateData("Key Four", "Nickname Five", 46, 87.5));
            output.Add(CreateData("Key Seven", "Nickname Eight", 22, null));
            output.Add(CreateData("Key Ten", "Nickname Eleven", null, null));
            return output;
        }

        /*// GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }*/

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
