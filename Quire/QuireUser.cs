using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Quire
{
    class QuireUser
    {
        public string id { get; set; }

        [JsonProperty(PropertyName = "UserName")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "Password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "Friends")]
        public string Friends { get; set; }

        [JsonProperty(PropertyName = "SignUpDate")]
        public string SignUpDate { get; set; }

    }
}
