using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Quire
{
    class SharedQuireItem 
    {

        public string id { get; set; }

        [JsonProperty(PropertyName = "UserName")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "QuireTime")]
        public string QuireTime { get; set; }

        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "Checked")]
        public bool Checked { get; set; }

        [JsonProperty(PropertyName = "Urgent")]
        public bool Urgent { get; set; }

        [JsonProperty(PropertyName = "Visible")]
        public bool Visible { get; set; }

        [JsonProperty(PropertyName = "ShareTags")]
        public string ShareTags { get; set; }

        [JsonProperty(PropertyName = "Accepted")]
        public bool Accepted { get; set; }

        [JsonProperty(PropertyName = "From")]
        public string From { get; set; }

        [JsonProperty(PropertyName = "To")]
        public string To { get; set; }

    }
}
