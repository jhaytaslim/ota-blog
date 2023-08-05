
namespace Application.Helpers
{
    public class ResponseMessages
    {
        // public const string Duplicate = $"Duplicate record already exist";
        public const string RetrievalSuccessResponse = "Data retrieved successfully";
        public const string RetrievalFailedResponse = "Data retrieved failed";
        public const string CreationSuccessResponse = "Data created successfully";
        public const string CreationFailedResponse = "Unsuccessful Data creation ";
        public const string LoginSuccessResponse = "Login successful";
        public const string WrongEmailOrPassword = "Wrong email or password provided";
        public const string UserNotFound = "User not found";
        public const string InvalidToken = "Invalid token";
        public const string MissingClaim = "MissingClaim:";
        public static readonly string DataRetrievedSuccessfully = "Data retrieved successfully";
        public static readonly string UnAuthorized = "Unauthorized resource request";

        public static Func<string, string> DeleteSuccessResponse = (value) => $"{value} deleted successfully";

        public static Func<string, string> ItemDoesNotExist = (value) => $"{value} does not exist";

        public static Func<string, string> Duplicate = (value) => $"Duplicate {value} record already exist";

        public static Func<string, string> UpdateSuccessResponse = (value) => $"{value} record updated successfully";
        public static Func<string, string> InvalidOrganizationItemResponse = (value) => $"{value} is invalid in this context";
        public static Func<string, string> InvalidInputResponse = (field) => $"{field} has invalid value";
    }
}
