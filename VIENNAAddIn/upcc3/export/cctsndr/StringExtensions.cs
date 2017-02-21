// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;

namespace VIENNAAddIn.upcc3.export.cctsndr
{
    public static class StringExtensions
    {
        public static string ToXmlName(this string n)
        {
            if (String.IsNullOrEmpty(n))
                return "";
            n = n.Replace(' ', '_');
            n = n.Replace('/', '_');
            return n;
        }
    }
}