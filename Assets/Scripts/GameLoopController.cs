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
		GameOver,
		GameCompleted
	};
	
	[SerializeField] private PlayerMovement player;
	[SerializeField] private Launcher playerLauncher;
	[SerializeField] private EnemyManager enemyManager;
	[SerializeField] private TitleScreenController titleScreenController;
	[SerializeField] private AudioClip gameCompleteClip;
	
	private GameLoopState gameLoopState;
	private int waveNumber;
	private int score;
	private float scoreMultiplier;
	
	private bool fireIsDown;
	
	void Awake()
	{
		gameLoopState = GameLoopState.Init;
		fireIsDown = false;
		SetPlayerRenderers(false);
		
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
			case GameLoopState.GameCompleted:
			{
				OnGameComplete();
				break;
			}
		}
	}
	
	private void OnInit()
	{
		score = 0;
		waveNumber = 0;
		UpdateScoresAndWave();

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
		titleScreenController.ShowInGameScreen();
		UpdateScoresAndWave();
		fireIsDown = false;
		scoreMultiplier = 1.0f;
		
		SetPlayerRenderers(true);
		gameLoopState = GameLoopState.StartWave;
	}
	
	private void OnStartWave()
	{
		enemyManager.StartWave(++waveNumber);
		gameLoopState = GameLoopState.InGame;
		UpdateScoresAndWave();
		audio.Play();
	}
		
	private void OnInGame(float deltaTime)
	{
		player.UpdatePlayerRotation(Input.GetAxis("Horizontal"), deltaTime);
		
		if (Input.GetButton("Fire"))
			playerLauncher.StartAutoFire();
		else
			playerLauncher.StopAutoFire();
		
		scoreMultiplier = Mathf.Clamp(scoreMultiplier - (0.2f * deltaTime), 1.0f, 100.0f);
	}
	
	private void OnGameOver()
	{
		PlayerPrefs.SetInt(
			"BestScore",
			Mathf.Max(PlayerPrefs.GetInt("BestScore", 0), score)
		);
		PlayerPrefs.Save();
		
		playerLauncher.StopAutoFire();
		enemyManager.StopEnemySpawning();
		titleScreenController.ShowGameOverScreen();
		
		Invoke("SwitchToTitleState", 2f);
	}
	
	private void OnGameComplete()
	{
		PlayerPrefs.SetInt(
			"BestScore",
			Mathf.Max(PlayerPrefs.GetInt("BestScore", 0), score)
		);
		PlayerPrefs.Save();
		
		audio.PlayOneShot(gameCompleteClip);
		
		playerLauncher.StopAutoFire();
		titleScreenController.ShowGameCompleteScreen();
		
		Invoke("SwitchToTitleState", 10f);
	}
	
	private void SwitchToTitleState()
	{
		SetPlayerRenderers(false);
		gameLoopState = GameLoopState.TitleScreen;
	}
	
	private void PlayerDestroyed()
	{
		player.audio.Play();
		SetPlayerRenderers(false);
		
		gameLoopState = GameLoopState.GameOver;
		UpdateScoresAndWave();
	}
	
	private void EnemyDestroyed()
	{
		scoreMultiplier *= 1.15f;
		score += Mathf.RoundToInt(10.0f * scoreMultiplier);
		UpdateScoresAndWave();

		if (enemyManager.IsWaveClear && gameLoopState == GameLoopState.InGame)
		{
			if (waveNumber < 10)
				gameLoopState = GameLoopState.StartWave;
			else
				gameLoopState = GameLoopState.GameCompleted;
		}
	}
	
	private void UpdateScoresAndWave()
	{
		titleScreenController.UpdateScoreAndWaveLabels(
			score,
			waveNumber,
			Mathf.Max(score, PlayerPrefs.GetInt("BestScore", 0))
		);
	}
	
	private void SetPlayerRenderers(bool enabled)
	{
		Renderer [] renderers = player.GetComponentsInChildren<Renderer>();
		for (int index = 0; index < renderers.Length; index++)
			renderers[index].enabled = enabled;
	}
}
