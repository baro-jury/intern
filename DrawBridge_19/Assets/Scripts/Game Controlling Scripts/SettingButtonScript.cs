using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingButtonScript : MonoBehaviour
{
    [SerializeField] GameObject bgmSliderButton;
    [SerializeField] GameObject audioSliderButton;

    [SerializeField] Sprite onButton;
    [SerializeField] Sprite offButton;

    private void Start()
    {
        MoveSlider(Controller.instance.bgmOn, bgmSliderButton);
        MoveSlider(Controller.instance.audioOn, audioSliderButton);
    }

    void MoveSlider(bool setOn, GameObject sliderButton)
    {
        if (setOn)
        {
            Vector3 pos = sliderButton.GetComponent<RectTransform>().anchoredPosition;
            pos.x = 50;
            sliderButton.GetComponent<RectTransform>().anchoredPosition = pos;

            sliderButton.GetComponent<Image>().sprite = onButton;
        }
        else
        {
            Vector3 pos = sliderButton.GetComponent<RectTransform>().anchoredPosition;
            pos.x = -50;
            sliderButton.GetComponent<RectTransform>().anchoredPosition = pos;

            sliderButton.GetComponent<Image>().sprite = offButton;
        }
    }

    void ClickButton(GameObject button, ref bool isOn)
    {   
        isOn = !isOn;
        Controller.instance.PlayButtonSound();
        MoveSlider(isOn, button);
    }

    public void _ClickBgmButton()
    {
        ClickButton(bgmSliderButton, ref Controller.instance.bgmOn);
    }

    public void _ClickAudioButton()
    {
        ClickButton(audioSliderButton, ref Controller.instance.audioOn);
    }
}
