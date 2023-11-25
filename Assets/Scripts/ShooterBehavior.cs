using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShooterBehavior : MonoBehaviour
{
    [Header("Enemy Movement & Rotation")]
    private Transform playerPos;
    [SerializeField] private float currentEnemySpeed;
    private float enemySpeed;
    [SerializeField] float rotationModifier;

    [Header("Enemy Fire Rate")]
    [SerializeField] private float enemyFireRate;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float distanceToStartShot;
    private bool canShot = false;

    [Header("Enemy Sprites")]
    [SerializeField] private Sprite[] enemySprites;
    [SerializeField] private SpriteRenderer enemyCurrentSprite;

    [Header("Enemy Health")]
    [SerializeField] public int currentEnemyHealth;
    private int maxEnemyHealth;
    [SerializeField] private Slider enemyHeathUI;

    [SerializeField] private GameObject explosionPrefab;

    [SerializeField] private LayerMask ObstacleLayer;

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
        enemySpeed = currentEnemySpeed;
        maxEnemyHealth = currentEnemyHealth;
        enemyHeathUI.maxValue = maxEnemyHealth;
    }

    void Start()
    {
        StartCoroutine(Shots());
    }

    private void FixedUpdate()
    {
            Vector3 playerPosDif = playerPos.transform.position - transform.position;
            float angle = Mathf.Atan2(playerPosDif.y, playerPosDif.x) * Mathf.Rad2Deg - rotationModifier;
            Quaternion rotateTowards = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateTowards, Time.deltaTime * currentEnemySpeed);
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, playerPos.position) >= distanceToStartShot)
        {
            canShot = false;
            currentEnemySpeed = enemySpeed;
            transform.position = Vector2.MoveTowards(transform.position, playerPos.transform.position, currentEnemySpeed * Time.deltaTime);
        }
            

        else
        {
            currentEnemySpeed = 0;
            canShot = true;
        }

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

    public void EnemyGetHit(int damageTaken)
    {
        currentEnemyHealth -= damageTaken;
    }

    IEnumerator Shots()
    {
        yield return new WaitForSeconds(enemyFireRate);

        if (canShot)
        {    
            Instantiate(bulletPrefab, bulletSpawnPoint);
        }
        StartCoroutine(Shots());
    }
}
