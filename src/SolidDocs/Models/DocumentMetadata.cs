namespace SolidDocs.Models
{
    /// <summary>
    /// Metadata for a document instance
    /// </summary>
    public class DocumentMetadata
    {
        /// <summary>
        /// Unique document identifier
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Name of the template used to create this document
        /// </summary>
        public string TemplateName { get; set; } = string.Empty;

        /// <summary>
        /// Document file name
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Current status of the document
        /// </summary>
        public DocumentStatus Status { get; set; } = DocumentStatus.Draft;

        /// <summary>
        /// When the document was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When the document was signed (if applicable)
        /// </summary>
        public DateTime? SignedAt { get; set; }

        /// <summary>
        /// Variables used to populate the document template
        /// </summary>
        public Dictionary<string, string> Variables { get; set; } = new();

        /// <summary>
        /// Path to the exported PDF file (if finalized)
        /// </summary>
        public string? PdfPath { get; set; }
    }

    /// <summary>
    /// Document status enumeration
    /// </summary>
    public enum DocumentStatus
    {
        /// <summary>
        /// Document is in draft state
        /// </summary>
        Draft,

        /// <summary>
        /// Document has been signed
        /// </summary>
        Signed,

        /// <summary>
        /// Document has been finalized and PDF exported
        /// </summary>
        Finalized
    }
} 