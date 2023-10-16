using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Controller : MonoBehaviour
{
    [SerializeField]    
    private Rigidbody2D rigidBody;

    [SerializeField]
    private Transform rotationPoint;
    
    [SerializeField]
    private int enemy2Health;
    
    
    // Cannon Shoting    
    public Bullet prefab;
    [SerializeField]
    private float RightbulletSpeed;
    [SerializeField]
    private float RightshootingInterval;
    [SerializeField]
    private Transform RightshootingPoint;
    [SerializeField]
    private float LeftbulletSpeed;
    [SerializeField]
    private float LeftshootingInterval;
    [SerializeField]
    private Transform LeftshootingPoint;

    //Player
    private Transform playerTransform;
    [SerializeField]
    private GameObject player;


    [SerializeField]
    private float maxDetectionDistance;

    [SerializeField]
    private float rotationSpeed;

    private bool RightcanShoot;
    private bool LeftcanShoot;

    private Quaternion targetRotation;

    void Start()
    {
        Debug.Log(player.layer);
        Debug.Log(gameObject.layer);
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        RightcanShoot = true;
        LeftcanShoot = true;
    }

    void Update() 
    {
        if (enemy2Health <= 0)
        {
            Destroy(gameObject);
        }
        if (CanSeePlayer() && Vector2.Distance(transform.position, playerTransform.position) <= maxDetectionDistance)
        {
            targetRotation = Quaternion.LookRotation(Vector3.forward, playerTransform.position - rotationPoint.position);

            transform.rotation = Quaternion.LerpUnclamped(rotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (RightcanShoot)
            {
                Shooting(RightbulletSpeed,RightshootingPoint);
                RightcanShoot = false;
                StartCoroutine(ShootingCooldown("Right",RightshootingInterval));
            }
            if (LeftcanShoot)
            {
                Shooting(LeftbulletSpeed,LeftshootingPoint);
                LeftcanShoot = false;
                StartCoroutine(ShootingCooldown("Left",LeftshootingInterval));
            }
        }
    }
    bool CanSeePlayer()
    {
        if (player.layer == gameObject.layer)
        {
            return true;
        }

        return false;
    }

    public void loseHealth(int losingHealth)
    {
        enemy2Health -= losingHealth;
    }

    void Shooting(float bulletSpeed, Transform shootingPoint)
    {
        Bullet bullet = Instantiate(prefab, shootingPoint.position, shootingPoint.rotation);
        bullet.speed = bulletSpeed;
    }

    IEnumerator ShootingCooldown(string shootingFrom, float interval)
    {
        yield return new WaitForSeconds(interval);
        if (shootingFrom == "Left")
        {
            LeftcanShoot = true;
        }
        else
        {
            RightcanShoot = true;
        }
    }    
}

