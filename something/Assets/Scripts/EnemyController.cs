
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum Enemy {Bullet, BulletSpawner, DeathWall, Circle, Charge, ChargeSpawner}
    public Enemy enemy;
    public GameObject spawn;
    public float spawnTime;
    public Transform[] spawnPoints;

    Rigidbody2D rb2d;

    public Animator anim;

    public Vector2 enemyPos;
    public float moveSpeed;

    public int direction;
    public bool vertical;

    private int horDirection;
    private int vertDirection;

    private float timeToBoof;
    private float timeToJump;

    public GameManager gameManager;

    //circle attributes
    public float RotateSpeed = 5f;
    public float Radius = 5f;

    private Vector2 _centre;
    private float _angle;

    //charge attributes
    private float jumpForce = 1.875f * 732;

    //charge / circle attributes
    public BoxCollider2D foot;

    //charge spawner attributes
    private bool spawned;
    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        if(enemy != Enemy.BulletSpawner && enemy != Enemy.ChargeSpawner)
            rb2d = GetComponent<Rigidbody2D>();
        enemyPos = new Vector2(transform.position.x, transform.position.y);
        if(enemy == Enemy.BulletSpawner)
            InvokeRepeating("Spawn", spawnTime, spawnTime);
        if(enemy == Enemy.Bullet)
            Destroy(gameObject, 5f);
        if(enemy == Enemy.Circle)
        {
            horDirection = 1;
            vertDirection = 1;
            timeToBoof = 0f;
            _centre = enemyPos;
        }
        if (enemy == Enemy.Charge)
        {
            timeToJump = 1;
            Destroy(gameObject, 6f);
            rb2d.velocity = new Vector2(moveSpeed * direction, 0);
            gameManager = GetComponentInParent<EntityStorage>().gameManager;
        }
        if (enemy == Enemy.ChargeSpawner)
            spawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        enemyPos = new Vector2(transform.position.x, transform.position.y/* + 0.5f*/);
        if (anim != null && enemy != Enemy.Circle)
            anim.SetInteger("Direction", direction);
        if(enemy == Enemy.Circle)
        {
            _angle += RotateSpeed * Time.deltaTime;

            var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
            transform.position = _centre + offset;
        }
        if (enemy == Enemy.Charge && timeToJump > 0)
            timeToJump -= Time.deltaTime;
        if (enemy == Enemy.Charge && Physics2D.Raycast(foot.transform.position, new Vector2(0, -1)).distance <= 4f && Physics2D.Raycast(foot.transform.position, new Vector2(0, -1)).collider.gameObject.name == "kirk_color")
        {
            Debug.Log("Jomped");
            gameManager.setRestart(true);
        }
        /*if (enemy == Enemy.Charge && Physics2D.Raycast(foot.transform.position, new Vector2(0, -1)).collider.gameObject.name == "kirk_color") //need to fix (hitting own collider as of right now)
            gameManager.setRestart(true);*/
        if (enemy == Enemy.ChargeSpawner && !spawned && Vector2.Distance(enemyPos, player.playerPos)<= 40)
        {
            Debug.Log("distanc  e: " + Vector2.Distance(enemyPos, player.playerPos));
            Instantiate(spawn, enemyPos, transform.rotation, FindObjectOfType<EntityStorage>().transform);
            spawned = true;
        }
    }

    void FixedUpdate()
    {
        if(enemy == Enemy.Bullet)
            rb2d.velocity = !vertical?(new Vector2(moveSpeed * direction, 0)):(new Vector2(0,moveSpeed*direction));
        if (enemy == Enemy.DeathWall || enemy == Enemy.Charge)
            rb2d.velocity = new Vector2(moveSpeed * direction, rb2d.velocity.y);
        if (enemy == Enemy.Charge && timeToJump <= 0)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.isKinematic = true;
            rb2d.isKinematic = false;
            rb2d.AddForce(Vector2.up * jumpForce);
            timeToJump = 5;
        }
    }

    void Spawn()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(spawn, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation, FindObjectOfType<EntityStorage>().transform);
    }
}
