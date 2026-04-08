using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Zawdni.api.Data;
using Zawdni.api.Models;
using Zawdni.Models;
using Zawdni.Models.DTO;

namespace Zawdni.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ZawdniDbContext _dbContext;

        public OrderController(ZawdniDbContext zawdniDbContext)
        {
            _dbContext = zawdniDbContext;
        }

        /// <summary>
        /// return all orders but paginated so the server dont boom
        /// </summary>
        [HttpGet("Orders")]
        public IActionResult GetAll(int PageNumber = 1, int PageSize = 10)
        {
            var OrderCount = _dbContext.orders.Count();
            var orders = _dbContext.orders
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .Select(o => new OrderDTO
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    DeliveryDate = o.DeliveryDate,
                    State = o.State
                })
                .ToList();

            var result = new
            {
                orders = orders,
                ordersCount = OrderCount,
                pageSize = PageSize,
                pageNumber = PageNumber
            };
            return Ok(result);
        }

        /// <summary>
        /// order details
        /// </summary>
        [HttpGet("OrderDetails/{Id}")]
        public IActionResult OrderDetails(int Id)
        {
            var order = _dbContext.orders
                .Include(o => o.orderProducts)
                    .ThenInclude(op => op.product)
                .FirstOrDefault(o => o.Id == Id);

            if (order == null)
                return NotFound("No order found");

            var result = new OrderDTO
            {
                Id = order.Id,
                State = order.State,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate,
                Total = order.Total,
                UserID = order.UserID,
                Products = order.orderProducts.Select(op => new OrderProductDTO
                {
                    ProductId = op.ProductId,
                    Quantity = op.Quantity,
                    UnitPrice = op.UnitPrice
                }).ToList()
            };

            return Ok(result);
        }

        /// <summary>
        /// adding a new order
        /// </summary>
        [HttpPost("AddNewOrder")]
        public IActionResult NewOrder([FromBody] OrderDTO orderDTO)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var orderInDB = new Order
            {
                State = false,
                OrderDate = DateTime.Now,
                DeliveryDate = null,
                UserID = int.Parse(userIdClaim),
                Total = 0
            };

            _dbContext.orders.Add(orderInDB);
            _dbContext.SaveChanges();

            decimal total = 0;
            foreach (var item in orderDTO.Products)
            {
                var product = _dbContext.products.Find(item.ProductId);
                if (product == null)
                    return NotFound($"Product {item.ProductId} not found");

                _dbContext.orderProducts.Add(new OrderProduct
                {
                    OrderId = orderInDB.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });

                total += product.Price * item.Quantity;
            }

            orderInDB.Total = total;
            _dbContext.SaveChanges();

            return Ok("Order has been added successfully");
        }

        /// <summary>
        /// confirm order and set delivery date
        /// </summary>
        [HttpPut("ConfirmOrder/{id}")]
        public IActionResult ConfirmOrder(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var order = _dbContext.orders.Find(id);
            if (order == null)
                return NotFound();

            if (order.UserID != int.Parse(userIdClaim))
                return Forbid();

            order.State = true;
            order.DeliveryDate = DateTime.Now;
            _dbContext.SaveChanges();

            return Ok("Order has been confirmed");
        }

        /// <summary>
        /// add a product to an existing order
        /// </summary>
        [HttpPost("AddProductToOrder/{orderId}")]
        public IActionResult AddProduct(int orderId, [FromBody] OrderProductDTO item)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var order = _dbContext.orders.Find(orderId);
            if (order == null)
                return NotFound("Order not found");

            if (order.UserID != int.Parse(userIdClaim))
                return Forbid();

            var product = _dbContext.products.Find(item.ProductId);
            if (product == null)
                return NotFound($"Product {item.ProductId} not found");

            _dbContext.orderProducts.Add(new OrderProduct
            {
                OrderId = orderId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });

            order.Total += product.Price * item.Quantity;
            _dbContext.SaveChanges();

            return Ok("Product added");
        }

        /// <summary>
        /// remove a product from an order
        /// </summary>
        [HttpDelete("RemoveProductFromOrder/{orderId}/{productId}")]
        public IActionResult RemoveProduct(int orderId, int productId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var order = _dbContext.orders.Find(orderId);
            if (order == null)
                return NotFound("Order not found");

            if (order.UserID != int.Parse(userIdClaim))
                return Forbid();

            var orderProduct = _dbContext.orderProducts
                .FirstOrDefault(op => op.OrderId == orderId && op.ProductId == productId);
            if (orderProduct == null)
                return NotFound("Product not in order");

            order.Total -= orderProduct.UnitPrice * orderProduct.Quantity;
            _dbContext.orderProducts.Remove(orderProduct);
            _dbContext.SaveChanges();

            return Ok("Product removed");
        }

        /// <summary>
        /// update quantity of a product in an order
        /// </summary>
        [HttpPut("UpdateProductQuantity/{orderId}/{productId}")]
        public IActionResult UpdateQuantity(int orderId, int productId, [FromBody] int newQuantity)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var order = _dbContext.orders.Find(orderId);
            if (order == null)
                return NotFound("Order not found");

            if (order.UserID != int.Parse(userIdClaim))
                return Forbid();

            var orderProduct = _dbContext.orderProducts
                .FirstOrDefault(op => op.OrderId == orderId && op.ProductId == productId);
            if (orderProduct == null)
                return NotFound("Product not in order");

            order.Total -= orderProduct.UnitPrice * orderProduct.Quantity;
            order.Total += orderProduct.UnitPrice * newQuantity;
            orderProduct.Quantity = newQuantity;
            _dbContext.SaveChanges();

            return Ok("Quantity updated");
        }

        /// <summary>
        /// deleting order
        /// </summary>
        [HttpDelete("DeleteOrder/{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var order = _dbContext.orders.Find(id);
            if (order == null)
                return NotFound("Order not found");

            if (order.UserID != int.Parse(userIdClaim))
                return Forbid();

            _dbContext.orders.Remove(order);
            _dbContext.SaveChanges();

            return Ok("Order Deleted");
        }
    }
}
// im the one who wrote this code :bashar66629@gmail.com dont fucking mail me about this shit