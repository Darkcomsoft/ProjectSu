using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Audio.OpenAL;
using OpenTK;
using OpenTK.Audio;
using ProjectSu;
using System.IO;
using System.Threading;

namespace ProjectSu.src.Engine
{
    /// <summary>
    /// SoundEngine, AudioThread
    /// </summary>
    public class SoundSystem : ClassBase
    {
		ALContext _audioContext;
        
        private Thread soundThread;

        private bool Running = false;
        private bool finished = false;

		public SoundSystem()
        {
            //talvez remover isso depois, 
            //por que a nova versao do opentk parece que tem um methdo melhor para checar se voce nao tem openal
            if (Application.NoSoundMode) { return; }
            //

            Running = true;
            soundThread = new Thread(new ThreadStart(StartSoundSystem));
            soundThread.Name = "SoundEngine Thread";
            soundThread.Start();
            
            
            while (_audioContext == null){   }
            _audioContext.MakeCurrent();
        }


        private void StartSoundSystem()
        {
            Debug.Log("Starting Sound Engine!");
            _audioContext = new AudioContext(AudioContext.DefaultDevice, 0, 0, true);

            _audioContext.Process();
            
            Debug.Log("DefaultDevice: " + AudioContext.DefaultDevice);
            Debug.Log("Version: " + AL.Get(ALGetString.Version));
            Debug.Log("Vendor: " + AL.Get(ALGetString.Vendor));
            Debug.Log("Renderer: " + AL.Get(ALGetString.Renderer));

            Debug.Log("Sound Engine Started!");

            while (_audioContext.IsProcessing && Running) { Thread.Sleep(1000); }

            Alc.MakeContextCurrent(ContextHandle.Zero);
            _audioContext.Dispose();
            finished = true;
        }

        protected override void OnDispose()
        {
            Running = false;
            while (!finished) { }
            Debug.Log("Sound Engine Stoped!");
            base.OnDispose();
        }

        private void CheckError(string stage)
        {
            ALError error = AL.GetError();

            if (error != ALError.NoError)
            {
                Debug.LogError("OpenAL ERROR: " + error);
            }
        }
    }

    public class WavLoader
    {
        public int _Channels { get; set; }
        public int _Bits { get; set; }
        public int _Rate { get; set; }
        public byte[] _Data { get; set; }
        public ALFormat _Format { get; set; }

        public WavLoader(string Path, string filename)
        {
            string finalPath = string.Concat(Path, filename, ".wav");

            if (!File.Exists(finalPath))
            {
                throw new Exception("Dont found this sound file: " + finalPath);
            }

            using (var reader = new BinaryReader(File.Open(finalPath, FileMode.Open)))
            {
                var signature = new string(reader.ReadChars(4));
                if (signature != "RIFF")
                {
                    Debug.LogError("Is not a wave file.");
                    return;
                }

                reader.ReadInt32();

                var format = new string(reader.ReadChars(4));
                if (format != "WAVE")
                {
                    Debug.LogError("Is not a wave file.");
                    return;
                }

                var formatSignature = new string(reader.ReadChars(4));
                if (formatSignature != "fmt ")
                {
                    Debug.LogError("This Type of Wave file is not supported.");
                    return;
                }

                reader.ReadInt32();
                reader.ReadInt16();
                _Channels = reader.ReadInt16();
                _Rate = reader.ReadInt32();

                reader.ReadInt32();
                reader.ReadInt16();
                _Bits = reader.ReadInt16();

                var dataSignature = new string(reader.ReadChars(4));
                if (dataSignature != "data")
                {
                    Debug.LogError("This Type of Wave file is not supported.");
                    return;
                }

                reader.ReadInt32();

                _Data = reader.ReadBytes((int)reader.BaseStream.Length);
                _Format = Getsoundformat(_Channels, _Bits);
            }
        }

        private static ALFormat Getsoundformat(int channels, int bits)
        {
            switch (channels)
            {
                case 1: 
                    return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: 
                    return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new Exception("format is not supported.");
            }
        }
    }
}
