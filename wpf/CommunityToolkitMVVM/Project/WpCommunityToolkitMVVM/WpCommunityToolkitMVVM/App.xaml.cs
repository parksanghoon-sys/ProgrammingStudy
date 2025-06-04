using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows;
using WpCommunityToolkitMVVM.ViewModels;
using WpCommunityToolkitMVVM.Views;

namespace WpCommunityToolkitMVVM
{
    public interface ILoginService
    {
        bool IsLogin(string Id, string msg);
    }
    public class LoginService : ILoginService
    {
        public bool IsLogin(string Id, string msg)
        {
            return LoginInternal(Id, msg);
        }
        private bool LoginInternal(string?id, string?msg)
        {
            return string.IsNullOrEmpty(msg) && string.IsNullOrEmpty(id);
        }
    }
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            IServiceProvider serviceProvider = ConfigurationService();
            Ioc.Default.ConfigureServices(serviceProvider);
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
        private IServiceProvider ConfigurationService()
        {
            ServiceCollection service = new ServiceCollection();
            service.AddSingleton<MainWindowViewModel>();
            service.AddTransient<ReceiveViewModel>();
            service.AddTransient<ILoginService, LoginService>();
            return service.BuildServiceProvider();
        }
    }
}
