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
        private readonly IProductAttachmentService _productAttachmentService;
        public ProductController(IProductService productService, IProductAttachmentService productAttachmentService)
        {
            _productService = productService;
            _productAttachmentService = productAttachmentService;
        }
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromForm] Product product)
        {
          var result = await _productService.Create(product);
            
            if(result.Success)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }




        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            var result =  _productService.GetAll();
            
            if (result.Success)
                return Ok(result.Data.ToList());
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

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(int productId, [FromForm] Product product)
        {
           

            var result = await _productService.Update(productId, product);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }

        [HttpPut("UpdateProductAttachment")]
        public async Task<IActionResult> UpdateProductAttachment(int productAttachmentId, [FromForm] ProductAttachment productAttachment)
        {


            var result = await _productAttachmentService.Update(productAttachmentId, productAttachment);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }

        [HttpDelete("DeleteProductAttachment/{productAttachmentId}")]
        public async Task<IActionResult> DeleteProductAttachment(int productAttachmentId)
        {
            var result = await _productAttachmentService.Delete(productAttachmentId);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }



    }
}
