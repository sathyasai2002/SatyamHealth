namespace SatyamHealthCare.Exceptions
{
    public class EntityNotFoundException:RepositoryException
    {
        public EntityNotFoundException(string entityName, int id) : base($"The {entityName} with ID {id} was not found.") { }
    }

    public class EntityAddFailedException : RepositoryException {
        public EntityAddFailedException(string entityName)
               : base($"Failed to add {entityName}.") { }

        public EntityAddFailedException(string entityName, Exception innerException)
           : base($"Failed to add {entityName}.", innerException) { }

    }

    public class EntityUpdateFailedException : RepositoryException {

        public EntityUpdateFailedException(string entityName, int id)
               : base($"Failed to update {entityName} with ID {id}.") { }
        public EntityUpdateFailedException(string entityName, int id, Exception innerException)
            : base($"Failed to update {entityName} with ID {id}.", innerException) { }

    }

    public class EntityDeleteFailedException : RepositoryException {

        public EntityDeleteFailedException(string entityName, int id)
            : base($"Failed to delete {entityName} with ID {id}.") { }

        public EntityDeleteFailedException(string entityName, int id, Exception innerException)
          : base($"Failed to delete {entityName} with ID {id}.", innerException) { }

    }

}
