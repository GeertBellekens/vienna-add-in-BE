using System.IO;
using VIENNAAddIn.upcc3.export.cctsndr;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.import.mapping
{
    public class MappingPrettyPrinter:IMappingVisitor
    {
        private readonly TextWriter writer;
        private readonly string indentStr;
        private int depth = 0;

        public MappingPrettyPrinter(TextWriter writer, string indentStr)
        {
            this.writer = writer;
            this.indentStr = indentStr;
        }

        private void LineBreak()
        {
            writer.WriteLine();
        }

        private void Print(string format, params object[] arg)
        {
            writer.Write(format, arg);
        }

        private void Indent()
        {
            writer.Write(indentStr.Times(depth));
        }

        public void VisitBeforeChildren(IMapping mapping)
        {
            Indent();
            Print(mapping.ToString());
            LineBreak();
            ++depth;
        }

        public void VisitAfterChildren(IMapping mapping)
        {
            --depth;
        }
    }
}