using System.Collections;
using UnityEngine;

public class UISign : MonoBehaviour
{
    #region Variables
    private float lifeSpan;
    private bool isCoroutineRunning = false;
    #endregion

    #region PublicMethods
    /// <summary>
    /// Montre l'element durant un temps limite
    /// </summary>
    public void ShowUI()
    {
        lifeSpan = .75f;
        if (!isCoroutineRunning) StartCoroutine(COuiShown());
    }

    /// <summary>
    /// Arrete l'UI le moment venu
    /// </summary>
    public void StopUI()
    {
        lifeSpan = 0f;
    }
    #endregion

    #region Coroutines
    private IEnumerator COuiShown()
    {
        isCoroutineRunning = true;

        while(lifeSpan > 0)
        {
            lifeSpan -= Time.deltaTime;
            yield return null;
        }

        isCoroutineRunning = false;
        gameObject.SetActive(false);
    }
    #endregion
}
