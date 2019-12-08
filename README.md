# SpaceInvader_Project
~~~~~~~~~~~~~~~~~~~~// EnemyBullet # code ~~~~~~~~~~~~~~~~~~~~~~~~~
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    float speed;
    Vector2 _direction;
    bool isReady;

    private void Awake()
    {
        speed = 5f;
        isReady = false;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            Vector2 position = transform.position;
            position += _direction * speed * Time.deltaTime;
            transform.position = position;

            Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));  // top right point of the screen
            Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)); // bottom left point of the screen

            if (transform.position.x < min.x || transform.position.x > max.x || transform.position.y < min.y || transform.position.y > max.y)
            {
                Destroy(gameObject);
            }
        }

    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction.normalized;

        isReady = true;

    }
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
~~~~~~~~~~~~~~~~~EnemySpawner#code~~~~~~~~~~~~~~~~~~~~~
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySp : MonoBehaviour
{
    public GameObject sendEnemy;

    float maxSpawnRateInSecond = 5f;
    // Start is called before the first frame update
    void Start()
    {
        //ScheduleEnemySpawner();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnEnemy()
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        GameObject anEnemy = (GameObject)Instantiate(sendEnemy);
        anEnemy.transform.position = new Vector2(Random.Range(min.x, max.x), max.y);

        ScheduleEnemySpawn();
    }

    void ScheduleEnemySpawn()
    {
        float spawnInNSeconds;

        if (maxSpawnRateInSecond > 1f)
        {
            spawnInNSeconds = Random.Range(1f, maxSpawnRateInSecond);
        }
        else
        {
            spawnInNSeconds = 1f;
        }

        InvokeSpawnEnemy(spawnInNSeconds);
    }

    void InvokeSpawnEnemy(float inSeconds)
    {
        Invoke("SpawnEnemy", inSeconds);
    }

    void IncreaseSpawnRate()
    {
        if (maxSpawnRateInSecond > 1f)
        {
            maxSpawnRateInSecond--;
        }
        else
        {
            CancelInvoke("IncreaseSpawnRate");
        }
    }

    public void ScheduleEnemySpawner()
    {
        InvokeSpawnEnemy(maxSpawnRateInSecond);
        InvokeRepeating("IncreaseSpawnRate", 0f, 30f);
    }

    public void UnscheduleEnemySpawner()
    {
        CancelInvoke("SpawnEnemy");
        CancelInvoke("IncreaseSpawnRate");
    }
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
~~~~~~~~~~~~EnemyGun#code~~~~~~~~~~~~~~~~~~
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public GameObject BulletEnemy;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("EnemyBulletFire", 1f);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void EnemyBulletFire()
    {
        // get a reference to the player ship 
        GameObject playerShip = GameObject.Find("PlayerGo");

        if (playerShip != null)
        {
            GameObject bullet = (GameObject)Instantiate(BulletEnemy); // Instantiate the enemy bullet

            bullet.transform.position = transform.position; // bullet initials 

            Vector2 direction = playerShip.transform.position - bullet.transform.position; // compute the direction of the bullet towards the player ship


            bullet.GetComponent<enemyBullet>().SetDirection(direction); // Set bullet Direction

        }
    }
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
