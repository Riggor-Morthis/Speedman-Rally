using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject leftArrow;
    [SerializeField]
    private GameObject rightArrow;
    [SerializeField]
    private GameObject dangerSign;
    #endregion

    #region PublicMethods
    public void HideEverything()
    {
        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);
        dangerSign.gameObject.SetActive(false);
    }

    /// <summary>
    /// 4- left arrow, 2- right arrow, 1- danger sign
    /// </summary>
    public void ChangeUI(int uiElement, bool shown)
    {
        if(uiElement >= 4)
        {
            uiElement -= 4;
            leftArrow.gameObject.SetActive(shown);
        }
        if(uiElement >= 2)
        {
            uiElement -= 2;
            rightArrow.gameObject.SetActive(shown);
        }
        if (uiElement >= 1)
        {
            dangerSign.gameObject.SetActive(shown);
        }
    }
    #endregion
}
