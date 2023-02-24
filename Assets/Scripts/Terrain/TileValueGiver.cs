using UnityEngine;

public class TileValueGiver : MonoBehaviour
{
    #region Variables
    //right, left, warning
    private bool[] activation = new bool[3];
    private bool[] deactivation = new bool[3];
    #endregion

    #region UnityMethods
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<UIManager>().ChangeUI(GetUIIndex(activation), true);
        other.GetComponent<UIManager>().ChangeUI(GetUIIndex(deactivation), false);
    }
    #endregion

    #region PublicMethods
    public void AddUI(int ui) => activation[ui] = true;
    public void RemoveUI(int ui) => deactivation[ui] = false;
    #endregion

    #region PrivateMethods
    /// <summary>
    /// Converti notre format (bool) en format d'ui (ints)
    /// </summary>
    private int GetUIIndex(bool[] bools)
    {
        return (bools[0] ? 4 : 0) + (bools[1] ? 2 : 0) + (bools[2] ? 1 : 0);
    }
    #endregion
}
