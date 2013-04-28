using UnityEngine;
using System.Collections;

public class GameLoopController : MonoBehaviour
{
	private enum GameLoopState
	{
		Init,
		TitleScreen,
		StartGame,
		InGame,
		GameOver
	};
	
	[SerializeField] private TextMesh scoreLabel;
	[SerializeField] private TextMesh waveLabel;
	[SerializeField] private PlayerMovement player;
	
	private GameLoopState gameLoopState;
	private int waveNumber;
	private int score;
	
	void Awake()
	{
		gameLoopState = GameLoopState.Init;
	}
	
	void Start()
	{
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
		UpdateGUI();

		gameLoopState = GameLoopState.StartGame;
	}
	
	private void OnTitleScreen()
	{
	}
	
	private void OnStartGame()
	{
		score = 0;
		waveNumber = 1;
		UpdateGUI();
		
		gameLoopState = GameLoopState.InGame;
	}
	
	private void OnInGame(float deltaTime)
	{
		player.UpdatePlayerRotation(Input.GetAxis("Horizontal"), deltaTime);
	}
	
	private void OnGameOver()
	{
	}
	
	private void UpdateGUI()
	{
		scoreLabel.text = string.Format("{0:00000000}", score);
		waveLabel.text = string.Format("{0:00}", waveNumber);
	}
}
