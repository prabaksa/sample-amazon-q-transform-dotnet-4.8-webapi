using System;
using ProductsWebAPI.Models;
using ProductsWebAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;





namespace ProductsWebAPI.Controllers
{
[ApiController]    public class ProductsController :ControllerBase    {

        private readonly IProductsService _productsService;
        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService ?? throw new ArgumentNullException(nameof(productsService));
        }


        [Route("api/products")]
        [HttpGet]
        public IActionResult ListProducts()
        {
            try
            {
                var products = _productsService.GetAllProducts();
                return Ok(products);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [Route("api/products/{id:int}")]
        [HttpGet]
        public IActionResult GetProduct(int id)
        {
            //Validation
            if (id <= 0)
            {
                return BadRequest("Invalid product ID. ID must be greater than 0.");
            }

            try
            {
                var product = _productsService.GetProduct(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        // POST api/product
        [Route("api/products")]
        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product value)
        {
            //Validation
            if (value == null)
            {
                return BadRequest("Product data cannot be null");
            }

            try
            {
                _productsService.SaveProduct(value);
                return Created($"api/products/{value.Id}", value);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        // PUT api/product/5

        [Route("api/products/{id:int}")]
        [HttpPut]
        public IActionResult UpdateProduct(int id, [FromBody] Product newValue)
        {
            //Validation
            if (id <= 0)
            {
                return BadRequest("Invalid product ID. ID must be greater than 0.");
            }

            if (newValue == null)
            {
                return BadRequest("Product data cannot be null");
            }

            try
            {
                //Check for existing product before update
                var existingProduct = _productsService.GetProduct(id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                _productsService.UpdateProduct(id, newValue);
                return Ok(newValue);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        // DELETE api/products/5
        [Route("api/products/{id:int}")]
        [HttpDelete]
        public IActionResult DeleteProduct(int id)
        {
            //Validation
            if (id <= 0)
            {
                return BadRequest("Invalid product ID. ID must be greater than 0.");
            }

            try
            {
                _productsService.DeleteProduct(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}