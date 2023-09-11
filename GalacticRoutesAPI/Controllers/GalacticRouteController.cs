using GalacticRoutesAPI.Managers;
using GalacticRoutesAPI.Middleware;
using GalacticRoutesAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GalacticRoutesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalacticRouteController : ControllerBase
    {
        private readonly GalacticRouteManager _galacticRouteManager;
        private readonly SpaceTravelerManager _spaceTravelerManager;

        public GalacticRouteController(
            GalacticRouteManager galacticRouteManager,
            SpaceTravelerManager spaceTravelerManager
            )
        {
            this._galacticRouteManager = galacticRouteManager;
            this._spaceTravelerManager = spaceTravelerManager;
        }

        [HttpGet]
        [ApiKey]
        public ActionResult<GalacticRoute> GetNewRoute([FromHeader] string apiKey)
        {
            SpaceTraveler spaceTraveler = _spaceTravelerManager.GetWithApiKeyValue(apiKey);
            GalacticRoute galacticRoute = _galacticRouteManager.GetRandomRoute(spaceTraveler);
            return StatusCode(StatusCodes.Status200OK, galacticRoute);
        }
    }
}
