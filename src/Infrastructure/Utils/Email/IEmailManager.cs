namespace Infrastructure.Data.Utils.Email;
public interface IEmailManager
{
    void SendSingleEmail(string receiverAddress, string message, string subject);
    void SendSingleEmailWithAttachment(string receiverAddress, string message, string subject, string fileName,
        string fileContent, string type);
    string GetCreateOrganizationEmailTemplate(string adminName, string organizationName, string emailLink);
    string GetEmployeeInvitationEmailTemplate(string employeeName, string emailLink);
}

