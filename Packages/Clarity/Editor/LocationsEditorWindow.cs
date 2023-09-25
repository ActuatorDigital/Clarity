using System;
using System.Collections.Generic;
using System.IO;
using Actuator;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SettingsManagement;
using UnityEngine;

public class LocationsEditorWindow : EditorWindow
{
    [System.Serializable]
    public class LocationsSettingsBlob
    {
        [System.Serializable]

        public class LocationSettingEntry
        {
            public string Name = string.Empty;
            public string Path = string.Empty;
        }

        public List<LocationSettingEntry> Paths = new List<LocationSettingEntry>()
        {
            new LocationSettingEntry()
            {
            },
        };
    }

    [UserSetting()]
    private static UserSetting<LocationsSettingsBlob> LocationsSettings = new UserSetting<LocationsSettingsBlob>(ClarityEditorSettings.Instance, $"locations.{nameof(LocationsSettingsBlob)}", new LocationsSettingsBlob(), SettingsScope.Project);

    [UserSettingBlock(nameof(LocationsSettings))]
    static void ConditionalValueGUI(string searchContext)
    {
        EditorGUI.BeginChangeCheck();

        for (int i = 0; i < LocationsSettings.value.Paths.Count; i++)
        {
            var item = LocationsSettings.value.Paths[i];
            EditorGUILayout.BeginHorizontal();
            item.Name = EditorGUILayout.TextField(item.Name);
            item.Path = EditorGUILayout.TextField(item.Path);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                LocationsSettings.value.Paths.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();
        }
        if (GUILayout.Button("Add"))
        {
            LocationsSettings.value.Paths.Add(new LocationsSettingsBlob.LocationSettingEntry());
        }

        SettingsGUILayout.DoResetContextMenuForLastRect(LocationsSettings);

        if (EditorGUI.EndChangeCheck())
        {
            ClarityEditorSettings.Instance.Save();
            LocationsSettings.ApplyModifiedProperties();
        }
    }

    private Vector2 _scrollPos;

    [MenuItem("Window/Clarity/Locations")]
    public static void ShowWindow()
    {
        var window = GetWindow<LocationsEditorWindow>();
        window.titleContent = new GUIContent("Locations");
        window.Show();
    }

    public void OnGUI()
    {
        _scrollPos = GUILayout.BeginScrollView(_scrollPos);
        LocationsSettingsBlob settings = LocationsSettings.value;

        foreach (var item in settings.Paths)
        {
            var path = item.Path;
            if (SpecialStrings.TryGetValue(path, out var specialString))
            {
                path = specialString(path);
            }
            DrawRow(item.Name, path);
        }

        GUILayout.EndScrollView();
    }

    private static void DrawRow(string name, string path)
    {
        GUILayout.BeginHorizontal();
        EditorGUI.BeginDisabledGroup(true);
        GUILayout.Label(name);
        var fullPath = Path.GetFullPath(path);
        var exists = File.Exists(fullPath) || Directory.Exists(fullPath);
        if (exists)
        {
            EditorGUI.EndDisabledGroup();
        }
        if (GUILayout.Button("Open"))
        {
            Application.OpenURL($"file://{fullPath}");
        }
        GUILayout.TextField(fullPath);
        if (!exists)
        {
            EditorGUI.EndDisabledGroup();
        }
        GUILayout.EndHorizontal();
    }

    private static readonly Dictionary<string, Func<string, string>> SpecialStrings = new Dictionary<string, Func<string, string>>()
    {
        {"Application.persistentDataPath", s => Application.persistentDataPath},
        {"TestContext.CurrentContext.TestDirectory", s => TestContext.CurrentContext.TestDirectory},
    };
}