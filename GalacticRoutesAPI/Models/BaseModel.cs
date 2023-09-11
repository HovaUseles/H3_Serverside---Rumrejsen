namespace GalacticRoutesAPI.Models
{
    public abstract class BaseModel
    {
        public string Id { get; }

        public BaseModel()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
