using GalacticRoutesAPI.Models;
using GalacticRoutesAPI.Repositories.Interfaces;

namespace GalacticRoutesAPI.Repositories
{
    public class ApiKeyRepostory : GenericMockRepository<ApiKey>, IApiKeyRepository
    {
        public ApiKey GetByValue(string value)
        {
            ApiKey? key = _mockData.FirstOrDefault(ak => ak.Value == value);
            if (key == null)
            {
                throw new KeyNotFoundException();
            }
            return key;
        }
    }
}
