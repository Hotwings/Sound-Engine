using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sound_Engine_Library.Sounds
{
	public abstract class Sound
	{
		public double Frequency { get; set; }
		public double Durration { get; set; }
		public double StartTime { get; set; }
		public int Amplitude { get; set; }

		public Sound(double frequency, double startTime, double durration, int amplitude)
		{
			Frequency = frequency;
			Durration = durration;
			StartTime = startTime;
			Amplitude = amplitude;
		}

		public abstract short Generate(int sample, SoundGenerator soundGenerator);

	}
}
