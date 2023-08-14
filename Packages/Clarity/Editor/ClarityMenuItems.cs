using UnityEditor;

public static class ClarityMenuItems
{
    [MenuItem("Edit/Clarity/Find All Lights")]
    public static void FindAllLights()
    {
        SearchableEditorWindowUtil.SetSearchFilter("t:light");
    }
}