using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TShirt.API.Data.Model;
using TShirt.API.Model;

namespace TShirt.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TShirtController : Controller
    {
        private readonly ITShirtService _tShirt;

        public TShirtController(ITShirtService tshirt)
        {
            _tShirt = tshirt;
        }

        /// <summary>
        /// This my first api and this will insert a tshirt data
        /// </summary>
        /// <param name="shirt"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<ActionResult<Shirt>> AddTshirt([FromForm] Shirt shirt) => Ok(await _tShirt.AddTShirt(shirt));
       
        [HttpGet("gettshirts")]
        public async Task<ActionResult<IEnumerable<Shirt>>> GetAllTShirts() => Ok(await _tShirt.GetAllTShirts());

        [HttpGet("gettShirt")]
        public async Task<ActionResult<IEnumerable<Shirt>>> GetTShirtById(int id) => Ok(await _tShirt.GetTShirtById(id));

        [HttpGet("getsizes")]
        public async Task<ActionResult<IEnumerable<Option>>> GetAllSizes() => Ok(await _tShirt.GetAllSizes());

        [HttpGet("getstyles")]
        public async Task<ActionResult<IEnumerable<Option>>> GetAllStyles() => Ok(await _tShirt.GetAllStyles());

        [HttpDelete("delete")]
        public async Task<ActionResult<bool>> DeleteTshirt(int id) => Ok(await _tShirt.DeleteTShirt(id, 1));

        [HttpGet("image")]
        public IActionResult GetFile(string file)
        {
            byte[] byteArray = _tShirt.GetFile(file);
            return new FileContentResult(byteArray, "application/octet-stream");
        }

        [HttpPost("update")]
        public async Task<ActionResult<bool>> UpdateTShirt([FromForm] Shirt shirt) => Ok(await _tShirt.UpdateShirt(shirt));

    }
}
