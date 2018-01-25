namespace Digimezzo.Foundation.Core.Helpers
{
    /// <summary>
    /// A helper object containing a name and a value
    /// </summary>
    public class NameValue
    {
        private string name;
        private int value;

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public override string ToString()
        {
            return this.Name.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            return this.Value == ((NameValue)obj).Value;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }
    }
}
