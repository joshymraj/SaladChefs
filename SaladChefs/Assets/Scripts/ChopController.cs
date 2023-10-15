using UnityEngine;
using UnityEngine.UI;

public class ChopController : MonoBehaviour
{
    public GameObject chopStatusHUD;
    public Slider choppingStatusSlider;
    public Image choppingIngredientImage;
    public ChoppingBoardController choppingBoardController;
    public ChoppingPlateController choppingPlateController;

    [HideInInspector]
    public SaladPlateController saladPlateController;

    bool _chop;
    float _currentItemChoppingTime;
    float _currentItemChoppingTimer;

    IngredientData _currentChoppingItem;

    public ChoppingCompletedEvent ChoppingCompletedEvent
    {
        get;
        private set;
    }

    void Awake()
    {
        ChoppingCompletedEvent = new ChoppingCompletedEvent();
    }

    // Use this for initialization
    void Start()
    {
        _currentItemChoppingTime = 0;
        _currentItemChoppingTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(_chop)
        {
            _currentItemChoppingTimer += Time.deltaTime;
            choppingStatusSlider.value = _currentItemChoppingTimer / _currentItemChoppingTime;
            if(_currentItemChoppingTimer > _currentItemChoppingTime)
            {
                _chop = false;
                OnChoppingCompleted();
            }
        }
    }

    public void ShowHUD()
    {
        choppingIngredientImage.sprite = _currentChoppingItem.Image;
        chopStatusHUD.SetActive(true);
    }

    public void HideHUD()
    {
        chopStatusHUD.SetActive(false);
    }

    public void AddToChoppingPlate(Ingredient rawIngredient)
    {
        choppingPlateController.Add(rawIngredient);
    }

    public bool AddToChoppingBoard()
    {
        if(choppingPlateController.HasIngredientsInChoppingPlate)
        {
            _currentChoppingItem = choppingBoardController.Add(choppingPlateController.Take());
            choppingStatusSlider.value = 0;
            return true;
        }
        return false;
    }

    public void Chop()
    {
        _currentItemChoppingTime = _currentChoppingItem.ChopTime;
        _currentItemChoppingTimer = 0;
        _chop = true;
    }

    public void AddToSaladPlate(Ingredient choppedIngredient)
    {
        saladPlateController.Add(choppedIngredient);
    }

    public void ClearChoppingPlate()
    {
        choppingPlateController.Clear();
    }

    public void ClearChoppingBoard()
    {
        choppingBoardController.Clear();
    }

    public void ClearSaladPlate()
    {
        saladPlateController.Clear();
    }

    void OnChoppingCompleted()
    {
        choppingBoardController.Remove();
        ChoppingCompletedEvent.Dispatch(_currentChoppingItem.IngredientName);
    }
}
