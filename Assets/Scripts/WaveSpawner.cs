using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING };
    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }
    public Transform[] spawnPoints;
    public Wave[] waves;
    private int nextWave = 0;
    public float waveInterval = 5f;
    private float waveCountdown;
    public float WaveCountdown
    {
        get { return waveCountdown; }
    }
    public int NextWave
    {
        get { return nextWave + 1; }
    }
    private float searchCountdown = 1f;
    private SpawnState state = SpawnState.COUNTING;
    public SpawnState State
    {
        get { return state; }
    }
    void Start()
    {
        waveCountdown = waveInterval;
        // GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;
    }
    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                OnWaveComplete();
                return;
            }
            else
            {
                return;
            }
        }
        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }
    void OnWaveComplete()
    {
        state = SpawnState.COUNTING;
        waveCountdown = waveInterval;
        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
        } else
        {
            nextWave++;
        }
    }
    bool EnemyIsAlive()
    {

        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                return false;
            }
        }
        return true;
    }
    IEnumerator SpawnWave(Wave _wave)
    {
        state = SpawnState.SPAWNING;
        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }
        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        if (spawnPoints.Length == 0) {
            Debug.Log("No spwan points");
        }
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
    }
    //     void OnUpgradeMenuToggle(bool active)
    // {
    //     GetComponent<WaveSpawner>().enabled = !active;
    // }
}
