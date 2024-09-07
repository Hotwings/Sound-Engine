using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sound_Engine_Library.Sounds
{
	public class SineWave : Sound
	{
		public SineWave(double frequency, double startTime, double durration, int amplitude) : base(frequency, startTime, durration, amplitude)
		{
		}

		public override short Generate(int sample, SoundGenerator soundGenerator)
		{
			if (sample > StartTime * soundGenerator.samplesPerSecond && sample < (StartTime + Durration) * soundGenerator.samplesPerSecond)
				return (short)(Amplitude * Math.Sin(Frequency * ((double)sample / soundGenerator.samplesPerSecond) * 2.0 * Math.PI));
			else
				return 0;

		}

	}
}
