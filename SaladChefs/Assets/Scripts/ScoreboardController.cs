using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class ScoreboardController : MonoBehaviour
{
    public GameObject hintIcon;
    public Text hintMessage;
    public Text scoreText;
    public Text timeText;
    public GameObject speedBoostIndicator;

    public float hintAutoHideDuration = 2; // in seconds
    public Text selectedRecipeText;
    public Text noRecipeSelectedText;

    public GameObject[] recipeCombos;

    //public RecipeComboHUD[] recipeComboHUDs;

    public int ChefIndex
    {
        get;
        set;
    }

    public ScoreboardTimedOutEvent ScoreboardTimedOutEvent
    {
        get;
        private set;
    }

    WaitForSeconds _scoreTimer;

    int _timeLeft;

    void Awake()
    {
        ScoreboardTimedOutEvent = new ScoreboardTimedOutEvent();
        _scoreTimer = new WaitForSeconds(1);   
    }

    IEnumerator UpdateTime()
    {
        while (_timeLeft > 0)
        {
            yield return _scoreTimer;
            _timeLeft -= 1;
            timeText.text = _timeLeft.ToString();
        }
        hintIcon.SetActive(false);
        hintMessage.text = "Timed Out";
        hintMessage.fontSize = 20;
        hintMessage.alignment = TextAnchor.MiddleCenter;
        OnTimedOut();
        yield break;
    }

    public void InitializeScore(int points, int time)
    {
        scoreText.text = points.ToString();
        timeText.text = time.ToString();
        _timeLeft = time;
        ClearHint();
        hintMessage.alignment = TextAnchor.MiddleLeft;
        StartCoroutine(UpdateTime());
    }

    public void FinalizeScore()
    {
        StopAllCoroutines();
    }

    public void AddPoints(int points)
    {
        StartCoroutine(AnimateScore(points, 2 / (float)points));
    }

    IEnumerator AnimateScore(int points, float delay)
    {
        int currentPoints = int.Parse(scoreText.text);
        int finalPoints = currentPoints + points;
        while(currentPoints != finalPoints)
        {
            currentPoints += (int)Mathf.Sign(points);
            yield return new WaitForSeconds(delay);
            scoreText.text = currentPoints.ToString();
        }
    }

    public void AddTime(int time)
    {
        _timeLeft += time;
        timeText.text = _timeLeft.ToString();
    }

    public void ShowSpeedBoost() => speedBoostIndicator.SetActive(true);

    public void HideSpeedBoost() => speedBoostIndicator.SetActive(false);

    public void ShowHint(string message, bool autoHide)
    {
        if(string.IsNullOrEmpty(message))
        {
            return;
        }

        hintIcon.SetActive(true);
        hintMessage.text = message;
        if(autoHide)
        {
            StartCoroutine(HideHint());
        }
    }

    IEnumerator HideHint()
    {
        yield return new WaitForSeconds(hintAutoHideDuration);
        hintIcon.SetActive(false);
        hintMessage.text = string.Empty;
    }

    public void ClearHint()
    {
        hintIcon.SetActive(false);
        hintMessage.text = string.Empty;
    }

    public void ShowRecipe(SaladData saladData)
    {
        for (int i = 0; i < recipeCombos.Length; i++)
        {
            recipeCombos[i].SetActive(false);
        }

        noRecipeSelectedText.gameObject.SetActive(false);
        selectedRecipeText.gameObject.SetActive(true);
        selectedRecipeText.text = saladData.SaladName.ToString();

        recipeCombos[saladData.Ingredients.Length - 2].SetActive(true);

        RecipeComboHUD selectedRecipeHUD = recipeCombos[saladData.Ingredients.Length - 2].GetComponent<RecipeComboHUD>();

        selectedRecipeHUD.Show(saladData.Ingredients);
    }

    public void ClearRecipe()
    {
        for (int i = 0; i < recipeCombos.Length; i++)
        {
            recipeCombos[i].SetActive(false);
        }

        noRecipeSelectedText.gameObject.SetActive(true);
        selectedRecipeText.gameObject.SetActive(false);
    }

    void OnTimedOut()
    {
        PlayerScore finalScore = new PlayerScore
        {
            ChefIndex = ChefIndex,
            PlayerName = string.Concat("Chef ", (ChefIndex + 1).ToString()),
            Rank = 0,
            Score = int.Parse(scoreText.text)
        };

        ScoreboardTimedOutEvent.Dispatch(finalScore);
    }
}
