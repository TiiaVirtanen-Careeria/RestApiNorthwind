using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestApiNorthwind.Models;

namespace RestApiNorthwind.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        //Alustettiin tietokanta yhteys PERINTEINEN TAPA
        //northwindContext db = new northwindContext();

        //DEpendency injektion tapa
        private northwindContext db;

        public CustomersController(northwindContext dbparametri)
        {
            db = dbparametri;
        }

        //Hakee kaikki asiakkaat
        [HttpGet]
        public ActionResult GetAllCustomers()
        {
            try
            {
                var asiakkaat = db.Customers.ToList();
                return Ok(asiakkaat);
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }

        //Hakee yhden asiakkaan pääavaimella
        [HttpGet("{id}")] //Näinkin voi tehdä
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

        //Asiakkaan poistaminen
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            try
            {
                var asiakas = db.Customers.Find(id);

                if (asiakas != null) // Jos id:llä löytyy asiakas
                {
                    db.Customers.Remove(asiakas);
                    db.SaveChanges();
                    return Ok("Asiakas " + asiakas.CompanyName + " poistetiin!");
                }
                else
                {
                    return NotFound("Asiakas id:llä " + id + " ei löytynyt");
                }
            }
            catch (Exception e)
            {

                return BadRequest(e.InnerException);
            }
        }

        //Asiakkaan muokkaaminen
        [HttpPut("{id}")]
        public ActionResult EditCustomer(string id, [FromBody]Customer customer) 
        {
            try
            {
                var asiakas = db.Customers.Find(id);

                if (asiakas != null)
                {
                    asiakas.CompanyName = customer.CompanyName;
                    asiakas.ContactName = customer.ContactName;
                    asiakas.Address = customer.Address;
                    asiakas.City = customer.City;
                    asiakas.Region = customer.Region;
                    asiakas.PostalCode = customer.PostalCode;
                    asiakas.Country = customer.Country;
                    asiakas.Phone = customer.Phone;
                    asiakas.Fax = customer.Fax;

                    db.SaveChanges();
                    return Ok("Muokattu asiakasta " + asiakas.CompanyName);
                }
                else
                {
                    return NotFound("Asiakas id:llä " + id + " ei löytynyt");
                }
            }
            catch (Exception e)
            {

                return BadRequest(e.InnerException);
            }
        }

        // Hakee nimen osalla: /api/companyname/hakusana
        [HttpGet("companyname/{cname}")]
        public ActionResult GetByName(string cname)
        {
            try
            {
                var cust = db.Customers.Where(c => c.CompanyName.Contains(cname));
                //var cust = from c in db.Customers where c.CompanyName.Contains(cname) select c; <-- sama mutta traditional
                //var cust = db.Customers.Where(c => c.CompanyName == cname); <-- perfect match

                return Ok(cust);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
