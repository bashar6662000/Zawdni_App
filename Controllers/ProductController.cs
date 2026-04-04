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
                .Select(p=> new ProductDTO
                { Id=p.Id,
                Name=p.Name,
                Price=p.Price,
                Quntity=p.Quntity,
                Description=p.Description,
                })
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
                Name = ProductDTO.Name,
                Price = ProductDTO.Price,
                Quntity = ProductDTO.Quntity,
                Description = ProductDTO.Description,
            };

            _dbContext.products.Add(Product);
            _dbContext.SaveChanges();

            return Ok("the product "+" "+ProductDTO.Name+" "+"has been aded successfully");
        }


        /// <summary>
        /// to update product info
        /// </summary>
        /// <param name="Id">the ID of the required product</param>
        /// <param name="productDTO">for more security </param>
        /// <returns></returns>
        [HttpPut("EditProduct")]
        public ActionResult EditProduct(int Id, [FromBody] ProductDTO productDTO )
        {

            var ProductInDB=_dbContext.products.Find(Id);

            if (ProductInDB == null)
                return BadRequest();



            ProductInDB.Name = productDTO.Name;
            ProductInDB.Price = productDTO.Price;
            ProductInDB.Quntity = productDTO.Quntity;
            ProductInDB.Description = productDTO.Description;
           

            _dbContext.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// to remove product from the database
        /// </summary>
        /// <param name="Id">the ID of the required product</param>
        /// <returns></returns>
        [HttpDelete("DeleteProduct")]
        public ActionResult DeleteProduct(int Id)
        {
            var product = _dbContext.products.Find(Id);
            if (product == null)
                return BadRequest("No such a product");
            _dbContext.products.Remove(product);
            _dbContext.SaveChanges();

            return Ok();
        }

       
    }
}
// for any question please send me a message via my email :bashar66629@gmail.com