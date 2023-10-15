using UnityEngine;

using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject[] chefs; // not prefabs
    public Transform[] chefStartingPositions;

    public GameObject[] guestPrefabs;
    public Transform[] guestSpawnPoints;
    public Transform[] guestSaladSpawnPoints;

    public GameObject saladPlatePrefab;
    public GameObject[] powerUpPrefabs;

    public PowerUpSpawnData powerUpSpawnData;

    public TutorialController tutorialController;
    public HUDController hudController;
    public LeaderboardController leaderboardController;
    public GameOverController gameOverController;

    public float guestSpawnInterval = 2;

    DataManager _dataManager;

    CustomQueue<int> _vacantSeats;

    GameObject[] _spawnedGuests;

    GameObject[] _activePowerUps;

    PlayerScore[] _finalScores;

    bool _isGameRunning;

    int _chefTimeOutStatus;

    WaitForSeconds _guestSpawnDuration;

    void Start()
    {
        _dataManager = new DataManager();
        _guestSpawnDuration = new WaitForSeconds(guestSpawnInterval);
        _spawnedGuests = new GameObject[guestSpawnPoints.Length];
        _vacantSeats = new CustomQueue<int>();
        _finalScores = new PlayerScore[2];

        int chefTimeOutStatus = 0;
        chefTimeOutStatus |= 1;
        chefTimeOutStatus |= 2;

        hudController.GameManager = this;
        leaderboardController.DismissedEvent.gameManager = this;
        ShowTutorial();
    }

    void ShowTutorial()
    {
        tutorialController.DismissedEvent.GameManager = this;
        tutorialController.ShowNext();
    }

    void StartGame()
    {
        hudController.Show(true);
        _vacantSeats = new CustomQueue<int>();
        for(int i = 0; i < guestSpawnPoints.Length; i++)
        {
            _vacantSeats.Enqueue(i);
        }
        SpawnChefs();
        _isGameRunning = true;
        StartCoroutine(SpawnGuests(false));
    }

    void SpawnChefs()
    {
        for(int i = 0; i < chefs.Length; i++)
        {
            chefs[i].SetActive(true);

            chefs[i].GetComponent<CharacterController>().enabled = false;
            chefs[i].transform.position = chefStartingPositions[i].position;
            chefs[i].transform.rotation = chefStartingPositions[i].rotation;
            chefs[i].GetComponent<CharacterController>().enabled = true;

            ChefController currentChefController = chefs[i].GetComponent<ChefController>();
            currentChefController.Init(i);
            currentChefController.TakeNewSaladPlate(Instantiate(saladPlatePrefab));
            currentChefController.InteractionStartedEvent.gameManager = this;
            currentChefController.InteractionEndedEvent.gameManager = this;
            currentChefController.OrderTakenEvent.gameManager = this;
            currentChefController.ActionEvent.gameManager = this;
            currentChefController.PowerUpCollectedEvent.gameManager = this;
            currentChefController.SaladTrashedEvent.gameManager = this;
            currentChefController.PowerUpUsedEvent.gameManager = this;
        }
    }

    IEnumerator SpawnGuests(bool notify)
    {
        int seatIndex = -1;
        while (_isGameRunning)
        {
            if (_vacantSeats.Size > 0)
            {
                seatIndex = _vacantSeats.Remove(Random.Range(0, _vacantSeats.Size));
                int characterToSpawnIndex = Random.Range(0, guestPrefabs.Length);
                GameObject spawnedGuest = Instantiate(guestPrefabs[characterToSpawnIndex]);
                spawnedGuest.transform.parent = guestSpawnPoints[seatIndex];
                spawnedGuest.transform.position = guestSpawnPoints[seatIndex].position;
                spawnedGuest.transform.rotation = guestSpawnPoints[seatIndex].rotation;

                GuestController spawnedGuestController = spawnedGuest.GetComponent<GuestController>();
                spawnedGuestController.OrderReceivedEvent.gameManager = this;
                spawnedGuestController.TimeOutEvent.gameManager = this;
                spawnedGuestController.guestCharacterIndex = characterToSpawnIndex;
                spawnedGuestController.seatIndex = seatIndex;
                spawnedGuestController.seatSaladSpawnPoint = guestSaladSpawnPoints[seatIndex];
                _spawnedGuests[seatIndex] = spawnedGuest;
                if (notify)
                {
                    hudController.ShowPopupMessage("New guest just popped in.", true);
                }
                notify = true;
            }

            if (_vacantSeats.Size == 0)
            {
                _vacantSeats = new CustomQueue<int>();
            }
            yield return _guestSpawnDuration;
        }
        yield break;
    }

    void Reset()
    {
        for(int i = 0; i < _spawnedGuests.Length; i++)
        {
            if(_spawnedGuests[i] != null && _spawnedGuests[i].activeSelf)
            {
                Destroy(_spawnedGuests[i]);
            }
        }
    }

    void OnGameOver()
    {
        _isGameRunning = false;

        Reset();

        hudController.Hide();

        gameOverController.RestartGameEvent.GameManager = this;
        gameOverController.ShowLeaderboardEvent.GameManager = this;

        if(_finalScores[0].Score > _finalScores[1].Score)
        {
            _dataManager.SaveScore(_finalScores[0]);
            gameOverController.Show(_finalScores[0]);
        }
        else if(_finalScores[1].Score > _finalScores[0].Score)
        {
            _dataManager.SaveScore(_finalScores[1]);
            gameOverController.Show(_finalScores[1]);
        }
        else
        {
            gameOverController.ShowTie();
        }
    }

    public void HandleChefInteractionStarted(Interactable interactable, string actionHint, int chefIndex)
    {
        hudController.ShowHint(chefIndex, actionHint, false);
    }

    public void HandleChefInteractionEnded(Interactable interactable, int chefIndex)
    {
        hudController.ClearHint(chefIndex);
    }

    public void HandleChefAction(Interactable interactable, int chefIndex, KeyCode key, string feedbackMessage)
    {
        if(!string.IsNullOrEmpty(feedbackMessage))
        {
            hudController.ShowHint(chefIndex, feedbackMessage, true);
        }
    }

    public void HandleChefOrderTaken(SaladData salad, int chefIndex)
    {
        hudController.ShowRecipe(chefIndex, salad);
    }

    public void HandleChefSaladTrashed(int noOfIngredients, int chefIndex)
    {
        chefs[chefIndex].GetComponent<ChefController>().ClearAndPutbackSaladPlate();
        int trashPenaltyPoints = noOfIngredients * -10;
        hudController.AddPoints(trashPenaltyPoints, chefIndex);
    }

    public void HandleChefPowerUpCollected(PowerUpController powerUpController)
    {
        switch (powerUpController.powerUpType)
        {
            case PowerUp.Points:
                hudController.AddPoints((int)powerUpController.Value, powerUpController.chefIndex);
                break;

            case PowerUp.Speed:
                chefs[powerUpController.chefIndex].GetComponent<ChefController>().BoostSpeed(powerUpController.Value);
                hudController.ShowSpeedBoost(powerUpController.chefIndex);
                break;

            case PowerUp.Time:
                hudController.AddTime((int)powerUpController.Value, powerUpController.chefIndex);
                break;
        }

        if (powerUpController.gameObject.activeSelf)
        {
            Destroy(powerUpController.gameObject);
        }
    }

    public void HandleChefPowerUpUsed(PowerUp powerUp, int chefIndex)
    {
        if(powerUp == PowerUp.Speed)
        {
            hudController.HideSpeedBoost(chefIndex);
        }
    }

    public void HandleScoreboardTimedOut(PlayerScore finalScore)
    {
        _finalScores[finalScore.ChefIndex] = finalScore;
        Destroy(chefs[finalScore.ChefIndex].GetComponent<ChefController>().SaladPlate);
        chefs[finalScore.ChefIndex].SetActive(false);
        _chefTimeOutStatus |= finalScore.ChefIndex + 1;

        if (_chefTimeOutStatus == (int)(Mathf.Pow(2, chefs.Length) - 1))
        {
            if(_activePowerUps != null) // Destroy all active powerups.
            {
                for(int i = 0; i < chefs.Length; i++)
                {
                    if(_activePowerUps[i] != null && _activePowerUps[i].activeSelf)
                    {
                        //Debug.Log("Cleaning up spawned powerup.");
                        Destroy(_activePowerUps[i]);
                        //Debug.Log("Cleaned up powerup.");
                    }
                }
            }
            OnGameOver();
        }
    }

    public void HandleLeaderboardDismissed()
    {
        OnGameOver();
    }

    public void HandleGuestOrderReceived(int servedChefIndex, bool hasFulfiled, PowerUp tip, int tipValue, int seatIndex)
    {
        if(hasFulfiled)
        {
            if (tipValue > 0)
            {
                hudController.ShowPopupMessage("Fantastic!!! Chef " + servedChefIndex.ToString() + " rocks. Now collect your tip ASAP", true);
            }
            else
            {
                hudController.ShowPopupMessage("Good job Chef " + servedChefIndex.ToString(), true);
            }
            SpawnPowerUp(servedChefIndex, (int)tip, 50);
            hudController.ClearRecipe(servedChefIndex);
            int points = chefs[servedChefIndex].GetComponent<ChefController>().SaladPlate.GetComponent<SaladPlateController>().NoOfIngredientsInPlate * 25;
            StartCoroutine(RemoveGuest(seatIndex, 4));
            chefs[servedChefIndex].GetComponent<ChefController>().TakeNewSaladPlate(Instantiate(saladPlatePrefab));
            chefs[servedChefIndex].GetComponent<ChefController>().CurrentServingSeatIndex = 0;
            hudController.AddPoints(points, servedChefIndex);
            //Debug.Log("Well done chef " + servedChefIndex.ToString());
        }
        else
        {
            hudController.ShowPopupMessage("Oh no!!! Chef " + servedChefIndex.ToString() + " just served a wrong salad. You're BOOKED.", false);
            hudController.AddPoints(-50, servedChefIndex);
        }
    }

    void SpawnPowerUp(int chefIndex, int powerUpIndex, int value)
    {
        if(_activePowerUps == null)
        {
            _activePowerUps = new GameObject[2];
        }
        GameObject powerUpGameObject = Instantiate(powerUpPrefabs[powerUpIndex]);
        _activePowerUps[chefIndex] = powerUpGameObject;
        SpawnRangeData spawnRangeData = powerUpSpawnData.SpawnAreas[Random.Range(0, powerUpSpawnData.SpawnAreas.Count)];
        float xPos = Random.Range(spawnRangeData.StartX, spawnRangeData.EndX);
        float yPos = 1;
        float zPos = Random.Range(spawnRangeData.StartZ, spawnRangeData.EndZ);
        powerUpGameObject.transform.position = new Vector3(xPos, yPos, zPos);
        powerUpGameObject.transform.rotation = Quaternion.identity;
        powerUpGameObject.GetComponent<PowerUpController>().chefIndex = chefIndex;
        powerUpGameObject.GetComponent<PowerUpController>().Value = value;
    }

    IEnumerator RemoveGuest(int seatIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        _vacantSeats.Enqueue(seatIndex);
        if (_spawnedGuests[seatIndex].GetComponent<GuestController>().SaladPlate != null)
        {
            Destroy(_spawnedGuests[seatIndex].GetComponent<GuestController>().SaladPlate);
        }
        Destroy(_spawnedGuests[seatIndex]);
    }

    public void HandleGuestTimedOut(int seatIndex)
    {
        if(!_isGameRunning)
        {
            return;
        }

        StartCoroutine(RemoveGuest(seatIndex, 0));

        for(int i = 0; i < chefs.Length; i++)
        {
            if(chefs[i].GetComponent<ChefController>().CurrentServingSeatIndex == seatIndex)
            {
                hudController.ClearRecipe(i);
                chefs[i].GetComponent<ChefController>().ResetCurrentPreparation();
            }

            hudController.AddPoints(-10, 0);
            hudController.AddPoints(-10, 1);
        }

        hudController.ShowPopupMessage("One of our guest left unhappy. Both of you get penalty points.", false);
    }

    public void HandleShowLeaderboard()
    {
        leaderboardController.Show(_dataManager.GetPlayerScoreData());
    }

    public void HandleRestartGame()
    {
        StartGame();
        //Debug.Log("Start new game after Game Over.");
    }

    public void HandleTutorialCompleted()
    {
        //Debug.Log("Training Over. Go to gameplay");
        StartGame();
    }
}
