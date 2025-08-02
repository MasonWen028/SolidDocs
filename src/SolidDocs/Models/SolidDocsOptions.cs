namespace SolidDocs.Models
{
    /// <summary>
    /// Configuration options for SolidDocs library
    /// </summary>
    public class SolidDocsOptions
    {
        /// <summary>
        /// Root path for storing SolidDocs files (templates, documents, PDFs)
        /// Defaults to "wwwroot/soliddocs"
        /// </summary>
        public string RootPath { get; set; } = "wwwroot/soliddocs";

        /// <summary>
        /// JWT secret key for securing editor sessions
        /// Must be at least 32 characters long
        /// </summary>
        public string JwtSecret { get; set; } = string.Empty;

        /// <summary>
        /// JWT issuer for token validation
        /// </summary>
        public string JwtIssuer { get; set; } = "SolidDocs";

        /// <summary>
        /// JWT audience for token validation
        /// </summary>
        public string JwtAudience { get; set; } = "OnlyOffice";

        /// <summary>
        /// JWT token expiration time in hours
        /// </summary>
        public int JwtExpirationHours { get; set; } = 1;

        /// <summary>
        /// Base URL for the application (used for generating file URLs)
        /// </summary>
        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// Route prefix for SolidDocs endpoints
        /// Defaults to "soliddocs"
        /// </summary>
        public string RoutePrefix { get; set; } = "soliddocs";

        /// <summary>
        /// Whether to enable CORS for SolidDocs endpoints
        /// </summary>
        public bool EnableCors { get; set; } = true;

        /// <summary>
        /// CORS policy name for SolidDocs endpoints
        /// </summary>
        public string CorsPolicyName { get; set; } = "SolidDocsCors";
    }
} 