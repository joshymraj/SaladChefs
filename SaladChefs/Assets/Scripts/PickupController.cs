using UnityEngine;
using UnityEngine.UI;

public class PickupController : MonoBehaviour
{
    public GameObject pickupStatusHUD;
    public GameObject[] pickupItems; // cart items; total of pickUpCapacity
    public int pickUpCapacity = 2;

    public CustomQueue<Ingredient> PickedItems
    {
        get;
        private set;
    }

    public bool CanPickItems
    {
        get
        {
            return PickedItems.Size < pickUpCapacity;
        }
    }

    public bool HasAlreadyPicked(Ingredient ingredient)
    {
        if(PickedItems.Size == 0)
        {
            return false;
        }

        return PickedItems.IndexOf(ingredient) > -1;
    }

    void Start()
    {
        PickedItems = new CustomQueue<Ingredient>();
    }

    public void ShowHUD()
    {
        pickupStatusHUD.SetActive(true);
    }

    public void HideHUD()
    {
        pickupStatusHUD.SetActive(false);
    }

    public void Pick(IngredientData ingredientData)
    {
        PickedItems.Enqueue(ingredientData.IngredientName);

        for (int i = 0; i < pickUpCapacity; i++)
        {
            if (!pickupItems[i].activeSelf)
            {
                pickupItems[i].SetActive(true);
                pickupItems[i].GetComponent<PickupItemController>().SetItem(ingredientData.Image, ingredientData.IngredientName);
                break;
            }
        }
    }

    public bool CanUnPick(Ingredient ingredient)
    {
        return PickedItems.IndexOf(ingredient) > -1;
    }

    public void UnPick(IngredientData ingredientData)
    {
        int unPickItemIndex = PickedItems.IndexOf(ingredientData.IngredientName);
        if (unPickItemIndex > -1)
        {
            PickedItems.Remove(unPickItemIndex);
            for (int i = 0; i < pickUpCapacity; i++)
            {
                if (pickupItems[i].activeSelf && pickupItems[i].GetComponent<PickupItemController>().Ingredient == ingredientData.IngredientName)
                {
                    pickupItems[i].gameObject.SetActive(false);
                    break;
                }
            }
        }

        if (PickedItems.Size == 0)
        {
            PickedItems = new CustomQueue<Ingredient>();
        }
    }

    public Ingredient TakeOut()
    {
        Ingredient droppedItem = PickedItems.Dequeue();

        for (int i = 0; i < pickUpCapacity; i++)
        {
            if (pickupItems[i].activeSelf && pickupItems[i].GetComponent<PickupItemController>().Ingredient == droppedItem)
            {
                pickupItems[i].gameObject.SetActive(false);
                break;
            }
        }

        if(PickedItems.Size == 0)
        {
            PickedItems = new CustomQueue<Ingredient>();
        }

        return droppedItem;
    }

    public void Clear()
    {
        PickedItems = null;
        for (int i = 0; i < pickupItems.Length; i++)
        {
            pickupItems[i].SetActive(false);
        }
    }
}
