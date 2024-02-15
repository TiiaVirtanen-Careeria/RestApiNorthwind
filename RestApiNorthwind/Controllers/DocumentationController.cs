using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestApiNorthwind.Models;

namespace RestApiNorthwind.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentationController : ControllerBase
    {
        private northwindContext db;

        public DocumentationController(northwindContext dbparametri)
        {
            db = dbparametri;
        }
        private const int code = 000;
        [HttpPost("{keycode}")]
        public ActionResult GetDocumentation(int keycode) 
        {
            if (keycode == code)
            {
                var dokkarit = db.Documentations.ToList();
                return Ok(dokkarit);
            }
            else
            {
                return NotFound("Documentation missing! " + DateTime.Now);
            }
        }
    }
}
