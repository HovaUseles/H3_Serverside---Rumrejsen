using GalacticRoutesAPI.Models;

namespace GalacticRoutesAPI.Repositories.Interfaces
{
    public interface IApiKeyRepository : IGenericRepository<ApiKey>
    {
        /// <summary>
        /// Get a ApiKey by its value
        /// </summary>
        /// <returns>The Api key if it exist</returns>
        public ApiKey GetByValue(string value);
    }
}
