using UnityEngine;

public class CrashGiver : MonoBehaviour
{
    #region UnityMethods
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement pm;
        if ((pm = other.GetComponent<PlayerMovement>()) != null)
        {
            pm.CrashTheCar();
        }
    }
    #endregion
}
