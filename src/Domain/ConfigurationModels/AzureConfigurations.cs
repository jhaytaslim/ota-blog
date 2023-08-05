namespace Domain.ConfigurationModels;

public class AzureConfigurations
{
    public string Section { get; set; } = "Azure";
    public string BlobContainerName { get; set; }
    public string BlobConnectionString { get; set; }
}
