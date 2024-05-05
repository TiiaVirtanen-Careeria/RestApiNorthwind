using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using RestApiNorthwind.Models;

namespace RestApiNorthwind.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly northwindContext db;

        public UsersController(northwindContext dbparametri)
        {
            db = dbparametri;
        }

        [HttpGet]
        public ActionResult GetAll() 
        {
            var users = db.Users;

            foreach (var user in users)
            {
                user.Password = null;
            }
            return Ok(users);
        }

        //Uuden lisääminen
        [HttpPost]
        public ActionResult PostCreateNew([FromBody] User u)
        {
            try
            {
                db.Users.Add(u);
                db.SaveChanges();
                return Ok("Lisättiin käyttäjä " + u.UserName);
            }
            catch (Exception e)
            {

                return BadRequest("Lisääminen ei onnistunut. Lisätietoja: " + e);
            }
        }
    }
}
