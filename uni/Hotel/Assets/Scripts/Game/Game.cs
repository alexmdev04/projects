using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game instance { get; private set; }
    //public Level levelCurrent { get; private set; }
    public bool inLevel { get; private set; }
    public GameObject levelRootPrefab;
    public GameObject menuLevel;
    [SerializeField] Vector3 menuLevelStartPosition;
    [SerializeField] bool skipTutorials;
    //MenuLevel MenuLevel;
    [HideInInspector] public UnityEvent levelLoaded = new();
    public Volume globalVolume;
    bool waitingForUnload;

    public struct levelLoadData
    {
        public string levelAssetKey;
        //public Level.levelDifficultiesEnum levelDifficulty;
        public bool useFade;
        public Vector3 overrideStartRotation;
    }
    void Awake()
    {
        instance = this;
        //MenuLevel = menuLevel.GetComponent<MenuLevel>();
        //MenuLevel.skipTutorial = skipTutorials;
        //levelLoaded.AddListener(delegate { inLevel = true; });
        Addressables.InitializeAsync();
    #if !UNITY_EDITOR
        SceneManager.LoadScene("ui", LoadSceneMode.Additive);
    #endif
    }   
    void Start()
    {
    #if UNITY_EDITOR
        //MenuLevelState(true);
        //if (unloadAllLevelScenesOnStart) { UnloadAllLevelScenesOnStart(); }
        //int tutorialComplete = PlayerPrefs.GetInt("tutorialComplete", 0);
        //if (tutorialComplete == 0)
        //{
        //    StartCoroutine(MenuLevel.TutorialStart());
        //}
    #endif
    }
    void Update()
    {
        //inLevel = levelCurrent != null;
    }
}