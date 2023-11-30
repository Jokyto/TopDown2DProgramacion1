using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using System.Threading;
using System.ComponentModel;
using UnityEditorInternal;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")] //Player

    private Animator animator;
    private Rigidbody2D rigidBody;

    [SerializeField] private int playerPoints;
    [SerializeField] private int playerHealth;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    private float horizontal;
    private float vertical;

    [Header("Shooting Settings")] //Cannon Shoting

    [SerializeField]private float bulletSpeed;
    private bool canShoot;
    public Bullet bulletPrefab;
    public Transform shotingPoint;
    

    [Header("Map Collision")] //Map collision

    [SerializeField]private TilemapCollider2D aquaPrefab;
    private bool isLosingHealth = false;

    [Header("Scene Loader")] //SceneLoader
    public LoadScene sceneLoader;


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        canShoot = true;
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
    }

    private void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position +  movementSpeed * Time.fixedDeltaTime * vertical * (Vector2)(Quaternion.Euler(0f, 0f, rigidBody.rotation) * Vector2.up));
        rigidBody.MoveRotation(rigidBody.rotation - rotationSpeed * Time.fixedDeltaTime * horizontal);
    }

    private void LateUpdate()
    {
        if (playerHealth <= 0)
        {
            animator.SetFloat("Muerto", 3);
            animator.SetFloat("Vivo", 3);
            movementSpeed = 0;
        }

        else
        {
            animator.SetFloat("Vivo", 1);
        }
    }


    bool CanShoot()
    {
        if (gameObject.layer == 0 && canShoot)
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
    public void SetCanShoot(bool SetBool)
    {
        canShoot = SetBool;
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

