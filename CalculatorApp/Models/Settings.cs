using System;
using System.IO;
using System.Xml.Serialization;

namespace CalculatorApp.Models
{
    [Serializable]
    public static class Settings
    {
        public static bool DigitGroupingEnabled { get; set; } = true;
        public static string CalculatorMode { get; set; } = "Standard";
        public static string NumberBase { get; set; } = "DEC";
        public static bool PrecedenceEnabled { get; set; } = false;
        public static string CurrentTheme { get; set; } = "Light";

        [Serializable]
        private class SerializableSettings
        {
            public bool DigitGroupingEnabled { get; set; }
            public string CalculatorMode { get; set; }
            public string NumberBase { get; set; }
            public bool PrecedenceEnabled { get; set; }
            public string CurrentTheme { get; set; }
        }

        private const string SETTINGS_FILE = "calculator_settings.xml";

        public static void Load()
        {
            try
            {
                if (File.Exists(SETTINGS_FILE))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SerializableSettings));
                    using (FileStream stream = File.OpenRead(SETTINGS_FILE))
                    {
                        SerializableSettings settings = (SerializableSettings)serializer.Deserialize(stream);
                        DigitGroupingEnabled = settings.DigitGroupingEnabled;
                        CalculatorMode = settings.CalculatorMode;
                        NumberBase = settings.NumberBase;
                        PrecedenceEnabled = settings.PrecedenceEnabled;
                        CurrentTheme = settings.CurrentTheme;
                    }
                }
            }
            catch
            {
                ResetToDefaults();
            }
        }

        public static void Save()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SerializableSettings));
                using (FileStream stream = File.Create(SETTINGS_FILE))
                {
                    SerializableSettings settings = new SerializableSettings
                    {
                        DigitGroupingEnabled = DigitGroupingEnabled,
                        CalculatorMode = CalculatorMode,
                        NumberBase = NumberBase,
                        PrecedenceEnabled = PrecedenceEnabled,
                        CurrentTheme = CurrentTheme
                    };
                    serializer.Serialize(stream, settings);
                }
            }
            catch
            {
               
            }
        }

        public static void ResetToDefaults()
        {
            DigitGroupingEnabled = true;
            CalculatorMode = "Standard";
            NumberBase = "DEC";
            PrecedenceEnabled = false;
            CurrentTheme = "Light";
        }
    }
}