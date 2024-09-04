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

		List<Sound> sounds = new List<Sound>();

		public double TimeLength
		{
			get
			{
				return sounds.Max(x => x.StartTime + x.Durration);
			}
		}


		public SoundGenerator()
		{
			sounds.Add(new Sound(WaveType.Sine, 600, 0, 2, 1000));
			sounds.Add(new Sound(WaveType.Sine, 700, 1, 2, 1000));
		}

		public void Generate(Stream stream)
		{
			using BinaryWriter writer = new(stream, System.Text.Encoding.UTF8, true);

			short frameSize = channels * ((bitsPerSample + 7) / 8);
			int bytesPerSecond = samplesPerSecond * frameSize;
			int samples = (int)(samplesPerSecond * TimeLength);
			int dataChunkSize = samples * frameSize;
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

			for (int i = 0; i < samples; i++)
			{
				short yValue = (short)sounds.Sum(x => x.Generate(i));
				writer.Write(yValue);
			}

			//writer.Close();
			stream.Position = 0;

		}

	}
}
