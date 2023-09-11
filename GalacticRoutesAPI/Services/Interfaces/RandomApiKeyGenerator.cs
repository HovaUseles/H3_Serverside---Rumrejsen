using GalacticRoutesAPI.Models;

namespace GalacticRoutesAPI.Services.Interfaces
{
    public class RandomApiKeyGenerator : IApiKeyGenerator
    {
        public string GenerateKey()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
