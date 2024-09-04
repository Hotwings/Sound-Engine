using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sound_Generator
{
	enum WaveType
	{
		Sine,
		Triangle,
		Square,
		Saw
	}
	internal class Sound
	{
		public WaveType WaveType { get; set; }
		public double Frequency { get; set; }
		public double Durration { get; set; }
		public double StartTime { get; set; }
		public int Amplitude { get; set; }

		public Sound(WaveType waveType, double frequency, double startTime, double durration, int amplitude)
		{
			WaveType = waveType;
			Frequency = frequency;
			Durration = durration;
			StartTime = startTime;
			Amplitude = amplitude;
		}

		public short Generate(int sample)
		{
			if (sample > StartTime * SoundGenerator.samplesPerSecond && sample < (StartTime + Durration) * SoundGenerator.samplesPerSecond)
				return (short)(Amplitude * Math.Sin(Frequency * ((double)sample / SoundGenerator.samplesPerSecond) * 2.0 * Math.PI));
			else
				return 0;

		}
	}
}
