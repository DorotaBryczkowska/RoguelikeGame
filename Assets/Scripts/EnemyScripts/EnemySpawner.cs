using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;

    [SerializeField]
    private int minEnemies = 2;
    [SerializeField]
    private int maxEnemies = 5;
    [SerializeField]
    private float activationDistance = 6f;
    [SerializeField]
    private float spawnDelay = 2f;

    private bool isActivated = false;

    void Update()
    {
        if (!isActivated && Vector2.Distance((Vector2)transform.position, (Vector2)GameObject.FindGameObjectWithTag("Player").transform.position) < activationDistance)
        {
            isActivated = true;
            StartCoroutine(SpawnEnemies());
        }
    }

    private IEnumerator SpawnEnemies()
    {
        int enemyCount = Random.Range(minEnemies, maxEnemies + 1);
        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
