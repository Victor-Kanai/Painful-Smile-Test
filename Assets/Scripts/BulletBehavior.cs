using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float timer;
    [SerializeField] private bool isSpecial;
    [SerializeField] private int damageDealt;
    [SerializeField] private GameObject explosionPrefab;
    private bool hasSpawnedLeft;
    void Start()
    {
        if(transform.parent.name == "Spawn Point Left")
            hasSpawnedLeft = true;
        else
            hasSpawnedLeft = false;
        transform.parent = null;
        StartCoroutine(TimeBeforeSelfDestruct());
    }

    void Update()
    {
        if (!isSpecial)
            transform.Translate(Vector2.up * Time.deltaTime * 2);
        else if (isSpecial && hasSpawnedLeft)
        {
            transform.Translate(Vector2.left * Time.deltaTime * 2);
        }
        else if (isSpecial && !hasSpawnedLeft)
        {
            transform.Translate(Vector2.right * Time.deltaTime * 2);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            collision.gameObject.GetComponent<PlayerBehavior>().GetHit(damageDealt);
            Destroy(gameObject);
        }
        else if(collision.collider.tag == "EnemyChaser")
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            collision.gameObject.GetComponent<ChaserBehavior>().EnemyGetHit(damageDealt);

            if (collision.gameObject.GetComponent<ChaserBehavior>().currentEnemyHealth <= 0)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>().KilledShip();
            }

            Destroy(gameObject);
        }
        else if (collision.collider.tag == "EnemyShooter")
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            collision.gameObject.GetComponent<ShooterBehavior>().EnemyGetHit(damageDealt);

            if (collision.gameObject.GetComponent<ShooterBehavior>().currentEnemyHealth <= 0)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>().KilledShip();
            }

            Destroy(gameObject);
        }
        else
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    IEnumerator TimeBeforeSelfDestruct()
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
