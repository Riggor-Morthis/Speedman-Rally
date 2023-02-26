using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UpgradeMenu : MonoBehaviour
{
    #region Variables
    [SerializeField, Tooltip("Pour jouer des sons")]
    private MenuAudio menuAudio;

    private PlayerInputs inputs;
    private AsyncOperation asyncLoad;
    private bool isLoading;

    private ButtonColor[] upgradeButtons;
    private int currentButtonIndex = -1;
    private bool inputLock;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        inputs = GetComponent<PlayerInputs>();
        upgradeButtons = GetComponentsInChildren<ButtonColor>();
    }

    private void Start()
    {
        menuAudio.PlayStartSound();
        StartCoroutine(COLoadTrack());
    }

    private void Update()
    {
        if (currentButtonIndex == -1)
        {
            upgradeButtons[0].SwitchColor();
            currentButtonIndex = 0;
        }

        NavigateMenu();
        if (inputs.GetMenuInput()) ExecuteUpgrade();
    }
    #endregion

    #region PrivateMethods
    private void NavigateMenu()
    {
        if (inputs.pedalsInput == 0) inputLock = false;
        else if (!inputLock)
        {
            menuAudio.PlayLightSound();
            if (inputs.pedalsInput > 0)
            {
                inputLock = true;
                upgradeButtons[currentButtonIndex].SwitchColor();
                currentButtonIndex--;
                if (currentButtonIndex < 0)
                    currentButtonIndex = upgradeButtons.Length - 1;
                upgradeButtons[currentButtonIndex].SwitchColor();
            }
            else
            {
                inputLock = true;
                upgradeButtons[currentButtonIndex].SwitchColor();
                currentButtonIndex++;
                if (currentButtonIndex > upgradeButtons.Length - 1)
                    currentButtonIndex = 0;
                upgradeButtons[currentButtonIndex].SwitchColor();
            }
        }
    }

    private void ExecuteUpgrade()
    {
        menuAudio.PlayStartSound();
        switch (currentButtonIndex)
        {
            case 0:
                ChampionshipData.IncreaseAcceleration();
                break;
            case 1:
                ChampionshipData.IncreaseTopSpeed();
                break;
            case 2:
                ChampionshipData.IncreaseHandling();
                break;
            case 3:
                ChampionshipData.IncreaseBraking();
                break;
            default:
                ChampionshipData.IncreaseAcceleration();
                break;
        }
        StartCoroutine(COFinishUpgrade());
    }
    #endregion

    #region Coroutines
    private IEnumerator COLoadTrack()
    {
        isLoading = true;
        asyncLoad = SceneManager.LoadSceneAsync(1);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < .9f) yield return null;

        isLoading = false;
        yield return null;
    }

    private IEnumerator COFinishUpgrade()
    {
        for(int i = 0; i < 15; i++)
        {
            upgradeButtons[currentButtonIndex].SwitchColor();
            yield return new WaitForSeconds(.2f);
        }

        while (isLoading) yield return null;
        asyncLoad.allowSceneActivation = true;
    }
    #endregion
}
