using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using WpfNavigation.Navigation.Interfaces;

namespace WpfNavigation.Navigation
{
	public static class DataTemplateManager
	{
		public static void RegisterDataTemplate<TViewModel, TView>()
			where TViewModel : IPageViewModel
			where TView : FrameworkElement
		{
			RegisterDataTemplate(typeof(TViewModel), typeof(TView));
		}

		public static void RegisterDataTemplate(Type viewModelType, Type viewType)
		{
			DataTemplate template = CreateTemplate(viewModelType, viewType);
			object key = template.DataTemplateKey;

			if (!Application.Current.Resources.Contains(key))
			{
				Application.Current.Resources.Add(key, template);
			}
		}

		public static void RegisterPageViewModels(IViewTypeLocator? viewTypeLocator = null)
		{
			viewTypeLocator ??= new ViewTypeLocator();

			Type[] viewModelTypes = Assembly.GetCallingAssembly()
				.GetTypes()
				.Where(type => typeof(IPageViewModel).IsAssignableFrom(type) && !type.IsAbstract)
				.ToArray();

			foreach (Type viewModelType in viewModelTypes)
			{
				Type viewType = viewTypeLocator.Locate(viewModelType);
				RegisterDataTemplate(viewModelType, viewType);
			}
		}

		private static DataTemplate CreateTemplate(Type viewModelType, Type viewType)
		{
			if (!viewModelType.IsAssignableTo(typeof(IPageViewModel)))
			{
				throw new ArgumentException("Type does not implement interface 'IPageViewModel'.", nameof(viewModelType));
			}

			string xaml = $"<DataTemplate DataType=\"{{x:Type viewModels:{viewModelType.Name}}}\"><views:{viewType.Name} /></DataTemplate>";

			ParserContext context = new()
			{
				XamlTypeMapper = new XamlTypeMapper([])
			};

			context.XamlTypeMapper.AddMappingProcessingInstruction("viewModels", viewModelType.Namespace, viewModelType.Assembly.FullName);
			context.XamlTypeMapper.AddMappingProcessingInstruction("views", viewType.Namespace, viewType.Assembly.FullName);

			context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
			context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
			context.XmlnsDictionary.Add("viewModels", "viewModels");
			context.XmlnsDictionary.Add("views", "views");

			return (DataTemplate)XamlReader.Parse(xaml, context);
		}
	}
}