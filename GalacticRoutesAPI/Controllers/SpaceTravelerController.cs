using GalacticRoutesAPI.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GalacticRoutesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpaceTravelerController : ControllerBase
    {
        private readonly SpaceTravelerManager _spaceTravelerManager;

        public SpaceTravelerController(
            SpaceTravelerManager spaceTravelerManager)
        {
            this._spaceTravelerManager = spaceTravelerManager;
        }

        [HttpGet]
        public IActionResult GetTravelers() 
        {
            return StatusCode(StatusCodes.Status200OK, _spaceTravelerManager.GetAll());
        }
    }
}
