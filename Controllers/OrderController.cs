using Microsoft.AspNetCore.Mvc;
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
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [HttpGet("Orders")]

        public IActionResult GetAll(int PageNumber=1,int PageSize=10)
        {
            var OrderCount=_dbContext.orders.Count();
            var orders= _dbContext.orders
                .Skip((PageNumber-1)*PageSize)
                .Take(PageSize)
                .ToList();

            var result = new
            {
                orders = orders,
                ordersCount = OrderCount,
                pageSiaze = PageSize,
                pageNumber = PageNumber
            };
            return Ok(result);
        }

       /// <summary>
       /// order Deatils
       /// </summary>
       /// <param name="Id"></param>
       /// <returns></returns>
        [HttpGet("OrderDetails/{Id}")]
        public IActionResult GetOrderDetails(int Id)
        {
            if(Id.Equals(null))
                return NotFound("No order");

            var order = _dbContext.orders.Find(Id);
            return Ok(order);

        }

        /// <summary>
        /// just adding a new order 
        /// </summary>
        /// <param name="value"></param>
        [HttpPost("AddNewOrder")]
        public IActionResult NewOrder([FromBody] OrderDTO orderDTO)
        {
            var order = new Order
            {
                Name = orderDTO.Name,
                State = orderDTO.State,
            };

            _dbContext.orders.Add(order);
            return Ok("Order has been added ");
        }

        /// <summary>
        /// updating order info 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPost("UpdateOrder/{id}")]
        public IActionResult UpdateOrder(int id, [FromBody] OrderDTO orderDTO)
        {
            var OrderinDB=_dbContext.orders.Find(id);

            if (OrderinDB == null)
                return NotFound();
            OrderinDB.Name=orderDTO.Name;
            OrderinDB.State=orderDTO.State;
            return Ok("OrderUpdated");
        }

        /// <summary>
        /// delting order 
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("DeleteOrder/{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var Order= _dbContext.orders.Find(id);
            if(Order==null)
                return NotFound("order not found");
            _dbContext.orders.Remove(Order);
            return Ok("Order Deleted");
        }
    }
}
// im the one who wrote this code :bashar66629@gmail.com dont fucking mail me about this shit