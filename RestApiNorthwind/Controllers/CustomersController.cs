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
            try
            {
                var asiakkaat = db.Customers.ToList();
                return Ok(asiakkaat);
            }
            catch(Exception e) 
            { 
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }

        //Hakee yhden asiakkaan pääavaimella
        [HttpGet(("{id}"))] //Näinkin voi tehdä
        // [Route("{id}")] // Sulkujen sisällä vapaasti nimettävä
        public ActionResult GetOneCustomerById(string id)
        {
            try 
            {
                var asiakas = db.Customers.Find(id);
                if (asiakas != null)
                {
                    return Ok(asiakas);
                }
                else
                {
                    //return BadRequest("Asiakasta id:llä " + id + " ei löydy!"); //perinteinen tapa liittää muuttuja
                    return NotFound($"Asiakasta id:llä {id} ei löydy!"); //String interpolation
                }
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e);
            }
        }

        //Uuden lisääminen
        [HttpPost]
        public ActionResult AddNew([FromBody] Customer cust) 
        {
            try
            {
                db.Customers.Add(cust);
                db.SaveChanges();
                return Ok($"Lisättiin uusi asiakas {cust.CompanyName} from {cust.City}");
            }
            catch (Exception e)
            {

                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }
    }
}
