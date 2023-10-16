using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public Rigidbody2D bullet;
    [SerializeField] float destroyTimer = 5f;
    void FixedUpdate()
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, bullet.rotation);
        bullet.MovePosition(bullet.position + speed * Time.fixedDeltaTime * (Vector2)(rotation * Vector2.up));
        DestroyTimer();
    }

    private void DestroyTimer()
    {
        destroyTimer -= Time.deltaTime;
        if (destroyTimer < 0f)
        {
            Destroy(gameObject);
        };
    }
    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);    
    }
}

