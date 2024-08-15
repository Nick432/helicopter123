using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] SceneAsset scene;

    public void LoadNewScene()
    {
        SceneManager.LoadScene(scene.name);
    }
}
