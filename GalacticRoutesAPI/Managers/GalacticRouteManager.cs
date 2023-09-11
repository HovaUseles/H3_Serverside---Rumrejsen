using GalacticRoutesAPI.Models;
using GalacticRoutesAPI.Repositories;

namespace GalacticRoutesAPI.Managers
{
    public class GalacticRouteManager
    {
        private readonly IGenericRepository<GalacticRoute> _galacticRouteRepository;

        public GalacticRouteManager(IGenericRepository<GalacticRoute> galacticRouteRepository)
        {
            this._galacticRouteRepository = galacticRouteRepository;
        }

        /// <summary>
        /// Gets a random route based on the Space Traveler. The route cannot be the same at the latest route traveled
        /// </summary>
        /// <param name="traveler">The traveler to pick a route for</param>
        /// <returns>The randomly picked route</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public GalacticRoute GetRandomRoute(SpaceTraveler traveler)
        {
            if (traveler == null)
            {
                throw new ArgumentNullException(nameof(traveler));
            }

            IEnumerable<GalacticRoute> availableRoutes;
            // Get latest traveled route
            GalacticRoute? latestRoute = traveler.PreviousRoutes?.Peek();

            // Cadets are only allowed to take short routes
            if (traveler.IsCadet)
            {
                availableRoutes = _galacticRouteRepository.GetAll().Where(gr => gr.MinDuration < TimeSpan.FromDays(365));
            }
            else
            {
                availableRoutes = _galacticRouteRepository.GetAll();
            }

            if(latestRoute != null)
            {
                availableRoutes = availableRoutes.Where(gr => gr.Id != latestRoute.Id);
            }

            Random random = new Random();
            int numberOfRoutes = availableRoutes.Count();
            GalacticRoute chosenRoute = availableRoutes.ElementAt(random.Next(0, numberOfRoutes));
            return chosenRoute;
        }

        public GalacticRoute Add(GalacticRoute route)
        {
            return _galacticRouteRepository.Add(route);
        }
    }
}
