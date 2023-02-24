using UnityEngine;

public class BorderValueGiver : MonoBehaviour
{
    #region UnityMethods
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement pm;
        if ((pm = other.GetComponent<PlayerMovement>()) != null)
        {
            pm.ChangeOffroadIndex(1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerMovement pm;
        if ((pm = other.GetComponent<PlayerMovement>()) != null) pm.ChangeOffroadIndex(-1);
    }
    #endregion
}
