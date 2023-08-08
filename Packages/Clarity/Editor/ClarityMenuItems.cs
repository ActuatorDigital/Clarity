using UnityEditor;
using UnityEngine;
using System.Reflection;

public static class ClarityMenuItems
{
    [MenuItem("Edit/Clarity/Find All Lights")]
    public static void FindAllLights()
    {
        SetSearchFilter("t:light");
    }

    private static void SetSearchFilter(string filter)
    {
        SearchableEditorWindow[] windows = (SearchableEditorWindow[])Resources.FindObjectsOfTypeAll(typeof(SearchableEditorWindow));
        SearchableEditorWindow hierarchy = null;

        foreach (SearchableEditorWindow window in windows)
        {

            if (window.GetType().ToString() == "UnityEditor.SceneHierarchyWindow")
            {

                hierarchy = window;
                break;
            }
        }

        if (hierarchy == null)
            return;

        MethodInfo setSearchType = typeof(SearchableEditorWindow).GetMethod("SetSearchFilter", BindingFlags.NonPublic | BindingFlags.Instance);
        object[] parameters = new object[] { filter, 0, false, false };

        setSearchType.Invoke(hierarchy, parameters);
    }
}
