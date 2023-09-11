using GalacticRoutesAPI.Exceptions;
using GalacticRoutesAPI.Models;
using GalacticRoutesAPI.Repositories;
using GalacticRoutesAPI.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace GalacticRoutesAPI.Services
{
    public class ApiKeyValidator : IApiKeyValidator
    {
        private readonly IGenericRepository<Request> _requestRepository;
        private readonly IGenericRepository<SpaceTraveler> _spaceTravelerRepository;

        public ApiKeyValidator(
            IGenericRepository<Request> requestRepository,
            IGenericRepository<SpaceTraveler> spaceTravelerRepository
            )
        {
            this._requestRepository = requestRepository;
            this._spaceTravelerRepository = spaceTravelerRepository;
        }

        public bool ValidateApiKey(ApiKey apiKey)
        {
            if (apiKey == null)
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            // Key expired
            if(apiKey.ExpirationDate < DateTime.Now) 
            {
                return false;
            }

            SpaceTraveler spaceTraveler = _spaceTravelerRepository.GetAll().FirstOrDefault(st => st.ApiKey.Id == apiKey.Id);

            if(spaceTraveler == null)
            {
                return false;
            }
            
            
            if (spaceTraveler.IsCadet)
            {
                // Check for requests in the last 30 minutes
                var thirtyMinutesAgo = DateTime.Now.AddMinutes(-30);
                var recentRequests = spaceTraveler.Requests.Where(r => r.RequestTime > thirtyMinutesAgo).Count();

                if (recentRequests >= 5)
                {
                    throw new RateLimitExceededException("Number of allowed requests exceeded", recentRequests);
                }
            }

            return true;
        }
    }
}
