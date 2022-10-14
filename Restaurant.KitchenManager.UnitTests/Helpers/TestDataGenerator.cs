using System;
using System.Collections.Generic;
using Restaurant.KitchenManager.API.Models;

namespace Restaurant.KitchenManager.UnitTests.Helpers
{
    public static class TestDataGenerator
    {
        public static Topping GenerateFirstTopping()
        {
            var topping = new Topping
            {
                Id = "0ae5e88d-7463-481d-a62b-a4e44819a21e",
                ToppingId = "95f91764-aa7c-4ea8-a5e9-72b1cea4e43f",
                Name = "Mozzarella",
                Kind = "Cheese"
            };

            return topping;
        }

        public static Topping GenerateSecondTopping()
        {
            var topping = new Topping
            {
                Id = "eac058c8-b1f8-46a4-adaa-4be516064c89",
                ToppingId = "d61b9ea4-4822-4fd7-abda-cb2e22499f1f",
                Name = "Pepperoni",
                Kind = "Meat"
            };

            return topping;
        }

        public static Topping GenerateThirdTopping()
        {
            var topping = new Topping
            {
                Id = "143fc3c1-4a07-4052-9a3f-b97340641a79",
                ToppingId = "253a15e1-da2b-4bd8-b163-eba331a6eca9",
                Name = "Tomato",
                Kind = "Vegetable"
            };

            return topping;
        }

        public static List<Topping> GenerateToppings()
        {
            var toppings = new List<Topping>
            {
                GenerateFirstTopping(),
                GenerateSecondTopping(),
                GenerateThirdTopping()
            };

            return toppings;
        }

        public static Pizza GenerateFirstPizza()
        {
            var pizza = new Pizza
            {
                Id = "",
                PizzaId = "",
                Toppings = new List<Topping>
                {
                    GenerateFirstTopping(),
                    GenerateSecondTopping()
                },
                Crust = PizzaCrust.Thin,
                Diameter = 10,
                Price = 5
            };

            return pizza;
        }

        public static Pizza GenerateSecondPizza()
        {
            var pizza = new Pizza
            {
                Id = "",
                PizzaId = "",
                Toppings = new List<Topping>
                {
                    GenerateFirstTopping(),
                    GenerateThirdTopping()
                },
                Crust = PizzaCrust.Cracker,
                Diameter = 12,
                Price = 7
            };

            return pizza;
        }

        public static Pizza GenerateThirdPizza()
        {
            var pizza = new Pizza
            {
                Id = "",
                PizzaId = "",
                Toppings = new List<Topping>
                {
                    GenerateSecondTopping(),
                    GenerateThirdTopping()
                },
                Crust = PizzaCrust.Thick,
                Diameter = 18,
                Price = 10
            };

            return pizza;
        }

        public static List<Pizza> GeneratePizzas()
        {
            var pizzas = new List<Pizza>
            {
                GenerateFirstPizza(),
                GenerateSecondPizza(),
                GenerateThirdPizza()
            };

            return pizzas;
        }
    }
}
