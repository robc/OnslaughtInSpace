using UnityEngine;
using System.Collections;

public class TitleScreenController : MonoBehaviour
{
	/**
	 * Want states:
	 * Title Screen (title text, instructions text, press space to start text, etc.)
	 * In Game Screen (score, wave, etc.)
	 * Game Over Screen (Game Over message, etc.)
	 */
	
	/**
	 * Assumptions:
	 * Keep the score, wave & copyright always visible.
	 * For Game Over, show the Game Over message
	 */
	
	[SerializeField] private TextMesh titleLabel;
	[SerializeField] private TextMesh startGameActionLabel;
	[SerializeField] private TextMesh instructionsCreditsLabel;
	[SerializeField] private TextMesh gameOverLabel;
	[SerializeField] private TextMesh scoreLabel;
	[SerializeField] private TextMesh waveLabel;
	
	public void ShowTitleScreen()
	{
		titleLabel.gameObject.SetActive(true);
		startGameActionLabel.gameObject.SetActive(true);
		gameOverLabel.gameObject.SetActive(false);
	}
	
	public void ShowInGameScreen()
	{
		titleLabel.gameObject.SetActive(false);
		startGameActionLabel.gameObject.SetActive(false);
		gameOverLabel.gameObject.SetActive(false);
	}
	
	public void ShowGameOverScreen()
	{
		titleLabel.gameObject.SetActive(false);
		startGameActionLabel.gameObject.SetActive(false);
		gameOverLabel.gameObject.SetActive(true);
	}
	
	public void UpdateScoreAndWaveLabels(int score, int wave)
	{
		scoreLabel.text = string.Format("{0:00000000}", score);
		waveLabel.text = string.Format("{0:00}", wave);
	}
}
