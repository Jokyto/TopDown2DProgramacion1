using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : MonoBehaviour
{
    [SerializeField]private float speed = 2.0f; // Velocidad de movimiento del enemigo.
    [SerializeField]private float visionRange = 6.0f; // Rango de visi�n del enemigo.

    [SerializeField] private bool isMovingForward = true; // Indica si el enemigo se est� moviendo hacia adelante o hacia atr�s.

    [SerializeField] private Transform playerTransform; 
    [SerializeField] private GameObject player;

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform; // Encuentra el jugador por su etiqueta.
    }

    private void Update()
    {
        if (CanSeePlayer() && Vector3.Distance(transform.position, playerTransform.position) < visionRange) // El jugador est� dentro del rango de visi�n del enemigo, as� que persigue al jugador.
        {
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
}
