// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Text;

namespace VIENNAAddInUtils
{
    public static class StringExtensions
    {
        public static string FirstCharToUpperCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            return str.Substring(0, 1).ToUpper() + str.Substring(1, str.Length - 1);
        }

        public static string FirstCharToLowerCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            return str.Substring(0, 1).ToLower() + str.Substring(1, str.Length - 1);
        }

        public static string Plural(this string str)
        {
            if (str.EndsWith("s") || str.EndsWith("x"))
            {
                return str + "es";
            }
            if (str.EndsWith("y"))
            {
                return str.Substring(0, str.Length - 1) + "ies";
            }
            return str + "s";
        }

        public static string Minus(this string str, string suffix)
        {
            if (str.EndsWith(suffix))
            {
                return str.Substring(0, str.Length - suffix.Length);
            }
            return str;
        }

        public static string Times(this string str, int times)
        {
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < times; i++)
            {
                stringBuilder.Append(str);
            }
            return stringBuilder.ToString();
        }

        public static string DefaultTo(this string str, string defaultValue)
        {
            return String.IsNullOrEmpty(str) ? defaultValue : str;
        }
    }
}