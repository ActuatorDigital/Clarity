using System.Linq;
using UnityEditor;

public static class ClarityMenuItems
{
    [MenuItem("Edit/Clarity/Find All Lights")]
    public static void FindAllLights()
    {
        SearchableEditorWindowUtil.SetSearchFilter("t:light");
    }

    [MenuItem("Edit/Clarity/Open Build Scenes")]
    public static void OpenBuildScenes()
    {
        var scenes = EditorBuildSettings.scenes;
        scenes = scenes.Where(x => x.guid != default).ToArray();
        if (scenes.Length == 0)
        {
            return;
        }
        var zeroScene = scenes[0];
        var allOtherScenes = scenes.Skip(1).OrderBy(x => x.path);
        var allScenes = Enumerable.Concat(new[] { zeroScene }, allOtherScenes).ToArray();

        var allScenesInDB = allScenes.Select(x => new UnityEditor.SceneManagement.SceneSetup { path = x.path }).ToArray();
        allScenesInDB[0].isLoaded = true;
        allScenesInDB[0].isActive = true;
        UnityEditor.SceneManagement.EditorSceneManager.RestoreSceneManagerSetup(allScenesInDB);
    }
}