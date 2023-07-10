using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IRepositoryWrapper _db;

        public ProductController(IRepositoryWrapper db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("/ProductList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductListing>>> GetProductList()
        {
            var model = await _db.ProductService.GetProductList();
            return model == null ? BadRequest("Invalid") : Ok(model);
        }

        [HttpGet]
        [Route("{ProductId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetProductDetailsById(int ProductId)
        {
            var model = await _db.ProductService.GetProductDetailsById(ProductId);
            return model == null ? NotFound("Invalid") : Ok(model);
        }

        [HttpDelete]
        [Route("{ProductId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteProductDetailsById(int ProductId)
        {
            var model = await _db.ProductService.DeleteProductById(ProductId);
            return model == false ? BadRequest("Invalid") : Ok();
        }

        [HttpPut]
        [Route("{ProductId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateProductDetailsById(int ProductId, ProductDetails productDetails)
        {
            if (!ModelState.IsValid || ProductId != productDetails.ProductID)
            {
                return BadRequest("Invalid");
            }
            var model = await _db.ProductService.UpdateProductDetailsById(ProductId, productDetails);
            return model == null ? BadRequest("Invalid") : Ok(model);
        }

        [HttpPost]
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
