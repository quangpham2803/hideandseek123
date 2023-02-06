using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UnityEditor.Presets;

public class SceneEditor : ScriptableObject {
    [MenuItem("SceneShorcut/Start Game", false, 100)]
    static public void StartGame() {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
            EditorSceneManager.OpenScene("Assets/Scenes/Menu/Menu.unity");
            EditorApplication.ExecuteMenuItem("Edit/Play");
        }
    }
    [MenuItem("SceneShorcut/Menu scene", false, 3)]
    static public void InitScene() {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
            EditorSceneManager.OpenScene("Assets/Scenes/Menu/Menu.unity");
        }
    }
    [MenuItem("SceneShorcut/Game play(MapDark) scene", false, 3)]
    static public void LoginScene() {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
            EditorSceneManager.OpenScene("Assets/Scenes/Menu/MapDark.unity");
        }
    }
    [MenuItem("SceneShorcut/Clear Cached", false, 2)]
    static public void ClearCached() {
        PlayerPrefs.DeleteAll();
    }

    //[MenuItem("SceneShorcut/Apply Preset", false, 2)]
    //static public void ApplyPreset() {

    //}
}


