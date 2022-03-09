﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AD.TwitterTools.JsonTypes
{

    public class UserMention
    {
        [JsonProperty("screenname")]
        public string ScreenName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("indices")]
        public List<int> Indices { get; set; }

    }

}
