using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestApiNorthwind.Models;

namespace RestApiNorthwind.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private northwindContext db;
        public EmployeesController(northwindContext dbparametri)
        {
            db = dbparametri;
        }

        [HttpGet("{id}")]
        public ActionResult GetOneEmployeeById(int id) 
        {
            try
            {
                var tyontekija = db.Employees.Find(id);
                if (tyontekija != null)
                {
                    return Ok(tyontekija);
                }
                else
                {
                    return NotFound($"Työntekijää ei löydy id:llä {id}!");
                }
            }
            catch (Exception e)
            {

                return BadRequest($"Tapahtui virhe. Lue lisää: {e.InnerException}");
            }
        }

        [HttpGet]
        public ActionResult GetAllEmployees()
        {
            try
            {
                var tyontekijat = db.Employees.ToList();
                return Ok(tyontekijat);
            }
            catch (Exception e)
            {
                return BadRequest($"Tapahtui virhe. Lue lisää: {e.InnerException}");
            }
        }

        [HttpGet("companyname/{lname}")]
        public ActionResult GetByName(string lname)
        {
            try
            {
                var tyontekija = db.Employees.Where(e => e.LastName == lname);

                return Ok(tyontekija);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult AddNew([FromBody] Employee tyon)
        {
            try
            {
                db.Employees.Add(tyon);
                db.SaveChanges();
                return Ok($"Lisättiin uusi työntekijä {tyon.FirstName} {tyon.LastName}");
            }
            catch (Exception e)
            {

                return BadRequest($"Tapahtui virhe. Lue lisää: {e.InnerException}");
            }
        }

        [HttpPut("{id}")]
        public ActionResult EditEmployee(int id, [FromBody]Employee employee)
        {
            try
            {
                var tyontekija = db.Employees.Find(id);

                if (tyontekija != null)
                {
                    tyontekija.LastName = employee.LastName;
                    tyontekija.FirstName = employee.FirstName;
                    tyontekija.BirthDate = employee.BirthDate;
                    tyontekija.Address = employee.Address;
                    tyontekija.City = employee.City;
                    tyontekija.PostalCode = employee.PostalCode;
                    tyontekija.Country = employee.Country;
                    tyontekija.HomePhone = employee.HomePhone;


                    db.SaveChanges();
                    return Ok($"Muokattu työntekijää {tyontekija.FirstName} {tyontekija.LastName}");
                }
                else
                {
                    return NotFound($"Työntekijää id:llä {id} ei löytynyt");
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
                var tyontekija = db.Employees.Find(id);

                if (tyontekija != null)
                {
                    db.Employees.Remove(tyontekija);
                    db.SaveChanges();
                    return Ok($"Työntekijä {tyontekija.FirstName} {tyontekija.LastName} poistettiin!");
                }
                else
                {
                    return NotFound($"Työntekijää id:llä {id} ei löytynyt");
                }
            }
            catch (Exception e)
            {

                return BadRequest(e.InnerException);
            }
        }
    }
}
