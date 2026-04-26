namespace ObjectPoolSystem.Domain.Exceptions;

public class PoolExhaustedException : Exception
{
    public PoolExhaustedException()
        : base("The object pool is exhausted. No resources became available within the timeout period.")
    {
    }

    public PoolExhaustedException(string message)
        : base(message)
    {
    }

    public PoolExhaustedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
