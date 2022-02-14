namespace Storage.Core.Exceptions
{
    public class InvalidFileCategoryException : ApplicationException
    {
        public InvalidFileCategoryException(string msg) : base(msg)
        {
        }
    }
}
