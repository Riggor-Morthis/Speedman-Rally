using UnityEngine;

public class MainScreen : MonoBehaviour
{
    #region Variables
    [SerializeField, Tooltip("Pour lancer chargement course")]
    private LoadingScreen loadingScreen;
    [SerializeField, Tooltip("Pour les inputs joueurs")]
    private PlayerInputs inputs;
    [SerializeField, Tooltip("Pour jouer des sons")]
    private MenuAudio menuAudio;

    [Space]

    [SerializeField, Tooltip("Montre les controles")]
    private GameObject controlsScreen;
    [SerializeField, Tooltip("Montre les credits")]
    private GameObject creditsScreen;

    private ButtonColor[] menuButtons;
    private int currentButtonIndex;
    private bool inputLock;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        menuButtons = GetComponentsInChildren<ButtonColor>();
    }

    private void Start()
    {
        menuAudio.PlayLightSound();
    }

    private void Update()
    {
        if (currentButtonIndex == -1)
        {
            menuButtons[0].SwitchColor();
            currentButtonIndex = 0;
        }

        NavigateMenu();
        if (inputs.GetMenuInput()) ExecuteButton();
    }

    private void OnEnable()
    {
        currentButtonIndex = -1;
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
                menuButtons[currentButtonIndex].SwitchColor();
                currentButtonIndex--;
                if (currentButtonIndex < 0)
                    currentButtonIndex = menuButtons.Length - 1;
                menuButtons[currentButtonIndex].SwitchColor();
            }
            else
            {
                inputLock = true;
                menuButtons[currentButtonIndex].SwitchColor();
                currentButtonIndex++;
                if (currentButtonIndex > menuButtons.Length - 1)
                    currentButtonIndex = 0;
                menuButtons[currentButtonIndex].SwitchColor();
            }
        }
    }

    private void ExecuteButton()
    {
        if (currentButtonIndex == 0) loadingScreen.StartRacing();
        else if (currentButtonIndex == 1)
        {
            menuAudio.PlayStartSound();
            controlsScreen.SetActive(true);
            gameObject.SetActive(false);
        }
        else if (currentButtonIndex == 2)
        {
            menuAudio.PlayStartSound();
            creditsScreen.SetActive(true);
            gameObject.SetActive(false);
        }
        else Application.Quit();
    }
    #endregion
}
