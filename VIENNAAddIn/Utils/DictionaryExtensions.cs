using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VIENNAAddIn.Utils
{
    ///<summary>
    /// Extension methods for dictionaries.
    ///</summary>
    public static class DictionaryExtensions
    {

        ///<summary>
        /// Retrieves a value from the dictionary. If the key does not yet exist, a new instance of the value type is added to the dictionary for the given key.
        ///</summary>
        ///<param name="dictionary"></param>
        ///<param name="key"></param>
        ///<typeparam name="TKey"></typeparam>
        ///<typeparam name="TValue"></typeparam>
        ///<returns></returns>
        public static TValue GetAndCreate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key) where TValue:new()
        {
            TValue value;
            if (!dictionary.TryGetValue(key, out value))
            {
                value = new TValue();
                dictionary[key] = value;
            }
            return value;
        }

        /// <summary>
        /// Adds the given value to the values stored for the given key.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddValueToList<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key, TValue value)
        {
            dictionary.GetAndCreate(key).Add(value);
        }

        ///<summary>
        /// Print the dictionary to Console.Out in tabular form.
        ///</summary>
        ///<param name="dictionary"></param>
        public static void PrintAsTable<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            dictionary.PrintAsTable(Console.Out);
        }

        ///<summary>
        /// Print the dictionary to the writer in tabular form.
        ///</summary>
        ///<param name="dictionary"></param>
        ///<param name="writer"></param>
        ///<typeparam name="TKey"></typeparam>
        ///<typeparam name="TValue"></typeparam>
        public static void PrintAsTable<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TextWriter writer)
        {
            int keyColumnWidth = "Key".Length;
            foreach (var key in dictionary.Keys)
            {
                keyColumnWidth = Math.Max(keyColumnWidth, key.ToString().Length);
            }
            int valueColumnWidth = "Value".Length;
            foreach (var value in dictionary.Values)
            {
                valueColumnWidth = Math.Max(valueColumnWidth, value.ToString().Length);
            }
            var hr = "+" + new String('-', keyColumnWidth + valueColumnWidth + 5) + "+";
            writer.WriteLine(hr);
            writer.WriteLine("| {0," + (-keyColumnWidth) + "} | {1," + (-valueColumnWidth) + "} |", "Key", "Value");
            writer.WriteLine(hr);
            foreach (var key in dictionary.Keys)
            {
                writer.WriteLine("| {0," + (-keyColumnWidth) + "} | {1," + (-valueColumnWidth) + "} |", key, dictionary[key]);
            }
            writer.WriteLine(hr);
        }

        ///<summary>
        ///</summary>
        ///<param name="dictionary"></param>
        ///<typeparam name="TKey"></typeparam>
        ///<typeparam name="TValue"></typeparam>
        ///<returns></returns>
        public static string ToStringDebug<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            var result = new StringBuilder();
            result.Append("[");
            foreach (var key in dictionary.Keys)
            {
                result.Append(string.Format("({0} => {1})", key, dictionary[key]));
            }
            result.Append("]");
            return result.ToString();
        }
    }
}