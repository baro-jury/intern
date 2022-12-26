using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private Slider slider;
    public float HP = 10f;
    private float HPBurn = 1f;

    void Awake()
    {
        slider = GameObject.Find("HPSlider").GetComponent<Slider>();

        slider.minValue = 0f;
        slider.maxValue = HP;
        slider.value = slider.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (HP > 0)
        {
            HP -= HPBurn * Time.deltaTime;
            slider.value = HP;
        }
        else
        {
            Destroy(player);
            GameplayController.instance._GameOver();
        }
    }
}
