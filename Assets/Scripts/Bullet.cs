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
    private Grid grid;
    void Start()
    {
        grid = FindObjectOfType<Grid>();
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
        Debug.Log(enemy1.gameObject);
        if (collision.rigidbody != null)
        {
            if (collision.rigidbody.name == playerPrefab.name)
            {
                Debug.Log("Player got hit by enemy at " + collision.transform.position);
                collision.gameObject.GetComponent<PlayerController>().loseHealth(1);
            }
            else if (collision.gameObject.tag == enemy2.gameObject.tag)
            {
                Debug.Log("Enemy2 got hit by player at " + collision.transform.position);
                collision.gameObject.GetComponent<Enemy2Controller>().loseHealth(1);
            }
            else if (collision.gameObject.tag == enemy1.gameObject.tag)
            {
                Debug.Log("Enemy1 got hit by player at " + collision.transform.position);
                collision.gameObject.GetComponent<Enemy1Controller>().loseHealth(1);
            }
        }
        else if (collision.gameObject.name == "Ladrillos")
        {
            Vector3 hitPosition = gameObject.transform.position;

            Tilemap closestTilemap = null;
            Vector3Int closestCell = Vector3Int.zero;
            float closestDistance = float.MaxValue;

            Tilemap[] tilemaps = grid.GetComponentsInChildren<Tilemap>();
            foreach (Tilemap tilemap in tilemaps)
            {
                if (tilemap.name == "Ladrillos")
                {
                    BoundsInt bounds = tilemap.cellBounds;
                    for (int x = bounds.x; x < bounds.x + bounds.size.x; x++)
                    {
                        for (int y = bounds.y; y < bounds.y + bounds.size.y; y++)
                        {
                            Vector3Int cellPosition = new Vector3Int(x, y, 0);
                            Vector3 tileCenter = tilemap.GetCellCenterWorld(cellPosition);
                            float distance = Vector3.Distance(hitPosition, tileCenter);

                            if (distance < closestDistance)
                            {
                                closestDistance = distance;
                                closestTilemap = tilemap;
                                closestCell = cellPosition;
                            }
                        }
                    }
                }
            }
            if (closestTilemap != null)
            {
                closestTilemap.SetTile(closestCell, null);
            }
        }
        Destroy(gameObject);
    }
}

