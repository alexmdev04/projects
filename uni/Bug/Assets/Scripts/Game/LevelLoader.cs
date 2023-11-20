using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.Events;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance { get; private set; }
    public Level levelCurrent { get; private set; }
    public bool inLevel { get; private set; }
    public GameObject levelRootPrefab;
    public GameObject menuLevel;
    [SerializeField] Vector3 menuLevelStartPosition;
    [SerializeField] bool skipTutorials;
    uiMenuLevel uiMenuLevel;
    SceneInstance levelCurrentSceneInstance;
    [HideInInspector] public UnityEvent levelLoaded = new();
    void Awake()
    {
        instance = this;
        uiMenuLevel = menuLevel.GetComponent<uiMenuLevel>();
        uiMenuLevel.skipTutorial = skipTutorials;
        levelLoaded.AddListener(delegate { inLevel = true; });
    }   
    void Start()
    {
#if UNITY_EDITOR
        MenuLevelState(true);
        if (unloadAllLevelScenesOnStart) { UnloadAllLevelScenesOnStart(); }
        //int tutorialComplete = PlayerPrefs.GetInt("tutorialComplete", 0);
        //if (tutorialComplete == 0)
        //{
        //    StartCoroutine(uiMenuLevel.TutorialStart());
        //}
#endif
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) { ChangeLevel("Level0"); }
        //inLevel = levelCurrent != null;
    }
    /// <summary>
    /// Changes to a new level by unloading the current level and loading the given level
    /// </summary>
    /// <param name="levelAssetKey"></param>
    /// <param name="levelDifficulty"></param>
    public void ChangeLevel(string levelAssetKey, Level.levelDifficultiesEnum levelDifficulty = Level.levelDifficultiesEnum.normal)
    {
        inLevel = false;
        if (levelCurrent != null)
        {
            levelCurrent.Unload();
            AsyncOperation sceneUnload = SceneManager.UnloadSceneAsync(levelCurrent.gameObject.scene);
            UnloadSceneInstance(levelCurrentSceneInstance, levelCurrent.assetKey);
            sceneUnload.completed += delegate { LoadLevel(levelAssetKey, levelDifficulty); };
            levelCurrent = null;
        }
        else
        {
            Debug.Log("No level is loaded, unload skipped.");
            LoadLevel(levelAssetKey, levelDifficulty);
        }
        Grapple.instance.Disable();
        Grapple.instance.GrapplePointReset();
    }
    /// <summary>
    /// Loads the given level
    /// </summary>
    /// <param name="levelAssetKey"></param>
    /// <param name="levelDifficulty"></param>
    void LoadLevel(string levelAssetKey, Level.levelDifficultiesEnum levelDifficulty = Level.levelDifficultiesEnum.normal)
    {
        AsyncOperationHandle sceneLoad = Addressables.LoadSceneAsync(levelAssetKey, LoadSceneMode.Additive);
        sceneLoad.Completed += LoadLevelCompleted;
    }
    /// <summary>
    /// Sets the current level when it is loaded
    /// </summary>
    /// <param name="sceneLoad"></param>
    void LoadLevelCompleted(AsyncOperationHandle sceneLoad)
    {
        levelCurrentSceneInstance = (SceneInstance)sceneLoad.Result;
        levelCurrent = levelCurrentSceneInstance.Scene.GetRootGameObjects()[0].GetComponent<Level>();
        MenuLevelState(false);
        uiMessage.instance.New(levelCurrent.assetKey + " - " + levelCurrent.inGameName + " loaded!");
        levelLoaded.Invoke();
    }
    /// <summary>
    /// Loads the prompted level with the ability to edit the objectives of the level
    /// </summary>
    void LoadLevelOverrideObjectives(
        string levelSceneName,
        Level.levelDifficultiesEnum levelDifficulty,
        bool oGrappleDistanceEnabled,
        bool oGrappleUsesEnabled,
        bool oTimeLimitEnabled,
        float oGrappleDistanceValue = 0,
        int oGrappleUsesValue = 0,
        float oGrappleTimeLimitValue = 0)
    {

    }
    /// <summary>
    /// Unloads the currently loaded level and returns to the main menu
    /// </summary>
    public void UnloadLevelCurrent()
    {
        inLevel = false;
        if (levelCurrent != null)
        {
            levelCurrent.Unload();
            string levelAssetKey = levelCurrent.assetKey;
            AsyncOperation sceneUnload = SceneManager.UnloadSceneAsync(levelCurrent.gameObject.scene);
            sceneUnload.completed += delegate { MenuLevelState(true, true); };
            UnloadSceneInstance(levelCurrentSceneInstance, levelAssetKey);
            levelCurrent = null;
        }
        else
        {
            uiMessage.instance.New("No level to unload / Already on main menu");
            MenuLevelState(true);
        }
    }
    void UnloadSceneInstance(SceneInstance sceneInstance, string assetKey)
    {
        AsyncOperationHandle sceneInstanceUnload = Addressables.UnloadSceneAsync(sceneInstance, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        sceneInstanceUnload.Completed += delegate { uiMessage.instance.New("Unloaded " + assetKey); };
    }
    /// <summary>
    /// Sets the menu level's active state
    /// </summary>
    /// <param name="state"></param>
    public void MenuLevelState(bool state, bool notify = false)
    {
        menuLevel.SetActive(state);
        if (state)
        {
            if (notify) { uiMessage.instance.New("Returned to main menu"); }
            Player.instance.TeleportInstant(menuLevelStartPosition);
        }
    }

    #region Unload all level scenes on start if in editor
    #if UNITY_EDITOR
    [SerializeField] bool unloadAllLevelScenesOnStart = true;
    List<UnityEngine.SceneManagement.Scene> scenes = new List<UnityEngine.SceneManagement.Scene> ();
    void UnloadAllLevelScenesOnStart()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++) { scenes.Add(SceneManager.GetSceneAt(i)); }
        foreach (UnityEngine.SceneManagement.Scene scene in scenes)
        {
            if (scene.GetRootGameObjects()[0].GetComponent<Level>() != null)
            {
                Debug.LogWarning("The scene " + scene.name + " was found on startup and was unloaded.");
                #pragma warning disable CS0618 // Type or member is obsolete
                SceneManager.UnloadScene(scene);
                #pragma warning restore CS0618 // Type or member is obsolete
            }
        }
    }
    #endif
    #endregion
}