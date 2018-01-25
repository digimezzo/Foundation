namespace Digimezzo.Foundation.Core.Packaging
{
    public class ExternalComponent
    {
        private string name;
        private string description;
        private string url;
        private string licenseUrl;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public string LicenseUrl
        {
            get { return licenseUrl; }
            set { licenseUrl = value; }
        }
    }
}
