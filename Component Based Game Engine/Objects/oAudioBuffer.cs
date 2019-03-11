using System;
using OpenTK;
using OpenTK.Audio.OpenAL;
using System.IO;

namespace Component_Based_Game_Engine.Objects
{
    public class oAudioBuffer
    {
        private Stream stream;
        private int channels;
        private int bits;
        private int rate;
        private int myBuffer;
        private byte[] sound_data;
        private Vector3 emitterPosition;

        public oAudioBuffer()
        {
        }

        public void LoadObject(string filename)
        {
            myBuffer = AL.GenBuffer();

            stream = File.Open(filename, FileMode.Open);

            using (BinaryReader reader = new BinaryReader(stream))
            {
                // RIFF header
                string signature = new string(reader.ReadChars(4));
                if (signature != "RIFF")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                int riff_chunck_size = reader.ReadInt32();

                string format = new string(reader.ReadChars(4));
                if (format != "WAVE")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                // WAVE header
                string format_signature = new string(reader.ReadChars(4));
                if (format_signature != "fmt ")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int format_chunk_size = reader.ReadInt32();
                int audio_format = reader.ReadInt16();
                int num_channels = reader.ReadInt16();
                int sample_rate = reader.ReadInt32();
                int byte_rate = reader.ReadInt32();
                int block_align = reader.ReadInt16();
                int bits_per_sample = reader.ReadInt16();

                string data_signature = new string(reader.ReadChars(4));
                if (data_signature != "data")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int data_chunk_size = reader.ReadInt32();

                channels = num_channels;
                bits = bits_per_sample;
                rate = sample_rate;
                sound_data = reader.ReadBytes((int)reader.BaseStream.Length);
            }

            ALFormat sound_format =
                channels == 1 && bits == 8 ? ALFormat.Mono8 :
                channels == 1 && bits == 16 ? ALFormat.Mono16 :
                channels == 2 && bits == 8 ? ALFormat.Stereo8 :
                channels == 2 && bits == 16 ? ALFormat.Stereo16 :
                (ALFormat)0; // unknown
            AL.BufferData(myBuffer, sound_format, sound_data, sound_data.Length, rate);
            if (AL.GetError() != ALError.NoError)
            {

            }
        }

        public void Play(bool isLooping, bool isPlaying, int mySource, Vector3 position)
        {
            //Updates position of audio emitter based on entities position
            emitterPosition = position;
            AL.Source(mySource, ALSource3f.Position, ref emitterPosition);

            //Plays the source if it is not already playing
            if (!isPlaying)
            {
                AL.Source(mySource, ALSourcei.Buffer, myBuffer); // Attaches buffer to source
                AL.Source(mySource, ALSourceb.Looping, isLooping); // Sets looping property of the audio
                AL.SourcePlay(mySource);
            }
        }
    }
}
