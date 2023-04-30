using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{

    public GameObject prbBoomEffect;
    public float fallSpeedThreshold;
    public float staticSpeedThreshold;
    public Vector3 offset;
    public float destroyTime;
    private Rigidbody2D rb2d;
    private bool beingDestroyed;
    private float destroyTimer;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(beingDestroyed)
        {
            return;
        }
        if(Mathf.Abs(rb2d.velocity.x) <= staticSpeedThreshold)
        {
            beingDestroyed = true;
            var boomEffect = Instantiate(prbBoomEffect);
            boomEffect.transform.position = transform.position + offset;
            boomEffect.SetActive(true);
        }
    }

    private void Update()
    {
        if(beingDestroyed)
        {
            destroyTimer += Time.deltaTime;
            if(destroyTimer > destroyTime)
            {
                Destroy(gameObject);
            }
        }
    }

    void FixedUpdate()
    {
        if (rb2d.velocity.y < fallSpeedThreshold)
        {
            Destroy(gameObject);
        }
    }
}
