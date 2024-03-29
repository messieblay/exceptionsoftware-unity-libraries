﻿using System.Collections;
using UnityEditor;

public class EditorCoroutine
{
    public static EditorCoroutine Start(IEnumerator _routine)
    {
        EditorCoroutine coroutine = new EditorCoroutine(_routine);
        coroutine.Start();
        return coroutine;
    }

    readonly IEnumerator routine;
    EditorCoroutine(IEnumerator _routine)
    {
        routine = _routine;
    }

    void Start()
    {
        EditorApplication.update += Update;
    }
    public void Stop()
    {
        EditorApplication.update -= Update;
        SceneView.RepaintAll();
    }

    void Update()
    {
        /* NOTE: no need to try/catch MoveNext,
         * if an IEnumerator throws its next iteration returns false.
         * Also, Unity probably catches when calling EditorApplication.update.
         */

        //Debug.Log("update");
        if (!routine.MoveNext())
        {
            Stop();
        }
    }
}
