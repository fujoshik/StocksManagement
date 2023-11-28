namespace Accounts.Domain.Exceptions
{
    public class UnsuccessfulTransactionException : Exception
    {
        public UnsuccessfulTransactionException()
            : base("The transaction was unsuccessfull") { }

        public UnsuccessfulTransactionException(Exception inner)
            : base("The transaction was unsuccessfull", inner) { }
    }
}
