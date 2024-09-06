﻿using Sound_Generator.Sounds;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sound_Engine.Controls
{
	/// <summary>
	/// Interaction logic for SoundControl.xaml
	/// </summary>
	public partial class SoundControl : UserControl
	{
		public Sound sound { get; set; }

		public SoundControl()
		{
			InitializeComponent();


		}

		private void AfterInit(object sender, RoutedEventArgs e)
		{
			if (sound != null)
				texbox.Text = sound.Frequency.ToString();
		}
	}
}
