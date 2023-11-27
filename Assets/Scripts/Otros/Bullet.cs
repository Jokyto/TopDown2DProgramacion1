using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    public float speed;
    public Rigidbody2D bullet;

    [SerializeField] float destroyTimer = 5f;

    public PlayerController playerPrefab;
    public Enemy2Controller enemy2;
    public Enemy1Controller enemy1;
    private Tilemap bricks;

    void Start()
    {
        bricks = GameObject.Find("Ladrillos").GetComponent<Tilemap>();
    }

    void FixedUpdate()
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, bullet.rotation);
        bullet.MovePosition(bullet.position + speed * Time.fixedDeltaTime * (Vector2)(rotation * Vector2.up));
        DestroyTimer();
    }

    private void DestroyTimer()
    {
        destroyTimer -= Time.deltaTime;

        if (destroyTimer < 0f)
        {
            Destroy(gameObject);
        };
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.rigidbody != null)
        {
            if (collision.rigidbody.name == playerPrefab.name)
            {
                Debug.Log("Perdiste vida");
                collision.gameObject.GetComponent<PlayerController>().LoseHealth(1);
            }

            else if (collision.gameObject.tag == enemy2.gameObject.tag)
            {
                collision.gameObject.GetComponent<Enemy2Controller>().loseHealth(1);
            }

            else if (collision.gameObject.tag == enemy1.gameObject.tag)
            {
                collision.gameObject.GetComponent<Enemy1Controller>().loseHealth(1);
            }
        }

        else if (collision.gameObject.name == "Ladrillos")
        {
            Vector3 hitPosition = collision.GetContact(0).point;
            Vector3Int cellPosition = bricks.WorldToCell(Vector3Int.FloorToInt(hitPosition));
            bricks.SetTile(cellPosition,null);
        }

        Destroy(gameObject);
    }
}

