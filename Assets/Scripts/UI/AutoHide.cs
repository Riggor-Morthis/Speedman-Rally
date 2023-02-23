using UnityEngine;
using System.Collections;

public class AutoHide : MonoBehaviour
{
    #region Variables
    private bool isCoroutineRunning;
    #endregion

    #region UnityMethods
    private void OnEnable()
    {
        StartCoroutine(COAutoHide());
    }

    private void OnDisable()
    {
        if (isCoroutineRunning)
        {
            isCoroutineRunning = false;
            StopCoroutine(COAutoHide());
        }
    }
    #endregion

    #region Couroutines
    private IEnumerator COAutoHide()
    {
        isCoroutineRunning = true;
        yield return new WaitForSecondsRealtime(2.5f);
        isCoroutineRunning = false;
        gameObject.SetActive(false);
    }
    #endregion
}
