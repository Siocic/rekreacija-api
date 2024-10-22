using EmailConsumer;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables();

        var configuration = builder.Build();

        var emailService = new EmailService(configuration);

        var registrationEmailConsumer = new RegistrationEmailConsumer(configuration, emailService);
        registrationEmailConsumer.SendEmailForRegistration();
        Console.WriteLine("Registration Email Consumer started");
        Thread.Sleep(Timeout.Infinite);
    }
}