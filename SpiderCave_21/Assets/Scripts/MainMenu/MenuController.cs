using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject menuPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void _TapToStart()
    {
        menuPanel.SetActive(false);
    }

    public void _TapToPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void _BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
