using System.Diagnostics;

namespace Sound_Generator
{
	public class SoundGenerator
	{
		const int RIFF = 0x46464952;
		const int WAVE = 0x45564157;
		const int FORMAT = 0x20746D66;
		const int DATA = 0x61746164;
		const int waveSize = 4;
		const int formatChunkSize = 16;
		const int headerSize = 8;
		const short compressionType = 1;
		const short channels = 1;
		public const int samplesPerSecond = 44100;
		const short bitsPerSample = 16;

		public int Samples
		{
			get
			{
				return (int)(samplesPerSecond * TimeLength);
			}
		}

		public double TimeLength
		{
			get
			{
				return sounds.Max(x => x.StartTime + x.Durration);
			}
		}

		private readonly List<Sound> sounds = [];


		public SoundGenerator()
		{
			sounds.Add(new Sound(WaveType.Sine, 175, 0, 10, 1000));
			sounds.Add(new Sound(WaveType.Sine, 179, 1, 9, 1000));
			sounds.Add(new Sound(WaveType.Sine, 182, 4, 6, 1000));
			sounds.Add(new Sound(WaveType.Sine, 186, 7, 3, 1000));
		}

		public short GetAmplitudeForSample(int sample)
		{
			return (short)sounds.Sum(x => x.Generate(sample)); ;
		}

		public async Task<short[]> GenerateWaveForm()
		{
			short[] ret = new short[Samples];
			List<Task> tasks = [];

			for (int i = 0; i < Samples; i++)
			{
				tasks.Add(Task.Factory.StartNew((object? sample) =>
				{
					ret[(int)sample] = GetAmplitudeForSample((int)sample);
				}, state: i));

			}
			await Task.WhenAll(tasks);
			return ret;
		}

		public async Task GenerateWav(Stream stream, short[]? waveForm = null)
		{
			using BinaryWriter writer = new(stream, System.Text.Encoding.UTF8, true);

			short frameSize = channels * ((bitsPerSample + 7) / 8);
			int bytesPerSecond = samplesPerSecond * frameSize;
			int dataChunkSize = Samples * frameSize;
			int fileSize = waveSize + headerSize + formatChunkSize + headerSize + dataChunkSize;


			writer.Write(RIFF);
			writer.Write(fileSize);
			writer.Write(WAVE);
			writer.Write(FORMAT);
			writer.Write(formatChunkSize);
			writer.Write(compressionType);
			writer.Write(channels);
			writer.Write(samplesPerSecond);
			writer.Write(bytesPerSecond);
			writer.Write(frameSize);
			writer.Write(bitsPerSample);
			writer.Write(DATA);
			writer.Write(dataChunkSize);

			foreach (short value in waveForm ?? await GenerateWaveForm())
			{
				writer.Write(value);
			}



			//writer.Close();
			stream.Position = 0;

		}

	}
}
