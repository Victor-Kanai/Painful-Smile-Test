using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChaserBehavior : MonoBehaviour
{
    [Header("Enemy Movement & Rotation")]
    private Transform playerPos;
    [SerializeField] private float enemySpeed;
    [SerializeField] float rotationModifier;

    [Header("Enemy Sprites")]
    [SerializeField] private Sprite[] enemySprites;
    [SerializeField] private SpriteRenderer enemyCurrentSprite;

    [Header("Enemy Health")]
    [SerializeField] public int currentEnemyHealth;
    private int maxEnemyHealth;
    [SerializeField] private Slider enemyHeathUI;

    [SerializeField] private int damgeDealt;
    [SerializeField] private LayerMask ObstacleLayer;
    [SerializeField] private GameObject explosionPrefab;

    private void Awake()
    {
        RaycastHit2D hitLandUp = Physics2D.Raycast(transform.position, Vector2.up, 10, ObstacleLayer);
        RaycastHit2D hitLandDown = Physics2D.Raycast(transform.position, Vector2.down, 10, ObstacleLayer);
        RaycastHit2D hitLandLeft = Physics2D.Raycast(transform.position, Vector2.left, 10, ObstacleLayer);
        RaycastHit2D hitLandRight = Physics2D.Raycast(transform.position, Vector2.right, 10, ObstacleLayer);

        if (hitLandUp && hitLandDown && hitLandLeft && hitLandRight)
        {
            Destroy(gameObject);
        }

        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        maxEnemyHealth = currentEnemyHealth;
        enemyHeathUI.maxValue = maxEnemyHealth;
    }

    private void FixedUpdate()
    {
            Vector3 PlayerDifPos = playerPos.transform.position - transform.position;
            float angle = Mathf.Atan2(PlayerDifPos.y, PlayerDifPos.x) * Mathf.Rad2Deg - rotationModifier;
            Quaternion rotateTowards = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateTowards, Time.deltaTime * enemySpeed);
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerPos.transform.position, enemySpeed * Time.deltaTime);

        ChangeHealthUI();

        if (currentEnemyHealth <= 0)
        {
            enemyCurrentSprite.sprite = enemySprites[3];
            enemySpeed = 0;
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject, .5f);
        }

        else if (currentEnemyHealth <= maxEnemyHealth / 2f)
            enemyCurrentSprite.sprite = enemySprites[2];

        else if (currentEnemyHealth <= maxEnemyHealth / 1.25f)
            enemyCurrentSprite.sprite = enemySprites[1];

        else
            enemyCurrentSprite.sprite = enemySprites[0];
    }

    private void ChangeHealthUI()
    {
        enemyHeathUI.value = currentEnemyHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerBehavior>().GetHit(damgeDealt);
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            gameObject.GetComponent<ChaserBehavior>().currentEnemyHealth = 0;
            gameObject.GetComponent<ChaserBehavior>().enemySpeed = 0;
            Destroy(gameObject, .5f);
        }
    }

    public void EnemyGetHit(int damageTaken)
    {
        currentEnemyHealth -= damageTaken;
    }
}
