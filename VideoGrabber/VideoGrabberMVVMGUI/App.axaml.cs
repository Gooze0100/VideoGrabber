using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using VideoGrabberMVVMGUI.DI;
using VideoGrabberMVVMGUI.Models;
using VideoGrabberMVVMGUI.Views;

namespace VideoGrabberMVVMGUI
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            BindingPlugins.DataValidators.RemoveAt(0);

            ServiceCollection collection = new();
            collection.AddCommonServices();

            ServiceProvider services = collection.BuildServiceProvider();

            var vm = services.GetRequiredService<MainViewModel>();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = vm
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}