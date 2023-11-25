using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBehavior : MonoBehaviour
{
    [SerializeField] private Slider gameSessionSlider;
    [SerializeField] private Slider spawnRateSlider;
    [SerializeField] private TextMeshProUGUI gameSessionTxt;
    [SerializeField] private TextMeshProUGUI spawnRateTxt;

    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject optionsPanel;

    private void Awake()
    {
        spawnRateSlider.value = PlayerPrefs.GetFloat("SpawnRate");
        gameSessionSlider.value = PlayerPrefs.GetFloat("SessionTimer");
    }

    void Start()
    {
        optionsPanel.SetActive(false);

        gameSessionSlider.onValueChanged.AddListener(delegate { SaveChangesSessionTimer(); });

        spawnRateSlider.onValueChanged.AddListener(delegate { SaveChangesSpawnRateTimer(); });
    }

    void Update()
    {
        gameSessionTxt.text = gameSessionSlider.value.ToString();
        spawnRateTxt.text = spawnRateSlider.value.ToString();

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            PlayerPrefs.DeleteKey("SpawnRateTimer");
            PlayerPrefs.DeleteKey("SpawnRate");
            PlayerPrefs.DeleteKey("GameSession");
            PlayerPrefs.DeleteKey("SessionTimer");
        }
    }

    private void SaveChangesSpawnRateTimer()
    {
        if (!PlayerPrefs.HasKey("SpawnRateTimer"))
        {
            PlayerPrefs.SetString("SpawnRateTimer", "");
            PlayerPrefs.SetFloat("SpawnRate", spawnRateSlider.value);
        }
        else
        {
            PlayerPrefs.SetFloat("SpawnRate", spawnRateSlider.value);
        }
    }

    private void SaveChangesSessionTimer()
    {
        if (!PlayerPrefs.HasKey("GameSession"))
        {
            PlayerPrefs.SetString("GameSession", "");
            PlayerPrefs.SetFloat("SessionTimer", gameSessionSlider.value);
        }
        else
        {
            PlayerPrefs.SetFloat("SessionTimer", gameSessionSlider.value);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Options()
    {
        optionsPanel.SetActive(true);
        mainMenuCanvas.SetActive(false);
        
    }

    public void Return()
    {
        optionsPanel.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }
}
