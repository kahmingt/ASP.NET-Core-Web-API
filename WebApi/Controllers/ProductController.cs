using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IRepositoryWrapper _db;

        public ProductController(IRepositoryWrapper db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductListing>>> GetProductList()
        {
            var model = await _db.ProductService.GetProductList();
            return model == null ? BadRequest("Invalid") : Ok(model);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetProductDetailsById(int id)
        {
            var model = await _db.ProductService.GetProductDetailsById(id);
            return model == null ? NotFound("Invalid") : Ok(model);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteProductDetailsById(int id)
        {
            var model = await _db.ProductService.DeleteProductById(id);
            return model == false ? BadRequest("Invalid") : Ok();
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateProductDetailsById(int id, ProductDetails productDetails)
        {
            if (!ModelState.IsValid || id != productDetails.ProductID)
            {
                return BadRequest("Invalid");
            }
            var model = await _db.ProductService.UpdateProductDetailsById(id, productDetails);
            return model == null ? BadRequest("Invalid") : Ok(model);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDetails productDetails)
        {
            if (!(ModelState.IsValid))
            {
                return BadRequest("Invalid");
            }
            else
            {
                var model = await _db.ProductService.CreateProduct(productDetails);
                return model == null ? BadRequest("Invalid") : Ok(model);
            }
        }
    }
}
