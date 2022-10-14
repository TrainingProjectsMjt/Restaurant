using System;
using System.Collections.Generic;
using Restaurant.KitchenManager.API.Models;

namespace Restaurant.KitchenManager.UnitTests.Helpers
{
    public static class TestDataGenerator
    {
        public static Topping GenerateMozzarellaTopping()
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

        public static Topping GenerateHamTopping()
        {
            var topping = new Topping
            {
                Id = "4bf99077-2449-4a6a-b51e-44ffbda03188",
                ToppingId = "7f1b5a0a-0f4f-413c-8456-7834e571855d",
                Name = "Ham",
                Kind = "Meat"
            };

            return topping;
        }

        public static Topping GeneratePineappleTopping()
        {
            var topping = new Topping
            {
                Id = "c64ea445-bbda-4331-a6a0-dcb1db510550",
                ToppingId = "ec7771a9-4875-4de0-a9ea-47b26ebe4328",
                Name = "Pineapple",
                Kind = "Fruit"
            };

            return topping;
        }

        public static Topping GeneratePepperoniTopping()
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

        public static Topping GenerateBaconTopping()
        {
            var topping = new Topping
            {
                Id = "c917e825-f67b-42e5-ae8c-b6965b098e38",
                ToppingId = "f5e1cfc3-ac32-4594-ac6b-0d4f6693946e",
                Name = "Bacon",
                Kind = "Meat"
            };

            return topping;
        }

        public static Topping GenerateBellPepperTopping()
        {
            var topping = new Topping
            {
                Id = "e7557271-9486-4f91-b8ec-e36711704748",
                ToppingId = "85e2ab4a-3d34-4581-b5ff-4c010f746246",
                Name = "Bell Pepper",
                Kind = "Vegetable"
            };

            return topping;
        }

        public static Topping GenerateOnionTopping()
        {
            var topping = new Topping
            {
                Id = "c7352c69-b736-406b-97da-24f3ddddb056",
                ToppingId = "bda855e7-d583-4b93-8660-59eb677e5841",
                Name = "Onion",
                Kind = "Vegetable"
            };

            return topping;
        }

        public static Topping GenerateOliveTopping()
        {
            var topping = new Topping
            {
                Id = "b69224a3-e28c-48d1-8e62-fa82892e33ee",
                ToppingId = "9f3d744c-4157-487f-b05a-766c3e1636d8",
                Name = "Olive",
                Kind = "Fruit"
            };

            return topping;
        }

        public static Topping GenerateParmesanTopping()
        {
            var topping = new Topping
            {
                Id = "ef42a043-1d1f-4227-a118-8eba8230e574",
                ToppingId = "d83b18fa-48d2-4573-be8a-76e0625ce3b9",
                Name = "Parmesan",
                Kind = "Cheese"
            };

            return topping;
        }

        public static Topping GenerateBasilTopping()
        {
            var topping = new Topping
            {
                Id = "3bb4e24b-fce4-432e-90bf-d064fd2ee8d9",
                ToppingId = "c0ad3718-1a26-43d9-9a24-54d02be6da35",
                Name = "Basil",
                Kind = "Vegetable"
            };

            return topping;
        }

        public static List<Topping> GenerateAllToppings()
        {
            var toppings = new List<Topping>
            {
                GenerateMozzarellaTopping(),
                GenerateHamTopping(),
                GeneratePineappleTopping(),
                GeneratePepperoniTopping(),
                GenerateBaconTopping(),
                GenerateBellPepperTopping(),
                GenerateOnionTopping(),
                GenerateOliveTopping(),
                GenerateParmesanTopping(),
                GenerateBasilTopping()
            };

            return toppings;
        }

        public static Pizza GenerateHawaiianPizza()
        {
            var pizza = new Pizza
            {
                Id = "345155ee-f301-47a1-b5ac-de578562197f",
                PizzaId = "8d5edc78-828e-4ac8-9db0-eed560a2cc9d",
                Name = "Hawaiian",
                ToppingNames = new List<string>
                {
                    GenerateMozzarellaTopping().Name,
                    GenerateHamTopping().Name,
                    GeneratePineappleTopping().Name
                },
                Crust = PizzaCrust.Thin,
                Diameter = 10,
                Price = 5
            };

            return pizza;
        }

        public static Pizza GeneratePepperoniPizza()
        {
            var pizza = new Pizza
            {
                Id = "5e96f665-7c9c-4d62-8cc2-ed81427476aa",
                PizzaId = "a70d8cd3-012f-4d93-be0d-e1ff44ac9d24",
                Name = "Pepperoni",
                ToppingNames = new List<string>
                {
                    GenerateMozzarellaTopping().Name,
                    GeneratePepperoniTopping().Name
                },
                Crust = PizzaCrust.Cracker,
                Diameter = 12,
                Price = 7
            };

            return pizza;
        }

        public static Pizza GenerateSupremePizza()
        {
            var pizza = new Pizza
            {
                Id = "a3b002fc-8294-488d-a619-ca1029e5cd93",
                PizzaId = "d8984af3-4b9f-4c9c-a21f-be3842c90d92",
                Name = "Supreme",
                ToppingNames = new List<string>
                {
                    GenerateBaconTopping().Name,
                    GeneratePepperoniTopping().Name,
                    GenerateBellPepperTopping().Name,
                    GenerateOnionTopping().Name,
                    GenerateOliveTopping().Name,
                    GenerateParmesanTopping().Name,
                    GenerateBasilTopping().Name
                },
                Crust = PizzaCrust.Thick,
                Diameter = 18,
                Price = 10
            };

            return pizza;
        }

        public static List<Pizza> GenerateAllPizzas()
        {
            var pizzas = new List<Pizza>
            {
                GenerateHawaiianPizza(),
                GeneratePepperoniPizza(),
                GenerateSupremePizza()
            };

            return pizzas;
        }
    }
}
