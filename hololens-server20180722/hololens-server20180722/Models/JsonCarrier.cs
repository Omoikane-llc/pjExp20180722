using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hololens_server20180722.Models {
    public class JsonCarrier {
        [JsonProperty(PropertyName = "actionType")]
        public string ActionType { get; set; }

        [JsonProperty(PropertyName = "partitionKey")]
        public string PartitionKey { get; set; }

        [JsonProperty(PropertyName = "rowKey")]
        public string RowKey { get; set; }

        [JsonProperty(PropertyName = "roomNumber")]
        public string RoomNumber;

        [JsonProperty(PropertyName = "isMyTurn")]
        public string IsMyTurn;

        [JsonProperty(PropertyName = "lastPosition")]
        public string LastPosition;

        [JsonProperty(PropertyName = "pieacesState")]
        public List<string> PieacesState;

        [JsonProperty(PropertyName = "errorMessage")]
        public string ErrorMessage { get; set; }

    }
}