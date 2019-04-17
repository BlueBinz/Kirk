using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum Enemy {Bullet, BulletSpawner, DeathWall, BounceBoss}
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

    // Start is called before the first frame update
    void Start()
    {
        if(enemy != Enemy.BulletSpawner)
            rb2d = GetComponent<Rigidbody2D>();
        enemyPos = new Vector2(transform.position.x, transform.position.y);
        if(enemy == Enemy.BulletSpawner)
            InvokeRepeating("Spawn", spawnTime, spawnTime);
        if(enemy == Enemy.Bullet)
            Destroy(gameObject, 5f);
        if(enemy == Enemy.BounceBoss)
        {
            horDirection = 1;
            vertDirection = 1;
            timeToBoof = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        enemyPos = new Vector2(transform.position.x, transform.position.y/* + 0.5f*/);
        if (anim != null && enemy != Enemy.BounceBoss)
            anim.SetInteger("Direction", direction);
        //up
        Debug.DrawRay(new Vector2(enemyPos.x, enemyPos.y + 2.1f), new Vector2(0, 1));
        if (enemy == Enemy.BounceBoss && timeToBoof <= 0 && (Physics2D.Raycast(new Vector2(enemyPos.x, enemyPos.y + 2.1f), new Vector2(0, 1)).distance <= .01))
        {
            vertDirection = -1;
            timeToBoof = .5f;
        }
        //down
        else if (enemy == Enemy.BounceBoss && timeToBoof <= 0 && (Physics2D.Raycast(new Vector2(enemyPos.x, enemyPos.y - 2.1f), new Vector2(0, -1)).distance <= .01))
        {
            vertDirection = 1;
            timeToBoof = .5f;
        }
        //right
        if (enemy == Enemy.BounceBoss && timeToBoof <= 0 && (Physics2D.Raycast(new Vector2(enemyPos.x + 2.1f, enemyPos.y), new Vector2(1, 0)).distance <= .01))
        {
            horDirection = -1;
            timeToBoof = .5f;
        }
        //left
        else if (enemy == Enemy.BounceBoss && timeToBoof <= 0 && (Physics2D.Raycast(new Vector2(enemyPos.x - 2.1f, enemyPos.y), new Vector2(-1, 0)).distance <= .01))
        {
            horDirection = 1;
            timeToBoof = .5f;
        }
        if (enemy == Enemy.BounceBoss && timeToBoof > 0)
            timeToBoof -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if(enemy != Enemy.BulletSpawner && enemy != Enemy.BounceBoss)
            rb2d.velocity = !vertical?(new Vector2(moveSpeed * direction, 0)):(new Vector2(0,moveSpeed*direction));
        if (enemy == Enemy.BounceBoss)
        {
            //bounce
            rb2d.velocity = new Vector2(moveSpeed * horDirection, moveSpeed * vertDirection);
        }
            
        /*if ((Physics2D.Raycast(new Vector2(enemyPos.x, enemyPos.y - .5f) + new Vector2(direction, 0) * 1.9f / 2, new Vector2(direction, 0)).distance <= .1))
        {
            //Debug.Log(playerNo + "" + playerPos + "" + Physics2D.Raycast(playerPos, new Vector2(direction, 0)).point);
            Debug.DrawRay(enemyPos, new Vector2(direction, 0), Color.red, .5f);
            //Debug.Log(Physics2D.Raycast(playerPos + new Vector2(direction, 0) * .5f, new Vector2(direction, 0)).collider.name);
            direction *= -1;
        }*/
    }

    void Spawn()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(spawn, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation, FindObjectOfType<EntityStorage>().transform);
    }
}
