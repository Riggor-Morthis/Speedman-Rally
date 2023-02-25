using UnityEngine;

public class ControlsScreen : MonoBehaviour
{
    #region Variables
    [SerializeField, Tooltip("Ecran principal")]
    private GameObject menuScreen;
    [SerializeField, Tooltip("Pour les inputs joueurs")]
    private PlayerInputs inputs;
    [SerializeField, Tooltip("Pour jouer des sons")]
    private MenuAudio menuAudio;
    #endregion

    #region UnityMethods
    private void Update()
    {
        if (inputs.GetMenuInput())
        {
            menuAudio.PlayStartSound();
            menuScreen.SetActive(true);
            gameObject.SetActive(false);
        }
    }
    #endregion
}
