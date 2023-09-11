using GalacticRoutesAPI.Models;
using GalacticRoutesAPI.Repositories;

namespace GalacticRoutesAPI.Managers
{
    public class RequestManager
    {
        private readonly IGenericRepository<Request> _requestRepository;

        public RequestManager(IGenericRepository<Request> requestRepository)
        {
            this._requestRepository = requestRepository;
        }

        public Request AddRequest(Request request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return _requestRepository.Add(request);
        }
    }
}
