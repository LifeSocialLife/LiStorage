using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LiStorage.Helpers
{
    public static class EncodingHelper
    {
        /// <summary>
        /// Encode stream to byte[]
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Tuple<bool, byte[]> StreamToBytes(Stream input)
        {
            if (input == null) return new Tuple<bool, byte[]>(false, new byte[0]);
            if (!input.CanRead) new Tuple<bool, byte[]>(false, new byte[0]);

            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;

                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return new Tuple<bool, byte[]>(true, ms.ToArray());

            }
        }

        /// <summary>
        /// Base64 (string) to byte[]
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Base64ToBytes(string data)
        {
            return Convert.FromBase64String(data);
        }

        /// <summary>
        /// Base64 (string) to string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Base64ToString(string data)
        {
            if (String.IsNullOrEmpty(data)) return "";
            byte[] bytes = System.Convert.FromBase64String(data);
            return System.Text.UTF8Encoding.UTF8.GetString(bytes);
        }

        public static string BytesToBase64(byte[] data)
        {
            if (data == null) return "";
            if (data.Length < 1) return "";
            return System.Convert.ToBase64String(data);
        }

        public static string StringToBase64(string data)
        {
            if (String.IsNullOrEmpty(data)) return "";
            byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(data);
            return System.Convert.ToBase64String(bytes);
        }

        public static List<string> CsvToStringList(string csv)
        {
            if (String.IsNullOrEmpty(csv))
            {
                return new List<string>();
            }

            List<string> ret = new List<string>();

            string[] array = csv.Split(',');

            if (array != null && array.Length > 0)
            {
                foreach (string curr in array)
                {
                    if (String.IsNullOrEmpty(curr)) continue;
                    ret.Add(curr.Trim());
                }
            }

            return ret;
        }

        public static string StringListToCsv(List<string> strings)
        {
            if (strings == null || strings.Count < 1) return "";

            int added = 0;
            string ret = "";

            foreach (string curr in strings)
            {
                if (added == 0)
                {
                    ret += curr;
                }
                else
                {
                    ret += "," + curr;
                }

                added++;
            }

            return ret;
        }
    }
}
