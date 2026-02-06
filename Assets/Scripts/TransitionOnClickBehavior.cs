using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionOnClickBehavior : MonoBehaviour
{
    public string sceneName;

    private void OnMouseDown()
    {
        SceneManager.LoadScene(sceneName);
    }
}
