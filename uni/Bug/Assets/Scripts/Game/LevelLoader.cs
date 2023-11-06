using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance { get; private set; }
    public Level levelCurrent { get; private set; }
    [SerializeField] GameObject menuLevel;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) { ChangeLevel("Level0"); }
    }
    /// <summary>
    /// Changes to a new level by unloading the current level and loading the given level
    /// </summary>
    /// <param name="levelSceneName"></param>
    /// <param name="levelDifficulty"></param>
    public void ChangeLevel(string levelAssetKey, Level.levelDifficultiesEnum levelDifficulty = Level.levelDifficultiesEnum.normal)
    {
        AsyncOperation sceneUnload = null;
        if (levelCurrent != null)
        {
            sceneUnload = SceneManager.UnloadSceneAsync(levelCurrent.gameObject.scene);
            levelCurrent = null;
            sceneUnload.completed += delegate { LoadLevel(levelAssetKey, levelDifficulty); };
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
    /// <param name="levelSceneName"></param>
    /// <param name="levelDifficulty"></param>
    void LoadLevel(string levelAssetKey, Level.levelDifficultiesEnum levelDifficulty = Level.levelDifficultiesEnum.normal)
    {
        AsyncOperationHandle sceneLoad = Addressables.LoadSceneAsync(levelAssetKey, LoadSceneMode.Additive);
        sceneLoad.Completed += delegate { LoadLevelCompleted(ref sceneLoad); };
    }
    /// <summary>
    /// Sets the current level when it is loaded
    /// </summary>
    /// <param name="sceneLoad"></param>
    void LoadLevelCompleted(ref AsyncOperationHandle sceneLoad)
    {
        levelCurrent = ((SceneInstance)sceneLoad.Result).Scene.GetRootGameObjects()[0].GetComponent<Level>();
        MenuLevelState(false);
        uiMessage.instance.New(levelCurrent.levelSceneName + " - " + levelCurrent.levelInGameName + " loaded!");
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
    /// Sets the menu level's active state
    /// </summary>
    /// <param name="state"></param>
    public void MenuLevelState(bool state)
    {
        menuLevel.SetActive(state);
    }
}