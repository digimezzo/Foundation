using Digimezzo.Foundation.Core.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Timers;
using System.Xml.Linq;

namespace Digimezzo.Foundation.Core.Settings
{
    public delegate void SettingChangedEventHandler(object sender, SettingChangedEventArgs e);

    public class SettingsClient
    {
        private static SettingsClient instance;
        private Timer timer;
        private object timerMutex = new object();
        private bool delayWrite;
        private string baseSettingsFile = Path.Combine(ProcessExecutable.ExecutionFolder(), "BaseSettings.xml");
        private XDocument baseSettingsDoc;
        private string applicationFolder;
        private string settingsFile;
        private XDocument settingsDoc;

        private SettingsClient()
        {
            this.timer = new System.Timers.Timer(100); // a 10th of a second
            this.timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);

            // Check in BaseSettings.xml if we're using the portable application
            this.baseSettingsDoc = XDocument.Load(this.baseSettingsFile);

            if (this.applicationFolder == null)
            {
                bool isPortable = false;

                // Set the path of Settings.xml
                if (this.TryGetBaseValue<bool>("Configuration", "IsPortable", ref isPortable))
                {
                    // Sets the application folder
                    if (isPortable)
                    {
                        this.applicationFolder = System.IO.Path.Combine(ProcessExecutable.ExecutionFolder(), ProcessExecutable.Name());
                    }
                    else
                    {
                        this.applicationFolder = System.IO.Path.Combine(WindowsPaths.AppData(), ProcessExecutable.Name());
                    }
                }
                else
                {
                    // By default, we save in the user's Roaming folder
                    this.applicationFolder = System.IO.Path.Combine(WindowsPaths.AppData(), ProcessExecutable.Name());
                }

                this.TryCreateApplicationFolder();
            }

            this.settingsFile = Path.Combine(this.applicationFolder, "Settings.xml");

            // Make sure there is a settings file.
            if (!File.Exists(this.settingsFile))
            {
                File.Copy(this.baseSettingsFile, this.settingsFile, true);
            }

            // Load the settings in memory
            this.LoadSettings();
        }

        public static bool IsMigrationNeeded()
        {
            return SettingsClient.Instance.CheckSettingsVersion() != 0;
        }

        public static void Migrate()
        {
            if (SettingsClient.Instance.CheckSettingsVersion() == 0)
            {
                // Settings are up to date: do nothing.
            }
            else if (SettingsClient.Instance.CheckSettingsVersion() == 1)
            {
                // Upgrade settings
                SettingsClient.Instance.UpgradeSettings();
            }
            else if (SettingsClient.Instance.CheckSettingsVersion() == -1)
            {
                // Downgrade/reset settings
                SettingsClient.Instance.DowngradeSettings();
            }
        }

