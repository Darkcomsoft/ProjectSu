using Projectsln.darkcomsoft.src.engine;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Projectsln.darkcomsoft.src.misc
{
    public static class Utilits
    {
        /// <summary>
        /// Create a class instance at runtime, procedural, is the same used in the code when you call "new" keyword
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
    }
}