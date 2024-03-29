﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadMainMenu() {
        SceneManager.LoadScene("Start Scene");
    }
    public void LoadScene(string name) {
        SceneManager.LoadScene(name);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
