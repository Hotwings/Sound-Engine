using System.Diagnostics;
using System.IO;
using Sound_Engine_Library.Sounds;

namespace Sound_Engine_Library
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
		const short bitsPerSample = 16;


		public short channels { get; private set; } = 1;
		public int samplesPerSecond { get; private set; } = 44100;


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

		public readonly List<Sound> sounds = [];
		public short[] WaveForm { get; private set; } = [];
		public MemoryStream Stream { get; set; } = new MemoryStream();

		public SoundGenerator()
		{

		}
		public SoundGenerator(short channels, int samplesPerSecond)
		{
			this.channels = channels;
			this.samplesPerSecond = samplesPerSecond;
		}


		public short GetAmplitudeForSample(int sample)
		{
			return (short)sounds.Sum(x => x.Generate(sample, this)); ;
		}

		public async Task GenerateWaveForm()
		{
			WaveForm = new short[Samples];
			List<Task> tasks = [];

			for (int i = 0; i < Samples; i++)
			{
				tasks.Add(Task.Factory.StartNew((object? sample) =>
				{
					if (sample != null)//Not nessecary but it gets rid of a warning
						WaveForm[(int)sample] = GetAmplitudeForSample((int)sample);
				}, state: i));

			}
			await Task.WhenAll(tasks);
		}

		public void SaveWavFile(string filename)
		{
			using FileStream fileStream = new("sound.wav", FileMode.Create);

			WriteWavFileToStream(fileStream);

		}

		/// <summary>
		/// WARNING!!! You are responsible for closing your own stream.
		/// </summary>
		/// <param name="stream">Stream to write the file to. Sets the stream's position to 0 and leaves it open.</param>
		public void WriteWavFileToStream(Stream? stream = null, short[]? waveForm = null)
		{
			stream ??= Stream;
			waveForm ??= WaveForm;

			using BinaryWriter writer = new(stream, System.Text.Encoding.UTF8, true);

			short frameSize = (short)(channels * ((bitsPerSample + 7) / 8));
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

			foreach (short value in waveForm)
			{
				writer.Write(value);
			}



			//writer.Close();
			stream.Position = 0;

		}

	}
}
