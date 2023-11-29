using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using System.Threading;
using System.ComponentModel;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    //Player

    private Animator animator;
    private Rigidbody2D rigidBody;
    private bool isDead;

    private float horizontal;
    private float vertical;

    [SerializeField]private float movementSpeed;
    [SerializeField]private float rotationSpeed;
    [SerializeField]private int playerHealth;
    [SerializeField]private int playerPoints;

    [Header("Shooting Settings")]
    //Cannon Shoting
    [SerializeField]private float bulletSpeed;

    public Bullet bulletPrefab;
    public Transform shotingPoint;
    

    [Header("Map Collision")]
    //Map collision
    [SerializeField]private TilemapCollider2D aquaPrefab;
    private bool isLosingHealth = false;

    [Header("Scene Loader")]
    //SceneLoader
    public LoadScene sceneLoader;


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update() 
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (CanShoot() && Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            Bullet bullet = Instantiate(bulletPrefab, shotingPoint.position, shotingPoint.rotation);
            bullet.speed = bulletSpeed;
        }

        if (playerHealth <= 0) 
        {
            animator.SetBool("IsDead",true);
            isDead = true;

           /* if (isDead == true)
            {
                Destroy(gameObject);
                sceneLoader.LoadGivenScene("Prueba");
            }*/
        }
    }

    private void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position +  movementSpeed * Time.fixedDeltaTime * vertical * (Vector2)(Quaternion.Euler(0f, 0f, rigidBody.rotation) * Vector2.up));
        rigidBody.MoveRotation(rigidBody.rotation - rotationSpeed * Time.fixedDeltaTime * horizontal);
    }

    private void LateUpdate()
    {
        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);
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

        if (collider.gameObject.tag == "Engine")
        {
            HealHealth(1);
            Destroy(collider.gameObject);
        }
    } 
    void OnTriggerStay2D(Collider2D collider)
    {

        if (collider.gameObject.name == "Agua" && !isLosingHealth)
        {
            isLosingHealth = true;
            StartCoroutine(TileDamageCooldown(6));
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

        if (collider.gameObject.name == "Agua" && isLosingHealth)
        {
            isLosingHealth = false;
        }
    }

    public int GetLife()
    {
        return playerHealth;
    }
    public void LoseHealth(int losingHealth)
    {
        playerHealth -= losingHealth;
    }
    public void HealHealth(int healHealth)
    {
        playerHealth += healHealth;
    }
    public int GetPoints()
    {
        return playerPoints;
    }
    public void AddPoint(int points)
    {
        playerPoints += points;
    }
    private IEnumerator TileDamageCooldown(float interval)
    {
        while (isLosingHealth)
        {
            AddPoint(1);
            LoseHealth(1);
            yield return new WaitForSeconds(interval);
        }
    }
}

