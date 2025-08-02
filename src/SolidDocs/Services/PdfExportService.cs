using SolidDocs.Models;

namespace SolidDocs.Services
{
    /// <summary>
    /// Implementation of PDF export service
    /// </summary>
    internal class PdfExportService : IPdfExportService
    {
        private readonly SolidDocsOptions _options;

        public PdfExportService(SolidDocsOptions options)
        {
            _options = options;
        }

        public async Task<string> ExportToPdfAsync(string documentId, string documentPath)
        {
            // For MVP, we'll simulate PDF export by creating a placeholder file
            // In production, this would call OnlyOffice ConvertService API
            var pdfFileName = $"{documentId}.pdf";
            var pdfsPath = Path.Combine(_options.RootPath, "SignedPdfs");
            var pdfPath = Path.Combine(pdfsPath, pdfFileName);

            // Simulate PDF conversion by creating a placeholder file
            // In real implementation, you would:
            // 1. Call OnlyOffice ConvertService API
            // 2. Convert DOCX to PDF
            // 3. Save the PDF file
            
            await File.WriteAllTextAsync(pdfPath, $"PDF Export for document {documentId} - {DateTime.UtcNow}");
            
            return pdfPath;
        }

        public async Task<byte[]> GetPdfAsync(string documentId)
        {
            var pdfFileName = $"{documentId}.pdf";
            var pdfsPath = Path.Combine(_options.RootPath, "SignedPdfs");
            var pdfPath = Path.Combine(pdfsPath, pdfFileName);

            if (!File.Exists(pdfPath))
                throw new FileNotFoundException($"PDF file not found for document {documentId}");

            return await File.ReadAllBytesAsync(pdfPath);
        }
    }
} 