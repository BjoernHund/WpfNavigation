using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfNavigation.Navigation;
using WpfNavigation.Navigation.Interfaces;

namespace WpfNavigation.ViewModels
{
	public partial class MainViewModel : ObservableObject
	{
		[ObservableProperty]
		private INavigationService _navigationService;

		public MainViewModel(INavigationService navigationService)
		{
			DataTemplateManager.RegisterPageViewModels();

			NavigationService = navigationService;
			navigationService.NavigateTo<HomeViewModel>();
		}

		[RelayCommand]
		private void Navigate(string pageName)
		{
			switch (pageName)
			{
				case "Home":
					NavigationService.NavigateTo<HomeViewModel>();
					break;
				case "Discovery":
					NavigationService.NavigateTo<DiscoveryViewModel>();
					break;
				case "Featured":
					NavigationService.NavigateTo<FeaturedViewModel>();
					break;
			}
		}
	}
}