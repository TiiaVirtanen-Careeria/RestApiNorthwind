using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestApiNorthwind.Models;

namespace RestApiNorthwind.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        //Alustettiin tietokanta yhteys
        northwindContext db = new northwindContext();

        //Hakee kaikki asiakkaat
        [HttpGet]
        public ActionResult GetAllCustomers() 
        {
            var asiakkaat = db.Customers.ToList();
            return Ok(asiakkaat);
        }
    }
}
