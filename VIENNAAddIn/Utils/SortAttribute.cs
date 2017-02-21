/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections;
using System.Text;
using System.Xml.Schema;

namespace VIENNAAddIn.Utils
{
    /// <summary>
    /// Sort an Array List based on "position" tagged value for BBIE/ASBIE in XSD generation 
    /// </summary>
    public class SortByPosition : IComparer
    {
        // Calls CaseInsensitiveComparer.Compare
        int IComparer.Compare(object x, object y)
        {
            return ((new CaseInsensitiveComparer()).Compare(Convert.ToInt16(((ArrayList)x)[0]), Convert.ToInt16(((ArrayList)y)[0])));
        }
    }

    /// <summary>
    /// Sort an Array List alphabetically for BBIE/ASBIE in XSD generation 
    /// </summary>
    public class SortByName : IComparer
    {
        // Calls CaseInsensitiveComparer.Compare
        int IComparer.Compare(object x, object y)
        {
            return (new CaseInsensitiveComparer()).Compare(((ArrayList)x)[0].ToString(), ((ArrayList)y)[0].ToString());
        }   
    }

}
