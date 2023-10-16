using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    //Player
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private Rigidbody2D rigidBody;
    
    [SerializeField]
    private int playerHealth;

    //Cannon Shoting
    [SerializeField]
    private float bulletSpeed;
    public Bullet bulletPrefab;
    public Transform shotingPoint;
    private float horizontal;
    private float vertical;

    //Map collision
    [SerializeField]
    private TilemapCollider2D aquaPrefab;

    //SceneLoader
    public LoadScene sceneLoader;
    void Update() {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (CanShoot() && Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            Debug.Log("Shooting");
            Bullet bullet = Instantiate(bulletPrefab, shotingPoint.position, shotingPoint.rotation);
            bullet.speed = bulletSpeed;
        }

        if (playerHealth <= 0) 
        {
            Destroy(gameObject);
            sceneLoader.LoadGivenScene("Prueba2");
        }
    }
    bool CanShoot()
    {
        if (gameObject.layer == 0)
        {
            return true;
        }

        return false;
    }
    void FixedUpdate() {
        rigidBody.MovePosition(rigidBody.position +  movementSpeed * Time.fixedDeltaTime * vertical * (Vector2)(Quaternion.Euler(0f, 0f, rigidBody.rotation) * Vector2.up));
        rigidBody.MoveRotation(rigidBody.rotation - rotationSpeed * Time.fixedDeltaTime * horizontal);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        Debug.Log(collider);
        if (collider.gameObject.name == "Agua")
        {
            loseHealth(1);
        }
        if (collider.gameObject.name == "Bosque")
        {
            gameObject.layer = 6;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Bosque")
        {
            gameObject.layer = 0;
        }
    }

    public void loseHealth(int losingHealth)
    {
        playerHealth -= losingHealth;
    }
}

