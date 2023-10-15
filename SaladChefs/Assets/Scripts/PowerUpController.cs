using System.Collections;

using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public PowerUp powerUpType;

    public float rotateSpeed = 50;

    [HideInInspector]
    public float Value;

    [HideInInspector]
    public int chefIndex;

    public float pickUpTime = 30;

    void Start()
    {
        AutoDestroy();
    }

    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(pickUpTime);
        if (gameObject.activeSelf)
        {
            Destroy(gameObject);
        }
    }
}
