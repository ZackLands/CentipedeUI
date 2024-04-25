using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField, Header("Canvas Elements")]
    GameObject mainMenu;

    [SerializeField]
    GameObject gameplay;

    [SerializeField]
    InputField nameField;

    [SerializeField, Header("Objects References")]
    CentipedeManager _centipedeManager;

    [SerializeField]
    MushroomField field;

    [SerializeField]
    GameObject player;

    [SerializeField]
    Result resultsScreen;

    [SerializeField]
    Text score_text;

    int score;

    void Start()
    {
        player.SetActive(false);
        if (PlayerPrefs.HasKey("Name"))
        {
            nameField.text = PlayerPrefs.GetString("Name");
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

    public void Play()
    {
        player.SetActive(true);
        _centipedeManager.SpawnCentipedes();
        mainMenu.SetActive(false);
        gameplay.SetActive(true);
    }

    public void NextLevel()
    {
        _centipedeManager.UpdateDifficulty();
        field.UpdateDifficulty();

        _centipedeManager.SpawnCentipedes();
    }

    void PlayerTouched()
    {
        GameOver(score);
    }

    public void GameOver(int s)
    {
        gameplay.SetActive(false);
        player.SetActive(false);
        resultsScreen.gameObject.SetActive( true );
        resultsScreen.SetScore(s);
    }

    public void SetScore(int i)
    {
        score += i;
        score_text.text = score.ToString();
    }

    public void OnRetryPressed()
    {
        // Don't bother resetting everything carefully, just reload!
        SceneManager.LoadScene(0);
    }
}