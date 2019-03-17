namespace Autofake
{
    public interface IProgram
    {
        void Execute();
    }

    public interface ILogger
    {
        void Log(
            LogLevel logLevel, 
            string message);
    }

    public interface IEmailSender
    {
        void Send(
            string toAddress, 
            string plainTextContent);
    }

    public enum LogLevel
    {
        Information,
        Warning,
        Error
    }
}
