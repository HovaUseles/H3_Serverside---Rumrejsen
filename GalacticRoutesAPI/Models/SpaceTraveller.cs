namespace GalacticRoutesAPI.Models
{
    public class SpaceTraveler : BaseModel
    {
        public string Name { get; set; }
        public bool IsCadet { get; set; }
        public List<Request> Requests { get; set; }
        public Stack<GalacticRoute> PreviousRoutes { get; set; }
        public ApiKey ApiKey { get; set; }

        public SpaceTraveler(
            string name,
            bool isCadet)
        {
            Name = name;
            IsCadet = isCadet;
            Requests = new List<Request>();
        }
    }
}
