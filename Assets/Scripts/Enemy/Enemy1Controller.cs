using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy1Controller : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f; // Velocidad de movimiento del enemigo.
    [SerializeField] private float visionRange = 6.0f; // Rango de visi�n del enemigo.
    [SerializeField] private int enemy1Health;

    private bool isMovingForward = true; // Indica si el enemigo se est� moviendo hacia adelante o hacia atr�s.

    [SerializeField] private Transform playerTransform; 
    [SerializeField] private GameObject player;

    private bool isCollisioning = false;

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform; // Encuentra el jugador por su etiqueta.
    }

    private void Update()
    {
        if (enemy1Health <= 0)
        {
            Debug.Log("Enemigo Eliminado");
            Destroy(gameObject);
            player.GetComponent<PlayerController>().AddPoint(5);
        }

        if (CanSeePlayer() && Vector3.Distance(transform.position, playerTransform.position) < visionRange) // El jugador est� dentro del rango de visi�n del enemigo, as� que persigue al jugador.
        {
            Debug.Log("Te esta viendo el enemigo");
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
        }

        else
        {
            if (isMovingForward)// El jugador no est� en el rango de visi�n, por lo que sigue el patr�n de movimiento.
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }

            else
            {
                transform.Translate(-Vector3.forward * speed * Time.deltaTime);
            }
        }

        if (transform.position.z >= 10.0f)// Si el enemigo llega al final de su recorrido, invierte su direcci�n.
        {
            isMovingForward = false;
        }

        else if (transform.position.z <= 0.0f)
        {
            isMovingForward = true;
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
    //To damage the player

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.rigidbody != null)
        {
            if (collision.rigidbody.name == player.name && !isCollisioning)
            {
                isCollisioning = true;
                StartCoroutine(DamageCooldown(collision.gameObject.GetComponent<PlayerController>(),3));
            }
        }
    }

    //To stop damaging the player
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.rigidbody != null)
        {
            if (collision.rigidbody.name == player.name)
            {
                isCollisioning = false;
            }
        }
    }

    public void loseHealth(int losingHealth)
    {
        enemy1Health -= losingHealth;
    }

    //Cooldown to damage the player
    IEnumerator DamageCooldown(PlayerController gameObject, float interval)
    {
        while (isCollisioning)
        {
            gameObject.LoseHealth(1);
            yield return new WaitForSeconds(interval);
        }
    }
}
