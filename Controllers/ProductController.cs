using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tutorial_api_2.Models;
using tutorial_api_2.Services;

namespace tutorial_api_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : BaseController
    {
        private IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("Products")]
        public IActionResult GetAllProduct()
        {
            return Ok(_productService.GetAllProduct());
        }

        [HttpGet("Product/{id}")]
        public IActionResult GetProductById(int id)
        {
            return Ok(_productService.GetProduct(id));
        }

        [HttpPost("Product")]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            try
            {
                _productService.CreateProduct(product);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("Product/{id}")]
        public IActionResult UpdateProductById(int id, [FromBody] Product product)
        {
            try
            {
                _productService.UpdateProduct(id, product);

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("Product/{id}")]
        public IActionResult DeleteProductById(int id)
        {
            try
            {
                _productService.DeleteProduct(id);

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
