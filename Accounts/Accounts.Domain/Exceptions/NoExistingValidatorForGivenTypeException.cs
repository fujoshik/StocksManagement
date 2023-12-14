namespace Accounts.Domain.Exceptions
{
    public class NoExistingValidatorForGivenTypeException : Exception
    {
        public NoExistingValidatorForGivenTypeException(Type dtoType)
            : base($"Requested validator for type: {dtoType}, was not found!")
        {
        }
    }
}
