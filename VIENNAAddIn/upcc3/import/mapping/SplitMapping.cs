using System;
using System.Collections.Generic;
using System.Text;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;

namespace VIENNAAddIn.upcc3.import.mapping
{
    public class SplitMapping : ElementMapping, IEquatable<SplitMapping>
    {
        public SplitMapping(SourceItem sourceElement, IEnumerable<IBcc> targetBccs,
                            IEnumerable<SimpleTypeToCdtMapping> cdtMappings): base(sourceElement)
        {
            CdtMappings = new List<SimpleTypeToCdtMapping>(cdtMappings);
            TargetBccs = new List<IBcc>(targetBccs);
        }

        public string ElementName
        {
            get { return SourceItem.Name; }
        }

        public List<SimpleTypeToCdtMapping> CdtMappings { get; set; }

        public SimpleTypeToCdtMapping GetCdtMappingForTargetBcc(IBcc targetBcc)
        {
            ICdt cdt = targetBcc.Cdt;
            foreach (SimpleTypeToCdtMapping cdtMapping in CdtMappings)
            {
                if (cdtMapping.TargetCDT.Id == cdt.Id)
                {
                    return cdtMapping;
                }
            }
            return null;
        }

        public override string BIEName
        {
            get { throw new NotImplementedException(); }
        }

        public List<IBcc> TargetBccs { get; private set; }

        public IEnumerable<IAcc> TargetAccs
        {
            get
            {
                var targetAccs = new List<IAcc>();
                foreach (IBcc bcc in TargetBccs)
                {
                    IAcc acc = bcc.Acc;
                    if (!targetAccs.Contains(acc))
                    {
                        targetAccs.Add(acc);
                    }
                }
                return targetAccs;
            }
        }

        #region IEquatable<SplitMapping> Members

        public bool Equals(SplitMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (other.TargetBccs.Count != TargetBccs.Count)
            {
                return false;
            }

            for (int i = 0; i < other.TargetBccs.Count; i++)
            {
                if (other.TargetBccs[i].Id != TargetBccs[i].Id)
                {
                    return false;
                }
                //    if (((ICdtSup)other.TargetBccs[i]).Id != ((ICdtSup)TargetBccs[i]).Id)
                //    {
                //        return false;
                //    }
            }

            return Equals(other.SourceItem.Name, SourceItem.Name);
        }

        #endregion

        public override string ToString()
        {
            var s = new StringBuilder();
            foreach (IBcc targetBcc in TargetBccs)
            {
                s.Append(targetBcc.Name).Append(",");
                //if (targetBcc is ICdtSup)
                //{
                //    s.Append("SUP[").Append(((ICdtSup)targetBcc).Name).Append("],");
                //}
            }
            return string.Format("SplitMapping <SourceItem: {0}, Target BCCs: {1}>", SourceItem.Name, s);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (SplitMapping)) return false;
            return Equals((SplitMapping) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((SourceItem != null ? SourceItem.GetHashCode() : 0)*397) ^
                       (TargetBccs != null ? TargetBccs.GetHashCode() : 0);
            }
        }

        public static bool operator ==(SplitMapping left, SplitMapping right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SplitMapping left, SplitMapping right)
        {
            return !Equals(left, right);
        }

        public string GetBbieName(IBcc targetBcc)
        {
            return SourceItem.Name + "_" + targetBcc.Name;
        }

        public override bool ResolveTypeMapping(SchemaMapping schemaMapping)
        {
            // do nothing
            return true;
        }
    }
}