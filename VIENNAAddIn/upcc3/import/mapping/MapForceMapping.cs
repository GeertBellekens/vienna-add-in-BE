using System;
using System.Collections.Generic;
using System.IO;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.import.mapping
{
    /// <summary>
    /// Basically represents a MapForce mapping attribute of the form:
    /// <mapping version="11">
    /// with all necessary information used for the import of an XML Schema document model.
    /// </summary>
    public class MapForceMapping : IEquatable<MapForceMapping>
    {
        private readonly Dictionary<string, string> edges;
        public Graph Graph { get; private set; }

        public MapForceMapping(IEnumerable<SchemaComponent> schemaComponents, IEnumerable<ConstantComponent> constantComponents, IEnumerable<FunctionComponent> functionComponents, Graph graph)
        {
            Graph = graph;

            edges = new Dictionary<string, string>();
            Console.Out.WriteLine("Assembling Edges");
            foreach (Vertex vertex in Graph.Vertices)
            {
                foreach (Edge edge in vertex.Edges)
                {
                    string sourceKey = vertex.Key;
                    string targetKey = edge.TargetVertexKey;
                    edges[sourceKey] = targetKey;
                }
            }
            Console.Out.WriteLine("Done.");
            
            SchemaComponents = new List<SchemaComponent>(schemaComponents);
            ConstantComponents = new List<ConstantComponent>(constantComponents);
            FunctionComponents = new List<FunctionComponent>(functionComponents);
        }

        public IEnumerable<SchemaComponent> SchemaComponents { get; private set; }
        public IEnumerable<ConstantComponent> ConstantComponents { get; private set; }
        public IEnumerable<FunctionComponent> FunctionComponents{ get; private set; }
        

        #region IEquatable<MapForceMapping> Members

        public bool Equals(MapForceMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || (other.SchemaComponents.IsEqualTo(SchemaComponents) && other.ConstantComponents.IsEqualTo(ConstantComponents) && other.FunctionComponents.IsEqualTo(FunctionComponents));
        }

        #endregion

        public void PrettyPrint(TextWriter writer, string indent)
        {
            foreach (SchemaComponent schemaComponent in SchemaComponents)
            {
                schemaComponent.PrettyPrint(writer, indent);
            }
            foreach (var constantComponent in ConstantComponents)
            {
                constantComponent.PrettyPrint(writer, indent);
            }
            foreach (FunctionComponent functionComponent in FunctionComponents)
            {
                functionComponent.PrettyPrint(writer, indent);
            }
            Graph.PrettyPrint(writer, indent);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(MapForceMapping)) return false;
            return Equals((MapForceMapping)obj);
        }

        public override int GetHashCode()
        {
            return (SchemaComponents != null ? SchemaComponents.GetHashCode() : 0);
        }

        public string GetConstant(string name)
        {
            foreach (ConstantComponent constantComponent in ConstantComponents)
            {
                if (constantComponent.Value.StartsWith(name + ":"))
                {
                    return constantComponent.Value.Substring(name.Length + 1).Trim();
                }
            }
            return null;
        }

        public IEnumerable<SchemaComponent> GetInputSchemaComponents()
        {
            foreach (SchemaComponent schemaComponent in SchemaComponents)
            {
                if (schemaComponent.IsInputSchema())
                {
                    yield return schemaComponent;
                }
            }
        }

        /// <summary>
        /// Returns a set of TargetSchemaComponents, i.e, OutputSchemas.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SchemaComponent> GetTargetSchemaComponents()
        {
            foreach (SchemaComponent schemaComponent in SchemaComponents)
            {
                if (schemaComponent.IsOutputSchema())
                {
                    yield return schemaComponent;
                }
            }
        }

        public string GetMappingTargetKey(string sourceKey)
        {
            string targetKey;
            if (edges.TryGetValue(sourceKey, out targetKey))
            {
                return targetKey;
            }
            return null;
        }
    }
}