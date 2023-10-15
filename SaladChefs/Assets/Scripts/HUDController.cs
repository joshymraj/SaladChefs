using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public GameObject scoreboardHUD;

    public ScoreboardController[] scoreboardControllers;

    public PopupNotificationController popupNotificationController;

    public float hintMessageAutoHideDuration = 2f;

    public int playTime = 300; // in seconds

    public GameManager GameManager
    {
        get;
        set;
    }

    public void Show(bool clearAll)
    {
        scoreboardHUD.SetActive(true);
        for (int i = 0; i < scoreboardControllers.Length; i++)
        {
            scoreboardControllers[i].ScoreboardTimedOutEvent.gameManager = GameManager;
            scoreboardControllers[i].ChefIndex = i;
            scoreboardControllers[i].InitializeScore(0, playTime);
        }
    }

    public void AddPoints(int points, int chefIndex)
    {
        scoreboardControllers[chefIndex].AddPoints(points);
    }

    public void AddTime(int time, int chefIndex)
    {
        scoreboardControllers[chefIndex].AddTime(time);
    }

    public void ShowSpeedBoost(int chefIndex)
    {
        scoreboardControllers[chefIndex].ShowSpeedBoost();
    }

    public void HideSpeedBoost(int chefIndex)
    {
        scoreboardControllers[chefIndex].HideSpeedBoost();
    }

    public void ShowPopupMessage(string message, bool isGoodNewsMessage)
    {
        popupNotificationController.Show(message, isGoodNewsMessage);
    }

    public void ShowHint(int chefIndex, string message , bool autoHide)
    {
        scoreboardControllers[chefIndex].ShowHint(message, autoHide);
    }

    public void ShowRecipe(int chefIndex, SaladData salad)
    {
        scoreboardControllers[chefIndex].ShowRecipe(salad);
    }

    public void ClearRecipe(int chefIndex)
    {
        scoreboardControllers[chefIndex].ClearRecipe();
    }

    public void ClearHint(int chefIndex)
    {
        scoreboardControllers[chefIndex].ClearHint();
    }

    public void Hide()
    {
        scoreboardHUD.SetActive(false);
        popupNotificationController.Hide();
    }
}
