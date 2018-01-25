using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimezzo.Foundation.Core.Helpers
{
    /// <summary>
    /// A helper object to use as a result of a function
    /// </summary>
    public class OperationResult
    {
        private List<string> messages;

        public bool Result { get; set; }

        public OperationResult()
        {
            this.messages = new List<string>();
        }

        public void AddMessage(string iMessage)
        {
            this.messages.Add(iMessage);
        }

        public string GetFirstMessage()
        {
            if (this.messages.Count > 0)
            {
                return this.messages.First();
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetMessages()
        {

            StringBuilder sb = new StringBuilder();

            foreach (string item in this.messages)
            {
                sb.AppendLine(item + Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
