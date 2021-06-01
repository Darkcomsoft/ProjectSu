using ProjectIND.darkcomsoft.src.debug;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using OpenTK.Mathematics;

namespace ProjectIND.darkcomsoft.src.misc
{
    /// <summary>
    /// All type of Utilits
    /// </summary>
    public static class Utilits
    {
        private static int m_gcClearTime = 0;

        /// <summary>
        /// This function call the <see cref="GC.Collect"/>, but this is not need, so to make sure the gc is called this will be called until a int variable reach 5000 units, so its no every frame
        /// </summary>
        public static void GC_Collect()
        {
            m_gcClearTime++;
            if (m_gcClearTime >= 5000)
            {
                GC.Collect();
                m_gcClearTime = 0;
            }
        }

        /// <summary>
        /// Open the <paramref name="URL"/> in the OS default browser
        /// </summary>
        /// <param name="URL"></param>
        public static void GoToSite(string URL)
        {
            System.Diagnostics.Process.Start(URL);
        }

        /// <summary>
        /// Create a class instance at runtime, is the same used in the code when you call "new" keyword
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static I CreateInstance<I>(Type type) where I : class
        {
            return Activator.CreateInstance(type) as I;
        }

        /// <summary>
        /// Generate a unique id. Length is for how long you want to be the id, 1 is normal(short)
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static int UniqueID(int Length)
        {
            Random random = new Random();
            DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            int currentEpochTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
            int z1 = random.Next(0, 1000000);
            int z2 = random.Next(0, 1000);
            return (currentEpochTime / z1 + z2 * Length);
        }

        /// <summary>
        /// Get ip from a url using a web address, this get the first address from the web address, EX: myserver.com or survival.myserver.com
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetipfromURL(string url)
        {
            url = url.Replace("http://", ""); //remove http://
            url = url.Replace("https://", ""); //remove https://
            url = url.Substring(0, url.IndexOf("/")); //remove everything after the first /

            try
            {
                IPHostEntry hosts = Dns.GetHostEntry(url);
                if (hosts.AddressList.Length > 0)
                {
                    return hosts.AddressList[0].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Could not get IP for URL " + url + " ERROR: " + e.StackTrace);
                return string.Empty;
            }
        }

        /// <summary>
        /// Check if the openGL return a erro, if return a error print to console
        /// </summary>
        /// <param name="msg"></param>
        public static void GL_CheckError(string msg)
        {
            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Debug.LogError("[" + msg + "] " + error.ToString(), "OpenGL");
            }
        }

        [DllImport("kernel32", SetLastError = true)]
        static extern IntPtr LoadLibrary(string lpFileName);

        public static bool CheckDLLExist(string dllName)
        {
            return LoadLibrary(dllName) != IntPtr.Zero;
        }
    }

    public class Plane
    {
        public Vector3 Normal;
        public float D;

        public float A => Normal.X;
        public float B => Normal.Y;
        public float C => Normal.Z;

        public Plane(Vector4 v) : this(v.Xyz, v.W)
        {
        }

        public Plane(Vector3 normal, float d)
        {
            Normal = normal;
            D = d;
        }

        public void Normalize()
        {
            var x = 1 / Normal.Length;
            Normal *= x;
            D *= x;
        }
    }

    public static class JsonHelper
    {
        public static string SerializeObject<T>(this T toSerialize)
        {
            /*JsonConvert.SerializeObject(product);
            ne xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }*/

            return JsonConvert.SerializeObject(toSerialize);
        }

        public static T DeSerializeObject<T>(string jsonstring)
        {
            /*XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringReader textReader = new StringReader(xmlstring))
            {
                return (T)xmlSerializer.Deserialize(textReader);
            }*/

            return JsonConvert.DeserializeObject<T>(jsonstring);
        }

        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonConvert.DeserializeObject<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonConvert.SerializeObject(wrapper);
        }

        #region ArrayCord

        public static T[,] FromJsonChunk<T>(string json)
        {
            WrapperChunk<T> wrapper = JsonConvert.DeserializeObject<WrapperChunk<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[,] array)
        {
            WrapperChunk<T> wrapper = new WrapperChunk<T>();
            wrapper.Items = array;
            return JsonConvert.SerializeObject(wrapper);
        }
        #endregion

        [System.Serializable]
        public class Wrapper<T>
        {
            public T[] Items;
        }

        [System.Serializable]
        public class WrapperChunk<T>
        {
            public T[,] Items;
        }
    }

    /// <summary>
    /// Some Compress and Decompress functions
    /// </summary>
    public static class DataCompressor
    {
        /// <summary>
        /// Compresses the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string CompressString(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            System.Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }

        /// <summary>
        /// Decompresses the string.
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns></returns>
        public static string DecompressString(string compressedText)
        {
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

        /// <summary>
        /// Compresses the string. to bytes
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static byte[] CompressStringToByte(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            System.Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return gZipBuffer;
        }

        /// <summary>
        /// Decompresses the byte array to string
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns></returns>
        public static string DecompressByteToString(byte[] compressedText)
        {
            byte[] gZipBuffer = compressedText;
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }
    }
}