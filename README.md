# SolidDocs

[![English](https://img.shields.io/badge/Language-English-blue.svg)](README.md)
[![中文](https://img.shields.io/badge/Language-中文-red.svg)](README.zh-CN.md)

A self-contained document collaboration and signing library that provides OnlyOffice integration for ASP.NET Core applications.

## 📁 Project Structure

```
SolidDocs/
├── src/
│   └── SolidDocs/              # SolidDocs library project
│       ├── Models/             # Model classes
│       ├── Services/           # Service implementations
│       ├── SolidDocsBuilderExtensions.cs
│       ├── SolidDocs.csproj
│       └── README.md
├── examples/
│   └── SolidDocsExample/       # Usage example project
│       ├── Program.cs
│       └── SolidDocsExample.csproj
├── docs/
│   └── REFACTORING_SUMMARY.md  # Refactoring summary document
├── SolidDocs.sln              # Solution file
└── README.md                  # Project documentation
```

## 🚀 Quick Start

### 1. Build the Library

```bash
# Build the SolidDocs library
dotnet build src/SolidDocs/SolidDocs.csproj

# Build the example project
dotnet build examples/SolidDocsExample/SolidDocsExample.csproj
```

### 2. Run the Example

```bash
# Run the example project
dotnet run --project examples/SolidDocsExample/SolidDocsExample.csproj
```

### 3. Use in Your Project

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

var app = builder.Build();

// Use SolidDocs middleware
app.UseSolidDocs();

app.Run();
```

## 📋 API Endpoints

### Template Management
- `POST /soliddocs/templates/upload` - Upload template
- `GET /soliddocs/templates` - Get template list
- `DELETE /soliddocs/templates/{name}` - Delete template

### Document Management
- `POST /soliddocs/documents/create` - Create document
- `GET /soliddocs/documents/{id}/editor` - Get editor link
- `GET /soliddocs/documents/{id}/status` - Get document status
- `POST /soliddocs/documents/{id}/sign` - Sign document
- `POST /soliddocs/documents/{id}/finalize` - Finalize document
- `POST /soliddocs/documents/{id}/export` - Export to PDF
- `GET /soliddocs/documents/{id}/download` - Download PDF
- `GET /soliddocs/documents/{id}/file` - Get document file

## 🔧 Development

### Build All Projects

```bash
dotnet build
```

### Run Example

```bash
# Run the example project
dotnet run --project examples/SolidDocsExample/SolidDocsExample.csproj
```

### Clean Build

```bash
dotnet clean
```

## 📚 Documentation

- [SolidDocs Library Documentation](src/SolidDocs/README.md)
- [Refactoring Summary](docs/REFACTORING_SUMMARY.md)

## 🎯 Features

- ✅ **Self-contained Design** - All functionality in one library
- ✅ **Reusable** - Easy to integrate into any ASP.NET Core application
- ✅ **Configurable** - Flexible configuration through options pattern
- ✅ **Modern** - Uses ASP.NET Core Minimal API
- ✅ **Documented** - Complete XML documentation and README
- ✅ **JWT Security** - Secure mechanism for OnlyOffice integration
- ✅ **CORS Support** - Configurable cross-origin request support

## 📝 Notes

1. **MVP Version** - PDF export functionality is simulated
2. **In-Memory Storage** - Document metadata stored in memory
3. **File Storage** - Uses local file system
4. **Production Ready** - Requires database persistence and real PDF conversion

## 🔄 Next Steps

- [ ] Add database persistence
- [ ] Integrate real OnlyOffice ConvertService
- [ ] Add user authentication and authorization
- [ ] Add file version control
- [ ] Add audit logging
- [ ] Add caching mechanism
- [ ] Publish to NuGet

## 📄 License

MIT License 