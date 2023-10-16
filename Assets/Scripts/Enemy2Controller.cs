using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Controller : MonoBehaviour
{
    [SerializeField]    
    private Rigidbody2D rigidBody;
    [SerializeField]
    private LayerMask enemyLayer;
    [SerializeField]
    private Transform rotationPoint;
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
    private Transform player;
    [SerializeField]
    private LayerMask playerLayer;


    [SerializeField]
    private float maxDetectionDistance;

    [SerializeField]
    private float rotationSpeed;

    private bool RightcanShoot;
    private bool LeftcanShoot;

    private Quaternion targetRotation;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        RightcanShoot = true;
        LeftcanShoot = true;
    }

    void Update() 
    {
        if (Vector2.Distance(transform.position, player.position) <= maxDetectionDistance)
        {
            targetRotation = Quaternion.LookRotation(Vector3.forward, player.position - rotationPoint.position);

            transform.rotation = Quaternion.LerpUnclamped(rotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (RightcanShoot)
            {
                Rightshooting();
                RightcanShoot = false;
                StartCoroutine(RightShootingCooldown());
            }
            if (LeftcanShoot)
            {
                Leftshooting();
                LeftcanShoot = false;
                StartCoroutine(LeftShootingCooldown());
            }
        }
    }

    bool CanSeePlayer()
    {
        if (playerLayer == enemyLayer)
        {
            return true;
        }

        return false;
    }

    void Rightshooting()
    {
        Bullet Rightbullet = Instantiate(prefab, RightshootingPoint.position, RightshootingPoint.rotation);
        Rightbullet.speed = RightbulletSpeed;
    }
    void Leftshooting()
    {
        Bullet Leftbullet = Instantiate(prefab, LeftshootingPoint.position, LeftshootingPoint.rotation);
        Leftbullet.speed = LeftbulletSpeed;
    }

    IEnumerator RightShootingCooldown()
    {
        yield return new WaitForSeconds(RightshootingInterval);
        RightcanShoot = true;
    }    
    
    IEnumerator LeftShootingCooldown()
    {
        yield return new WaitForSeconds(LeftshootingInterval);
        LeftcanShoot = true;
    }
}

