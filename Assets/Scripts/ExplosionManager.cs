using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroyAnimation());
    }

    IEnumerator DestroyAnimation()
    {
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }
}
