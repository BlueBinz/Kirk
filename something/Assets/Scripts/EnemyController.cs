using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum Enemy {Bullet, BulletSpawner}
    public Enemy enemy;
    public GameObject spawn;
    public float spawnTime;
    public Transform[] spawnPoints;

    Rigidbody2D rb2d;

    public Animator anim;

    public Vector2 enemyPos;
    public CapsuleCollider2D enemyCollider;
    public float moveSpeed;

    public int direction;


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        enemyPos = new Vector2(transform.position.x, transform.position.y);
        if(enemy == Enemy.BulletSpawner)
            InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        enemyPos = new Vector2(transform.position.x, transform.position.y + 0.5f);
        if (anim != null)
            anim.SetInteger("Direction", direction);
    }

    void FixedUpdate()
    {
        rb2d.velocity = (new Vector2(moveSpeed * direction, rb2d.velocity.y));
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
        Instantiate(spawn, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}
