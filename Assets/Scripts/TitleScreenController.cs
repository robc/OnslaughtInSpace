using UnityEngine;
using System.Collections;

public class TitleScreenController : MonoBehaviour
{
	[SerializeField] private TextMesh titleLabel;
	[SerializeField] private TextMesh startGameActionLabel;
	[SerializeField] private TextMesh instructionsCreditsLabel;
	[SerializeField] private TextMesh gameOverLabel;
	[SerializeField] private TextMesh gameCompleteMessageLabel;
	[SerializeField] private TextMesh gameCompleteTitleLabel;
	[SerializeField] private TextMesh creditsLabel;
	
	[SerializeField] private TextMesh scoreLabel;
	[SerializeField] private TextMesh highScoreLabel;
	[SerializeField] private TextMesh waveLabel;

	public void ShowTitleScreen()
	{
		titleLabel.gameObject.SetActive(true);
		startGameActionLabel.gameObject.SetActive(true);
		instructionsCreditsLabel.gameObject.SetActive(true);
		
		gameOverLabel.gameObject.SetActive(false);
		gameCompleteTitleLabel.gameObject.SetActive(false);
		gameCompleteMessageLabel.gameObject.SetActive(false);
		creditsLabel.gameObject.SetActive(false);
	}
	
	public void ShowInGameScreen()
	{
		titleLabel.gameObject.SetActive(false);
		startGameActionLabel.gameObject.SetActive(false);
		gameOverLabel.gameObject.SetActive(false);
		instructionsCreditsLabel.gameObject.SetActive(false);

		gameCompleteTitleLabel.gameObject.SetActive(false);
		gameCompleteMessageLabel.gameObject.SetActive(false);
		creditsLabel.gameObject.SetActive(false);
	}
	
	public void ShowGameOverScreen()
	{
		gameOverLabel.gameObject.SetActive(true);

		titleLabel.gameObject.SetActive(false);
		startGameActionLabel.gameObject.SetActive(false);
		instructionsCreditsLabel.gameObject.SetActive(false);
		
		gameCompleteTitleLabel.gameObject.SetActive(false);
		gameCompleteMessageLabel.gameObject.SetActive(false);
		creditsLabel.gameObject.SetActive(false);
	}
	
	public void ShowGameCompleteScreen()
	{
		gameOverLabel.gameObject.SetActive(false);
		
		titleLabel.gameObject.SetActive(false);
		startGameActionLabel.gameObject.SetActive(false);
		instructionsCreditsLabel.gameObject.SetActive(false);
		
		gameCompleteTitleLabel.gameObject.SetActive(true);
		gameCompleteMessageLabel.gameObject.SetActive(true);
		creditsLabel.gameObject.SetActive(true);
	}
	
	public void UpdateScoreAndWaveLabels(int score, int wave, int hiScore)
	{
		scoreLabel.text = string.Format("{0:00000000}", score);
		highScoreLabel.text = string.Format("{0:00000000}", hiScore);
		waveLabel.text = string.Format("{0:00}", wave);
	}
}
