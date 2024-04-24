using BlazorInputFile;

namespace Utilities.FileHelper;

public interface IFileManager
{
    Task<string> UploadFileAsync(IFileListEntry file);
    Task<string> EditFileAsync(string path, IFileListEntry file);
    Task<bool> DelFileAsync(string path);
}

public class FileManager : IFileManager
{
    private readonly string directory = "media";
    private readonly IWebHostEnvironment _environment;
    public FileManager(IWebHostEnvironment environment) => _environment = environment;

    public async Task<bool> DelFileAsync(string path)
    {
        if (string.IsNullOrEmpty(path))
            return await Task.FromResult(false);

        var fullpath = Path.Combine(_environment.ContentRootPath, $"wwwroot/{path}");
        var existe = File.Exists(fullpath);
        
        if (!existe)
            return await Task.FromResult(false);

        File.Delete(fullpath);
        return await Task.FromResult(true);
    }

    public async Task<string> UploadFileAsync(IFileListEntry fileEntry)
    {
        var ext = fileEntry.Name.Split(".").Last();
        var newName = $"{Guid.NewGuid().ToString()}.{ext}";
        var path = Path.Combine(_environment.ContentRootPath, $"wwwroot/{directory}", newName);
        var uploadpath = @$"{directory}/" + newName;
        var ms = new MemoryStream();
        await fileEntry.Data.CopyToAsync(ms, CancellationToken.None);

        using (FileStream file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            ms.WriteTo(file);
        }

        ms.Dispose();
        return await Task.FromResult(uploadpath);
    }

    public async Task<string> EditFileAsync(string path, IFileListEntry fileEntry)
    {
        var addresult = await UploadFileAsync(fileEntry);
        await DelFileAsync(path);

        return !string.IsNullOrEmpty(addresult) ? addresult : null;
    }
}