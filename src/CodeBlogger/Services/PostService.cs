using Microsoft.Maui.Controls;

namespace CodeBlogger.Services;

public class PostService : IPostService
{
    public event EventHandler<FileChangedEventArgs> PostChanged;
    public event EventHandler<FileChangedEventArgs> PostCreated;

    public bool IsFileTrackingSupported => Device.RuntimePlatform == Device.UWP;

    public PostService()
    {
        var directory = FileSystem.AppDataDirectory;
        var path = Path.Combine(directory, "CodeBlogger");

        if (IsFileTrackingSupported)
        {
            var watcher = new FileSystemWatcher(path);
            watcher.Filter = "*.md";
            watcher.EnableRaisingEvents = true;

            watcher.NotifyFilter = NotifyFilters.Attributes
                                     | NotifyFilters.CreationTime
                                     | NotifyFilters.DirectoryName
                                     | NotifyFilters.FileName
                                     | NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.Security
                                     | NotifyFilters.Size;

            watcher.Changed += Watcher_Changed;
            watcher.Created += Watcher_Created;
            watcher.Error += Watcher_Error; 
        }
    }

    private void Watcher_Created(object sender, FileSystemEventArgs e)
    {
        PostCreated?.Invoke(this, new FileChangedEventArgs(e.FullPath));
    }

    private void Watcher_Error(object sender, ErrorEventArgs e)
    {
        
    }

    

    private void Watcher_Changed(object sender, FileSystemEventArgs e)
    {
        PostChanged?.Invoke(this, new FileChangedEventArgs(e.FullPath));
    }

    public async Task SaveDraft(Post post)
    {
        var directory = FileSystem.AppDataDirectory;
        Directory.CreateDirectory(Path.Combine(directory, "CodeBlogger"));
        var path = Path.Combine(directory, "CodeBlogger", FormatFileName(post.Name));

        await File.WriteAllTextAsync(path, post.Content);
    }

    public async Task<Post> GetDraft(string filePath)
    {
        var directory = FileSystem.AppDataDirectory;
        var files = Directory.GetFiles(Path.Combine(directory, "CodeBlogger"));

        var text = await File.ReadAllTextAsync(filePath);

         return new Post() { Name = filePath, Content = text };
    }

    public async Task<List<Post>> GetAllDrafts()
    {
        var directory = FileSystem.AppDataDirectory;
        var files = Directory.GetFiles(Path.Combine(directory, "CodeBlogger"));

        var result = new List<Post>();

        foreach(var file in files)
        {
            var text = await File.ReadAllTextAsync(file);
            var filename = Path.GetFileName(file);

            result.Add(new Post() {Name = filename, Content = text });
        }

        return result;
    }

    public string FormatFileName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            name = DateTime.Now.ToString();
        }

        name = name.Replace(" ", "-");
        name = name.Replace("/", "-");
        name = name.Replace(":", "-");
        name = name.ToLower();
        name = $"{name}.md";

        return name;
    }
}

