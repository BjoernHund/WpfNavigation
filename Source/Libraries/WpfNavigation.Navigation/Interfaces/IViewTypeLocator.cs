namespace WpfNavigation.Navigation.Interfaces
{
	public interface IViewTypeLocator
	{
		Type Locate(Type viewModelType);
	}
}