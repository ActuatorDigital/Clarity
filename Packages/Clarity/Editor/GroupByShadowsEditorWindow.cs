using UnityEditor;
using UnityEngine;
using System.Linq;

public class GroupByShadowsEditorWindow : EditorWindow
{
    private Vector2 _scrollPos;

    [MenuItem("Window/Clarity/Group By Shadows")]
    public static void ShowWindow()
    {
        var window = GetWindow<GroupByShadowsEditorWindow>();
        window.titleContent = new GUIContent("Group By Shadows");
        window.Show();
    }

    public void OnGUI()
    {
        _scrollPos = GUILayout.BeginScrollView(_scrollPos);
        var allRenderables = FindObjectsOfType<Renderer>();
        GUILayout.Label($"Found {allRenderables.Length} renderables");

        var grouped = allRenderables.GroupBy(x =>
        {
            return (x.shadowCastingMode, x.receiveShadows);
        });

        GUILayout.Label($"Grouped Shadow Mode, Recv Shadows:");
        foreach (var group in grouped)
        {
            var name = group.Key.ToString();

            EditorGUILayout.LabelField(name);
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
