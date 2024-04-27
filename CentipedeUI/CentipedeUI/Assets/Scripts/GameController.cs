using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{    
    [SerializeField]
    GameObject gameplay;

    [SerializeField, Header("Objects References")]
    CentipedeManager _centipedeManager;

    [SerializeField]
    MushroomField field;

    [SerializeField]
    GameObject player;

    [SerializeField]
    Result resultsScreen;
    public GameObject results;

    [SerializeField]
    TextMeshProUGUI score_text;

    [SerializeField]
    TextMeshProUGUI highscore_text;

    public GameObject[] handObjects;

    int score;

    void Start()
    {
        _centipedeManager.SpawnCentipedes();
        SetHighScore();
    }

    private void Update()
    {
        if (MainMenu.Instance.isPaused)
        {
            foreach (GameObject obj in handObjects)
            {
                obj.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject obj in handObjects)
            {
                obj.SetActive(false);
            }
        }
    }

    void OnEnable()
    {
        GameEvents.GameOverEvent += PlayerTouched;
        GameEvents.IncreaseScore += SetScore;
        GameEvents.NextLevel += NextLevel;
    }

    void OnDestroy()
    {
        GameEvents.GameOverEvent -= PlayerTouched;
        GameEvents.IncreaseScore -= SetScore;
        GameEvents.NextLevel -= NextLevel;
    }

    void SetHighScore()
    {
        int lastScore = PlayerPrefs.GetInt("HighScore");

        highscore_text.text = lastScore.ToString(); 
    }

    public void NextLevel()
    {
        _centipedeManager.UpdateDifficulty();
        field.UpdateDifficulty();

        _centipedeManager.SpawnCentipedes();
    }

    public void LoadMainMenu() => SceneManager.LoadScene(0);

    void PlayerTouched()
    {
        GameOver(score);
    }

    public void GameOver(int s)
    {
        gameplay.SetActive(false);
        player.SetActive(false);
        results.SetActive(true);
        resultsScreen.gameObject.SetActive( true );
        resultsScreen.SetScore(s);
    }

    public void SetScore(int i)
    {
        score += i;
        score_text.text = score.ToString();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}