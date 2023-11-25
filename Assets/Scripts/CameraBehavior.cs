using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] float cameraSpeed;
    void Start()
    {
        
    }

    void Update()
    {
        if(Vector2.Distance(playerPos.transform.position, transform.position) > 2)
        {
            if (Vector2.Distance(transform.position, playerPos.position) > 2)
            {
                Vector3 moveToPlayer = new Vector3(playerPos.position.x, playerPos.position.y, transform.position.z);  
                transform.position = Vector3.MoveTowards(transform.position, moveToPlayer, cameraSpeed * Time.deltaTime);
            }
        }
    }
}
