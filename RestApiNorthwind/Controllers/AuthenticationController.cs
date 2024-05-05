using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestApiNorthwind.Models;
using RestApiNorthwind.Services.Interfaces;

namespace RestApiNorthwind.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticateService _authenticateService;

        public AuthenticationController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        // Tähän tulee Front endin kirjautumisyritys
        [HttpPost]
        public ActionResult Post([FromBody] Credentials tunnukse)
        {
            var loggedUser = _authenticateService.Authenticate(tunnukse.UserName, tunnukse.Password);

            if (loggedUser == null)
                return BadRequest(new { message = "Käyttäjätunus tai salasana on virheellinen" });

            return Ok(loggedUser); // Palautus front endiin (sis. vain loggedUser luokan mukaiset kentät)
        }
    }
}
