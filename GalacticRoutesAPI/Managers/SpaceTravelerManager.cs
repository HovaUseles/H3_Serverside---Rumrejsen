using GalacticRoutesAPI.Models;
using GalacticRoutesAPI.Repositories;
using GalacticRoutesAPI.Repositories.Interfaces;
using GalacticRoutesAPI.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace GalacticRoutesAPI.Managers
{
    public class SpaceTravelerManager
    {
        private readonly IGenericRepository<SpaceTraveler> _spaceTravelerRepository;
        private readonly IApiKeyRepository _apiKeyRepository;
        private readonly IApiKeyGenerator _apiKeyGenerator;

        public SpaceTravelerManager(
            IGenericRepository<SpaceTraveler> spaceTravelerRepository,
            IApiKeyRepository apiKeyRepository,
            IApiKeyGenerator apiKeyGenerator)
        {
            this._spaceTravelerRepository = spaceTravelerRepository;
            this._apiKeyRepository = apiKeyRepository;
            this._apiKeyGenerator = apiKeyGenerator;
        }

        public SpaceTraveler GetWithApiKeyValue(string apiKeyValue)
        {
            ApiKey apiKey = _apiKeyRepository.GetByValue(apiKeyValue);
            SpaceTraveler spaceTraveler = _spaceTravelerRepository.GetAll().FirstOrDefault(st => st.ApiKey.Id == apiKey.Id);
            return spaceTraveler;
        }

        public SpaceTraveler[] GetAll()
        {
            return _spaceTravelerRepository.GetAll();
        }

        public SpaceTraveler Add(SpaceTraveler spaceTraveler)
        {
            SpaceTraveler createdSpaceTraveler = _spaceTravelerRepository.Add(spaceTraveler);
            ApiKey createdApiKey = new ApiKey(
                    value: _apiKeyGenerator.GenerateKey(),
                    expirationDate: DateTime.Now.AddDays(7)
                );
            createdSpaceTraveler.ApiKey = _apiKeyRepository.Add(createdApiKey);
            return createdSpaceTraveler;
        }
    }
}
