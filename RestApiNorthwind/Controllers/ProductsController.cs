using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestApiNorthwind.Models;
using System.Xml.Linq;

namespace RestApiNorthwind.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private northwindContext db;
        public ProductsController(northwindContext dbparametri)
        {
            db = dbparametri;
        }

        [HttpGet("{id}")]
        public ActionResult GetOneProductById(int id)
        {
            try
            {
                var tuote = db.Products.Find(id);
                if (tuote != null)
                {
                    return Ok(tuote);
                }
                else
                {
                    return NotFound("Työntekijää ei löydy id:llä" + id + "!");
                }
            }
            catch (Exception e)
            {

                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }

        [HttpGet]
        public ActionResult GetAllProducts()
        {
            try
            {
                var tuote = db.Products.ToList();
                return Ok(tuote);
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }

        [HttpGet("companyname/{pname}")]
        public ActionResult GetByName(string pname)
        {
            try
            {
                var tuote = db.Products.Where(p => p.ProductName.Contains(pname));

                return Ok(tuote);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult AddNew([FromBody] Product tuote)
        {
            try
            {
                db.Products.Add(tuote);
                db.SaveChanges();
                return Ok("Uusi tuote lisätty " + tuote.ProductName);
            }
            catch (Exception e)
            {

                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }

        [HttpPut("{id}")]
        public ActionResult EditProduct(int id, [FromBody]Product tuo)
        {
            try
            {
                var tuote = db.Products.Find(id);

                if (tuote != null)
                {
                    tuote.ProductName = tuo.ProductName;
                    tuote.QuantityPerUnit = tuo.QuantityPerUnit;
                    tuote.UnitPrice = tuo.UnitPrice;
                    tuote.UnitsInStock = tuo.UnitsInStock;
                    tuote.UnitsOnOrder = tuo.UnitsOnOrder;
                    tuote.ReorderLevel = tuo.ReorderLevel;
                    tuote.Discontinued = tuo.Discontinued;



                    db.SaveChanges();
                    return Ok("Muokattu tuotetta " + tuote.ProductName);
                }
                else
                {
                    return NotFound("Tuotetta id:llä " + id + " ei löytynyt");
                }
            }
            catch (Exception e)
            {

                return BadRequest(e.InnerException);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var tuote = db.Products.Find(id);

                if (tuote != null)
                {
                    db.Products.Remove(tuote);
                    db.SaveChanges();
                    return Ok("Tuote " +  tuote.ProductName + " poistettiin!");
                }
                else
                {
                    return NotFound("Tuotetta " + id + " ei löytynyt!");
                }
            }
            catch (Exception e)
            {

                return BadRequest(e.InnerException);
            }
        }
    }
}
