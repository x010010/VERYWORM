using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AppleSpawner : MonoBehaviour
{
    public GameObject applePrefab;
    public float spawnAreaWidth = 10f;
    public float spawnAreaHeight = 10f;

    void Start()
    {
        SpawnApple();
    }

    public void SpawnApple()
    {
        float x = Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
        float y = Random.Range(-spawnAreaHeight / 2, spawnAreaHeight / 2);
        Vector2 spawnPosition = new Vector2(x, y);

        Instantiate(applePrefab, spawnPosition, Quaternion.identity);
    }
}
