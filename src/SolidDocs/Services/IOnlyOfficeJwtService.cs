namespace SolidDocs.Services
{
    /// <summary>
    /// Service for generating and validating JWT tokens for OnlyOffice
    /// </summary>
    internal interface IOnlyOfficeJwtService
    {
        /// <summary>
        /// Generate a JWT token for OnlyOffice editor
        /// </summary>
        string GenerateToken(string docId, string fileName, string fileUrl, string userId, string userName, bool canEdit = true);

        /// <summary>
        /// Validate a JWT token
        /// </summary>
        bool ValidateToken(string token);
    }
} 