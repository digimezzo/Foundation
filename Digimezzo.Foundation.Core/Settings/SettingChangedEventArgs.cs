using System;

namespace Digimezzo.Foundation.Core.Settings
{
    public class SettingChangedEventArgs : EventArgs
    {
        public SettingEntry Entry { get; set; }
    }
}
