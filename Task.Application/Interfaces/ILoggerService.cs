using Task.Domain.Enums;

namespace Task.Application.Interfaces
{
    public interface ILoggerService
    {
        void Log(string message, string IpAddress, LogLevel logLevel);
    }
}
