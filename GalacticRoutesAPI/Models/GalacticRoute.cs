namespace GalacticRoutesAPI.Models
{
    public class GalacticRoute : BaseModel
    {
        public string Name { get; set; }
        public string StartDestination { get; set; }
        public string EndDestination { get; set; }
        public string[] NavigationPoints { get; set; }
        public TimeSpan MinDuration { get; set; }
        public TimeSpan? MaxDuration { get; set; }
        public string[] Dangers { get; set; }
        public string FuelUsage { get; set; }
        public string Description { get; set; }

    }
}
