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
        public List<Field> GetFields()
        {
            return _fieldsService.GetFields();
        }

        // GET: FieldsController/Details/5
        [HttpGet("{id}")]
        public int? GetSizeById(int id)
        {
            return _fieldsService.GetSizeById(id);
        }
    }
}
