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

        //Muokkaaminen
        [HttpPut("{id}")]
        public ActionResult EditUser(int id, [FromBody] User user)
        {
            try
            {
                var kayttaja = db.Users.Find(id);

                if (kayttaja != null)
                {
                    kayttaja.FirstName = user.FirstName;
                    kayttaja.LastName = user.LastName;
                    kayttaja.Email = user.Email;
                    kayttaja.UserName = user.UserName;
                    kayttaja.Password = user.Password;
                    kayttaja.AccesslevelId = user.AccesslevelId;

                    db.SaveChanges();
                    return Ok("Muokattu käyttäjää " + kayttaja.FirstName + " " + kayttaja.LastName);
                }
                else
                {
                    return NotFound("Käyttäjää id:llä " + id + " ei löytynyt");
                }
            }
            catch (Exception e)
            {

                return BadRequest(e.InnerException);
            }
        }

        //Poistaminen
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var kayttaja = db.Users.Find(id);

                if (kayttaja != null)
                {
                    db.Users.Remove(kayttaja);
                    db.SaveChanges();
                    return Ok("Käyttäjä " + kayttaja.FirstName + " " + kayttaja.LastName + " poistetiin!");
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
    }
}
