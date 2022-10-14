using System;
using Newtonsoft.Json;

namespace Restaurant.KitchenManager.API.Models
{
    public class Topping
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [JsonProperty("toppingId")]
        public string ToppingId { get; set; } = Guid.NewGuid().ToString();
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("kind")]
        public string Kind { get; set; }
    }
}
