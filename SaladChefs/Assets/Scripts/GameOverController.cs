using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    public GameObject gameOverScreen;

    public Text winnerName;
    public Image winnerImage;
    public GameObject winnersImage;

    public Sprite[] playerImages;

    public GameOverRestartGameEvent RestartGameEvent
    {
        get;
        private set;
    }

    public GameOverShowLeaderboardEvent ShowLeaderboardEvent
    {
        get;
        private set;
    }

    // Use this for initialization
    void Start()
    {
        RestartGameEvent = new GameOverRestartGameEvent();
        ShowLeaderboardEvent = new GameOverShowLeaderboardEvent();
    }

    public void Show(PlayerScore playerScore)
    {
        gameOverScreen.SetActive(true);
        winnerImage.gameObject.SetActive(true);
        winnersImage.SetActive(false);
        winnerName.text = playerScore.PlayerName;
        winnerImage.sprite = playerImages[playerScore.ChefIndex];
    }

    public void ShowTie()
    {
        gameOverScreen.SetActive(true);
        winnerImage.gameObject.SetActive(false);
        winnersImage.SetActive(true);
        winnerName.text = "It's a tie!!!";
    }

    public void OnGameRestart()
    {
        gameOverScreen.SetActive(false);
        RestartGameEvent.Dispatch();
    }

    public void OnShowLeaderboard()
    {
        gameOverScreen.SetActive(false);
        ShowLeaderboardEvent.Dispatch();
    }
}
