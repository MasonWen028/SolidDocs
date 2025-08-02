namespace SolidDocs.Models
{
    /// <summary>
    /// Metadata for a document template
    /// </summary>
    public class TemplateMetadata
    {
        /// <summary>
        /// Template name (without extension)
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Template file name
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// File size in bytes
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// When the template was uploaded
        /// </summary>
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// MIME content type
        /// </summary>
        public string ContentType { get; set; } = string.Empty;
    }
} 