using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Restaurant.KitchenManager.API.Models
{
    public class Pizza
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [JsonProperty("pizzaId")]
        public string PizzaId { get; set; } = Guid.NewGuid().ToString();
        [JsonProperty("toppings")]
        public List<Topping> Toppings { get; set; } = new List<Topping>();
        [JsonProperty("crust")]
        public PizzaCrust Crust { get; set; }
        [JsonProperty("diameter")]
        public decimal Diameter { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
    }

    public enum PizzaCrust
    {
        Stuffed,
        Cracker,
        FlatBread,
        Thin,
        Cheese,
        Thick,
        Wrapping
    }
}
