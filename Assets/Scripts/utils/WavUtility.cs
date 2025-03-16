using System.IO;
using UnityEngine;

public static class WavUtility
{
    public static AudioClip ToAudioClip(byte[] fileBytes, int sample_rate = 16000, string name = "wav")
    {
        using (MemoryStream stream = new MemoryStream(fileBytes))
        using (BinaryReader reader = new BinaryReader(stream))
        {
            // Skip WAV header
            reader.ReadBytes(44);

            // Read WAV data
            int sampleCount = (fileBytes.Length - 44) / 2; // Assuming 16 bit audio
            float[] samples = new float[sampleCount];
            for (int i = 0; i < sampleCount; i++)
            {
                samples[i] = reader.ReadInt16() / 32768.0f; // 16 bit signed integer
            }

            stream.Close();
            reader.Close();

            // Create AudioClip
            AudioClip audioClip = AudioClip.Create(name, sampleCount, 1, sample_rate, false);
            audioClip.SetData(samples, 0);
            return audioClip;
        }
    }
}
