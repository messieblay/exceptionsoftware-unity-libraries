using System;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class SceneUtility
{

    public delegate void ProcessAllScenesDelegate(string name);

    public static void ProcessAllScenes(ProcessAllScenesDelegate _callback)
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            return;
        }

        EditorCoroutine.Start(ProcessAllScenesCoroutine(_callback));
    }

    static IEnumerator ProcessAllScenesCoroutine(ProcessAllScenesDelegate callback)
    {
        var sceneCount = EditorSceneManager.sceneCountInBuildSettings;

        Debug.Log(string.Format("Processing {0} scenes", sceneCount));

        var paths = EditorBuildSettings.scenes.Select(s => s.path).ToList();

        for (int i = 0; i < paths.Count; i++)
        {
            EditorSceneManager.OpenScene(paths[i], OpenSceneMode.Single);
            string sceneName = EditorSceneManager.GetActiveScene().name;
            EditorUtility.DisplayProgressBar("Procesnado escenas", $"Procesando {sceneName} ", (float)i / sceneCount);


            try
            {
                callback(sceneName);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while processing scene  '{sceneName}'");
                Debug.LogException(e);
            }
            yield return null;
        }

        EditorUtility.ClearProgressBar();
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
    }


    //[MenuItem("Swing/Scenes/Test Open All Scenes")]
    static void openAllScenes()
    {
        ProcessAllScenes(
            (name) =>
            {
                Debug.Log($"Scene '{name}' open successfully");
            });
    }


}
