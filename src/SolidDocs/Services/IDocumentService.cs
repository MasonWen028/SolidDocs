using SolidDocs.Models;

namespace SolidDocs.Services
{
    /// <summary>
    /// Service for managing document instances
    /// </summary>
    internal interface IDocumentService
    {
        /// <summary>
        /// Create a new document from a template
        /// </summary>
        Task<DocumentMetadata> CreateDocumentAsync(string templateName, Dictionary<string, string> variables);

        /// <summary>
        /// Get a document by ID
        /// </summary>
        Task<DocumentMetadata?> GetDocumentAsync(string id);

        /// <summary>
        /// Sign a document
        /// </summary>
        Task<bool> SignDocumentAsync(string id, string userId, string userName);

        /// <summary>
        /// Finalize a document and export to PDF
        /// </summary>
        Task<bool> FinalizeDocumentAsync(string id);

        /// <summary>
        /// Get the file path for a document
        /// </summary>
        Task<string> GetDocumentPathAsync(string id);

        /// <summary>
        /// Get all documents
        /// </summary>
        Task<List<DocumentMetadata>> GetDocumentsAsync();
    }
} 