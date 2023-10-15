using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class GuestController : MonoBehaviour
{
    public MenuData menu;

    public PowerUp[] tipTypes;

    public GameObject waitingStatusHUD;

    public Slider waitingStatusSlider;

    public float waitClockSpeed = 5;

    [Range(0, 1)]
    public float _patienceFactor = 1;

    public float serveTime = 15;

    [HideInInspector]
    public int guestCharacterIndex; // to be set by game manager to track which guest character is this.

    [HideInInspector]
    public int seatIndex; // to be set by game manager

    [HideInInspector]
    public Transform seatSaladSpawnPoint; // to be set by game manager.

    public GameObject SaladPlate // to be set by chef controller.
    {
        get;
        private set;
    }

    public SaladData SelectedSalad
    {
        get;
        private set;
    }

    public GuestOrderReceivedEvent OrderReceivedEvent
    {
        get;
        private set;
    }

    public GuestTimedOutEvent TimeOutEvent
    {
        get;
        private set;
    }

    float _actualWaitingTime;

    float _currentWaitingTime;

    Animator _animator;

    void Awake()
    {
        OrderReceivedEvent = new GuestOrderReceivedEvent();
        TimeOutEvent = new GuestTimedOutEvent();
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        if (gameObject.activeSelf)
        {
            PlaceOrder();
        }
    }

    void PlaceOrder()
    {
        _patienceFactor = 1;
        float totalChopTime = 0;
        SelectedSalad = menu.Salads[Random.Range(0, menu.Salads.Count)];
        foreach (IngredientData saladIngredient in SelectedSalad.Ingredients)
        {
            totalChopTime += saladIngredient.ChopTime;
        }
        _actualWaitingTime = totalChopTime + serveTime;
        _currentWaitingTime = _actualWaitingTime;

        //Debug.Log(totalChopTime.ToString() + " + " + serveTime.ToString() + " = Waiting time of guest at seat #" + seatIndex.ToString() + " is " + _actualWaitingTime + " seconds.");

        StartCoroutine(GuestUpdate());
    }

    IEnumerator GuestUpdate()
    {
        while(_currentWaitingTime > 0)
        {
            _currentWaitingTime -= Time.deltaTime * waitClockSpeed * _patienceFactor;
            waitingStatusSlider.value = _currentWaitingTime / _actualWaitingTime;
            yield return null;
        }

        OnTimedOut();
        yield break;
    }

    public void ReceiveOrder(Ingredient[] ingredients, int servedChefIndex, GameObject saladPlate)
    {
        if(_currentWaitingTime <= 0)
        {
            return;
        }

        SaladPlate = saladPlate;
        bool hasFulfiled = false;
        PowerUp tip = PowerUp.Points;
        int tipValue = 0;
        bool hasFoundIngredient = false;

        if (ingredients.Length == SelectedSalad.Ingredients.Length)
        {
            for (int i = 0; i < SelectedSalad.Ingredients.Length; i++)
            {
                hasFoundIngredient = false;

                for(int j = 0; j < ingredients.Length; j++)
                {
                    if(ingredients[j] == SelectedSalad.Ingredients[i].IngredientName)
                    {
                        hasFoundIngredient = true;
                        break;
                    }
                }

                if(!hasFoundIngredient)
                {
                    break;
                }
            }

            hasFulfiled = hasFoundIngredient;
        }

        if(hasFulfiled)
        {
            int timeLeft = (int)(_actualWaitingTime - _currentWaitingTime);
            float percentTimeLeft = timeLeft / _actualWaitingTime;
            if(percentTimeLeft >= 20)
            {
                tip = PowerUp.Speed;
                tipValue = (int)(SelectedSalad.Ingredients.Length * percentTimeLeft);
            }
            else
            {
                tip = tipTypes[Random.Range(0, tipTypes.Length)];
                switch (tip)
                {
                    case PowerUp.Points:
                        tipValue = SelectedSalad.Ingredients.Length * timeLeft;
                        break;

                    case PowerUp.Time:
                        tipValue = (int)(SelectedSalad.Ingredients.Length * percentTimeLeft * 10);
                        break;

                    case PowerUp.Speed:
                        tipValue = SelectedSalad.Ingredients.Length * 10;
                        break;
                }
            }

            StopAllCoroutines();
            waitingStatusHUD.SetActive(false);
        }

        OnOrderReceived(servedChefIndex, hasFulfiled, tip, tipValue);
    }

    void OnOrderReceived(int servedChefIndex, bool isSatisfied, PowerUp tip, int tipValue)
    {
        //Debug.Log("Guest at seat " + seatIndex.ToString() + " received order from chef " + servedChefIndex.ToString());
        OrderReceivedEvent.Dispatch(servedChefIndex, isSatisfied, tip, tipValue, seatIndex);
    }

    void OnTimedOut()
    {
        //Debug.Log("Guest at seat " + seatIndex.ToString() + " has fed up waiting and is leaving...");
        TimeOutEvent.Dispatch(seatIndex);
    }
}
