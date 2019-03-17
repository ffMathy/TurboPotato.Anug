using Microsoft.Extensions.DependencyInjection;

namespace Autofake
{
    class Program: IProgram
    {
        static void Main(string[] args)
        {
            var serviceCollection = IocHelper.CreateServiceCollection();
            var container = serviceCollection.BuildServiceProvider();

            var program = container.GetService<IProgram>();
            program.Execute();
        }

        public Program(
            IEmailSender emailSender,
            ILogger logger)
        {
            try
            {
                logger.Log(LogLevel.Information, "The program is starting up.");
                emailSender.Send("hello@example.com", "Hello world");
            } catch
            {
                logger.Log(LogLevel.Error, "An error has occured.");
            }
        }

        public void Execute()
        {

        }
    }
}
