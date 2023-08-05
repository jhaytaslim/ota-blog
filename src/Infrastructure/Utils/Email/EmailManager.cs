using Hangfire;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Data.Utils.Email;

public class EmailManager : IEmailManager
{
    private readonly SendGridClient _clientKey;
    private readonly IConfiguration _config;
    private readonly EmailAddress _from;

    public EmailManager(IConfiguration configuration)
    {
        _config = configuration;
        var sendGridKey = configuration["SENDGRID_KEY"];
        var senderEmail = configuration["SENDER_EMAIL"];
        _clientKey = new SendGridClient(sendGridKey);
        _from = new EmailAddress(senderEmail);
    }


    public void SendSingleEmail(string receiverAddress, string message, string subject)
    {
        BackgroundJob.Enqueue(() => SendSingleMail(receiverAddress, message, subject));
    }

    public void SendSingleEmailWithAttachment(string receiverAddress, string message, string subject, string fileName, string fileContent, string type)
    {
        BackgroundJob.Enqueue(() => SendSingleMailWithAttachment(receiverAddress, message, subject, fileName, fileContent, type));
    }

    public async Task SendSingleMail(string receiverAddress, string message, string subject)
    {
        var To = new EmailAddress(receiverAddress);
        var plainText = message;
        var htmlContent = message;
        var msg = MailHelper.CreateSingleEmail(_from, To, subject, plainText, htmlContent);
        var response = await _clientKey.SendEmailAsync(msg);

        // Throw an exception if the response is not successful, so that hangfire can retry
        if (!response.IsSuccessStatusCode)
            throw new Exception(response.StatusCode.ToString());

    }


    /// <summary>
    /// Access modifier should be public for Background service
    /// <see cref="https://stackoverflow.com/questions/54437221/how-to-resolve-only-public-methods-can-be-invoked-in-the-background-in-hangfire"/>
    /// </summary>
    public async Task SendSingleMailWithAttachment(string receiverAddress, string message, string subject,
        string fileName, string fileContent, string type = "application/pdf")
    {
        var To = new EmailAddress(receiverAddress);
        var plainText = message;
        var htmlContent = message;
        var msg = MailHelper.CreateSingleEmail(_from, To, subject, plainText, htmlContent);


        var attachment = new Attachment
        {
            Content = fileContent,
            ContentId = Guid.NewGuid().ToString(),
            Disposition = "inline",
            Filename = fileName,
            Type = type

        };

        msg.AddAttachment(attachment);
        var response = await _clientKey.SendEmailAsync(msg);

        //Throw an exception if the response is not successful, so that hangfire can retry
        if (!response.IsSuccessStatusCode)
            throw new Exception(response.StatusCode.ToString());
    }

    public string GetCreateOrganizationEmailTemplate(string adminName, string organizationName, string emailLink)
    {
        string body;
        var folderName = Path.Combine("wwwroot", "Templates", "CreateOrganization.html");
        var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        if (File.Exists(filepath))
            body = File.ReadAllText(filepath);
        else
            return null;

        string msgBody = body.Replace("{emailLink}", emailLink)
            .Replace("{Admin Name}", adminName)
            .Replace("{Organisation Name}", organizationName);

        return msgBody;
    }

    public string GetEmployeeInvitationEmailTemplate(string employeeName, string emailLink)
    {
        string body;
        var folderName = Path.Combine("wwwroot", "Templates", "Employee-Invitation.html");
        var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        if (File.Exists(filepath))
            body = File.ReadAllText(filepath);
        else
            return null;

        string msgBody = body.Replace("{emailLink}", emailLink).
            Replace("{Employee Name}", employeeName);

        return msgBody;
    }
}
