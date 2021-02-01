using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InGameHUD : MonoBehaviour
{

    public static InGameHUD instance { get; private set; }
    public Image[] ItemSlots, ItemIcons;
    public Text TimerText;
    public GameObject PausePanel, RipPanel, LoadingPanel;
    int SelectedItem = 0;
    public bool paused, lose = false;
    public float TimeLimit = 600;
    // Start is called before the first frame update
    void Start()
    {
        if (InGameHUD.instance == null) { InGameHUD.instance = this; }
        StartCoroutine(Countdown());
    }

    void Update()
    {
        TimeLimit -= Time.deltaTime;
    }

    public void Pause(bool GameOver = false)
    {
        if (lose) { return; }
        lose = GameOver;
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        PausePanel?.SetActive(paused && !GameOver);
        RipPanel?.SetActive(paused && GameOver);
    }

    public void Restart()
    {
        LoadingPanel?.SetActive(true);
        GameManager.instance.ReloadScene();
    }

    public void Resume()
    {
        if(lose) { return; }
        paused = false;
        PausePanel?.SetActive(false);
        RipPanel?.SetActive(false);
        Time.timeScale = 1;
    }

    public void BackToMenu() { GameManager.instance?.LoadScene("MainMenu"); }

    public void CycleItems(int i)
    {
        if (Player.instance.Inventory.Count < 1) { return; }
        SelectedItem += i;
        if (SelectedItem < 0) { SelectedItem = Player.instance.Inventory.Count - 1; }
        if (SelectedItem < 0) { SelectedItem = 0; }
    }

    IEnumerator Countdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            TimerText.text = Utils.ConvertSecondsToHHMMSS(TimeLimit);
            if (TimeLimit < 0) { Pause(true); }
        }
    }
}
