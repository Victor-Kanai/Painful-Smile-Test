using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerBehavior : MonoBehaviour
{
    private float startTime;
    [SerializeField] private TextMeshProUGUI timerTxt;
    private float remainingTime;
    [SerializeField] private GameObject endGameCanvas;
    [SerializeField] private TextMeshProUGUI scoreTxt;

    private void Awake()
    {
        remainingTime = PlayerPrefs.GetFloat("SessionTimer");
        startTime = remainingTime;
    }

    void Update()
    {
        remainingTime -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        timerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (remainingTime <= 0)
        {
            Time.timeScale = 0f;
            scoreTxt.text = "You Defeated " + GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>().score + " Pirates and survived " + startTime + " Minutes!";
            endGameCanvas.SetActive(true);
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
