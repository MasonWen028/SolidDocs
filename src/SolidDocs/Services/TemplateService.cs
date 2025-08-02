using SolidDocs.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;

namespace SolidDocs.Services
{
    /// <summary>
    /// Implementation of template management service
    /// </summary>
    internal class TemplateService : ITemplateService
    {
        private readonly SolidDocsOptions _options;

        public TemplateService(SolidDocsOptions options)
        {
            _options = options;
            EnsureDirectoriesExist();
        }

        public async Task<TemplateMetadata> UploadTemplateAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");

            if (!file.FileName.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Only .docx files are supported");

            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var uniqueFileName = $"{fileName}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.docx";
            var templatesPath = Path.Combine(_options.RootPath, "Templates");
            var filePath = Path.Combine(templatesPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return new TemplateMetadata
            {
                Name = fileName,
                FileName = uniqueFileName,
                FileSize = file.Length,
                ContentType = file.ContentType,
                UploadedAt = DateTime.UtcNow
            };
        }

        public async Task<List<TemplateMetadata>> GetTemplatesAsync()
        {
            var templates = new List<TemplateMetadata>();
            var templatesPath = Path.Combine(_options.RootPath, "Templates");
            
            if (!Directory.Exists(templatesPath))
                return templates;

            var files = Directory.GetFiles(templatesPath, "*.docx");

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                templates.Add(new TemplateMetadata
                {
                    Name = Path.GetFileNameWithoutExtension(fileInfo.Name),
                    FileName = fileInfo.Name,
                    FileSize = fileInfo.Length,
                    ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    UploadedAt = fileInfo.CreationTimeUtc
                });
            }

            return templates;
        }

        public async Task<bool> DeleteTemplateAsync(string name)
        {
            var templatesPath = Path.Combine(_options.RootPath, "Templates");
            var files = Directory.GetFiles(templatesPath, $"{name}*.docx");
            if (files.Length == 0) return false;

            try
            {
                File.Delete(files[0]);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> GetTemplatePathAsync(string name)
        {
            var templatesPath = Path.Combine(_options.RootPath, "Templates");
            var files = Directory.GetFiles(templatesPath, $"{name}*.docx");
            return files.Length > 0 ? files[0] : string.Empty;
        }

        private void EnsureDirectoriesExist()
        {
            var templatesPath = Path.Combine(_options.RootPath, "Templates");
            var documentsPath = Path.Combine(_options.RootPath, "Documents");
            var pdfsPath = Path.Combine(_options.RootPath, "SignedPdfs");

            Directory.CreateDirectory(templatesPath);
            Directory.CreateDirectory(documentsPath);
            Directory.CreateDirectory(pdfsPath);
        }
    }
} 