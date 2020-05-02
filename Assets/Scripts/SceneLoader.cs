using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }
    public void LoadScene(string name) {
        SceneManager.LoadScene(name);
    }
}
