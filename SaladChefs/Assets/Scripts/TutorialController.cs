using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public GameObject[] tutorialScreens;

    int _activeScreenIndex;

    public TutorialDismissedEvent DismissedEvent
    {
        get;
        private set;
    }

    void Awake()
    {
        _activeScreenIndex = -1;
        DismissedEvent = new TutorialDismissedEvent();
    }

    public void ShowNext()
    {
        if(_activeScreenIndex > -1)
        {
            tutorialScreens[_activeScreenIndex].SetActive(false);
        }
        _activeScreenIndex += 1;
        tutorialScreens[_activeScreenIndex].SetActive(true);
    }

    public void OnDismissed() // UIButton click event
    {
        DismissedEvent.Dispatch();
        tutorialScreens[tutorialScreens.Length - 1].SetActive(false);
    }
}
