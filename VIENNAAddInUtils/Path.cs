// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace VIENNAAddInUtils
{
    public class Path
    {
        public static readonly Path EmptyPath = new Path();

        private readonly List<string> parts = new List<string>();

        public Path()
        {
        }

        public Path(string firstPart) : this()
        {
            parts.Add(firstPart);
        }

        public Path(IEnumerable<string> parts) : this()
        {
            this.parts.AddRange(parts);
        }

        public Path(Path path): this(path.parts)
        {
        }

        public string FirstPart
        {
            get { return parts[0]; }
        }

        public Path Rest
        {
            get
            {
                return Length < 2 ? EmptyPath : new Path(parts.GetRange(1, Length - 1));
            }
        }

        public int Length
        {
            get { return parts.Count; }
        }

        public Path Append(string part)
        {
            parts.Add(part);
            return this;
        }

        public static Path operator /(Path lhs, string rhs)
        {
            return new Path(lhs).Append(rhs);
        }

        public static implicit operator Path(string firstPart)
        {
            return new Path(firstPart);
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            foreach (var part in parts)
            {
                result.Append("/").Append(part);
            }
            return result.ToString();
        }
    }
}