using Microsoft.Extensions.DependencyInjection;
using RubyerDemo.ViewModels;
using RubyerDemo.ViewModels.Samples;
using RubyerDemo.Views.Samples;
using ShowMeTheXAML;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RubyerDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Services = ConfigureServices();
        }

        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)Application.Current;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            XamlDisplay.Init();

            new MainWindow().Show();
        }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // View models
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<IconViewModel>();
            services.AddSingleton<ListViewModel>();
            services.AddSingleton<DataGridViewModel>();
            services.AddSingleton<TabControlViewModel>();
            services.AddSingleton<PageBarViewModel>();
            services.AddSingleton<MessageBoxViewModel>();
            services.AddSingleton<DialogViewModel>();
            services.AddSingleton<StepBarViewModel>();
            services.AddSingleton<DescriptionViewModel>();
            services.AddSingleton<HamburgerMenuViewModel>();
            services.AddSingleton<TextBoxViewModel>();
            services.AddSingleton<PasswordBoxViewModel>();
            services.AddSingleton<NumericBoxViewModel>();
            services.AddSingleton<ComboBoxViewModel>();
            services.AddSingleton<RenamerViewModel>();
            services.AddSingleton<DateTimePickerViewModel>();
            services.AddSingleton<GroupBoxViewModel>();
            services.AddSingleton<ExpanderViewModel>();
            services.AddSingleton<ListBoxViewModel>();
            services.AddSingleton<TreeViewViewModel>();
            services.AddSingleton<FlipViewViewModel>();

            services.AddSingleton<WechatViewModel>();
            services.AddSingleton<NetEaseCloudMusicViewModel>();

            return services.BuildServiceProvider();
        }
    }
}