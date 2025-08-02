# SolidDocs

[![English](https://img.shields.io/badge/Language-English-blue.svg)](README.md)
[![中文](https://img.shields.io/badge/Language-中文-red.svg)](README.zh-CN.md)

一个自包含的文档协作和签署库，为 ASP.NET Core 应用提供 OnlyOffice 集成。

## 📁 项目结构

```
SolidDocs/
├── src/
│   └── SolidDocs/              # SolidDocs 库项目
│       ├── Models/             # 模型类
│       ├── Services/           # 服务实现
│       ├── SolidDocsBuilderExtensions.cs
│       ├── SolidDocs.csproj
│       └── README.md
├── examples/
│   └── SolidDocsExample/       # 使用示例项目
│       ├── Program.cs
│       └── SolidDocsExample.csproj
├── docs/
│   └── REFACTORING_SUMMARY.md  # 重构总结文档
├── SolidDocs.sln              # 解决方案文件
└── README.md                  # 项目说明
```

## 🚀 快速开始

### 1. 构建库

```bash
# 构建 SolidDocs 库
dotnet build src/SolidDocs/SolidDocs.csproj

# 构建示例项目
dotnet build examples/SolidDocsExample/SolidDocsExample.csproj
```

### 2. 运行示例

```bash
# 运行示例项目
dotnet run --project examples/SolidDocsExample/SolidDocsExample.csproj
```

### 3. 在你的项目中使用

```csharp
using SolidDocs;

var builder = WebApplication.CreateBuilder(args);

// 添加 SolidDocs 服务
builder.Services.AddSolidDocs(options =>
{
    options.RootPath = "wwwroot/soliddocs";
    options.JwtSecret = "your-super-secret-key-with-at-least-32-characters";
    options.BaseUrl = "https://your-domain.com";
    options.RoutePrefix = "soliddocs";
    options.EnableCors = true;
});

var app = builder.Build();

// 使用 SolidDocs 中间件
app.UseSolidDocs();

app.Run();
```

## 📋 API 端点

### 模板管理
- `POST /soliddocs/templates/upload` - 上传模板
- `GET /soliddocs/templates` - 获取模板列表
- `DELETE /soliddocs/templates/{name}` - 删除模板

### 文档管理
- `POST /soliddocs/documents/create` - 创建文档
- `GET /soliddocs/documents/{id}/editor` - 获取编辑器链接
- `GET /soliddocs/documents/{id}/status` - 获取文档状态
- `POST /soliddocs/documents/{id}/sign` - 签署文档
- `POST /soliddocs/documents/{id}/finalize` - 最终确认
- `POST /soliddocs/documents/{id}/export` - 导出 PDF
- `GET /soliddocs/documents/{id}/download` - 下载 PDF
- `GET /soliddocs/documents/{id}/file` - 获取文档文件

## 🔧 开发

### 构建所有项目

```bash
dotnet build
```

### 运行示例

```bash
# 运行示例项目
dotnet run --project examples/SolidDocsExample/SolidDocsExample.csproj
```

### 清理构建

```bash
dotnet clean
```

## 📚 文档

- [SolidDocs 库文档](src/SolidDocs/README.md)
- [重构总结](docs/REFACTORING_SUMMARY.md)

## 🎯 特性

- ✅ **自包含设计** - 所有功能都在一个库中
- ✅ **可重用性** - 可以轻松集成到任何 ASP.NET Core 应用
- ✅ **可配置** - 通过选项模式进行灵活配置
- ✅ **现代化** - 使用 ASP.NET Core Minimal API
- ✅ **文档化** - 完整的 XML 文档和 README
- ✅ **JWT 安全** - 为 OnlyOffice 集成提供安全机制
- ✅ **CORS 支持** - 可配置的跨域请求支持

## 📝 注意事项

1. **MVP 版本** - PDF 导出功能为模拟实现
2. **内存存储** - 文档元数据存储在内存中
3. **文件存储** - 使用本地文件系统
4. **生产就绪** - 需要添加数据库持久化和真实的 PDF 转换

## 🔄 下一步

- [ ] 添加数据库持久化
- [ ] 集成真实的 OnlyOffice ConvertService
- [ ] 添加用户认证和授权
- [ ] 添加文件版本控制
- [ ] 添加审计日志
- [ ] 添加缓存机制
- [ ] 发布到 NuGet

## �� 许可证

MIT License 