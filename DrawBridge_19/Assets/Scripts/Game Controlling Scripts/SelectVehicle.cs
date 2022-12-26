using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectVehicle : MonoBehaviour
{
    [SerializeField] Text vehicleName;
    [SerializeField] GameObject checkmark;

    // Update is called once per frame
    void Update()
    {
        if (!Controller.instance.currentVehicle.Equals(vehicleName.text))
            checkmark.SetActive(false);
        else checkmark.SetActive(true);
    }

    public void _SelectVehicle()
    {
        Controller.instance.PlayButtonSound();
        
        Controller.instance.currentVehicle = vehicleName.text;
        LoadLevel.instance.StartNewLevel();
        LoadLevel.instance.LoadingLevel(Controller.instance.currentLevel);
    }
}
