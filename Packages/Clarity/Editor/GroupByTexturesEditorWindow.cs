using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GroupByTexturesEditorWindow : EditorWindow
{
    private Vector2 _scrollPos;

    [MenuItem("Window/Clarity/Group By Textures")]
    public static void ShowWindow()
    {
        var window = GetWindow<GroupByTexturesEditorWindow>();
        window.titleContent = new GUIContent("Group By Textures");
        window.Show();
    }

    public void OnGUI()
    {
        _scrollPos = GUILayout.BeginScrollView(_scrollPos);
        var allRenderables = FindObjectsOfType<Renderer>();
        GUILayout.Label($"Found {allRenderables.Length} renderables");

        var allMats = allRenderables.SelectMany(x => x.sharedMaterials);
        var allMatTextures = allMats
            .SelectMany(mat => GetTexturesFromMat(mat).Select(texture => (mat, texture)))
            .Where(x => x.texture != null && x.mat != null)
            .Distinct()
            .OrderByDescending(x => x.texture.height * x.texture.width);

        var grouped = allMatTextures
            .GroupBy(x =>
            {
                return (x.texture.graphicsFormat, x.texture.height, x.texture.width);
            });

        GUILayout.Label($"Grouped Texture Height, Texture Width:");
        foreach (var group in grouped)
        {
            var name = group.Key.ToString();

            EditorGUILayout.LabelField(name);
            EditorGUI.indentLevel++;
            foreach (var item in group)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField(item.texture, typeof(Texture), true);
                EditorGUILayout.ObjectField(item.mat, typeof(Material), true);
                if (GUILayout.Button("Isolate"))
                {
                    SearchableEditorWindowUtil.SetSearchFilter($"ref:{AssetDatabase.GetAssetPath(item.texture)}");
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;
        }
        GUILayout.EndScrollView();
    }

    private static IEnumerable<Texture> GetTexturesFromMat(Material mat)
    {
        if(mat == null)
        {
            yield break;
        }

        var shader = mat.shader;
        if(shader == null)
        {
            yield break;
        }

        for (int i = 0; i < ShaderUtil.GetPropertyCount(shader); i++)
        {
            if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
            {
                Texture texture = mat.GetTexture(ShaderUtil.GetPropertyName(shader, i));
                yield return texture;
            }
        }
    }
}