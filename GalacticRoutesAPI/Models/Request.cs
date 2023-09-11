namespace GalacticRoutesAPI.Models
{

    public class Request : BaseModel
    {
        public DateTime RequestTime { get; set;  } = DateTime.Now;


        public Request()
        {
            
        }
    }
}
