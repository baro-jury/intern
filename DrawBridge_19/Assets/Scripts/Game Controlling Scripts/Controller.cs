using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public static Controller instance;

    [HideInInspector] public PlayingState playingState;
    [HideInInspector] public GameState gameState;

    [HideInInspector] public int currentLevel;
    [HideInInspector] public string currentVehicle;

    [SerializeField] GameObject[] levelGrids;
    [HideInInspector] public int maxLevel;
    [HideInInspector] public bool[] passedLevels;
    [HideInInspector] public int maxVehicle;

    [SerializeField] GameObject vehicleGrid;
    [SerializeField] GameObject unknownVehicleButton;

    [SerializeField] GameObject commonMenu;
    [SerializeField] GameObject homeMenu;
    [SerializeField] GameObject vehicleMenu;
    [SerializeField] GameObject playingMenu;
    [SerializeField] GameObject levelMenu;

    [SerializeField] GameObject settingBackground;
    [SerializeField] GameObject settingPanel;
    [HideInInspector] public bool bgmOn;
    [HideInInspector] public bool audioOn;

    private void Awake()
    {
        LoadData();
        //#region Load default data
        //currentLevel = 1;
        //currentVehicle = "Bobber";

        //maxLevel = 1;
        //passedLevels = new bool[100];
        //maxVehicle = 8;

        //bgmOn = false;
        //audioOn = false;
        //#endregion

        if (instance == null) instance = this;
        else Destroy(this.gameObject);

        setOrthographicSize(0.6f);

        ToHome();
    }

    void LoadData()
    {
        PlayerData data = SaveSystem.LoadData();

        if (data != null)
        {
            currentLevel = data.currentLevel;
            currentVehicle = data.currentVehicle;
            
            maxLevel = data.maxLevel;
            passedLevels = data.passedLevels;
            maxVehicle = data.maxVehicle;

            bgmOn = data.bgmOn;
            audioOn = data.audioOn;
        }
        else
        {
            currentLevel = 1;
            currentVehicle = "Bobber";
            
            maxLevel = 1;
            passedLevels = new bool[100];
            maxVehicle = 1;

            bgmOn = true;
            audioOn = true;
        }
    }

    private void OnApplicationQuit()
    {
        SaveSystem.SaveData(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        //setOrthographicSize(0.6f);

        //ToHome();
    }

    void ToHome()
    {
        gameState = GameState.Home;

        levelMenu.SetActive(false);
        commonMenu.SetActive(true);

        homeMenu.SetActive(true);
        vehicleMenu.SetActive(false);
        playingMenu.SetActive(false);

        LoadLevel.instance.LoadingLevel(currentLevel);
        Time.timeScale = 0;
        Camera.main.orthographicSize /= 2;

        Camera.main.transform.position = HomeCameraPosition();
    }

    Vector3 HomeCameraPosition()
    {
        Vector3 homeCameraPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        homeCameraPosition.z = Camera.main.transform.position.z;
        homeCameraPosition.x -= 0.25f;

        return homeCameraPosition;
    }

    public List<Canvas> canvas = new List<Canvas>();
    public List<Camera> cameras = new List<Camera>();
    public void setOrthographicSize(float size)
    {
        float f = Screen.height / (Screen.width / 9f);
        if (f >= 16)
        {
            Camera.main.orthographicSize = size * (1f * Screen.height / Screen.width) * 9f;
            foreach (Camera c in cameras)
            {
                c.orthographicSize = size * (1f * Screen.height / Screen.width) * 9f;
            }
            foreach (Canvas c in canvas)
            {
                c.GetComponent<CanvasScaler>().matchWidthOrHeight = 0f;
            }

        }
        else
        {
            Camera.main.orthographicSize = size * (1f * Screen.width / Screen.height) * 16f * (16f / 9f);
            foreach (Camera c in cameras)
            {
                c.orthographicSize = size * (1f * Screen.height / Screen.width) * 9f;
            }
            foreach (Canvas c in canvas)
            {
                c.GetComponent<CanvasScaler>().matchWidthOrHeight = 1f;
            }

            for (int i = 0; i < levelGrids.Length; ++i)
            {
                Vector2 temp = levelGrids[i].GetComponent<RectTransform>().anchoredPosition;
                temp.x = 1920f / Screen.height * Screen.width / 100 * i;
                levelGrids[i].GetComponent<RectTransform>().anchoredPosition = temp;
            }

        }
    }

    public void PlayButtonSound()
    {
        AudioManager.instance.Play("Button");
    }

    public void _FromPlayingToLevelMenu()
    {
        if (gameState != GameState.Playing)
            return;

        PlayButtonSound();
        
        gameState = GameState.LevelMenu;
        
        LoadLevel.instance.StartNewLevel();
        commonMenu.SetActive(false);
        levelMenu.SetActive(true);

        CheckUnlockedPassedLevels();
    }

    void CheckUnlockedPassedLevels()
    {
        foreach (GameObject grid in levelGrids)
        {
            foreach (Transform level in grid.transform)
            {
                int levelNum;
                bool check = int.TryParse(level.name, out levelNum);

                if (!check) continue;

                if (levelNum <= maxLevel)
                {
                    foreach (Transform child in level)
                    {
                        if (child.name.Equals("Unlocked Level Button"))
                        {
                            child.gameObject.SetActive(true);
                            if (passedLevels[levelNum])
                                foreach (Transform grandchild in child)
                                    if (grandchild.name.Equals("Passed Check"))
                                        grandchild.gameObject.SetActive(true);
                        }
                        else
                            child.gameObject.SetActive(false);
                    }
                }
                else
                {
                    foreach (Transform child in level)
                    {
                        if (child.name.Equals("Locked Level Button"))
                            child.gameObject.SetActive(true);
                        else
                            child.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public void _FromLevelMenuToHome()
    {
        PlayButtonSound();
        ToHome();
    }

    IEnumerator FromPlayingToHomeCoroutine()
    {
        LoadLevel.instance.StartNewLevel();
        LoadLevel.instance.LoadingLevel(currentLevel);
        yield return new WaitForEndOfFrame();

        gameState = GameState.Home;

        playingMenu.SetActive(false);
        yield return MoveCamera(Camera.main.orthographicSize / 2, HomeCameraPosition(), 0.5f);

        homeMenu.SetActive(true);
    }

    public void _FromPlayingToHome()
    {
        PlayButtonSound();
        StartCoroutine(FromPlayingToHomeCoroutine());
    }

    IEnumerator FromVehicleMenuToHomeCoroutine()
    {
        gameState = GameState.VehicleMenu;

        vehicleMenu.SetActive(false);
        yield return MoveCamera(Camera.main.orthographicSize, HomeCameraPosition(), 0.3f);
        homeMenu.SetActive(true);
    }

    public void _FromVehicleMenuToHome()
    {
        PlayButtonSound();
        StartCoroutine(FromVehicleMenuToHomeCoroutine());
    }

    IEnumerator FromHomeToVehicleMenuCoroutine()
    {
        gameState = GameState.VehicleMenu;

        homeMenu.SetActive(false);

        Vector3 newCameraPosition = Camera.main.transform.position;
        float distance = 2.5f;
        if (Screen.height / (Screen.width / 9f) < 16)
            distance = Camera.main.orthographicSize / 4.8f * distance;
        newCameraPosition.y -= distance;
        yield return MoveCamera(Camera.main.orthographicSize, newCameraPosition, 0.3f);

        foreach (Transform child in vehicleGrid.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 1; i <= maxVehicle; ++i)
        {
            Instantiate(Resources.Load("Vehicle Button/" + i), vehicleGrid.transform);
        }
        for (int i = 0; i < 9 - maxVehicle; ++i)
        {
            Instantiate(unknownVehicleButton, vehicleGrid.transform);
        }

        vehicleMenu.SetActive(true);
    }

    public void _FromHomeToVehicleMenu()
    {
        PlayButtonSound();
        StartCoroutine(FromHomeToVehicleMenuCoroutine());
    }

    public void _FromLevelMenuToCurrentLevel()
    {
        PlayButtonSound();

        gameState = GameState.Playing;

        LoadLevel.instance.StartNewLevel();
        commonMenu.SetActive(true);
        playingMenu.SetActive(true);
        levelMenu.SetActive(false);
        
        LoadLevel.instance.LoadingLevel(currentLevel);
    }

    IEnumerator FromHomeToCurrentLevelCoroutine()
    {
        gameState = GameState.HomeToPlaying;
        
        homeMenu.SetActive(false);
        playingMenu.SetActive(true);

        yield return MoveCamera(Camera.main.orthographicSize * 2, new Vector3(0f, -2.8f, -10f), 0.5f);

        gameState = GameState.Playing;
        playingState = PlayingState.Draw;
    }

    public void _FromHomeToCurrentLevel()
    {
        PlayButtonSound();
        StartCoroutine(FromHomeToCurrentLevelCoroutine());
    }

    IEnumerator MoveCamera(float newSize, Vector3 newLocation, float seconds)
    {
        float oldSize = Camera.main.orthographicSize;
        float deltaSize = newSize - oldSize;
        Vector3 deltaLocation = newLocation - Camera.main.transform.position;

        if (oldSize < newSize)
        {
            while (Camera.main.orthographicSize < newSize || Camera.main.transform.position != newLocation)
            {
                Camera.main.orthographicSize += deltaSize * Time.unscaledDeltaTime / seconds;
                Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, newLocation, 
                    deltaLocation.magnitude * Time.unscaledDeltaTime / seconds);
                yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
            }
            Camera.main.orthographicSize = newSize;
        }
        else
        {
            while (Camera.main.orthographicSize > newSize || Camera.main.transform.position != newLocation)
            {
                Camera.main.orthographicSize += deltaSize * Time.unscaledDeltaTime / seconds;
                Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, newLocation,
                    deltaLocation.magnitude * Time.unscaledDeltaTime / seconds);
                yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
            }
            Camera.main.orthographicSize = newSize;
        }
    }

    public void _OpenSettingMenu()
    {
        PlayButtonSound();
        settingBackground.SetActive(true);
        StartCoroutine(ZoomOutPanel(settingPanel));
    }

    public void _CloseSettingMenu()
    {
        PlayButtonSound();
        //StartCoroutine(ClosePanel(settingPanel));
        settingBackground.SetActive(false);
        settingPanel.SetActive(false);
    }

    public IEnumerator ZoomOutPanel(GameObject panel)
    {
        panel.transform.localScale = new Vector3(0, 0, 0);
        panel.SetActive(true);
        float speed = 10f;
        Vector3 goal = new Vector3(1, 1, 1);

        while ((goal - panel.transform.localScale).magnitude > Mathf.Epsilon)
        {
            panel.transform.localScale = Vector3.MoveTowards(panel.transform.localScale, goal, speed * Time.unscaledDeltaTime);
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
    }

    //public IEnumerator ClosePanel(GameObject panel)
    //{
    //    float speed = 10f;
    //    Vector3 goal = new Vector3(0, 0, 0);

    //    while ((goal - panel.transform.localScale).magnitude > Mathf.Epsilon)
    //    {
    //        panel.transform.localScale = Vector3.MoveTowards(panel.transform.localScale, goal, speed * Time.unscaledDeltaTime);
    //        yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
    //    }

    //    panel.SetActive(false);
    //}

}

public enum PlayingState
{
    Draw, BikeMove, CameraMove, Finish
}

public enum GameState
{
    Home, VehicleMenu, LevelMenu, Playing,
    HomeToPlaying
}
