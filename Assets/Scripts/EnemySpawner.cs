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
