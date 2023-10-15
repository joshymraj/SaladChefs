using UnityEngine;

using System.Collections;

public class ChefController : MonoBehaviour
{
    [HideInInspector]
    public int chefIndex;

    [HideInInspector]
    public int points;

    [HideInInspector]
    public int time;

    public GameObject SaladPlate // to be set by game manager
    {
        get;
        private set;
    }

    public int CurrentServingSeatIndex
    {
        get;
        set;
    }

    public GameObject knife;

    public Transform knifeInHandPosition;
    public Transform knifeOnTablePosition;
    public Transform saladPlateInHandPosition;
    public Transform saladPlateOnTablePosition;
    public Transform chopPosition;

    public Animator animator;

    PlayerMotionController playerMotionController;
    ChopController chopController;
    PickupController pickupController;
    SaladPlateController saladPlateController;
    JoystickController joystickController;

    Interactable _currentInteractable;
    GuestController _currentInteractingGuest;
    IngredientData _currentInteractingIngredient;

    public ChefState State
    {
        get;
        private set;
    }

    public ChefInteractionStartedEvent InteractionStartedEvent
    {
        get;
        private set;
    }

    public ChefInteractionEndedEvent InteractionEndedEvent
    {
        get;
        private set;
    }

    public ChefActionEvent ActionEvent
    {
        get;
        private set;
    }

    public ChefOrderTakenEvent OrderTakenEvent
    {
        get;
        private set;
    }

    public ChefPowerUpCollectedEvent PowerUpCollectedEvent
    {
        get;
        private set;
    }

    public ChefPowerUpUsedEvent PowerUpUsedEvent
    {
        get;
        private set;
    }

    public ChefSaladTrashedEvent SaladTrashedEvent
    {
        get;
        private set;
    }

    void Awake()
    {
        State = ChefState.Idle;
        _currentInteractable = Interactable.None;
        InteractionStartedEvent = new ChefInteractionStartedEvent();
        InteractionEndedEvent = new ChefInteractionEndedEvent();
        OrderTakenEvent = new ChefOrderTakenEvent();
        ActionEvent = new ChefActionEvent();
        PowerUpCollectedEvent = new ChefPowerUpCollectedEvent();
        PowerUpUsedEvent = new ChefPowerUpUsedEvent();
        SaladTrashedEvent = new ChefSaladTrashedEvent();
    }

    void Start()
    {
        chopController = GetComponent<ChopController>();
        chopController.ChoppingCompletedEvent.chefController = this;

        pickupController = GetComponent<PickupController>();
        playerMotionController = GetComponent<PlayerMotionController>();

        joystickController = GetComponent<JoystickController>();
        joystickController.ActionKeyPressedEvent.chefController = this;
    }

    public void Init(int index)
    {
        chefIndex = index;
        _currentInteractable = Interactable.None;
    }

    public void TakeNewSaladPlate(GameObject newSaladPlate)
    {
        SaladPlate = newSaladPlate;
        SaladPlate.transform.parent = saladPlateOnTablePosition;
        SaladPlate.transform.position = saladPlateOnTablePosition.position;
        SaladPlate.transform.rotation = saladPlateOnTablePosition.rotation;
        SaladPlate.transform.localScale = Vector3.one;

        saladPlateController = SaladPlate.GetComponent<SaladPlateController>();
        saladPlateController.chefIndex = chefIndex;
        chopController.saladPlateController = saladPlateController;
    }

    public void ClearAndPutbackSaladPlate()
    {
        if (SaladPlate.activeSelf)
        {
            SaladPlate.transform.parent = saladPlateOnTablePosition;
            SaladPlate.transform.position = saladPlateOnTablePosition.position;
            SaladPlate.transform.rotation = saladPlateOnTablePosition.rotation;
            SaladPlate.transform.localScale = Vector3.one;
            saladPlateController.Clear();
        }
    }

    public void ResetCurrentPreparation() //TODO : to be called on guest time out and guest order received.
    {
        if(State == ChefState.CarryingSalad)
        {
            State = ChefState.Idle;
            joystickController.Disabled = true;
            animator.SetFloat(AnimatorParameter.ChefMove, 0);
            animator.SetTrigger(AnimatorParameter.ChefDrop);
            joystickController.Disabled = false;
        }
        CurrentServingSeatIndex = 0;
        ClearAndPutbackSaladPlate();
        //TODO : clear recipe in hud.
    }

