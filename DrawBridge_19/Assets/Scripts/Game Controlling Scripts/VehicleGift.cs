using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleGift : MonoBehaviour
{
    [SerializeField] GameObject denyButton;

    private void Start()
    {
        StartCoroutine(WaitUntilShowDeny(2f));
    }

    IEnumerator WaitUntilShowDeny(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        denyButton.SetActive(true);
    }

    public void _ClaimGift()
    {
        ++Controller.instance.maxVehicle;
        Destroy(gameObject);
    }

    public void _DenyGift()
    {
        Destroy(gameObject);
    }
}
