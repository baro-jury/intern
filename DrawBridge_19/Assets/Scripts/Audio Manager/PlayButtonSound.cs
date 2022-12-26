using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonSound : MonoBehaviour
{
    public void _PlayButtonSound()
    {
        AudioManager.instance.Play("Button");
    }
}
