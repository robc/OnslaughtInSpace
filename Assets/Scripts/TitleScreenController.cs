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
	
	// [SerializeField] private TextMesh titleLabel;
	[SerializeField] private TextMesh startGameActionLabel;
	[SerializeField] private TextMesh gameOverLabel;
	[SerializeField] private TextMesh scoreLabel;
	[SerializeField] private TextMesh waveLabel;
	
	public void ShowTitleScreen()
	{
		startGameActionLabel.gameObject.SetActive(true);
		gameOverLabel.gameObject.SetActive(false);
	}
	
	public void ShowInGameScreen()
	{
		startGameActionLabel.gameObject.SetActive(false);
		gameOverLabel.gameObject.SetActive(false);
	}
	
	public void ShowGameOverScreen()
	{
		startGameActionLabel.gameObject.SetActive(false);
		gameOverLabel.gameObject.SetActive(true);
	}
	
	public void UpdateScoreAndWaveLabels(int score, int wave)
	{
		scoreLabel.text = string.Format("{0:00000000}", score);
		waveLabel.text = string.Format("{0:00}", wave);
	}
}
