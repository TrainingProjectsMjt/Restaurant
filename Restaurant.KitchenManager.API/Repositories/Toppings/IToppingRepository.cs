using System;
using System.Collections.Generic;
using System.Text;
using Restaurant.KitchenManager.API.Models;
using System.Threading.Tasks;

namespace Restaurant.KitchenManager.API.Repositories.Toppings
{
    public interface IToppingRepository
    {
        /// <summary>
        /// Create a topping.
        /// </summary>
        /// <param name="topping"></param>
        /// <returns></returns>
        Task CreateTopping(Topping topping);

        /// <summary>
        /// Delete a topping.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="toppingId"></param>
        /// <returns></returns>
        Task DeleteTopping(string id, string toppingId);

        /// <summary>
        /// Retrieves all toppings.
        /// </summary>
        /// <returns></returns>
        Task<List<Topping>> GetAllToppings();

        /// <summary>
        /// Retrieve a topping by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Topping> GetToppingById(string id);

        /// <summary>
        /// Retrieve a topping by Name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<Topping> GetToppingByName(string name);

        /// <summary>
        /// Update a topping.
        /// </summary>
        /// <param name="topping"></param>
        /// <returns></returns>
        Task UpdateTopping(Topping topping);
    }
}
