namespace SolidDocs.Models
{
    /// <summary>
    /// Request model for creating a document
    /// </summary>
    public class CreateDocumentRequest
    {
        /// <summary>
        /// Name of the template to use
        /// </summary>
        public string TemplateName { get; set; } = string.Empty;

        /// <summary>
        /// Variables to replace in the template
        /// </summary>
        public Dictionary<string, string> Variables { get; set; } = new();
    }

    /// <summary>
    /// Request model for signing a document
    /// </summary>
    public class SignRequest
    {
        /// <summary>
        /// User ID of the signer
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// User name of the signer
        /// </summary>
        public string UserName { get; set; } = string.Empty;
    }
} 