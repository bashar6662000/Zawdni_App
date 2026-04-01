using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zawdni.api.Data;
using Zawdni.api.Models;
using Zawdni.Models.DTO;
namespace Zawdni.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ZawdniDbContext _dbContext;
        public ProductController(ZawdniDbContext context) 
        { 
            _dbContext = context;
        }
        /// <summary>
        /// return all products 
        /// </summary>
        /// <param name="PageNumber">page numer of the products</param>
        /// <param name="PageSize"> how many products </param>
        /// <returns></returns>
        /// 
        [HttpGet("AllProducts")]
        public ActionResult AllProducts(int PageNumber=1,int PageSize=10)
        {
            var total = _dbContext.products.Count();

            var Products = _dbContext.products
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var result = new
            {
                TotalCount = total,
                PageNumber = PageNumber,
                PageSize = PageSize,
                Data = Products
            };

            return Ok(result);

        }

        /// <summary>
        /// get product details 
        /// </summary>
        /// <param name="id">the id of the reqired product</param>
        /// <returns></returns>
        /// 
        [HttpGet("ProductDetails/{id}")]
        public ActionResult ProductDetails(int id)
        {
            var Product = _dbContext.products.Find(id);
            return Ok(Product);
        }

        /// <summary>
        /// Create new product
        /// </summary>
        /// <param name="ProductDTO">for more security</param>
        /// <returns></returns>
        /// 
        [HttpPost("CreateProduct")]
        public ActionResult CreateProduct([FromBody] ProductDTO ProductDTO )
        {
   
            if(ProductDTO == null)
            {
                return BadRequest();
            }

            var Product = new Product
            {
                Id = ProductDTO.Id,
                Name = ProductDTO.Name,
                Price = ProductDTO.Price,
                Quntity = ProductDTO.Quntity,
                Description = ProductDTO.Description,
            };

            _dbContext.products.Add(Product);
            _dbContext.SaveChanges();

            return Ok("the product "+" "+ProductDTO.Name+" "+"has been aded successfully");
        }



        [HttpPost("EditProduct")]
        public ActionResult EditProduct(int id, [FromBody] ProductDTO productDTO )
        {

            var ProductInDB=_dbContext.products.Find(id);

            if (ProductInDB == null)
                return BadRequest();



            ProductInDB.Name = productDTO.Name;
            ProductInDB.Price = productDTO.Price;
            ProductInDB.Quntity = productDTO.Quntity;
            ProductInDB.Description = productDTO.Description;
           

            _dbContext.SaveChanges();
            return Ok();
        }


        [HttpDelete("DeleteProduct")]
        public ActionResult DeleteProduct(int id)
        {
            var product = _dbContext.products.Find( id);
            if (product == null)
                return BadRequest("No such a user");
            _dbContext.products.Remove(product);
            _dbContext.SaveChanges();

            return Ok();
        }

       
    }
}
