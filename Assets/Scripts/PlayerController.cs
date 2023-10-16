using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    //Player
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private Rigidbody2D rigidBody;
    
    //Cannon Shoting
    [SerializeField]
    private float bulletSpeed;
    public Bullet prefab;
    public Transform shotingPoint;
    private float horizontal;
    private float vertical;

    void Update() {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            Debug.Log("Shooting");
            Bullet bullet = Instantiate(prefab, shotingPoint.position, shotingPoint.rotation);
            bullet.speed = bulletSpeed;
        }
    }

    void FixedUpdate() {
        rigidBody.MovePosition(rigidBody.position +  movementSpeed * Time.fixedDeltaTime * vertical * (Vector2)(Quaternion.Euler(0f, 0f, rigidBody.rotation) * Vector2.up));
        rigidBody.MoveRotation(rigidBody.rotation - rotationSpeed * Time.fixedDeltaTime * horizontal);
    }
}

