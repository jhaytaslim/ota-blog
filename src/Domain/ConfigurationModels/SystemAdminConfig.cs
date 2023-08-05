namespace Domain.ConfigurationModels;
public class SystemAdminConfig
{
    public string Section { get; set; } = "SystemAdmin";
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
}

