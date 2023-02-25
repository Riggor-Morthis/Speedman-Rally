using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LoadingScreen : MonoBehaviour
{
    #region Variables
    [SerializeField, Tooltip("L'ecran de menu")]
    private GameObject mainMenu;

    private SpriteRenderer spriteRenderer;
    private PlayerInput inputs;
    private AsyncOperation asyncLoad;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        inputs = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        StartCoroutine(LoadRacingScene());
    }
    #endregion

    #region PublicMethods
    public void StartRacing()
    {
        asyncLoad.allowSceneActivation = true;
    }
    #endregion

    #region Coroutines
    private IEnumerator LoadRacingScene()
    {
        asyncLoad = SceneManager.LoadSceneAsync(1);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < .9f) yield return null;

        spriteRenderer.enabled = false;
        inputs.enabled = true;
        mainMenu.gameObject.SetActive(true);
    }
    #endregion
}
