namespace GalacticRoutesAPI.Exceptions
{
    public class RateLimitExceededException : Exception
    {
        public int Rate { get; set; }

        public RateLimitExceededException(int rate)
        {
            Rate = rate;
        }
        public RateLimitExceededException(string message, int rate) : base(message)
        {
            Rate = rate;
        }
    }
}
