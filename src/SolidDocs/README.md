# SolidDocs

A self-contained document collaboration and signing library for ASP.NET Core powered by OnlyOffice.

## Features

- 📁 **Template Management**: Upload, list, and delete Word document templates
- 📄 **Document Generation**: Create documents from templates with variable replacement
- ✍️ **Document Signing**: User and agent signing workflows
- 📤 **PDF Export**: Export finalized documents to PDF
- 🔐 **JWT Security**: Secure editor sessions with JWT tokens
- 🌐 **OnlyOffice Integration**: Seamless integration with OnlyOffice editor

## Installation

```bash
dotnet add package SolidDocs
```

## Quick Start

### 1. Configure Services

```csharp
using SolidDocs;

var builder = WebApplication.CreateBuilder(args);

// Add SolidDocs services
builder.Services.AddSolidDocs(options =>
{
    options.RootPath = "wwwroot/soliddocs";
    options.JwtSecret = "your-super-secret-key-with-at-least-32-characters";
    options.BaseUrl = "https://your-domain.com";
    options.RoutePrefix = "soliddocs";
    options.EnableCors = true;
});
```

### 2. Use Middleware

```csharp
var app = builder.Build();

// Use SolidDocs middleware
app.UseSolidDocs();

app.Run();
```

## API Endpoints

### Templates

- `POST /soliddocs/templates/upload` - Upload a new template
- `GET /soliddocs/templates` - Get all templates
- `DELETE /soliddocs/templates/{name}` - Delete a template

### Documents

- `POST /soliddocs/documents/create` - Create a new document
- `GET /soliddocs/documents/{id}/editor` - Get editor URL
- `GET /soliddocs/documents/{id}/status` - Get document status
- `POST /soliddocs/documents/{id}/sign` - Sign a document
- `POST /soliddocs/documents/{id}/finalize` - Finalize a document
- `POST /soliddocs/documents/{id}/export` - Export to PDF
- `GET /soliddocs/documents/{id}/download` - Download PDF
- `GET /soliddocs/documents/{id}/file` - Get document file

## Configuration Options

```csharp
public class SolidDocsOptions
{
    public string RootPath { get; set; } = "wwwroot/soliddocs";
    public string JwtSecret { get; set; } = string.Empty;
    public string JwtIssuer { get; set; } = "SolidDocs";
    public string JwtAudience { get; set; } = "OnlyOffice";
    public int JwtExpirationHours { get; set; } = 1;
    public string BaseUrl { get; set; } = string.Empty;
    public string RoutePrefix { get; set; } = "soliddocs";
    public bool EnableCors { get; set; } = true;
    public string CorsPolicyName { get; set; } = "SolidDocsCors";
}
```

## Usage Examples

### Upload Template

```csharp
// Using HttpClient
using var client = new HttpClient();
using var formData = new MultipartFormDataContent();
using var fileStream = File.OpenRead("template.docx");
formData.Add(new StreamContent(fileStream), "file", "template.docx");

var response = await client.PostAsync("https://your-domain.com/soliddocs/templates/upload", formData);
```

### Create Document

```csharp
var request = new
{
    templateName = "contract",
    variables = new Dictionary<string, string>
    {
        { "companyName", "SolidDocs Inc" },
        { "clientName", "John Doe" },
        { "amount", "10000" }
    }
};

var response = await client.PostAsJsonAsync("https://your-domain.com/soliddocs/documents/create", request);
```

### Get Editor URL

```csharp
var response = await client.GetAsync("https://your-domain.com/soliddocs/documents/{id}/editor?userId=user1&userName=John");
var result = await response.Content.ReadFromJsonAsync<ApiResponse<EditorResponse>>();
var editorUrl = result.Data.EditorUrl;
```

### Sign Document

```csharp
var signRequest = new
{
    userId = "user1",
    userName = "John Doe"
};

var response = await client.PostAsJsonAsync("https://your-domain.com/soliddocs/documents/{id}/sign", signRequest);
```

## File Structure

```
wwwroot/soliddocs/
├── Templates/          # Uploaded templates
├── Documents/          # Generated documents
└── SignedPdfs/        # Exported PDFs
```

## Security

- JWT tokens are used to secure editor sessions
- JWT secret must be at least 32 characters long
- CORS is enabled by default for cross-origin requests
- File type validation ensures only .docx files are accepted

## Dependencies

- Microsoft.AspNetCore.Http.Abstractions
- System.IdentityModel.Tokens.Jwt
- DocumentFormat.OpenXml

## License

MIT License 