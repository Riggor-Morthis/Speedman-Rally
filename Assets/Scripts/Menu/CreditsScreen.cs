using UnityEngine;

public class CreditsScreen : MonoBehaviour
{
    #region Variables
    [SerializeField, Tooltip("Les deux ecrans a afficher")]
    private Sprite[] sprites = new Sprite[2];
    private SpriteRenderer spriteRenderer;
    private int currentIndex;

    [Space]

    [SerializeField, Tooltip("Ecran principal")]
    private GameObject menuScreen;
    [SerializeField, Tooltip("Pour les inputs joueurs")]
    private PlayerInputs inputs;
    [SerializeField, Tooltip("Pour jouer des sons")]
    private MenuAudio menuAudio;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (inputs.GetMenuInput())
        {
            currentIndex++;
            if(currentIndex == 1)
            {
                spriteRenderer.sprite = sprites[1];
                menuAudio.PlayStartSound();
            }
            else
            {
                menuAudio.PlayStartSound();
                menuScreen.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        currentIndex = 0;
        spriteRenderer.sprite = sprites[0];
    }
    #endregion
}
