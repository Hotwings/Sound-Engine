using Sound_Engine.Controls;

using Sound_Generator;
using Sound_Generator.Sounds;

using System.Collections.ObjectModel;
using System.IO;
using System.Media;
using System.Security.AccessControl;
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
		private int graphYZoomLevel = 30;
		private double graphXZoomLevel = 40;
		private int graphScroll = 0;


		SoundGenerator generator;

		public MainWindow()
		{
			generator = new();
			generator.sounds.Add(new SineWave(175, 0, 10, 1000));
			generator.sounds.Add(new SineWave(179, 0, 10, 1000));
			generator.sounds.Add(new SineWave(182, 0, 10, 1000));
			generator.sounds.Add(new SineWave(186, 0, 10, 1000));


			InitializeComponent();

			


		}

		protected override async void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);


			using MemoryStream stream = new();


			await generator.GenerateWaveForm();

			generator.GenerateWavFile(stream);
			generator.SaveWavFile("Sound.wav");

			using SoundPlayer player = new(stream);
			player.Play();
		}
		
		private void AfterInitialized(object sender, RoutedEventArgs e)
		{
			foreach (var sound in generator.sounds)
			{
				SoundsContainer.Children.Add(new SoundControl(sound));
			}

			DrawWaveForm();
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			base.OnRenderSizeChanged(sizeInfo);
			DrawWaveForm();
		}

		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			graphScroll = Math.Max(0, graphScroll + e.Delta);
			DrawWaveForm();
		}

		#region Graph
		private void DrawWaveForm()
		{
			graphContainer.Children.Clear();
			System.Windows.Shapes.Path path = new System.Windows.Shapes.Path
			{
				Stroke = Brushes.Black,
				StrokeThickness = 1
			};

			PathFigure myPathFigure = new()
			{
				// Set the starting point for the PathFigure.
				StartPoint = GetPointForGraph(generator.WaveForm, 0)
			};


			PointCollection points = [];
			for (int i = 1; i < graphContainer.ActualWidth * graphXZoomLevel; i++)
			{
				points.Add(GetPointForGraph(generator.WaveForm, i));
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
				Figures = myPathFigureCollection,
				Transform = new ScaleTransform(1 / graphXZoomLevel, 1)
			};



			path.Data = myPathGeometry;
			graphContainer.Children.Add(path);
		}

		private Point GetPointForGraph(short[] waveForm, int x)
		{
			return new Point(x, waveForm[x+graphScroll] / graphYZoomLevel + 200);
		}


		private void ZoomGraphY(object sender, RoutedEventArgs e)
		{
			graphYZoomLevel += (((Button)sender).Content.ToString() == "-") ? 1 : -1;

			DrawWaveForm();
		}
		private void ZoomGraphX(object sender, RoutedEventArgs e)
		{
			graphXZoomLevel += (((Button)sender).Content.ToString() == "-") ? 1 : -1;

			DrawWaveForm();
		}

		#endregion

	}
}