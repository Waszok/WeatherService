using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherLibrary
{
    public static class Utilities
    {
        #region JSON methods

        /// <summary>
        /// Generic method which return a proper object filled by data from json file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T JsonReader<T>(string path)
        {
            T items;
            using (System.IO.StreamReader r = new System.IO.StreamReader(path))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<T>(json);
            }
            return items;
        }

        /// <summary>
        /// Generic method which read from json file and deserialize it into a proper object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        public static void JsonReaderAnom<T>(string path)
        {
            using (System.IO.StreamReader r = new System.IO.StreamReader(path))
            {
                string json = r.ReadToEnd();
                T items = JsonConvert.DeserializeObject<T>(json);
            }
        }

        #endregion

    }
}
