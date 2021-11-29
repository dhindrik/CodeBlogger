using CodeBlogger.Components.Pages;
using CodeBlogger.Components.Services;
using CodeBlogger.WinForms.Services;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;

namespace CodeBlogger.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        var services = new ServiceCollection();
        services.AddBlazorWebView();

        services.AddSingleton<IPostService, PostService>();

        var blazorWebView = new BlazorWebView()
        {
            Dock = DockStyle.Fill,
            HostPage = @"wwwroot\index.html",
            Services = services.BuildServiceProvider(),
        };

        blazorWebView.RootComponents.Add<Editor>("#app");

        Controls.Add(blazorWebView);
   
    }
}
