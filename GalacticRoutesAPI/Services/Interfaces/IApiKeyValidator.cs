using GalacticRoutesAPI.Models;

namespace GalacticRoutesAPI.Services.Interfaces
{
    public interface IApiKeyValidator
    {
        public bool ValidateApiKey(ApiKey apiKey);
    }
}
