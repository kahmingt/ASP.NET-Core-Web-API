using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using WebApi.Database.Entities;
using WebApi.Models;
using WebApi.Repository;

namespace WebApi.Controllers
{
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _db;

        public ProductController(
            ILogger<ProductController> logger,
            IMapper mapper,
            IRepositoryWrapper db)
        {
            _logger = logger;
            _mapper = mapper;
            _db = db;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDetails productDetails)
        {
            if (!ModelState.IsValid || productDetails is null)
            {
                _logger.LogError("400 Bad Request. Invalid or null.");
                return BadRequest("Invalid");
            }
            try
            {
                var mappedModel = _mapper.Map<ProductDetails, Products>(productDetails);
                await _db.ProductRepository.CreateProductAsync(mappedModel);
                await _db.CommitChangesAsync();
                return CreatedAtRoute("CreateProduct", new { id = mappedModel.ProductId }, mappedModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("500 Internal Server Error. " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteProductDetailsByIdAsync(int id)
        {
            try
            {
                var model = await _db.ProductRepository.GetProductDetailsByIdAsync(id);
                if (model is null)
                {
                    _logger.LogError("404 Not Found. Model is null.");
                    return NotFound("Invalid");
                }
                else
                {
                    await _db.ProductRepository.UpdateProductDetailsByIdAsync(model);
                    await _db.CommitChangesAsync();
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("500 Internal Server Error. " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductDetails>> GetProductDetailsByIdAsync(int id)
        {
            try
            {
                var model = await _db.ProductRepository.GetProductDetailsByIdAsync(id);
                if (model is null)
                {
                    _logger.LogError("404 Not Found. Model is null.");
                    return NotFound("Invalid");
                }
                else
                {
                    var mappedModel = _mapper.Map<Products, ProductDetails>(model);
                    return Ok(mappedModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("500 Internal Server Error. " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProductListing>>> GetProductListAsync()
        {
            try
            {
                var model = await _db.ProductRepository.GetProductListAsync();
                if (model is null)
                {
                    _logger.LogError("400 Bad Request. Model is null.");
                    return BadRequest("Invalid");
                }
                else
                {
                    var mappedModel = _mapper.Map<IEnumerable<Products>, IEnumerable<ProductListing>>(model);
                    return Ok(mappedModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("500 Internal Server Error. " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateProductDetailsById(int id, [FromBody] ProductDetails productDetails)
        {
            if (!ModelState.IsValid || productDetails is null)
            {
                _logger.LogError("400 Bad Request. Invalid or null.");
                return BadRequest("Invalid");
            }
            try
            {
                var model = await _db.ProductRepository.GetProductDetailsByIdAsync(id);
                if (model is null)
                {
                    _logger.LogError("404 Not Found.");
                    return NotFound("Invalid");
                }
                else
                {
                    var mappedModel = _mapper.Map<ProductDetails, Products>(productDetails);
                    await _db.ProductRepository.UpdateProductDetailsByIdAsync(mappedModel);
                    await _db.CommitChangesAsync();
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("500 Internal Server Error. " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
