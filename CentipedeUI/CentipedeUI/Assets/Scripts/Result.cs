using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
	public static Result Instance { get; private set; }

	public TextMeshProUGUI resultName;
	public TextMeshProUGUI resultScore;
	public TextMeshProUGUI lastHighScore;

	[SerializeField] private MainMenu mainMenu;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else Instance = this;
	}

	private void Start()
    {		
		SaveData.Instance.Load();
    }

    private void Update()
    {
		mainMenu = FindObjectOfType(typeof(MainMenu)) as MainMenu;
	}

    public void SetScore( int score )
	{
		string name = mainMenu.nameInputText.text;
		Debug.Log(name);
		resultName.text = name.ToString();
		resultScore.text = "Score: " + score.ToString();
		SetHighScore(score);
	}

	public void SetHighScore(int currentScore)
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
