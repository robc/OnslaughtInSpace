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
		player.gameObject.SetActive(false);
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

		gameLoopState = GameLoopState.TitleScreen;
	}
	
	private void OnTitleScreen()
	{
		titleScreenController.ShowTitleScreen();
		
		if (fireIsDown && !Input.GetButton("Fire"))
			gameLoopState = GameLoopState.StartGame;
		fireIsDown = Input.GetButton("Fire");
	}
	
	private void OnStartGame()
	{
		score = 0;
		waveNumber = 0;
		titleScreenController.UpdateScoreAndWaveLabels(score, waveNumber);
		titleScreenController.ShowInGameScreen();
		fireIsDown = false;
		
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
		
		Invoke("SwitchToTitleState", 2f);
	}
	
	private void SwitchToTitleState()
	{
		gameLoopState = GameLoopState.TitleScreen;
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
