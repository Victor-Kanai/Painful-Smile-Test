using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float playerSpeed;
    [SerializeField] private int playerRotationSpeed;

    [Header("Player Fire Rate")]
    [SerializeField] private float simpleBulletFireRate;
    [SerializeField] private float specialBulletFireRate;

    [Header("Bullet Spawn Points & Bullet")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] bulletPrefabs;

    [Header("Player Health")]
    [SerializeField] private int playerCurrentHealth;
    private int playerMaxHealth;
    [SerializeField] Slider playerSlider;

    [Header("Player Sprites")]
    [SerializeField] private Sprite[] shipSprites;
    [SerializeField] private SpriteRenderer playerShip;

    [SerializeField] public int score;
    [SerializeField] private GameObject explosionPrefab;
    
    [Header("Game Over Canvas")]
    [SerializeField] private GameObject endGameCanvas;
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private TextMeshProUGUI scoreTxt;

    private void Awake()
    {
        Time.timeScale = 1f;
        playerMaxHealth = playerCurrentHealth;
        playerSlider.maxValue = playerMaxHealth;
    }
    void Start()
    {
        StartCoroutine(PlayerSimpleShot());
        StartCoroutine(PlayerSpecialShot());
    }

    void Update()
    {
        GetPlayerMovement();
        GetPlayerRotation();
        VerifyUpdateCurrentHealthAndPlayerSprite();
    }
    private void GetPlayerMovement()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            transform.Translate(new Vector2(0, vertical) * playerSpeed * Time.deltaTime);
    }
    private void GetPlayerRotation()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        transform.Rotate(0, 0, horizontal * playerRotationSpeed * -1 * Time.deltaTime);
    }

    private void VerifyUpdateCurrentHealthAndPlayerSprite()
    {
        playerSlider.value = playerCurrentHealth;

        if(playerCurrentHealth <= 0)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            playerShip.sprite = shipSprites[3];
            GameOver();
        }
            

        else if (playerCurrentHealth <= playerMaxHealth / 2f)
            playerShip.sprite = shipSprites[2];

        else if(playerCurrentHealth <= playerMaxHealth / 1.25f)
            playerShip.sprite = shipSprites[1];

        else
            playerShip.sprite = shipSprites[0];
    }

    public void GetHit(int damageTaken)
    {
        playerCurrentHealth -= damageTaken;
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        playerSpeed = 0;
        playerRotationSpeed = 0;
        titleTxt.text = "Defeat";
        scoreTxt.text = "You did not survived, but you killed: " + score + " pirates! Better luck next time.";
        endGameCanvas.SetActive(true);
    }

    public void KilledShip()
    {
        score++;
    }

    IEnumerator PlayerSimpleShot()
    {
        yield return new WaitForSeconds(simpleBulletFireRate);
        Instantiate(bulletPrefabs[0], spawnPoints[0]);
        StartCoroutine(PlayerSimpleShot());
    }
    IEnumerator PlayerSpecialShot()
    {
        yield return new WaitForSeconds(specialBulletFireRate);
        Instantiate(bulletPrefabs[1], spawnPoints[UnityEngine.Random.Range(1, spawnPoints.Length)]);
        StartCoroutine(PlayerSpecialShot());
    }
}
