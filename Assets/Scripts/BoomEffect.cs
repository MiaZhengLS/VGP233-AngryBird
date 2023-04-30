using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomEffect : MonoBehaviour
{
    public float destroyTime;
    private float destroyTimer;

    void Update()
    {
        destroyTimer += Time.deltaTime;
        if(destroyTimer > destroyTime )
        {
            Destroy(gameObject);
        }
    }
}
