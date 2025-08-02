namespace SolidDocs.Models
{
    /// <summary>
    /// Response model for editor URL generation
    /// </summary>
    public class EditorResponse
    {
        /// <summary>
        /// Document ID
        /// </summary>
        public string DocumentId { get; set; } = string.Empty;

        /// <summary>
        /// Editor URL for OnlyOffice integration
        /// </summary>
        public string EditorUrl { get; set; } = string.Empty;

        /// <summary>
        /// JWT token for authentication
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response model for API operations
    /// </summary>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Whether the operation was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Response message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Response data
        /// </summary>
        public T? Data { get; set; }
    }
} 