using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private UISign leftArrow;
    [SerializeField]
    private UISign rightArrow;
    [SerializeField]
    private UISign dangerSign;
    #endregion

    #region PublicMethods
    /// <summary>
    /// 4- left arrow, 2- right arrow, 1- danger sign
    /// </summary>
    public void ShowUI(int uiElement)
    {
        if(uiElement >= 4)
        {
            uiElement -= 4;
            leftArrow.gameObject.SetActive(true);
            leftArrow.ShowUI();
        }
        if(uiElement >= 2)
        {
            uiElement -= 2;
            rightArrow.gameObject.SetActive(true);
            rightArrow.ShowUI();
        }
        if (uiElement >= 1)
        {
            dangerSign.gameObject.SetActive(true);
            dangerSign.ShowUI();
        }
    }

    /// <summary>
    /// 4- left arrow, 2- right arrow, 1- danger sign
    /// </summary>
    public void StopUI(int uiElement)
    {
        if (uiElement >= 4)
        {
            uiElement -= 4;
            leftArrow.StopUI();
        }
        if (uiElement >= 2)
        {
            uiElement -= 2;
            rightArrow.StopUI();
        }
        if (uiElement >= 1) dangerSign.StopUI();
    }
    #endregion
}
