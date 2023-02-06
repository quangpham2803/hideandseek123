using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoggingBehaviour : MonoBehaviour
{
    private int sceneIndex;
    private string sceneName; 
    void Start()
    {
        var activeScene = SceneManager.GetActiveScene();
        sceneIndex = activeScene.buildIndex;
        sceneName = activeScene.name;
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, 
            new Parameter(FirebaseAnalytics.ParameterLevel, sceneIndex), 
            new Parameter(FirebaseAnalytics.ParameterLevelName, sceneName));
    }

    private void OnDestroy()
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd,
            new Parameter(FirebaseAnalytics.ParameterLevel, sceneIndex),
            new Parameter(FirebaseAnalytics.ParameterLevelName, sceneName));
    }
}
