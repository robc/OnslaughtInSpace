using UnityEngine;
using System.Collections;

public class GameLoopController : MonoBehaviour
{
	private enum GameLoopState
	{
		Init,
		TitleScreen,
		StartGame,
		StartWave,
		InGame,
		GameOver
	};
	
	[SerializeField] private PlayerMovement player;
	[SerializeField] private Launcher playerLauncher;
	[SerializeField] private EnemyManager enemyManager;
	[SerializeField] private TitleScreenController titleScreenController;
	
	private GameLoopState gameLoopState;
	private int waveNumber;
	private int score;
	
	private bool fireIsDown;
	
	void Awake()
	{
		gameLoopState = GameLoopState.Init;
		fireIsDown = false;
		
		enemyManager.OnEnemyDestroyed = EnemyDestroyed;
		enemyManager.OnTargetReached = PlayerDestroyed;
	}
	
	void Update()
	{
		switch(gameLoopState)
		{
			case GameLoopState.Init:
			{
				OnInit();
				break;
			}
			case GameLoopState.TitleScreen:
			{
				OnTitleScreen();
				break;
			}
			case GameLoopState.StartGame:
			{
				OnStartGame();
				break;
			}
			case GameLoopState.StartWave:
			{
				OnStartWave();
				break;
			}
			
			case GameLoopState.InGame:
			{
				OnInGame(Time.smoothDeltaTime);
				break;
			}
			case GameLoopState.GameOver:
			{
				OnGameOver();
				break;
			}
		}
	}
	
	private void OnInit()
	{
		score = 0;
		waveNumber = 0;
		titleScreenController.UpdateScoreAndWaveLabels(score, waveNumber);

		gameLoopState = GameLoopState.StartGame;
	}
	
	private void OnTitleScreen()
	{
	}
	
	private void OnStartGame()
	{
		score = 0;
		waveNumber = 0;
		titleScreenController.UpdateScoreAndWaveLabels(score, waveNumber);
		titleScreenController.ShowInGameScreen();
		
		player.gameObject.SetActive(true);
		gameLoopState = GameLoopState.StartWave;
	}
	
	private void OnStartWave()
	{
		enemyManager.StartWave(++waveNumber);
		gameLoopState = GameLoopState.InGame;
		titleScreenController.UpdateScoreAndWaveLabels(score, waveNumber);
	}
		
	private void OnInGame(float deltaTime)
	{
		player.UpdatePlayerRotation(Input.GetAxis("Horizontal"), deltaTime);
		
		if (fireIsDown && !Input.GetButton("Fire"))
			playerLauncher.FireBullet();
		fireIsDown = Input.GetButton("Fire");
	}
	
	private void OnGameOver()
	{
		enemyManager.StopEnemySpawning();
		titleScreenController.ShowGameOverScreen();
		
		// Should have a delay here before we switch over to the title screen, no?
	}
	
	private void PlayerDestroyed()
	{
		player.gameObject.SetActive(false);
		gameLoopState = GameLoopState.GameOver;
		titleScreenController.UpdateScoreAndWaveLabels(score, waveNumber);
	}
	
	private void EnemyDestroyed()
	{
		score += 10;
		titleScreenController.UpdateScoreAndWaveLabels(score, waveNumber);

		if (enemyManager.IsWaveClear)
			gameLoopState = GameLoopState.StartWave;
	}
}
