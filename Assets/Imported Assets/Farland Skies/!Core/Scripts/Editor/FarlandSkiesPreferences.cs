using System;
using UnityEditor;
using UnityEngine;

namespace Borodar.FarlandSkies.Core.Editor
{
    public class FarlandSkiesPreferences
    {
        private const string HOME_FOLDER_PREF_KEY = "Borodar.FarlandSkies.HomeFolder.";
        private const string HOME_FOLDER_DEFAULT = "Assets/Farland Skies";
        private const string HOME_FOLDER_HINT = "Change this setting to the new location of the \"Farland Skies\" folder if you move it around in your project.";

        public static EditorPrefsString HomeFolder = new EditorPrefsString(HOME_FOLDER_PREF_KEY + ProjectName, "Folder Location", HOME_FOLDER_DEFAULT);

        //---------------------------------------------------------------------
        // Messages
        //---------------------------------------------------------------------

        [PreferenceItem("Farland Skies")]
        public static void EditorPreferences()
        {
            EditorGUILayout.HelpBox(HOME_FOLDER_HINT, MessageType.Info);
            EditorGUILayout.Separator();
            HomeFolder.Draw();
        }

        //---------------------------------------------------------------------
        // Helpers
        //---------------------------------------------------------------------

        private static string ProjectName
        {
            get
            {
                var s = Application.dataPath.Split('/');
                var p = s[s.Length - 2];
                return p;
            }
        }

        //---------------------------------------------------------------------
        // Nested
        //---------------------------------------------------------------------

        public abstract class EditorPrefsItem<T>
        {
            public string Key;
            public string Label;
            public T DefaultValue;

            protected EditorPrefsItem(string key, string label, T defaultValue)
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException("key");
                }

                Key = key;
                Label = label;
                DefaultValue = defaultValue;
            }

            public abstract T Value { get; set; }
            public abstract void Draw();

            public static implicit operator T(EditorPrefsItem<T> s)
            {
                return s.Value;
            }
        }

        public class EditorPrefsString : EditorPrefsItem<string>
        {
            public EditorPrefsString(string key, string label, string defaultValue)
                : base(key, label, defaultValue)
            {
            }

            public override string Value
            {
                get { return EditorPrefs.GetString(Key, DefaultValue); }
                set { EditorPrefs.SetString(Key, value); }
            }

            public override void Draw()
            {
                EditorGUIUtility.labelWidth = 100f;
                Value = EditorGUILayout.TextField(Label, Value);
            }
        }
    }
}