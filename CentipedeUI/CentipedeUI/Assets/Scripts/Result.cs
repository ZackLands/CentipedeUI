using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
	public Text resultName;
	public Text resultScore;
	public Text lastHighScore;

	public void SetScore( int score )
	{
		string name = PlayerPrefs.GetString("Name");
		if (string.IsNullOrEmpty(name))
		{
			name = "AAA";
		}
		resultName.text = name;
		resultScore.text = score.ToString();
		SetHighScore(score);
	}

	void SetHighScore(int currentScore)
	{
		int lastScore = PlayerPrefs.GetInt("HighScore");

		if (currentScore >= lastScore)
		{
			lastScore = currentScore;
		}

		PlayerPrefs.SetInt("HighScore",lastScore);
		lastHighScore.text = $"Best: {lastScore}";
	}
}
