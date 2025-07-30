using EnergTestTask.BL.Interfaces;
using EnergTestTask.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnergTestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FieldsController : Controller
    {
        private readonly IKmlFieldsService _fieldsService;
        public FieldsController(IKmlFieldsService fieldsService)
        {
            _fieldsService = fieldsService;
        }
        // GET: All fields
        [HttpGet]
        public IActionResult GetFields()
        {
            return Ok(_fieldsService.GetFields());
        }

        // GET: FieldsController/Details/5
        [HttpGet("{id}")]
        public IActionResult GetSizeById(int id)
        {
            var size = _fieldsService.GetSizeById(id);
            if (size == null)
                return NotFound();

            return Ok(size);
        }


        [HttpGet("distance")]
        public double GetDistance([FromQuery] int id, [FromQuery] double pointLat, [FromQuery] double pointLng)
        {
            var point = new double[] {pointLat, pointLng };
            return _fieldsService.GetDistanceFromCenterToPoint(point, id);
        }

        [HttpGet("IsInArea")]
        public IActionResult PointInField([FromQuery] double pointLat, [FromQuery] double pointLng)
        {
            var point = new double[] { pointLat, pointLng };
            var result = _fieldsService.IsPointInArea(point);

            if (result is null)
                return Ok(false);

            return Ok(new { id = result.Value.Id, name = result.Value.Name });
        }

    }
}