    IEnumerator BoostSpeedForSeconds(float duration)
    {
        playerMotionController.UpdateSpeedBoost(true);

        yield return new WaitForSeconds(duration);

        playerMotionController.UpdateSpeedBoost(false);

        OnPowerUpUsed(PowerUp.Speed);
    }

    public void BoostSpeed(float duration)
    {
        StartCoroutine(BoostSpeedForSeconds(duration));
    }

    void OnTriggerEnter(Collider other)
    {
        _currentInteractable = Interactable.None;
        switch (other.tag)
        {
            case "ChoppingBoard":
                _currentInteractable = Interactable.ChoppingBoard;
                break;

            case "SaladPlate":
                if (saladPlateController.NoOfIngredientsInPlate > 0)
                {
                    _currentInteractable = Interactable.SaladPlate;
                }
                break;

            case "Ingredient":
                _currentInteractable = Interactable.Ingredient;
                _currentInteractingIngredient = other.gameObject.GetComponent<IngredientInfo>().ingredientData;
                break;

            case "RecipeBook":
                _currentInteractable = Interactable.RecipeBook;
                break;

            case "Trash":
                _currentInteractable = Interactable.TrashCan;
                break;

            case "Guest":
                _currentInteractable = Interactable.Guest;
                _currentInteractingGuest = other.gameObject.GetComponent<GuestController>();
                break;

            case "PowerUp":
                CollectPowerup(other.gameObject.GetComponent<PowerUpController>());
                break;

            default:
                _currentInteractable = Interactable.None;
                break;
        }

        if(_currentInteractable != Interactable.None)
        {
            OnInteractionStarted();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (_currentInteractable != Interactable.None)
        {
            OnInteractionEnded();
        }

        switch (other.tag)
        {
            case "ChoppingBoard":
                _currentInteractable = Interactable.None;
                break;

            case "SaladPlate":
                _currentInteractable = Interactable.None;
                break;

            case "Ingredient":
                _currentInteractable = Interactable.None;
                _currentInteractingIngredient = null;
                break;

            case "RecipeBook":
                _currentInteractable = Interactable.None;
                break;

            case "Trash":
                _currentInteractable = Interactable.None;
                break;

            case "Guest":
                _currentInteractable = Interactable.None;
                _currentInteractingGuest = null;
                //Debug.Log("Interaction with " + other.gameObject.name + " ended");
                break;       
                   
            default:
                _currentInteractable = Interactable.None;
                break;
        }
    }

    void OnInteractionStarted()
    {
        System.Text.StringBuilder actionHint = new System.Text.StringBuilder();

        switch (_currentInteractable)
        {
            case Interactable.Ingredient:
                pickupController.ShowHUD();
                actionHint.Append("Press \'");
                actionHint.Append(joystickController.actionKey1.ToString());
                actionHint.Append("\' to pick and \'");
                actionHint.Append(joystickController.actionKey2.ToString());
                actionHint.Append("\' to drop picked item.");
                break;

            case Interactable.Guest:
                switch(State)
                {
                    case ChefState.Idle:
                        actionHint.Append("Press \'");
                        actionHint.Append(joystickController.actionKey1.ToString());
                        actionHint.Append("\' to take order.");
                        break;

                    case ChefState.CarryingSalad:
                        actionHint.Append("Press \'");
                        actionHint.Append(joystickController.actionKey1.ToString());
                        actionHint.Append("\' to serve salad.");
                        break;

                    default:
                        break;
                }
                break;

            case Interactable.ChoppingBoard:
                switch(State)
                {
                    case ChefState.Idle:
                        actionHint.Append("Press \'");
                        actionHint.Append(joystickController.actionKey1.ToString());
                        actionHint.Append("\' to start chopping.");
                        break;

                    case ChefState.CarryingIngredient:
                        actionHint.Append("Press \'");
                        actionHint.Append(joystickController.actionKey1.ToString());
                        actionHint.Append("\' to take ingredients out of cart.");
                        break;
                }
                break;

            case Interactable.SaladPlate:
                actionHint.Append("Press \'");
                actionHint.Append(joystickController.actionKey1.ToString());
                actionHint.Append("\' to take salad plate.");
                break;

            case Interactable.TrashCan:
                if (State == ChefState.CarryingSalad)
                {
                    actionHint.Append("Press \'");
                    actionHint.Append(joystickController.actionKey1.ToString());
                    actionHint.Append("\' to trash your preparation.");
                }
                break;

            default:
                break;
        }

        if (actionHint != null)
        {
            InteractionStartedEvent.Dispatch(_currentInteractable, actionHint.ToString(), chefIndex);
        }
    }

    void OnInteractionEnded()
    {
        switch (_currentInteractable)
        {
            case Interactable.Ingredient:
                if (pickupController.PickedItems.Size == 0)
                {
                    State = ChefState.Idle;
                    pickupController.HideHUD();
                }
                break;
            default:
                break;
        }

        InteractionEndedEvent.Dispatch(_currentInteractable, chefIndex);
    }

    void OnOrderTaken(SaladData currentOrder)
    {
        OrderTakenEvent.Dispatch(currentOrder, chefIndex);
    }

    void OnAction(KeyCode key, string feedback)
    {
        ActionEvent.Dispatch(_currentInteractable, chefIndex, key, feedback);
    }

    void OnPowerUpCollected(PowerUpController powerUpController)
    {
        PowerUpCollectedEvent.Dispatch(powerUpController);
    }

    void OnPowerUpUsed(PowerUp powerUp)
    {
        PowerUpUsedEvent.Dispatch(powerUp, chefIndex);
    }

    void OnSaladTrashed(int noOfIngredientsTrashed)
    {
        SaladTrashedEvent.Dispatch(noOfIngredientsTrashed, chefIndex);
    }

    string ActOnSaladPlate()
    {
        string feedback = string.Empty;

        switch(State)
        {
            case ChefState.CarryingSalad:
                feedback = PutbackSalad();
                break;

            case ChefState.Idle:
                feedback = TakeSalad();
                break;

            default:
                break;
        }

        return feedback;
    }

    string ActOnGuest(GuestController guestController)
    {
        string feedback = string.Empty;

        switch (State)
        {
            case ChefState.Idle:
                feedback = TakeOrder(guestController);
                break;

            case ChefState.CarryingSalad:
                feedback = ServeSalad(guestController);
                break;
        }

        return feedback;
    }

    string ActOnIngredient(KeyCode key)
    {
        System.Text.StringBuilder feedback = null;

        if(key == joystickController.actionKey1)
        {
            if (pickupController.CanPickItems)
            {
                if (pickupController.HasAlreadyPicked(_currentInteractingIngredient.IngredientName))
                {
                    feedback = new System.Text.StringBuilder();
                    feedback.Append(_currentInteractingIngredient.IngredientName.ToString());
                    feedback.Append(" is already in your cart.");
                }
                else
                {
                    pickupController.Pick(_currentInteractingIngredient);
                    State = ChefState.CarryingIngredient;
                }
            }
            else
            {
                feedback = new System.Text.StringBuilder();
                feedback.Append("Your cart is full. Use ");
                feedback.Append(joystickController.actionKey2.ToString());
                feedback.Append(" to drop ingredient.");
            }
        }
        else if(key == joystickController.actionKey2)
        {
            if(pickupController.CanUnPick(_currentInteractingIngredient.IngredientName))
            {
                pickupController.UnPick(_currentInteractingIngredient);
            }
            else
            {
                feedback = new System.Text.StringBuilder();
                feedback.Append("You haven't picked ");
                feedback.Append(_currentInteractingIngredient.IngredientName.ToString());
            }
        }

        return feedback == null ? string.Empty : feedback.ToString();
    }

    string ActOnChoppingBoard()
    {
        string feedback = string.Empty;

        switch (State)
        {
            case ChefState.Idle:
                if (chopController.AddToChoppingBoard())
                {
                    chopController.ShowHUD();
                    State = ChefState.Chopping;
                    animator.SetFloat(AnimatorParameter.ChefMove, 0);
                    joystickController.Disabled = true;

                    transform.position = chopPosition.position;
                    transform.rotation = chopPosition.rotation;

                    knife.transform.parent = knifeInHandPosition;
                    knife.transform.position = knifeInHandPosition.position;
                    knife.transform.rotation = knifeInHandPosition.rotation;
                    knife.transform.localScale = Vector3.one;

                    chopController.Chop();
                }
                else
                {
                    feedback = "Hmm... There is no items to chop.";
                }
                break;

            case ChefState.CarryingIngredient:
                chopController.AddToChoppingPlate(pickupController.TakeOut());
                if(pickupController.PickedItems.Size == 0)
                {
                    State = ChefState.Idle;
                    pickupController.HideHUD();
                }
                break;

            default:
                break;
        }

        return feedback;
    }

    void CollectPowerup(PowerUpController powerUpController)
    {
        if(powerUpController.chefIndex == chefIndex)
        {
            OnPowerUpCollected(powerUpController);
        }
    }

    string TakeSalad()
    {
        string feedback = string.Empty;

        if (saladPlateController.NoOfIngredientsInPlate > 0)
        {
            State = ChefState.CarryingSalad;
            animator.SetTrigger(AnimatorParameter.ChefPick);
            SaladPlate.transform.parent = saladPlateInHandPosition;
            SaladPlate.transform.position = saladPlateInHandPosition.position;
            SaladPlate.transform.rotation = saladPlateInHandPosition.rotation;
            SaladPlate.transform.localScale = Vector3.one;
        }
        else
        {
            feedback = "Seems like you haven't prepared anything yet.";
        }

        return feedback;
    }

    string PutbackSalad()
    {
        string feedback = string.Empty;

        State = ChefState.Idle;
        animator.SetTrigger(AnimatorParameter.ChefDrop);
        SaladPlate.transform.parent = saladPlateOnTablePosition;
        SaladPlate.transform.position = saladPlateOnTablePosition.position;
        SaladPlate.transform.rotation = saladPlateOnTablePosition.rotation;
        SaladPlate.transform.localScale = Vector3.one;

        return feedback;
    }

    string TakeOrder(GuestController guestController)
    {
        string feedback = string.Empty;

        SaladData currentOrder = guestController.SelectedSalad;

        CurrentServingSeatIndex = guestController.seatIndex;

        OnOrderTaken(currentOrder);

        return feedback;
    }

    string ServeSalad(GuestController guestController)
    {
        string feedback = string.Empty;

        SaladPlate.transform.parent = guestController.seatSaladSpawnPoint;
        SaladPlate.transform.position = guestController.seatSaladSpawnPoint.position;
        SaladPlate.transform.rotation = guestController.seatSaladSpawnPoint.rotation;
        SaladPlate.transform.localScale = Vector3.one;
        guestController.ReceiveOrder(saladPlateController.Ingredients, chefIndex, SaladPlate);
        State = ChefState.Idle;
        animator.SetTrigger(AnimatorParameter.ChefDrop);

        return feedback;
    }

    string TrashSalad()
    {
        string feedback = string.Empty;

        if (State == ChefState.CarryingSalad)
        {
            int noOfIngredients = saladPlateController.NoOfIngredientsInPlate;
            saladPlateController.Clear();
            State = ChefState.Idle;
            animator.SetTrigger(AnimatorParameter.ChefDrop);
            SaladPlate.transform.parent = saladPlateOnTablePosition;
            SaladPlate.transform.position = saladPlateOnTablePosition.position;
            SaladPlate.transform.rotation = saladPlateOnTablePosition.rotation;
            SaladPlate.transform.localScale = Vector3.one;

            OnSaladTrashed(noOfIngredients);

            feedback = "You trashed your preparation and lost some points as penalty.";
        }

        return feedback;
    }

    void HandleAction1()
    {
        string feedback = string.Empty;

        switch(_currentInteractable)
        {
            case Interactable.SaladPlate:
                feedback = ActOnSaladPlate();
                break;

            case Interactable.Ingredient:
                feedback = ActOnIngredient(joystickController.actionKey1);
                break;

            case Interactable.ChoppingBoard:
                feedback = ActOnChoppingBoard();
                break;

            case Interactable.Guest:
                if (_currentInteractingGuest != null)
                {
                    ActOnGuest(_currentInteractingGuest);
                }
                break;

            case Interactable.TrashCan:
                feedback = TrashSalad();
                break;

            default:
                break;
        }

        OnAction(joystickController.actionKey1, feedback);
    }

    void HandleAction2()
    {
        string feedback = string.Empty;

        switch (_currentInteractable)
        {
            case Interactable.SaladPlate:
                break;

            case Interactable.Ingredient:
                feedback = ActOnIngredient(joystickController.actionKey2);
                break;
        }

        OnAction(joystickController.actionKey2, feedback);
    }

    public void HandleJoystickActionButtonPressed(KeyCode key)
    {
        if(key == joystickController.actionKey1)
        {
            HandleAction1();
        }
        else if(key == joystickController.actionKey2)
        {
            HandleAction2();
        }
    }

    public void HandleChoppingCompleted(Ingredient ingredient)
    {
        joystickController.Disabled = false;

        State = ChefState.Idle;

        knife.transform.parent = knifeOnTablePosition;
        knife.transform.position = knifeOnTablePosition.position;
        knife.transform.rotation = knifeOnTablePosition.rotation;
        knife.transform.localScale = Vector3.one;

        saladPlateController.Add(ingredient);

        chopController.HideHUD();
    }
}
