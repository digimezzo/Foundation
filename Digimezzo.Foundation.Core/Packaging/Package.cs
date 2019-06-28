using System;

namespace Digimezzo.Foundation.Core.Packaging
{
    public class Package
    {
        private string name { get; set; }
        private Version version { get; set; }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public Version Version
        {
            get
            {
                return this.version;
            }
        }

        public string UnformattedVersion
        {
            get
            {
                //  {0}: Major Version,
                //  {1}: Minor Version,
                //  {2}: Build Number,
                //  {3}: Revision
                return string.Format("{0}.{1}.{2}.{3}", this.version.Major, this.version.Minor, this.version.Build, this.version.Revision);
            }
        }

        public string FormattedVersion
        {
            get
            {
                //  {0}: Major Version,
                //  {1}: Minor Version,
                //  {2}: Build Number,
                //  {3}: Revision

                if (this.version.Build != 0)
                {
                    return string.Format("{0}.{1}.{2} (Build {3})", this.version.Major, this.version.Minor, this.version.Build, this.version.Revision);
                }
                else
                {
                    return string.Format("{0}.{1} (Build {2})", this.version.Major, this.version.Minor, this.version.Revision);
                }
            }
        }

        public string FormattedVersionNoBuild
        {
            get
            {
                //  {0}: Major Version,
                //  {1}: Minor Version,
                //  {2}: Build Number,
                //  {3}: Revision

                if (this.version.Build != 0)
                {
                    return string.Format("{0}.{1}.{2}", this.version.Major, this.version.Minor, this.version.Build, this.version.Revision);
                }
                else
                {
                    return string.Format("{0}.{1}", this.version.Major, this.version.Minor, this.version.Revision);
                }
            }
        }

        public string FormattedVersionNoBuildWithLabel
        {
            get
            {
                string versionType = this.Version.Revision.ToString().Substring(0, 1);
                string versionTypeNumber = this.Version.Revision.ToString().Substring(1, 3).TrimStart(new Char[] { '0' });
                string returnValue = string.Empty;

                switch (versionType)
                {
                    case "1":
                        returnValue = $"{this.FormattedVersionNoBuild} (Preview {versionTypeNumber})";
                        break;
                    case "2":
                        returnValue = $"{this.FormattedVersionNoBuild} (Beta {versionTypeNumber})";
                        break;
                    case "3":
                        returnValue = $"{this.FormattedVersionNoBuild} (Release Candidate {versionTypeNumber})";
                        break;
                    case "4":
                        returnValue = $"{this.FormattedVersionNoBuild} (Release)";
                        break;
                    default:
                        returnValue = this.FormattedVersionNoBuild;
                        break;
                }

                return returnValue;
            }
        }

        public string Filename
        {
            get
            {
                return string.Format("{0} {1}", this.name, this.FormattedVersionNoBuildWithLabel);
            }
        }

        public string InstallableFileExtension
        {
            get
            {
                return ".msi";
            }
        }

        public string PortableFileExtension
        {
            get
            {
                return ".zip";
            }
        }

        public string UpdateFileExtension
        {
            get
            {
                return ".update";
            }
        }

        public Package(string name, Version version)
        {
            this.name = name;
            this.version = version;
        }

        public bool IsOlder(Package referencePackage)
        {
            return this.version < referencePackage.version;
        }
    }
}
