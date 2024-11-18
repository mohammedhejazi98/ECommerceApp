using InventoryService.Data;
using InventoryService.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers
{
    /// <summary>
    /// Handles HTTP requests related to product operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(DataContext dataContext) : ControllerBase
    {
        #region Public Methods

        /// <summary>
        /// Retrieves all products from the database.
        /// </summary>
        /// <returns>
        /// A task that represents an asynchronous operation. The task result contains an enumerable collection of products.
        /// </returns>
        [HttpGet]
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var products =  dataContext.Products.AsEnumerable();
            return products;
        }

        #endregion Public Methods
    }
}
