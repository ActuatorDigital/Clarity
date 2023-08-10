using UnityEditor;
using UnityEditor.SettingsManagement;

namespace Actuator
{
    internal class ClarityEditorSettings
    {
        internal const string SETTINGS_KEY = "com.actuator.clarity.editor.settings";

        private static Settings _instance;

        internal static Settings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Settings(SETTINGS_KEY);

                return _instance;
            }
        }

        internal static class ClarityEditorSettingsProvider
        {
            private const string k_PreferencesPath = "Preferences/Clarity";

            [SettingsProvider]
            private static SettingsProvider CreateSettingsProvider()
            {
                var provider = new UserSettingsProvider(k_PreferencesPath,
                    Instance,
                    new[] { typeof(ClarityEditorSettingsProvider).Assembly });

                return provider;
            }
        }
    }
}