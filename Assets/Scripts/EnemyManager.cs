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
			
			numberOfEnemiesSpawned++;
			yield return new WaitForSeconds(delayBetweenEnemyGroups);
		}
	}
	
	private void EnemyDestroyed(EnemyCollision enemy)
	{
		spawnedEnemies.Remove(enemy);
		Destroy(enemy.gameObject);

		// Trigger an explosion effect.
		// Play an explosion sound.

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