        private void LoadSettings()
        {
            try
            {
                // Load the settings file in memory
                this.settingsDoc = XDocument.Load(this.settingsFile);
            }
            catch (Exception)
            {
                // After a crash, the settings file is sometimes empty.  If that
                // happens, copy the BaseSettings file (there is no way to restore
                // settings from a broken file anyway) and try to load the Settings
                // file again. 
                File.Copy(this.baseSettingsFile, this.settingsFile, true);
                this.settingsDoc = XDocument.Load(this.settingsFile);
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (this.timerMutex)
            {
                if (this.delayWrite)
                {
                    this.delayWrite = false;
                }
                else
                {
                    this.timer.Stop();
                    this.settingsDoc.Save(this.settingsFile);
                }
            }
        }

        public static SettingsClient Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SettingsClient();
                }
                return instance;
            }
        }

        private bool SettingExists<T>(string settingNamespace, string settingName)
        {
            T value = this.GetValue<T>(settingNamespace, settingName);

            return value != null;
        }

        private void TryCreateApplicationFolder()
        {
            if (!Directory.Exists(this.applicationFolder))
            {
                Directory.CreateDirectory(this.applicationFolder);
            }
        }

        // Queue XML file writes to minimize disk access
        private void QueueWrite()
        {
            lock (this.timerMutex)
            {
                if (!this.timer.Enabled)
                {
                    this.timer.Start();
                }
                else
                {
                    this.delayWrite = true;
                }
            }
        }

        private void WriteToFile()
        {
            this.timer.Stop();
            this.delayWrite = false;
            this.settingsDoc.Save(this.settingsFile);
        }

        private int CheckSettingsVersion()
        {
            this.LoadSettings();

            // Try to get the current settings version
            int currentVersion = 0;
            int baseVersion = this.GetBaseValue<int>("Configuration", "Version");

            try
            {
                currentVersion = this.GetValue<int>("Configuration", "Version");
            }
            catch (Exception)
            {
            }

            // Compare versions
            if (currentVersion == baseVersion)
            {
                return 0; // Do nothing
            }
            else if (currentVersion < baseVersion)
            {
                return 1; // Upgrade
            }

            return -1; // Downgrade/Reset
        }

        private void DowngradeSettings()
        {
            File.Copy(this.baseSettingsFile, this.settingsFile, true);

            // Make sure the settings are up to date in memory
            this.LoadSettings();
        }

        private void UpgradeSettings()
        {
            // Get the old settings
            List<SettingEntry> oldSettings = default(List<SettingEntry>);

            oldSettings = (from n in this.settingsDoc.Element("Settings").Elements("Namespace")
                           from s in n.Elements("Setting")
                           from v in s.Elements("Value")
                           select new SettingEntry
                           {
                               Namespace = n.Attribute("Name").Value,
                               Name = s.Attribute("Name").Value,
                               Value = v.Value
                           }).ToList();

            // Create a new Settings file, based on the new BaseSettings file
            File.Copy(this.baseSettingsFile, this.settingsFile, true);

            // Load the new Settings file in memory
            this.settingsDoc = XDocument.Load(this.settingsFile);

            // Try to write the old settings in the new Settings file
            foreach (SettingEntry item in oldSettings)
            {
                try
                {
                    // Don't migrate configuration settings
                    if (item.Namespace != "Configuration" && SettingExists<string>(item.Namespace, item.Name))
                    {
                        // We don't know the type of the setting. So set all settings as String
                        Set<string>(item.Namespace, item.Name, item.Value.ToString());
                    }
                }
                catch (Exception)
                {
                    // If we fail, we do nothing.
                }
            }
        }

        private T GetBaseValue<T>(string settingNamespace, string settingName)
        {

            lock (this.baseSettingsDoc)
            {
                XElement baseSetting = (from n in this.baseSettingsDoc.Element("Settings").Elements("Namespace")
                                        from s in n.Elements("Setting")
                                        from v in s.Elements("Value")
                                        where n.Attribute("Name").Value.Equals(settingNamespace) && s.Attribute("Name").Value.Equals(settingName)
                                        select v).FirstOrDefault();

                // For numbers, we need to provide CultureInfo.InvariantCulture. 
                // Otherwise, deserializing from XML can cause a FormatException.
                if (typeof(T) == typeof(float))
                {
                    float floatValue;
                    float.TryParse(baseSetting.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out floatValue);
                    return (T)Convert.ChangeType(floatValue, typeof(T));
                }
                else if (typeof(T) == typeof(double))
                {
                    float doubleValue;
                    float.TryParse(baseSetting.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out doubleValue);
                    return (T)Convert.ChangeType(doubleValue, typeof(T));
                }
                else
                {
                    return (T)Convert.ChangeType(baseSetting.Value, typeof(T));
                }
            }
        }

        private bool TryGetBaseValue<T>(string settingNamespace, string settingName, ref T value)
        {
            value = this.GetBaseValue<T>(settingNamespace, settingName);

            return value != null;
        }

        private void SetValue<T>(string settingNamespace, string settingName, T value)
        {
            lock (this.settingsDoc)
            {
                XElement setting = (from n in this.settingsDoc.Element("Settings").Elements("Namespace")
                                    from s in n.Elements("Setting")
                                    from v in s.Elements("Value")
                                    where n.Attribute("Name").Value.Equals(settingNamespace) && s.Attribute("Name").Value.Equals(settingName)
                                    select v).FirstOrDefault();

                if (setting != null)
                {
                    setting.SetValue(value);
                }

                this.QueueWrite();
            }
        }

        private T GetValue<T>(string settingNamespace, string settingName)
        {
            lock (this.settingsDoc)
            {
                XElement setting = (from n in this.settingsDoc.Element("Settings").Elements("Namespace")
                                    from s in n.Elements("Setting")
                                    from v in s.Elements("Value")
                                    where n.Attribute("Name").Value.Equals(settingNamespace) && s.Attribute("Name").Value.Equals(settingName)
                                    select v).FirstOrDefault();

                // For numbers, we need to provide CultureInfo.InvariantCulture. 
                // Otherwise, deserializing from XML can cause a FormatException.
                if (typeof(T) == typeof(float))
                {
                    float floatValue;
                    float.TryParse(setting.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out floatValue);
                    return (T)Convert.ChangeType(floatValue, typeof(T));
                }
                else if (typeof(T) == typeof(double))
                {
                    float doubleValue;
                    float.TryParse(setting.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out doubleValue);
                    return (T)Convert.ChangeType(doubleValue, typeof(T));
                }
                else
                {
                    return (T)Convert.ChangeType(setting.Value, typeof(T));
                }
            }
        }

        private void OnTimerElapsedEvent(object sender, ElapsedEventArgs e)
        {
            lock (this.timerMutex)
            {
                if (this.delayWrite)
                {
                    this.delayWrite = false;
                }
                else
                {
                    this.timer.Stop();
                    this.settingsDoc.Save(this.settingsFile);
                }
            }
        }

        public static string ApplicationFolder()
        {
            return SettingsClient.Instance.applicationFolder;
        }

        public static void Write()
        {
            SettingsClient.Instance.WriteToFile();
        }

        public static void Set<T>(string @namespace, string name, T value, bool raiseEvent = false)
        {
            SettingsClient.Instance.SetValue<T>(@namespace, name, value);

            if (raiseEvent)
            {
                SettingChanged(SettingsClient.Instance, new SettingChangedEventArgs() { Entry = new SettingEntry { Namespace = @namespace, Name = name, Value = value } });
            }
        }

        public static T Get<T>(string settingNamespace, string settingName)
        {
            return SettingsClient.Instance.GetValue<T>(settingNamespace, settingName);
        }

        public static T BaseGet<T>(string settingNamespace, string settingName)
        {
            return SettingsClient.Instance.GetBaseValue<T>(settingNamespace, settingName);
        }

        public static bool IsSettingChanged(SettingChangedEventArgs e, string @namespace, string name)
        {
            if (e.Entry == null)
            {
                return false;
            }

            return e.Entry.Namespace.Equals(@namespace) && e.Entry.Name.Equals(name);
        }

        public static event SettingChangedEventHandler SettingChanged = delegate { };
    }
}
