using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class SearchableEditorWindowUtil
{
    public static void SetSearchFilter(string filter)
    {
        SearchableEditorWindow[] windows = (SearchableEditorWindow[])Resources.FindObjectsOfTypeAll(typeof(SearchableEditorWindow));
        var window = windows.First();

        MethodInfo setSearchType = typeof(SearchableEditorWindow).GetMethod("SetSearchFilter", BindingFlags.NonPublic | BindingFlags.Instance);
        object[] parameters = new object[] { filter, 0, true, true };
        setSearchType.Invoke(window, parameters);
    }
}
