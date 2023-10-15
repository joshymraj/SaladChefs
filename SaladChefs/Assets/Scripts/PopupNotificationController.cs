using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupNotificationController : MonoBehaviour
{
    public GameObject popupWindow;
    public Image popupBg;
    public Text popupMessage;
    public float displayDuration = 1;
    public Color goodNewsBgColor;
    public Color badNewsBgColor;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ShowPopup(string message)
    {
        popupWindow.SetActive(true);
        popupMessage.text = message;
        yield return new WaitForSeconds(displayDuration);
        popupWindow.SetActive(false);
    }

    public void Show(string message, bool isGoodNews)
    {
        popupBg.color = isGoodNews ? goodNewsBgColor : badNewsBgColor;
        StartCoroutine(ShowPopup(message));
    }

    public void Hide()
    {
        popupWindow.SetActive(false);
    }
}
