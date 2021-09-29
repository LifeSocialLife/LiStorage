// <summary>
// Rundata Service.
// </summary>
// <copyright file="CommonHelper.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorage.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public static class CommonHelper
    {

        // [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]

        private static string? zzDebug { get; set; }

        // TODO move this to helper

        /// <summary>
        /// Json string to model.
        /// </summary>
        /// <typeparam name="T">model.</typeparam>
        /// <param name="json">json model as string.</param>
        /// <returns>model T.</returns>
        public static T DeserializeJson<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException(nameof(json));
            }

#pragma warning disable CS8603 // Possible null reference return.
            return JsonConvert.DeserializeObject<T>(json);
#pragma warning restore CS8603 // Possible null reference return.
        }

        // TODO move this to helper
        public static T DeserializeJson<T>(byte[] data)
        {
            if (data == null || data.Length < 1)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return DeserializeJson<T>(Encoding.UTF8.GetString(data));
        }

        //TODO Already exist in helper.
        public static string? SerializeJson(object obj, bool pretty)
        {
            if (obj == null) return null;
            string json;

            if (pretty)
            {
                json = JsonConvert.SerializeObject(
                  obj,
                  Newtonsoft.Json.Formatting.Indented,
                  new JsonSerializerSettings
                  {
                      NullValueHandling = NullValueHandling.Ignore,
                      DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                  });
            }
            else
            {
                json = JsonConvert.SerializeObject(obj,
                  new JsonSerializerSettings
                  {
                      NullValueHandling = NullValueHandling.Ignore,
                      DateTimeZoneHandling = DateTimeZoneHandling.Utc
                  });
            }

            return json;
        }

        /// <summary>
        /// Convert string to ushort.
        /// </summary>
        /// <param name="value">input string.</param>
        /// <returns>output value as true, ushort value or false if convert to ushort is not working.</returns>
        public static Tuple<bool, ushort> ConvertStringIntoUshort(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new Tuple<bool, ushort>(false, 0);
            }

            // Convert version string into uint16
            try
            {
                ushort newValue = ushort.Parse(value);
                zzDebug = "sdfdsf";
                return new Tuple<bool, ushort>(true, newValue);
            }
            catch (FormatException)
            {
                return new Tuple<bool, ushort>(false, 0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Tuple<bool, ushort> GetValueFromJsonStringReturnAsUshort(string json, string key)
        {
            var tmpData = GetValueFromJsonStringReturnAsString(json, key);

            if (tmpData.Item1)
            {
                var returnData = ConvertStringIntoUshort(tmpData.Item2);
                if (returnData.Item1)
                {
                    return new Tuple<bool, ushort>(true, returnData.Item2);
                }
            }

            return new Tuple<bool, ushort>(false, 0);
        }

        /// <summary>
        /// Get value from a json string using Key name.
        /// </summary>
        /// <param name="json">json as string.</param>
        /// <param name="key">Id to get the value from.</param>
        /// <returns>false if key not fund in string or true, value if key found inside string.</returns>
        public static Tuple<bool, string> GetValueFromJsonStringReturnAsString(string json, string key)
        {
            if (string.IsNullOrEmpty(json))
            {
                return new Tuple<bool, string>(false, string.Empty);
            }

            if (!json.Contains(key))
            {
                return new Tuple<bool, string>(false, string.Empty);
            }

            int startpos = json.IndexOf(key);

            // string tmpString = tmpConfigFileAsString.Substring(hej);
            string tmpString = json[startpos..];

            if (tmpString.Contains(":") && tmpString.Contains(","))
            {
                int tmpIdFirst = tmpString.IndexOf(":");
                int tmpIdLast = tmpString.IndexOf(",");
                string tmpVersionData = tmpString.Substring(tmpIdFirst + 1, tmpIdLast - tmpIdFirst - 1).Trim();

                zzDebug = "dsfdsf";

                return new Tuple<bool, string>(true, tmpVersionData);
            }

            zzDebug = "sfdsf";

            return new Tuple<bool, string>(false, string.Empty);
        }

        #region IsTrue

        public static bool IsTrue(bool val)
        {
            return val;
        }

        public static bool IsTrue(bool? val)
        {
            if (val == null) return false;
            return Convert.ToBoolean(val);
        }

        public static bool IsTrue(string val)
        {
            if (String.IsNullOrEmpty(val)) return false;
            val = val.ToLower().Trim();
            int valInt = 0;
            if (Int32.TryParse(val, out valInt)) if (valInt == 1) return true;
            if (String.Compare(val, "true") == 0) return true;
            return false;
        }

        #endregion

        #region String safe to use

        public static bool ContainsUnsafeCharacters(string data)
        {
            /*
             * 
             * Returns true if unsafe characters exist
             * 
             * 
             */

            // see https://kb.acronis.com/content/39790

            if (String.IsNullOrEmpty(data)) return false;
            if (data.Equals(".")) return true;
            if (data.Equals("..")) return true;

            if (
                (String.Compare(data.ToLower(), "com1") == 0) ||
                (String.Compare(data.ToLower(), "com2") == 0) ||
                (String.Compare(data.ToLower(), "com3") == 0) ||
                (String.Compare(data.ToLower(), "com4") == 0) ||
                (String.Compare(data.ToLower(), "com5") == 0) ||
                (String.Compare(data.ToLower(), "com6") == 0) ||
                (String.Compare(data.ToLower(), "com7") == 0) ||
                (String.Compare(data.ToLower(), "com8") == 0) ||
                (String.Compare(data.ToLower(), "com9") == 0) ||
                (String.Compare(data.ToLower(), "lpt1") == 0) ||
                (String.Compare(data.ToLower(), "lpt2") == 0) ||
                (String.Compare(data.ToLower(), "lpt3") == 0) ||
                (String.Compare(data.ToLower(), "lpt4") == 0) ||
                (String.Compare(data.ToLower(), "lpt5") == 0) ||
                (String.Compare(data.ToLower(), "lpt6") == 0) ||
                (String.Compare(data.ToLower(), "lpt7") == 0) ||
                (String.Compare(data.ToLower(), "lpt8") == 0) ||
                (String.Compare(data.ToLower(), "lpt9") == 0) ||
                (String.Compare(data.ToLower(), "con") == 0) ||
                (String.Compare(data.ToLower(), "nul") == 0) ||
                (String.Compare(data.ToLower(), "prn") == 0) ||
                (String.Compare(data.ToLower(), "con") == 0)
                )
            {
                return true;
            }

            for (int i = 0; i < data.Length; i++)
            {
                if (
                    ((int)(data[i]) < 32) ||    // below range
                    ((int)(data[i]) > 126) ||   // above range
                    ((int)(data[i]) == 47) ||   // slash /
                    ((int)(data[i]) == 92) ||   // backslash \
                    ((int)(data[i]) == 63) ||   // question mark ?
                    ((int)(data[i]) == 60) ||   // less than < 
                    ((int)(data[i]) == 62) ||   // greater than >
                    ((int)(data[i]) == 58) ||   // colon :
                    ((int)(data[i]) == 42) ||   // asterisk *
                    ((int)(data[i]) == 124) ||  // pipe |
                    ((int)(data[i]) == 34) ||   // double quote "
                    ((int)(data[i]) == 39) ||   // single quote '
                    ((int)(data[i]) == 94)      // caret ^
                    )
                {
                    return true;
                }
            }

            return false;
        }

        public static bool ContainsUnsafeCharacters(List<string> data)
        {
            if (data == null || data.Count < 1) return true;
            foreach (string curr in data)
            {
                if (ContainsUnsafeCharacters(curr)) return true;
            }
            return false;
        }

        #endregion

        #region Random

        public static string RandomString(int numChar)
        {
            string ret = "";
            if (numChar < 1) return "";
            int valid = 0;
            Random random = new Random((int)DateTime.Now.Ticks);
            int num = 0;

            for (int i = 0; i < numChar; i++)
            {
                num = 0;
                valid = 0;
                while (valid == 0)
                {
                    num = random.Next(126);
                    if (((num > 47) && (num < 58)) ||
                        ((num > 64) && (num < 91)) ||
                        ((num > 96) && (num < 123)))
                    {
                        valid = 1;
                    }
                }
                ret += (char)num;
            }

            return ret;
        }


        #endregion

    }
}
