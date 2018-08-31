using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using hololens_server20180722.Models;
using Swashbuckle.Swagger.Annotations;

namespace hololens_server20180722.Controllers {
    public class ValuesController : ApiController {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET api/values
        [SwaggerOperation("GetAll")]
        public IEnumerable<string> Get() {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public string Get(int id) {
            return "value";
        }

        // POST api/values
        [SwaggerOperation("Create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        public JsonCarrier Post(JsonCarrier data) {
            log.Info("Start Post " + data.ActionType);
            var act = data.ActionType;
            JsonCarrier res = null;

            switch (act) {
                case "status_update":
                    var statusUpdateService = new OthelloPlay();
                    res = statusUpdateService.UpdateStatus(data);
                    break;
                default:
                    res = new JsonCarrier { ErrorMessage = "invalid actionType" };
                    break;
            }

            log.Info("End Post ");
            return res;
        }

        // PUT api/values/5
        [SwaggerOperation("Update")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE api/values/5
        [SwaggerOperation("Delete")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public void Delete(int id) {
        }
    }
}
