using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using WpfNavigation.Navigation;
using WpfNavigation.Navigation.Interfaces;
using WpfNavigation.ViewModels;

namespace WpfNavigation
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			string? environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

			Services = ConfigureServices(environment);

			FrameworkElement.LanguageProperty.OverrideMetadata(
				typeof(FrameworkElement),
				new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
		}

		public static new App Current => (App)Application.Current;

		public IServiceProvider Services { get; }

		private IServiceProvider ConfigureServices(string? environment)
		{
			IConfigurationBuilder builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", false, true);

			if (!string.IsNullOrWhiteSpace(environment))
			{
				builder.AddJsonFile($"appsettings.{environment}.json", true, true);
			}

			IConfigurationRoot configurationRoot = builder.Build();

			IServiceCollection services = new ServiceCollection();
			services.AddLogging();

			RegisterServices(services);
			RegisterViewModels(services);

			return services.BuildServiceProvider();
		}

		private static void RegisterServices(IServiceCollection services)
		{
			services.AddSingleton<INavigationService, NavigationService>();
		}

		private static void RegisterViewModels(IServiceCollection services)
		{
			services.AddSingleton<MainViewModel>();
			services.AddSingleton<HomeViewModel>();
			services.AddSingleton<DiscoveryViewModel>();
			services.AddSingleton<FeaturedViewModel>();
		}
	}
}