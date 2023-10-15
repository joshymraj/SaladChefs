using UnityEngine;

public class LeaderboardController : MonoBehaviour
{
    public GameObject leaderboardScreen;

    public GameObject leaderboardListItemPrefab;

    public RectTransform leaderboardScrollViewContent;

    public Transform leaderboardListItemSpawnPoint;

    public Sprite[] chefImages;

    public float leaderboardListItemHeight;

    public LeaderboardDismissedEvent DismissedEvent
    {
        get;
        private set;
    }

    GameObject[] listItems;

    void Awake()
    {
        DismissedEvent = new LeaderboardDismissedEvent();
    }

    void LoadScores(PlayerScoreData playerScoreData)
    {
        if(playerScoreData.scores == null)
        {
            return;
        }

        leaderboardScrollViewContent.sizeDelta = new Vector2(0, playerScoreData.scores.Length * leaderboardListItemHeight);

        listItems = new GameObject[playerScoreData.scores.Length];

        for (int i = 0; i < playerScoreData.scores.Length; i++)
        {
            float spawnY = i * leaderboardListItemHeight;

            float correction = 200 - leaderboardListItemSpawnPoint.position.x;

            Vector3 itemPosition = new Vector3(leaderboardListItemSpawnPoint.position.x + correction, -spawnY, leaderboardListItemSpawnPoint.position.z);

            GameObject listItem = Instantiate(leaderboardListItemPrefab, itemPosition, leaderboardListItemSpawnPoint.rotation);

            listItems[i] = listItem;

            RectTransform rectTransform = listItem.GetComponent<RectTransform>();

            listItem.transform.SetParent(leaderboardListItemSpawnPoint, false);

            LeaderboardListItem leaderboardListItem = listItem.GetComponent<LeaderboardListItem>();

            PlayerScore playerScore = playerScoreData.scores[i];

            leaderboardListItem.rank.text = playerScore.Rank.ToString();
            leaderboardListItem.playerAvatar.sprite = chefImages[playerScore.ChefIndex];
            leaderboardListItem.name.text = playerScore.PlayerName;
            leaderboardListItem.score.text = playerScore.Score.ToString();
        }
    }

    public void Show(PlayerScoreData playerScoreData)
    {
        leaderboardScreen.SetActive(true);
        LoadScores(playerScoreData);
    }

    public void OnDismissed()
    {
        if(listItems != null)
        {
            int totalListItems = listItems.Length;
            for (int i = 0; i < totalListItems; i++)
            {
                Destroy(listItems[i]);
            }
        }
        leaderboardScreen.SetActive(false);
        DismissedEvent.Dispatch();
    }
}

public class PlayerScore
{
    public int Rank;
    public int ChefIndex;
    public string PlayerName;
    public int Score;
}

public class PlayerScoreData
{
    public PlayerScore[] scores;
}
