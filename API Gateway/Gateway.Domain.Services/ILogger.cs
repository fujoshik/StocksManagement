namespace Gateway.Domain.Services
{
    internal interface ILogger<T>
    {
        void LogError(string v);
        void LogInformation(string v);
        void LogWarning(string v);
    }
}