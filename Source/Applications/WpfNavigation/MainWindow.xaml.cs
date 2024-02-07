using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows;
using WpfNavigation.ViewModels;

namespace WpfNavigation
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			if (!DesignerProperties.GetIsInDesignMode(this))
			{
				DataContext ??= App.Current.Services.GetRequiredService<MainViewModel>();
			}
		}
	}
}