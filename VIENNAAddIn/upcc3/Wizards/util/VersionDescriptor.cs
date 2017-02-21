using System;

namespace VIENNAAddIn.upcc3.Wizards.util
{
    public struct VersionDescriptor
    {
        public string Major;
        public string Minor;
        public string Comment;

        public string ResourceDirectory
        {
            get { return Major + "_" + Minor; }
        }

        public override string ToString()
        {
            return string.Format("VersionDescriptor <\"{0}\", \"{1}\", \"{2}\">", Major, Minor, Comment);
        }
        
        public static VersionDescriptor ParseVersionString(string versionString)
        {                      
            string[] stringTokens = versionString.Split('|');

            if (stringTokens.Length == 3)
            {
                VersionDescriptor versionDescriptor = new VersionDescriptor
                                                          {
                                                              Major = stringTokens[0],
                                                              Minor = stringTokens[1],
                                                              Comment = stringTokens[2]
                                                          };

                return versionDescriptor;
            }
            
            throw new ArgumentException("expected version string: <Major|Minor|Comment>, but was: <{0}>", versionString);            
        }
    }
}