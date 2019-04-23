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

    //circle attributes
    public float RotateSpeed = 5f;
    public float Radius = 5f;

    private Vector2 _centre;
    private float _angle;

    //charge attributes
    private float jumpForce = 1.875f * 732;
    public BoxCollider2D foot;

    //charge spawner attributes
    private bool spawned;

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
            Destroy(gameObject, 12f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        enemyPos = new Vector2(transform.position.x, transform.position.y/* + 0.5f*/);
        if (anim != null && enemy != Enemy.Circle)
            anim.SetInteger("Direction", direction);
        /*
        //up
        Debug.DrawRay(new Vector2(enemyPos.x + 2.1f, enemyPos.y), new Vector2(1, 0));
        if (enemy == Enemy.BounceBoss && timeToBoof <= 0 && (Physics2D.Raycast(new Vector2(enemyPos.x, enemyPos.y + 2.1f), new Vector2(0, 1)).distance <= .01))
        {
            vertDirection = -1;
            timeToBoof = 1.5f;
        }
        //down
        else if (enemy == Enemy.BounceBoss && timeToBoof <= 0 && (Physics2D.Raycast(new Vector2(enemyPos.x, enemyPos.y - 2.1f), new Vector2(0, -1)).distance <= .01))
        {
            vertDirection = 1;
            timeToBoof = 1.5f;
        }
        //right
        if (enemy == Enemy.BounceBoss && timeToBoof <= 0 && (Physics2D.Raycast(new Vector2(enemyPos.x + 2.1f, enemyPos.y), new Vector2(1, 0)).distance <= .01))
        {
            horDirection = -1;
            timeToBoof = 1.5f;
        }
        //left
        else if (enemy == Enemy.BounceBoss && timeToBoof <= 0 && (Physics2D.Raycast(new Vector2(enemyPos.x - 2.1f, enemyPos.y), new Vector2(-1, 0)).distance <= .01))
        {
            horDirection = 1;
            timeToBoof = 1.5f;
        }
        if (enemy == Enemy.BounceBoss && timeToBoof > 0)
            timeToBoof -= Time.deltaTime;
            */
        if(enemy == Enemy.Circle)
        {
            _angle += RotateSpeed * Time.deltaTime;

            var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
            transform.position = _centre + offset;
        }
        if (timeToJump > 0)
            timeToJump -= Time.deltaTime;
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Player collided with" + collision.collider.gameObject.name);
        if (collision.gameObject.tag == "Level")
        {
            if (Mathf.Abs(collision.transform.position.x - enemyPos.x) < .1f)
                horDirection *= -1;
            if (Mathf.Abs(collision.transform.position.y - enemyPos.y) < .1f)
                vertDirection *= -1;
        }
    }
}
