using UnityEngine;

public class SpriteTurner : MonoBehaviour
{
    #region Variables
    private Transform playerTransform;
    #endregion

    #region UnityMethods
    private void Start()
    {
        playerTransform = Camera.allCameras[0].transform.parent;
    }

    private void LateUpdate()
    {
        if (transform.rotation != playerTransform.rotation)
            transform.rotation = playerTransform.rotation;
    }
    #endregion
}
