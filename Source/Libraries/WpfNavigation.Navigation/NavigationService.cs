using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using WpfNavigation.Navigation.Interfaces;

namespace WpfNavigation.Navigation
{
	public class NavigationService(IServiceProvider serviceProvider) : ObservableObject, INavigationService
	{
		private readonly IServiceProvider _serviceProvider = serviceProvider;

		private IPageViewModel? _currentView;

		public IPageViewModel? CurrentView
		{
			get => _currentView;
			set => SetProperty(ref _currentView, value);
		}

		public void NavigateTo<TViewModel>() where TViewModel : IPageViewModel
		{
			IPageViewModel viewModel = _serviceProvider.GetRequiredService<TViewModel>();
			CurrentView = viewModel;
		}

		public void NavigateTo(Type viewModelType)
		{
			if (!viewModelType.IsAssignableTo(typeof(IPageViewModel)))
			{
				throw new ArgumentException("Type does not implement interface 'IPageViewModel'.", nameof(viewModelType));
			}

			IPageViewModel viewModel = (IPageViewModel)_serviceProvider.GetRequiredService(viewModelType);
			CurrentView = viewModel;
		}

		public void NavigateTo(IPageViewModel viewModel)
		{
			CurrentView = viewModel;
		}
	}
}