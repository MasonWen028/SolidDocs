using SolidDocs.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace SolidDocs.Services
{
    /// <summary>
    /// Implementation of document management service
    /// </summary>
    internal class DocumentService : IDocumentService
    {
        private readonly SolidDocsOptions _options;
        private readonly ITemplateService _templateService;
        private readonly IPdfExportService _pdfExportService;
        private static readonly Dictionary<string, DocumentMetadata> _documents = new();

        public DocumentService(SolidDocsOptions options, ITemplateService templateService, IPdfExportService pdfExportService)
        {
            _options = options;
            _templateService = templateService;
            _pdfExportService = pdfExportService;
        }

        public async Task<DocumentMetadata> CreateDocumentAsync(string templateName, Dictionary<string, string> variables)
        {
            var templatePath = await _templateService.GetTemplatePathAsync(templateName);
            if (string.IsNullOrEmpty(templatePath))
                throw new ArgumentException($"Template '{templateName}' not found");

            var documentId = Guid.NewGuid().ToString();
            var documentFileName = $"{documentId}.docx";
            var documentsPath = Path.Combine(_options.RootPath, "Documents");
            var documentPath = Path.Combine(documentsPath, documentFileName);

            // Copy template to create new document
            File.Copy(templatePath, documentPath);

            // Replace variables in the document
            await ReplaceVariablesInDocument(documentPath, variables);

            var document = new DocumentMetadata
            {
                Id = documentId,
                TemplateName = templateName,
                FileName = documentFileName,
                Status = DocumentStatus.Draft,
                Variables = variables,
                CreatedAt = DateTime.UtcNow
            };

            _documents[documentId] = document;
            return document;
        }

        public async Task<DocumentMetadata?> GetDocumentAsync(string id)
        {
            return _documents.TryGetValue(id, out var document) ? document : null;
        }

        public async Task<bool> SignDocumentAsync(string id, string userId, string userName)
        {
            if (!_documents.TryGetValue(id, out var document))
                return false;

            if (document.Status != DocumentStatus.Draft)
                return false;

            document.Status = DocumentStatus.Signed;
            document.SignedAt = DateTime.UtcNow;
            return true;
        }

        public async Task<bool> FinalizeDocumentAsync(string id)
        {
            if (!_documents.TryGetValue(id, out var document))
                return false;

            if (document.Status != DocumentStatus.Signed)
                return false;

            var documentPath = await GetDocumentPathAsync(id);
            if (string.IsNullOrEmpty(documentPath))
                return false;

            // Export to PDF
            var pdfPath = await _pdfExportService.ExportToPdfAsync(id, documentPath);
            document.PdfPath = pdfPath;
            document.Status = DocumentStatus.Finalized;

            return true;
        }

        public async Task<string> GetDocumentPathAsync(string id)
        {
            var document = await GetDocumentAsync(id);
            if (document == null) return string.Empty;

            var documentsPath = Path.Combine(_options.RootPath, "Documents");
            var path = Path.Combine(documentsPath, document.FileName);
            return File.Exists(path) ? path : string.Empty;
        }

        public async Task<List<DocumentMetadata>> GetDocumentsAsync()
        {
            return _documents.Values.ToList();
        }

        private async Task ReplaceVariablesInDocument(string documentPath, Dictionary<string, string> variables)
        {
            using var doc = WordprocessingDocument.Open(documentPath, true);
            var body = doc.MainDocumentPart?.Document.Body;
            if (body == null) return;

            foreach (var variable in variables)
            {
                var placeholder = $"{{{{{variable.Key}}}}}";
                var replacement = variable.Value;

                // Replace text in paragraphs
                foreach (var paragraph in body.Elements<Paragraph>())
                {
                    foreach (var run in paragraph.Elements<Run>())
                    {
                        foreach (var text in run.Elements<Text>())
                        {
                            if (text.Text.Contains(placeholder))
                            {
                                text.Text = text.Text.Replace(placeholder, replacement);
                            }
                        }
                    }
                }
            }

            doc.Save();
        }
    }
} 