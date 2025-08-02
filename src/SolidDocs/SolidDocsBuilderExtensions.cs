using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using SolidDocs.Models;
using SolidDocs.Services;

namespace SolidDocs
{
    /// <summary>
    /// Extension methods for configuring SolidDocs in ASP.NET Core applications
    /// </summary>
    public static class SolidDocsBuilderExtensions
    {
        /// <summary>
        /// Add SolidDocs services to the application
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configureOptions">Action to configure SolidDocs options</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddSolidDocs(this IServiceCollection services, Action<SolidDocsOptions> configureOptions)
        {
            var options = new SolidDocsOptions();
            configureOptions(options);

            // Validate options
            if (string.IsNullOrEmpty(options.JwtSecret) || options.JwtSecret.Length < 32)
                throw new ArgumentException("JWT secret must be at least 32 characters long");

            if (string.IsNullOrEmpty(options.RootPath))
                throw new ArgumentException("Root path cannot be empty");

            // Register options as singleton
            services.AddSingleton(options);

            // Register internal services
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IOnlyOfficeJwtService, OnlyOfficeJwtService>();
            services.AddScoped<IPdfExportService, PdfExportService>();

            // Add CORS if enabled
            if (options.EnableCors)
            {
                services.AddCors(cors =>
                {
                    cors.AddPolicy(options.CorsPolicyName, policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
                });
            }

            return services;
        }

        /// <summary>
        /// Use SolidDocs middleware and configure endpoints
        /// </summary>
        /// <param name="app">The web application</param>
        /// <returns>The web application for chaining</returns>
        public static WebApplication UseSolidDocs(this WebApplication app)
        {
            var options = app.Services.GetRequiredService<SolidDocsOptions>();

            // Use CORS if enabled
            if (options.EnableCors)
            {
                app.UseCors(options.CorsPolicyName);
            }

            // Map SolidDocs endpoints
            var group = app.MapGroup($"/{options.RoutePrefix}");

            // Template endpoints
            group.MapPost("/templates/upload", async (IFormFile file, ITemplateService templateService) =>
            {
                try
                {
                    if (file == null)
                        return Results.BadRequest(new ApiResponse<object> { Success = false, Message = "No file provided" });

                    var template = await templateService.UploadTemplateAsync(file);
                    return Results.Ok(new ApiResponse<TemplateMetadata> { Success = true, Data = template });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new ApiResponse<object> { Success = false, Message = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Internal server error: {ex.Message}");
                }
            })
            .WithName("UploadTemplate")
            .WithSummary("Upload a new document template")
            .Accepts<IFormFile>("multipart/form-data");

            group.MapGet("/templates", async (ITemplateService templateService) =>
            {
                try
                {
                    var templates = await templateService.GetTemplatesAsync();
                    return Results.Ok(new ApiResponse<List<TemplateMetadata>> { Success = true, Data = templates });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Internal server error: {ex.Message}");
                }
            })
            .WithName("GetTemplates")
            .WithSummary("Get all available templates");

            group.MapDelete("/templates/{name}", async (string name, ITemplateService templateService) =>
            {
                try
                {
                    var result = await templateService.DeleteTemplateAsync(name);
                    if (result)
                        return Results.NoContent();
                    else
                        return Results.NotFound(new ApiResponse<object> { Success = false, Message = $"Template '{name}' not found" });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Internal server error: {ex.Message}");
                }
            })
            .WithName("DeleteTemplate")
            .WithSummary("Delete a template");

            // Document endpoints
            group.MapPost("/documents/create", async (CreateDocumentRequest request, IDocumentService documentService) =>
            {
                try
                {
                    var document = await documentService.CreateDocumentAsync(request.TemplateName, request.Variables);
                    return Results.Ok(new ApiResponse<DocumentMetadata> { Success = true, Data = document });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new ApiResponse<object> { Success = false, Message = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Internal server error: {ex.Message}");
                }
            })
            .WithName("CreateDocument")
            .WithSummary("Create a new document from template");

            group.MapGet("/documents/{id}/editor", async (string id, string userId, string userName, IDocumentService documentService, IOnlyOfficeJwtService jwtService) =>
            {
                try
                {
                    var document = await documentService.GetDocumentAsync(id);
                    if (document == null)
                        return Results.NotFound(new ApiResponse<object> { Success = false, Message = $"Document '{id}' not found" });

                    var documentPath = await documentService.GetDocumentPathAsync(id);
                    if (string.IsNullOrEmpty(documentPath))
                        return Results.NotFound(new ApiResponse<object> { Success = false, Message = "Document file not found" });

                    // Generate file URL
                    var fileUrl = $"{options.BaseUrl}/{options.RoutePrefix}/documents/{id}/file";
                    
                    // Generate JWT token for OnlyOffice
                    var token = jwtService.GenerateToken(id, document.FileName, fileUrl, userId, userName);

                    return Results.Ok(new ApiResponse<EditorResponse>
                    {
                        Success = true,
                        Data = new EditorResponse
                        {
                            DocumentId = id,
                            EditorUrl = $"/onlyoffice-editor?token={token}",
                            Token = token
                        }
                    });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Internal server error: {ex.Message}");
                }
            })
            .WithName("GetEditorUrl")
            .WithSummary("Get editor URL for document");

            group.MapGet("/documents/{id}/status", async (string id, IDocumentService documentService) =>
            {
                try
                {
                    var document = await documentService.GetDocumentAsync(id);
                    if (document == null)
                        return Results.NotFound(new ApiResponse<object> { Success = false, Message = $"Document '{id}' not found" });

                    return Results.Ok(new ApiResponse<DocumentStatus> { Success = true, Data = document.Status });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Internal server error: {ex.Message}");
                }
            })
            .WithName("GetDocumentStatus")
            .WithSummary("Get document status");

            group.MapPost("/documents/{id}/sign", async (string id, SignRequest request, IDocumentService documentService) =>
            {
                try
                {
                    var result = await documentService.SignDocumentAsync(id, request.UserId, request.UserName);
                    if (result)
                        return Results.Ok(new ApiResponse<object> { Success = true, Message = "Document signed successfully" });
                    else
                        return Results.BadRequest(new ApiResponse<object> { Success = false, Message = "Failed to sign document" });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Internal server error: {ex.Message}");
                }
            })
            .WithName("SignDocument")
            .WithSummary("Sign a document");

            group.MapPost("/documents/{id}/finalize", async (string id, IDocumentService documentService) =>
            {
                try
                {
                    var result = await documentService.FinalizeDocumentAsync(id);
                    if (result)
                        return Results.Ok(new ApiResponse<object> { Success = true, Message = "Document finalized successfully" });
                    else
                        return Results.BadRequest(new ApiResponse<object> { Success = false, Message = "Failed to finalize document" });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Internal server error: {ex.Message}");
                }
            })
            .WithName("FinalizeDocument")
            .WithSummary("Finalize a document");

            group.MapPost("/documents/{id}/export", async (string id, IDocumentService documentService, IPdfExportService pdfExportService) =>
            {
                try
                {
                    var document = await documentService.GetDocumentAsync(id);
                    if (document == null)
                        return Results.NotFound(new ApiResponse<object> { Success = false, Message = $"Document '{id}' not found" });

                    var documentPath = await documentService.GetDocumentPathAsync(id);
                    if (string.IsNullOrEmpty(documentPath))
                        return Results.NotFound(new ApiResponse<object> { Success = false, Message = "Document file not found" });

                    var pdfPath = await pdfExportService.ExportToPdfAsync(id, documentPath);
                    return Results.Ok(new ApiResponse<object> { Success = true, Message = "PDF exported successfully", Data = new { pdfPath } });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Internal server error: {ex.Message}");
                }
            })
            .WithName("ExportPdf")
            .WithSummary("Export document to PDF");

            group.MapGet("/documents/{id}/download", async (string id, IPdfExportService pdfExportService) =>
            {
                try
                {
                    var pdfBytes = await pdfExportService.GetPdfAsync(id);
                    return Results.File(pdfBytes, "application/pdf", $"{id}.pdf");
                }
                catch (FileNotFoundException)
                {
                    return Results.NotFound(new ApiResponse<object> { Success = false, Message = $"PDF file not found for document {id}" });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Internal server error: {ex.Message}");
                }
            })
            .WithName("DownloadPdf")
            .WithSummary("Download PDF file");

            group.MapGet("/documents/{id}/file", async (string id, IDocumentService documentService) =>
            {
                try
                {
                    var document = await documentService.GetDocumentAsync(id);
                    if (document == null)
                        return Results.NotFound(new ApiResponse<object> { Success = false, Message = $"Document '{id}' not found" });

                    var documentPath = await documentService.GetDocumentPathAsync(id);
                    if (string.IsNullOrEmpty(documentPath))
                        return Results.NotFound(new ApiResponse<object> { Success = false, Message = "Document file not found" });

                    var fileBytes = await File.ReadAllBytesAsync(documentPath);
                    return Results.File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", document.FileName);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Internal server error: {ex.Message}");
                }
            })
            .WithName("GetDocumentFile")
            .WithSummary("Get document file");

            return app;
        }
    }
} 