using Microsoft.AspNetCore.Http.Features;
using System.Diagnostics;
using Serilog;

const int port=5540;
string appDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
string logPath=Path.Combine( appDir, "PhotoDrop", "log");

Directory.CreateDirectory(logPath);
string logName = Path.Combine(logPath, "PhotoDrop.log");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(logName,
        rollingInterval: RollingInterval.Day)
    .CreateLogger();
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.Configure<FormOptions>(ConfigureFormOptions);
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 100 * 1024 * 1024; ; // 100 MB
});
var app = builder.Build();


app.UseStaticFiles();
app.UseRouting();

// アップロード先
var uploadDir = Path.Combine(GetUploadDirectory(),
    DateTime.Now.ToString("yyyyMMdd"));
Directory.CreateDirectory(uploadDir);

// HTMLページ
app.MapGet("/", async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync( WebPage.UploadPage);
});

// File upload
app.MapPost("/upload", async (HttpRequest request) =>
{
    //var file = request.Form.Files["file"];
    foreach (var file in request.Form.Files)
    {

        if (file == null || file.Length == 0)
            return Results.BadRequest("File Not Found");

        var filePath = Path.Combine(uploadDir, Path.GetFileName(file.FileName));

        using var stream = File.Create(filePath);
        await file.CopyToAsync(stream);

    }
    string html = """
<!DOCTYPE html>
<html lang="ja">
<head>
<meta charset="UTF-8">
<title>Upload Complete</title>
</head>
<body style="font-family: sans-serif; text-align: center;">
    <h2>アップロードが完了しました</h2>
    <button onclick="history.back()" style="font-size: 20px; padding: 10px 30px;">
        戻る
    </button>
</body>
</html>
""";
    return Results.Content(html,"text/html; charset=utf-8");
});

app.Run($"http://0.0.0.0:{port}");





static string GetUploadDirectory()
{
    string baseDir =
        Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

    string uploadDir = Path.Combine(baseDir, "PhotoDrop");

   // Directory.CreateDirectory(uploadDir);
    return uploadDir;
}

static void ConfigureFormOptions(FormOptions options)
{
    options.MultipartBodyLengthLimit = 100 * 1024 * 1024;
}

