using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
	[SerializeField] private GameObject enemyPrefab;
	[SerializeField] private Transform target;
	[SerializeField] private float delayBetweenEnemyGroups = 1.5f;
	[SerializeField] private float minSpawnDistance;
	[SerializeField] private float maxSpawnDistance;
	[SerializeField] private float minEnemyForce;
	[SerializeField] private float maxEnemyForce;
	
	private int currentWaveNumber;
	private int numberOfEnemiesSpawned;
	private float spawnDistanceDelta;
	private float enemyForceDelta;
	
	void Awake()
	{
		spawnDistanceDelta = maxSpawnDistance - minSpawnDistance;
		enemyForceDelta = maxEnemyForce - minEnemyForce;
	}
	
	public void StartWave(int waveNumber)
	{
		currentWaveNumber = waveNumber;
		numberOfEnemiesSpawned = 0;
		
		StartCoroutine("SpawnEnemiesForWave");
	}
	
	public bool IsSpawningComplete
	{
		get { return numberOfEnemiesSpawned == NumberOfEnemiesForCurrentWave; }
	}
	
	private int NumberOfEnemiesForCurrentWave
	{
		get
		{
			// This is intended to be more dynamic. But for now this is sufficient.
			return 10;
		}
	}
	
	private IEnumerator SpawnEnemiesForWave()
	{
		// Intent here is to spawn them in groups. Not necessary for now.
		// int totalToSpawn = NumberOfEnemiesForCurrentWave;
		// 
		
		for (int count = 0; count < NumberOfEnemiesForCurrentWave; count++)
		{
			GameObject enemy = (GameObject)Instantiate(enemyPrefab);
			
			float angle = Random.value * 359.0f;
			Vector3 position = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));
			enemy.transform.position = position * ((Random.value * spawnDistanceDelta) + minSpawnDistance);
			
			Vector3 vectorToTarget = target.transform.position - enemy.transform.position;
			float headingToTarget = Mathf.Atan2(vectorToTarget.x, vectorToTarget.z) * Mathf.Rad2Deg;
			enemy.transform.rotation = Quaternion.Euler(0, headingToTarget, 0);
			enemy.rigidbody.AddForce(enemy.transform.TransformDirection(Vector3.forward) * ((Random.value * enemyForceDelta) + minEnemyForce));

			yield return new WaitForSeconds(delayBetweenEnemyGroups);
		}
	}
}
