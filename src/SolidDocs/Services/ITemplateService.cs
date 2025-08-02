using SolidDocs.Models;
using Microsoft.AspNetCore.Http;

namespace SolidDocs.Services
{
    /// <summary>
    /// Service for managing document templates
    /// </summary>
    internal interface ITemplateService
    {
        /// <summary>
        /// Upload a new template file
        /// </summary>
        Task<TemplateMetadata> UploadTemplateAsync(IFormFile file);

        /// <summary>
        /// Get all available templates
        /// </summary>
        Task<List<TemplateMetadata>> GetTemplatesAsync();

        /// <summary>
        /// Delete a template by name
        /// </summary>
        Task<bool> DeleteTemplateAsync(string name);

        /// <summary>
        /// Get the file path for a template
        /// </summary>
        Task<string> GetTemplatePathAsync(string name);
    }
} 