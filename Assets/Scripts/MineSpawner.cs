using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSpawner : MonoBehaviour
{
    public GameObject minePrefab;
    public float spawnInterval = 2.0f;

    private float spawnTimer = 0.0f;
    private bool canSpawnMines = true;

    void Update()
    {
        if (canSpawnMines)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0)
            {
                SpawnMine();
                spawnTimer = spawnInterval;
            }
        }
    }

    void SpawnMine()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        Vector2 spawnPosition = new Vector2(
            Random.Range(Camera.main.transform.position.x - Camera.main.orthographicSize * Screen.width / Screen.height,
                Camera.main.transform.position.x + Camera.main.orthographicSize * Screen.width / Screen.height),
            Random.Range(Camera.main.transform.position.y - Camera.main.orthographicSize, Camera.main.transform.position.y + Camera.main.orthographicSize)
        );

        Instantiate(minePrefab, spawnPosition, Quaternion.identity);
    }

    public void SetCanSpawnMines(bool canSpawn)
    {
        canSpawnMines = canSpawn;
    }
}
