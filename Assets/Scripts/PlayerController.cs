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
    private bool isLosingHealth = false;

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
            sceneLoader.LoadGivenScene("Prueba");
        }
    }
    void FixedUpdate() {
        rigidBody.MovePosition(rigidBody.position +  movementSpeed * Time.fixedDeltaTime * vertical * (Vector2)(Quaternion.Euler(0f, 0f, rigidBody.rotation) * Vector2.up));
        rigidBody.MoveRotation(rigidBody.rotation - rotationSpeed * Time.fixedDeltaTime * horizontal);
    }
    bool CanShoot()
    {
        if (gameObject.layer == 0)
        {
            return true;
        }

        return false;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("The player collide with " + collider.gameObject.tag);
        if (collider.gameObject.tag == "Engine")
        {
            healHealth(1);
            Destroy(collider.gameObject);
        }
    } 
    void OnTriggerStay2D(Collider2D collider)
    {
        Debug.Log("The player is colliding with " + collider.gameObject.name);
        if (collider.gameObject.name == "Agua" && !isLosingHealth)
        {
            isLosingHealth = true;
            StartCoroutine(TileDamageCooldown(5));
        }
        if (collider.gameObject.name == "Bosque")
        {
            gameObject.layer = 6;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        Debug.Log("The player collided with " + collider.gameObject.name);
        if (collider.gameObject.name == "Bosque")
        {
            gameObject.layer = 0;
        }
        if (collider.gameObject.name == "Agua" && isLosingHealth)
        {
            isLosingHealth = false;
        }
    }
    public void loseHealth(int losingHealth)
    {
        if (playerHealth - losingHealth <= 0)
        {
            Debug.Log("The player is dead.");
        }
        playerHealth -= losingHealth;
    }
    public void healHealth(int healHealth)
    {
        playerHealth += healHealth;
    }
    private IEnumerator TileDamageCooldown(float interval)
    {
        while (isLosingHealth)
        {
            loseHealth(1);
            Debug.Log("Losing health: " + playerHealth);
            yield return new WaitForSeconds(interval);
        }
    }
}

