using Akka.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SimpleTrader.Bet;

public static class MailSender
{
    private static readonly string SendgridKey = Environment.GetEnvironmentVariable("SENDGRID_KEY") ?? throw new Exception("Missing SENDGRID_KEY environment variable");
    private static readonly string FromAddress = Environment.GetEnvironmentVariable("MAIL_FROM") ?? throw new ConfigurationException("Missing MAIL_FROM environment variable");
    private static readonly string ToAddress = Environment.GetEnvironmentVariable("MAIL_TO") ?? throw new ConfigurationException("Missing MAIL_TO environment variable");
    private static readonly string ToAddress2 = Environment.GetEnvironmentVariable("MAIL_TO2") ?? throw new ConfigurationException("Missing MAIL_TO2 environment variable");

    public static void Send(string title, string body)
    {
        var message = new SendGridMessage();

        message.Personalizations = new List<Personalization>
        {
            new(){
                Tos = new List<EmailAddress>
                {
                    new(){
                        Email = ToAddress,
                        Name = ToAddress 
                    }
                },
                Bccs = new List<EmailAddress>
                {
                    new(){
                        Email = ToAddress2,
                        Name = ToAddress2
                    }
                }
            }
        };

        message.From = new EmailAddress
        {
            Email = FromAddress,
            Name = FromAddress 
        };

        message.Subject = title;

        message.Contents = new List<Content>
        {
            new(){
                Type = "text/html",
                Value = body
            }
        };

        message.TrackingSettings = new TrackingSettings
        {
            ClickTracking = new ClickTracking
            {
                Enable = false,
                EnableText = false
            },
            OpenTracking = new OpenTracking
            {
                Enable = true,
                SubstitutionTag = "%open-track%"
            },
            SubscriptionTracking = new SubscriptionTracking
            {
                Enable = false
            }
        };

        var client = new SendGridClient(SendgridKey);
        var response = client.SendEmailAsync(message).Result;
        var responseBody = response.Body.ReadAsStringAsync().Result;

        Console.WriteLine(response.StatusCode.ToString());
        Console.WriteLine(responseBody);
        Console.WriteLine(response.Headers.ToString());
    }
}