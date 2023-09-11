using GalacticRoutesAPI.Models;
using GalacticRoutesAPI.Repositories;
using GalacticRoutesAPI.Repositories.Interfaces;
using GalacticRoutesAPI.Services.Interfaces;

namespace GalacticRoutesAPI.Managers
{
    public class ApiKeyManager
    {
        private readonly IApiKeyRepository _apiKeyRepository;
        private readonly IApiKeyValidator _apiKeyValidator;

        public ApiKeyManager(
            IApiKeyRepository apiKeyRepository,
            IApiKeyValidator apiKeyValidator)
        {
            this._apiKeyRepository = apiKeyRepository;
            this._apiKeyValidator = apiKeyValidator;
        }

        public ApiKey[] GetApiKeys()
        {
            return _apiKeyRepository.GetAll();
        }

        public bool ValidateApiKey(string apiKeyValue)
        {
            try
            {
                ApiKey key = _apiKeyRepository.GetByValue(apiKeyValue);
                return _apiKeyValidator.ValidateApiKey(key);
            }
            catch (Exception ex) when (
                ex is ArgumentNullException 
                ||
                ex is KeyNotFoundException
                )
            {
                return false;
            }
        }
    }
}
