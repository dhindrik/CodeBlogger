namespace CodeBlogger.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var services = new ServiceCollection();
        services.AddBlazorWebView();

        services.AddSingleton<IPostService, PostService>();
        var blazorWebView = new BlazorWebView()
        {
            HostPage = @"wwwroot\index.html",
            Services = services.BuildServiceProvider(),
            
        };

        blazorWebView.RootComponents.Add(new RootComponent()
        { 
            ComponentType = typeof(Editor),
            Selector = "#app"
        });
       
        Content = blazorWebView;
    }
}
