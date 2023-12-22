using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using System.Threading;
using System.ComponentModel;

using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")] //Player
    private Rigidbody2D rigidBody;
    private float horizontal;
    private float vertical;
    [SerializeField] private int playerPoints;
    [SerializeField] private int playerHealth;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float previousMovementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float previousRotationSpeed;
    [SerializeField] private bool isInvulnerable = false;
    [SerializeField] private bool hasInvulnerability = true;

    [Header("Shooting Settings")] //Cannon Shoting
    private bool canShoot;
    public Bullet bulletPrefab;
    public Transform shotingPoint;
    [SerializeField]private float bulletSpeed;

    [Header("Map Collision")] //Map collision
    private bool isLosingHealth = false;
    [SerializeField]private TilemapCollider2D aquaPrefab;

    [Header("Animations")] //Animations
    private Animator animator;

    [Header("Audio")]
    private AudioSource audioSource;
    [SerializeField] AudioClip OnDeath;
    [SerializeField] AudioClip Shoot;
    [SerializeField] AudioClip Object;
    [SerializeField] AudioClip Win;
    [SerializeField] AudioClip LevelUp;
    [SerializeField] AudioClip Power;


    [Header("Scene Loader")] //SceneLoader
    public LoadScene sceneLoader;
    [Header("Game Manager")] //SceneLoader
    [SerializeField] GameManager gameManager;


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
            audioSource.PlayOneShot(Shoot);
        }
        if (hasInvulnerability && Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(Invulnerable());
            StartCoroutine(InvulnerabilityTimer());
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
            movementSpeed = 0;
            StartCoroutine(OnDie());
        }

        else
        {
            animator.SetFloat("Vivo", 1);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        sceneLoader.LoadGivenScene("Derrota");
    }

    bool CanShoot()
    {
        if (((gameObject.layer == 0) || (gameObject.layer == 13)) && canShoot)
        {
            return true;
        }

        return false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //<-----------------Consumable Timer------------------>
        if (collider.gameObject.layer == 7)
        {
            gameManager.SetTimerMinutes(1);
            Destroy(collider.gameObject);
        }
        //<-----------------Consumable Points----------------->
        if (collider.gameObject.layer == 8)
        {
            AddPoint(10);
            Destroy(collider.gameObject);
        }
        //<---------------Consumable CanonSpeed-------------->
        if (collider.gameObject.layer == 9)
        {
            bulletSpeed += 10;
            audioSource.PlayOneShot(LevelUp);
            Destroy(collider.gameObject);
        }
        //<---------------Consumable TankSpeed--------------->
        if (collider.gameObject.layer == 10)
        {
            movementSpeed += 5;
            audioSource.PlayOneShot(LevelUp);
            Destroy(collider.gameObject);
        }
        //<-----------------Consumable Engine---------------->
        if (collider.gameObject.layer == 11)
        {
            HealHealth(1);
            audioSource.PlayOneShot(Object);
            Destroy(collider.gameObject);
        }
        
        //<------------------Win condition------------------->
        if (collider.gameObject.layer == 12)
        {
            audioSource.PlayOneShot(Win);
            sceneLoader.LoadGivenScene("Victoria");
        }
        
        //<------------------Colliding Mud------------------->
        if (collider.gameObject.layer == 14)
        {
            movementSpeed = movementSpeed/2;
            previousMovementSpeed = movementSpeed *2;

        }

    } 
    void OnTriggerStay2D(Collider2D collider)
    {
        //<------------------------Colliding with Water-------------------------->
        if (collider.gameObject.layer == 4 && !isLosingHealth && GetLife() > 0)
        {
            isLosingHealth = true;
            StartCoroutine(TileDamageCooldown(6));
        }
        //<------------------------Colliding with Forest------------------------->
        if (collider.gameObject.layer == 6)
        {
            gameObject.layer = 6;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        //<------------------------Colliding with Water-------------------------->
        if (collider.gameObject.layer == 4 && isLosingHealth)
        {
            isLosingHealth = false;
        }

        //<------------------------Colliding with Forest------------------------->
        if (collider.gameObject.layer == 6)
        {
            gameObject.layer = 0;
        }
        
        //<------------------------Colliding with Mud---------------------------->
        if (collider.gameObject.layer == 14)
        {

            movementSpeed = previousMovementSpeed;
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

    //<-----------Tile Damage Cooldown Coroutine------------------->
    private IEnumerator TileDamageCooldown(float interval)
    {
        while (isLosingHealth)
        {
            AddPoint(1);
            LoseHealth(1);
            yield return new WaitForSeconds(interval);
        }
    }

    //<-------------Player is Dying Coroutine--------------------->
    private IEnumerator OnDie()
    {
        animator.SetFloat("Muerto", 3);
        animator.SetFloat("Vivo", 3);
        audioSource.PlayOneShot(OnDeath);
        yield return new WaitForSeconds(1);
        Die();
    }

    //<---------Player is Invulnerable Coroutine------------------>
    private IEnumerator Invulnerable()
    {
        //PONER ACA ANIMATOR Y AUDIO SOURCE PARA QUE SUENE
        isInvulnerable = true;
        gameObject.layer = 13;
        yield return new WaitForSeconds(5);
        gameObject.layer = 0;
        isInvulnerable = false;
    }
    private IEnumerator InvulnerabilityTimer()
    {
        hasInvulnerability = false;
        yield return new WaitForSeconds(30);
        hasInvulnerability = true;
    }
}

