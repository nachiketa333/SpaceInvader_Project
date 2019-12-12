# SpaceInvader_Project ******All Code *********
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
****** Space Invader Level two ********

Fight with the Boss SpaceShip 
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//Enemy code 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D enemy; //reference to itself
    public float moveSpeed = 20.0f; //default move speed of the enemy
    public bool changeDirection = false; //by default set the bool to false
                                         // Use this for initialization
    void Start()
    {
        enemy = this.gameObject.GetComponent<Rigidbody2D>(); //make the connection to the reference
    }

    // Update is called once per frame
    void Update()
    {
        moveEnemy();
    }
    public void moveEnemy()
    {

        if (changeDirection == true)
        {
            enemy.velocity = new Vector2(1, 0) * -1 * moveSpeed; //get the enemy to move left
        }
        else if (changeDirection == false)
        {
            enemy.velocity = new Vector2(1, 0) * moveSpeed; //get the enemy to move right
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "RightWall")
        {
            Debug.Log("Hit the right wall");
            changeDirection = true;
        }
        if (col.gameObject.name == "LeftWall")
        {
            Debug.Log("Hit the left wall");
            changeDirection = false;
        }
    }
}

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//Enemy Bullets

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public Rigidbody2D projectile;
    public float moveSpeed = 15.0f;
 // Use this for initialization
    void Start()
    {
        projectile = this.gameObject.GetComponent<Rigidbody2D>();
    }

  // Update is called once per frame
    void Update()
    {
        projectile.velocity = new Vector2(0, -1) * moveSpeed;
    }

  //hit detection
    
   void OnCollisionEnter2D(Collision2D col)
    {
   //when it hits the player
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.SetActive(false);
        }
   //when it hits the bottom of the screen
        if (col.gameObject.name == "Bottom")
        {
            Object.Destroy(this.gameObject);
        }
    }
}


~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Enemy Shoot

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject projectile; //reference to the gameobject the enemy will shoot
    public Transform projectileSpawn; //reference to where the projectile will spawn...
    public float nextFire = 1.0f;
    public float currentTime = 0.0f;
    // Use this for initialization
    void Start()
    {
        projectileSpawn = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        enemyShoot();
    }
    public void enemyShoot()
    {
        currentTime += Time.deltaTime;
        if (currentTime > nextFire)
        {
            nextFire += currentTime;
            Instantiate(projectile, projectileSpawn.position, Quaternion.identity); //FIRE!
            nextFire -= currentTime;
            currentTime = 0.0f;
        }
    }
}

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Player code

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //controls how fast the player moves
    public float moveSpeed = 10.0f; //default speed, can change if you want.
    public Rigidbody2D player;
    // Use this for initialization
    void Start()
    {
        player = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //this.transform.Translate(Input.GetAxis("Horizontal"),0,0); //using transform ignores physics part 

        MovePlayer();

    }
    //this function allows for the player to move

    public void MovePlayer()
    {
        //movement through physics. 

        player.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * moveSpeed;
    }
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Player bullets

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveProjectile : MonoBehaviour
{
    public Rigidbody2D projectile;//reference to a rigidbody2d
    public float moveSpeed = 10.0f;
    // Use this for initialization
    void Start()
    {
        projectile = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        projectile.velocity = new Vector2(0, 1) * moveSpeed;
    }
    //add some hit detecion
    void OnCollisionEnter2D(Collision2D col)
    {
        //when it hits an enemy...
        if (col.gameObject.name == "Enemy")
        {
            col.gameObject.SetActive(false);
        }
        //when it hits the top of the screen
        if (col.gameObject.name == "Top")
        {
            Object.Destroy(this.gameObject);
        }
    }
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Fire code 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject projectile; //reference to the Capsule aka PlayerProjectile

    public Transform projectileSpawn; //reference to where the projectile will spawn from...

    public float nextFire = 1.0f; //time interval between shots
    public float currentTime = 0.0f; //current time to build up

    // Use this for initialization
    void Start()
    {
        projectileSpawn = this.gameObject.transform; //make link to where this game object is
    }

    // Update is called once per frame
    void Update()
    {

        shoot(); //Shoot
    }

    //this function allows for projectiles to shoot up aka forward of the ship
    public void shoot()
    {
        currentTime += Time.deltaTime; //build the timer up for current time
        if (Input.GetButton("Fire1") && currentTime > nextFire) //if player hits Fire and Current time is greater then the interval between...
        {
            nextFire += currentTime; //add the current time to next fire, so you cant shoot again

            Instantiate(projectile, projectileSpawn.position, Quaternion.identity); //shoot!!!

            nextFire -= currentTime; //subtract the current time from next fire, so we can "reset" interval to 1
            currentTime = 0.0f; //reset current time
        }
    }
}

************* Enemy Matrix **************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMatrix : MonoBehaviour
{
    public GameObject[] row1;
    public GameObject[] row2;

    public GameObject GameManager;

    int totalEnemies = 0;

    int row1Firing = 3;
    int row2Firing = 4;
   

    void Start()

    {
        totalEnemies = row1.Length + row2.Length;

        InvokeRepeating("Row1Fire", 0f, 3f);
        InvokeRepeating("Row2Fire", 1f, 3f);

    }



    public void killedEnemy()
    {
        if (totalEnemies <= 0)
        {
            GameManager.GetComponent<GameManager>().startspwning();
            

        }
        totalEnemies--;

    }
    void Row1Fire()
    {

        for (int i = 0; i < row1.Length; i++)
        {
            if (i % row1Firing == 0)
            {
                GameObject enemy = row1[i];
                if (enemy != null)
                    enemy.GetComponentInChildren<EnemyGun>().firebullet();
            }
        }
        row1Firing--;
        if (row1Firing <= 1)
        {
            row1Firing = 3;
        }
    }



    void Row2Fire()
    {
        for (int i = 0; i < row2.Length; i++)
        {
            if (i % row2Firing == 0)
            {
                GameObject enemy = row2[i];
                if (enemy != null)
                    enemy.GetComponentInChildren<EnemyGun>().firebullet();
            }
        }
        row2Firing--;
        if (row2Firing <= 1)
        {
            row2Firing = 4;
        }
    }





}
