using UnityEditor;
using UnityEngine;
using System.Linq;

public class GroupByShaderEditorWindow : EditorWindow
{
    private string _skipNamePartial = string.Empty;
    private Vector2 _scrollPos;

    [MenuItem("Window/Clarity/Group By Shaders")]
    public static void ShowWindow()
    {
        var window = GetWindow<GroupByShaderEditorWindow>();
        window.titleContent = new GUIContent("Group By Shader");
        window.Show();
    }

    public void OnGUI()
    {
        GUILayout.Label("Hide partial matches:", EditorStyles.boldLabel);
        _skipNamePartial = GUILayout.TextArea(_skipNamePartial);

        _scrollPos = GUILayout.BeginScrollView(_scrollPos);
        var allRenderables = FindObjectsOfType<Renderer>();
        GUILayout.Label($"Found {allRenderables.Length} renderables");

        var grouped = allRenderables.GroupBy(x =>
        {
            if(x != null
            && x.sharedMaterial != null)
                return x.sharedMaterial.shader;

            return null;
        });
        
        GUILayout.Label($"Shaders in use:");
        foreach (var group in grouped)
        {
            var name = group.Key == null ? "None" : group.Key.name;

            if (!string.IsNullOrEmpty(_skipNamePartial)
                && name.Contains(_skipNamePartial, System.StringComparison.InvariantCultureIgnoreCase))
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField(group.Key, typeof(Shader), true);
                EditorGUI.EndDisabledGroup();
                continue;
            }

            EditorGUILayout.ObjectField(group.Key, typeof(Shader), true);
            EditorGUI.indentLevel++;
            foreach (var rend in group)
            {
                EditorGUILayout.ObjectField(rend, typeof(Renderer), true);
            }
            EditorGUI.indentLevel--;
        }
        GUILayout.EndScrollView();
    }
}
