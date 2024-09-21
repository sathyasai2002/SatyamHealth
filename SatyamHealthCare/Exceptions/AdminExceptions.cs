namespace SatyamHealthCare.Exceptions
{
    public class AdminNotFoundException : Exception
    {
        public AdminNotFoundException() : base("Admin not found.") { }

        public AdminNotFoundException(string message) : base(message) { }

        public AdminNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class AdminDeleteException : Exception
    {
        public AdminDeleteException() : base("Failed to delete the admin.") { }

        public AdminDeleteException(string message) : base(message) { }

        public AdminDeleteException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class AdminUpdateException : Exception
    {
        public AdminUpdateException() : base("Failed to update the admin.") { }

        public AdminUpdateException(string message) : base(message) { }

        public AdminUpdateException(string message, Exception innerException) : base(message, innerException) { }
    }

}
