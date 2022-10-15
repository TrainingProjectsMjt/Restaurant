using System;
using System.Collections.Generic;
using System.Text;
using Restaurant.KitchenManager.API.Models;
using System.Threading.Tasks;

namespace Restaurant.KitchenManager.API.Repositories.Pizzas
{
    public interface IPizzaRepository
    {
        /// <summary>
        /// Create a pizza.
        /// </summary>
        /// <param name="pizza"></param>
        /// <returns></returns>
        Task CreatePizza(Pizza pizza);

        /// <summary>
        /// Delete a pizza.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pizzaId"></param>
        /// <returns></returns>
        Task DeletePizza(string id, string pizzaId);

        /// <summary>
        /// Retrieves all pizzas.
        /// </summary>
        /// <returns></returns>
        Task<List<Pizza>> GetAllPizzas();

        /// <summary>
        /// Retrieve a pizza by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Pizza> GetPizzaById(string id);

        /// <summary>
        /// Retrieve a pizza by Name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<Pizza> GetPizzaByName(string name);

        /// <summary>
        /// Update a pizza.
        /// </summary>
        /// <param name="pizza"></param>
        /// <returns></returns>
        Task UpdatePizza(Pizza pizza);
    }
}
