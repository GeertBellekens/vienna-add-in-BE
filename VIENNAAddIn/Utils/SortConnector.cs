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
    /// Sort an array list of Connector
    /// </summary>
    /// <returns>Sorted array list</returns>
    public class SortConnector : IComparer
    {
        // Calls CaseInsensitiveComparer.Compare
        int IComparer.Compare(object x, object y)
        {
            return ((new CaseInsensitiveComparer()).Compare(((XmlSchemaElement)x).Name, ((XmlSchemaElement)y).Name));
        }
    }

    /// <summary>
    /// Sort arrayList of array list of Connector
    /// </summary>
    /// <returns>Sorted array list</returns>
    public class SortConnectorByPosition : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            return ((new CaseInsensitiveComparer()).Compare(Convert.ToInt16(((ArrayList)x)[0]), Convert.ToInt16(((ArrayList)y)[0])));
        }
    }
}
