using InventoryService.Data;
using InventoryService.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(DataContext dataContext) : ControllerBase
    {
        #region Public Methods

        [HttpGet]
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var products =  dataContext.Products.AsEnumerable();
            return products;
        }

        #endregion Public Methods
    }
}
