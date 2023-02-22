using UnityEngine;

public class TileValueGiver : MonoBehaviour
{
    #region Variables
    //right, left, warning
    private bool[] activation = new bool[3];
    private bool[] deactivation = new bool[3];
    #endregion

    #region PublicMethods
    public void AddUI(int ui) => activation[ui] = true;

    public void RemoveUI(int ui) => deactivation[ui] = true;

    public void OnTriggerEnter(Collider other)
    {
        other.GetComponent<UIManager>().ShowUI(GetUIIndex(activation));
        other.GetComponent<UIManager>().StopUI(GetUIIndex(deactivation));
    }
    #endregion

    #region PrivateMethods
    private int GetUIIndex(bool[] bools)
    {
        return (bools[0] ? 4 : 0) + (bools[1] ? 2 : 0) + (bools[2] ? 1 : 0);
    }
    #endregion
}
