using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startWaveIndex = 0;
    [SerializeField] bool looping = false;
	// Use this for initialization
	IEnumerator Start () 
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        } while (looping);
		
	}

    private IEnumerator SpawnAllWaves()
    {
        for (int i = startWaveIndex; i < waveConfigs.Count; i++)
        {
            var currentWave = waveConfigs[i];
            yield return StartCoroutine(SpawnAllEnemiesIn(currentWave));
        }

    }

    private IEnumerator SpawnAllEnemiesIn(WaveConfig waveConfig)
    {
        for (int i = 0; i < waveConfig.GetNumberOfEnemies(); i++)
        {
            var enemy = Instantiate(waveConfig.GetEnemyPrefab(), waveConfig.GetWayPoints()[0].transform.position, Quaternion.identity);
            enemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
    }
}
