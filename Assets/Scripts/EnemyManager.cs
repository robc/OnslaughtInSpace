using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
	public System.Action OnEnemyDestroyed;
	public System.Action OnTargetReached;
	
	[SerializeField] private EnemyCollision enemyPrefab;
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
	private List <EnemyCollision> spawnedEnemies;
	
	void Awake()
	{
		spawnedEnemies = new List<EnemyCollision>();

		spawnDistanceDelta = maxSpawnDistance - minSpawnDistance;
		enemyForceDelta = maxEnemyForce - minEnemyForce;
	}
	
	public void StartWave(int waveNumber)
	{
		currentWaveNumber = waveNumber;
		numberOfEnemiesSpawned = 0;
		
		StartCoroutine("SpawnEnemiesForWave");
	}
	
	public void StopEnemySpawning()
	{
		StopCoroutine("SpawnEnemiesForWave");
	}
	
	public bool IsSpawningComplete
	{
		get { return numberOfEnemiesSpawned == NumberOfEnemiesForCurrentWave; }
	}
	
	public bool IsWaveClear
	{
		get { return IsSpawningComplete && spawnedEnemies.Count == 0; }
	}
	
	private int NumberOfEnemiesForCurrentWave
	{
		get
		{
			if (currentWaveNumber < 5)
				return 20;
			else if (currentWaveNumber < 8)
				return 40;
			else if (currentWaveNumber < 10)
				return 80;
			else
				return 160;
		}
	}
	
	private int NumberOfEnemiesPerGroup
	{
		get
		{
			if (currentWaveNumber < 5)
				return 2;
			else if (currentWaveNumber < 8)
				return 4;
			else if (currentWaveNumber < 10)
				return 8;
			else
				return 16;
		}
	}
	
	private IEnumerator SpawnEnemiesForWave()
	{
		int numberOfGroups = NumberOfEnemiesForCurrentWave / NumberOfEnemiesPerGroup;
		for (int count = 0; count < numberOfGroups; count++)
		{
			for (int member = 0; member < NumberOfEnemiesPerGroup; member++)
			{
				EnemyCollision enemy = (EnemyCollision)Instantiate(enemyPrefab);
				enemy.OnEnemyDestroyed = EnemyDestroyed;
				enemy.OnEnemyReachedTarget = EnemyReachedTarget;
				spawnedEnemies.Add(enemy);
				
				float angle = Random.value * 359.0f;
				Vector3 position = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));
				enemy.transform.position = position * ((Random.value * spawnDistanceDelta) + minSpawnDistance);
				
				Vector3 vectorToTarget = target.transform.position - enemy.transform.position;
				float headingToTarget = Mathf.Atan2(vectorToTarget.x, vectorToTarget.z) * Mathf.Rad2Deg;
				enemy.transform.rotation = Quaternion.Euler(0, headingToTarget, 0);
				enemy.rigidbody.AddForce(enemy.transform.TransformDirection(Vector3.forward) * ((Random.value * enemyForceDelta) + minEnemyForce));
				
				enemy.audio.Play();
				
				numberOfEnemiesSpawned++;
			}

			yield return new WaitForSeconds(delayBetweenEnemyGroups);
		}
	}
	
	private void EnemyDestroyed(EnemyCollision enemy)
	{
		// Trigger an explosion effect.

		spawnedEnemies.Remove(enemy);
		Destroy(enemy.gameObject);

		if (OnEnemyDestroyed != null)
			OnEnemyDestroyed();
	}
	
	private void EnemyReachedTarget(EnemyCollision enemy)
	{
		EnemyCollision [] enemies = spawnedEnemies.ToArray();
		spawnedEnemies.Clear();
		
		for (int count = 0; count < enemies.Length; count++)
			Destroy(enemies[count].gameObject);
		
		if (OnTargetReached != null)
			OnTargetReached();
	}
}
