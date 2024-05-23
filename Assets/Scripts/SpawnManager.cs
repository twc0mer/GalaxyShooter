using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject[] _powerupPrefabs;

    [SerializeField]
    private int _powerUpSpawnTimeMin = 10;

    [SerializeField]
    private int _powerUpSpawnTimeMax = 20;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private bool _stopSpawning = false;

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (!_stopSpawning)
        {
            if (_enemyContainer.transform.childCount < 10)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-8f, 8f), 7f, 0);
                GameObject enemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
                enemy.transform.parent = _enemyContainer.transform;
            }

            yield return new WaitForSeconds(3);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (!_stopSpawning)
        {
            if (_enemyContainer.transform.childCount < 10)
            {
                int powerupIndex = Random.Range(0, _powerupPrefabs.Length);
                Vector3 spawnPos = new Vector3(Random.Range(-8f, 8f), 7f, 0);

                Instantiate(_powerupPrefabs[powerupIndex], spawnPos, Quaternion.identity);
            }

            yield return new WaitForSeconds(Random.Range(_powerUpSpawnTimeMin, _powerUpSpawnTimeMax));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;

        foreach (Enemy enemy in _enemyContainer.transform.GetComponentsInChildren<Enemy>())
        {
            Destroy(enemy.gameObject);
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }
}
