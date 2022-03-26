using Business.Abstract;
using DataAccess.Entities.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromForm] Product product, IFormFile[] attachments)
        {
          var result = await _productService.CreateProduct(product,attachments);

            if(result.Success)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }



        [HttpGet("GetAllProduct")]
        public IActionResult GetAllProduct()
        {
            var result =  _productService.GetAll();

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }

        [HttpGet("GetFirstProductName")]
        public async Task<IActionResult> GetFirstProductName()
        {
            var result = await _productService.GetFirstProductName();

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }

        [HttpGet("GetProductById/{productId}")]
        public  IActionResult GetProductById(int productId)
        {
            var result=  _productService.GetById(productId);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }

        [HttpDelete("DeleteProduct/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var result = await _productService.Delete(productId);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }

        [HttpPut("UpdateProduct/{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId, Product product)
        {
           

            var result = await _productService.Update(productId, product);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }

    }
}
