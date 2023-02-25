using UnityEngine;

public class ButtonColor : MonoBehaviour
{
    #region Variables
    [SerializeField, Tooltip("les 2 etats du bouton")]
    private Sprite[] sprites = new Sprite[2];

    private SpriteRenderer spriteRenderer;
    private bool isSelected;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        isSelected = false;
        spriteRenderer.sprite = sprites[0];
    }
    #endregion

    #region PublicMethods
    public void SwitchColor()
    {
        isSelected = !isSelected;
        if(isSelected) spriteRenderer.sprite = sprites[1];
        else spriteRenderer.sprite = sprites[0];
    }
    #endregion
}
