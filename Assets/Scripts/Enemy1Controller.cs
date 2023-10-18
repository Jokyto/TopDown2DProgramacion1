using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : MonoBehaviour
{
    [SerializeField]private float speed = 2.0f; // Velocidad de movimiento del enemigo.
    [SerializeField]private float visionRange = 6.0f; // Rango de visión del enemigo.

    [SerializeField] private bool isMovingForward = true; // Indica si el enemigo se está moviendo hacia adelante o hacia atrás.

    [SerializeField] private Transform playerTransform; 
    [SerializeField] private GameObject player;

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform; // Encuentra el jugador por su etiqueta.
    }

    private void Update()
    {
        if (CanSeePlayer() && Vector3.Distance(transform.position, playerTransform.position) < visionRange) // El jugador está dentro del rango de visión del enemigo, así que persigue al jugador.
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
        }

        else
        {
            if (isMovingForward)// El jugador no está en el rango de visión, por lo que sigue el patrón de movimiento.
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(-Vector3.forward * speed * Time.deltaTime);
            }
        }

        if (transform.position.z >= 10.0f)// Si el enemigo llega al final de su recorrido, invierte su dirección.
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
}
