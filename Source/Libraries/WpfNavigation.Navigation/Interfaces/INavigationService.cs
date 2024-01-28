namespace WpfNavigation.Navigation.Interfaces
{
	public interface INavigationService
	{
		IPageViewModel? CurrentView { get; set; }

		void NavigateTo(IPageViewModel pageViewModel);

		void NavigateTo(Type pageViewModelType);

		void NavigateTo<TViewModel>() where TViewModel : IPageViewModel;
	}
}