namespace SolidDocs.Services
{
    /// <summary>
    /// Service for exporting documents to PDF
    /// </summary>
    internal interface IPdfExportService
    {
        /// <summary>
        /// Export a document to PDF
        /// </summary>
        Task<string> ExportToPdfAsync(string documentId, string documentPath);

        /// <summary>
        /// Get PDF bytes for download
        /// </summary>
        Task<byte[]> GetPdfAsync(string documentId);
    }
} 