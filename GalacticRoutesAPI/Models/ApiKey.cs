namespace GalacticRoutesAPI.Models
{
    public class ApiKey : BaseModel
    {
        public string Value { get; }
        public DateTime ExpirationDate { get; }

        public ApiKey(
            string value, 
            DateTime expirationDate) 
        {
            Value = value;
            ExpirationDate = expirationDate;
        }
    }
}
