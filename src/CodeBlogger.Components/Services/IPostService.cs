using System;
using CodeBlogger.Components.Models;

namespace CodeBlogger.Components.Services;

public interface IPostService
{
    event EventHandler<FileChangedEventArgs> PostCreated;
    event EventHandler<FileChangedEventArgs> PostChanged;

    Task SaveDraft(Post post);
    Task<List<Post>> GetAllDrafts();
    Task<Post> GetDraft(string filePath);
    string FormatFileName(string name);

    bool IsFileTrackingSupported { get; }
}

public class FileChangedEventArgs : EventArgs
{
    public string FilePath { get; set; }

    public FileChangedEventArgs(string filePath)
    {
        FilePath = filePath;
    }
}
