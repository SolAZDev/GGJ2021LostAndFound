using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private void Start()
    {
        if (instance == null) { instance = this; }
    }

    public async void LoadScene(string name)
    {
        InGameHUD.instance?.LoadingPanel.SetActive(true);
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }

    public async void ReloadScene()
    {
        InGameHUD.instance?.LoadingPanel.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public async void AddScene(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Additive);
    }

    public void SlowPlayerDown(bool slow = false)
    {
        Player.instance.Speed = slow ? 1 : 2.5f;
    }
}
