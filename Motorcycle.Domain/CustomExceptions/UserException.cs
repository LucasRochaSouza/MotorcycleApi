namespace Domain.CustomExceptions
{
    public class UserException : Exception
    {
        public UserException(params string[] messages) : base(string.Join(",\n", messages)) { }
    }
}