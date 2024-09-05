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

		protected override async void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);


			using MemoryStream stream = new();

			SoundGenerator generator = new();

			short[] waveForm = await generator.GenerateWaveForm();

			DrawWaveForm(waveForm);

			await generator.GenerateWav(stream);
			FileStream fileStream = new FileStream("sound.wav", FileMode.Create);
			stream.CopyTo(fileStream);
			stream.Position = 0;

			using SoundPlayer player = new(stream);
			player.Play();
		}

		private Point GetPoint(short[] waveForm, int x)
		{
			return new Point(x, waveForm[x * 10] / 10 + 100);
		}

		private void DrawWaveForm(short[] waveForm)
		{

			System.Windows.Shapes.Path path = new System.Windows.Shapes.Path
			{
				Stroke = Brushes.Black,
				StrokeThickness = 1
			};

			PathFigure myPathFigure = new()
			{
				// Set the starting point for the PathFigure.
				StartPoint = GetPoint(waveForm, 0)
			};


			PointCollection points = [];
			for (int i = 1; i < 500; i++)
			{
				points.Add(GetPoint(waveForm,i));
			}

			PolyBezierSegment segment = new()
			{
				Points = points
			};

			PathSegmentCollection myPathSegmentCollection = [segment];

			myPathFigure.Segments = myPathSegmentCollection;

			PathFigureCollection myPathFigureCollection = [myPathFigure];

			PathGeometry myPathGeometry = new()
			{
				Figures = myPathFigureCollection
			};

			path.Data = myPathGeometry;
			myGrid.Children.Add(path);
		}

	}
}