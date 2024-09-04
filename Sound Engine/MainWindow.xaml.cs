using Sound_Generator;

using System.IO;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sound_Engine
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();




		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.Key == Key.E)
			{
				using MemoryStream stream = new();
				
				SoundGenerator.Generate(stream);

				using SoundPlayer player = new(stream);
				player.Play();
			}
		}
	}
}