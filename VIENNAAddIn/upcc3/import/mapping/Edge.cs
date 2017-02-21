using System;
using System.IO;

namespace VIENNAAddIn.upcc3.import.mapping
{
    /// <summary>
    /// Represents a MapForce mapping element of the form:
    /// <edge vertexkey="78577400" edgekey="81255240"/>
    /// which reside in the tree of the graph element.
    /// </summary>
    public class Edge : IEquatable<Edge>
    {
        public string EdgeKey { get; private set; }
        public string TargetVertexKey { get; private set; }

        public Edge(string edgeKey, string targetVertexKey, string mappingFile)
        {
            EdgeKey = InputOutputKey.PrependPrefix(mappingFile, edgeKey);
            TargetVertexKey = InputOutputKey.PrependPrefix(mappingFile, targetVertexKey);
        }

        public bool Equals(Edge other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.EdgeKey, EdgeKey) && Equals(other.TargetVertexKey, TargetVertexKey);
        }

        public static bool operator ==(Edge left, Edge right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Edge left, Edge right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Edge)) return false;
            return Equals((Edge) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((EdgeKey != null ? EdgeKey.GetHashCode() : 0) * 397) ^ (TargetVertexKey != null ? TargetVertexKey.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return string.Format("Edge [Key: {0}, TargetVertexKey: {1}]", EdgeKey, TargetVertexKey);
        }

        public void PrettyPrint(TextWriter writer, string indent)
        {
            writer.WriteLine(indent + "Edge [" + EdgeKey + "] -> " + TargetVertexKey);
        }
    }
}