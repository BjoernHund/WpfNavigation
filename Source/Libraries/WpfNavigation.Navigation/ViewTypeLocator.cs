using WpfNavigation.Navigation.Interfaces;

namespace WpfNavigation.Navigation
{
	public class ViewTypeLocator : IViewTypeLocator
	{
		private readonly Dictionary<Type, Type> _cache = [];

		public Type Locate(Type viewModelType)
		{
			if (_cache.TryGetValue(viewModelType, out var cachedViewType))
			{
				return cachedViewType;
			}

			string viewName = GetViewName(viewModelType);
			Type? viewType = viewModelType.Assembly.GetType(viewName, true);
			if (viewType is null)
			{
				throw new InvalidOperationException($"Unable to find type '{viewName}' in current assembly.");
			}

			_cache.Add(viewModelType, viewType);
			return viewType;
		}

		private static string GetViewName(Type viewModelType)
		{
			if (viewModelType.FullName is null)
			{
				throw new ArgumentException($"Property '{nameof(viewModelType.FullName)}' can not be null.", nameof(viewModelType));
			}

			string viewNamespace = viewModelType.Namespace!.Replace("ViewModels", "Views");
			string viewName = viewModelType.Name!.Replace("ViewModel", "View");

			return $"{viewNamespace}.{viewName}";
		}
	}
}