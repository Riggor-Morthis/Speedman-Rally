using UnityEngine;
using System.Collections;

public class RaceCountdown : MonoBehaviour
{
    #region Variables
    [SerializeField, Tooltip("La ou on affiche nos lights")]
    private SpriteRenderer spriteRenderer;
    [SerializeField, Tooltip("Les trois etats de la lumiere")]
    private Sprite[] lights = new Sprite[3];
    [SerializeField, Tooltip("Fin de la course")]
    private Sprite stageWon;

    private PlayerMovement movement;
    private AudioManager audioManager;
    private bool isRacing;
    private float chronometer = 0f;
    private int minutes, seconds, milliseconds;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        audioManager = GetComponentInChildren<AudioManager>();
    }

    private void Start()
    {
        isRacing = true;
        StartCoroutine(COInitialCountdown());
    }
    #endregion

    #region PublicMethods
    public void StopTheClock()
    {
        isRacing = false;
        spriteRenderer.gameObject.SetActive(true);
        spriteRenderer.sprite = stageWon;
        movement.ChangeCarStatus(false);
        audioManager.PlayEndSound();

        Debug.Log(CalculateTime());
    }
    #endregion

    #region PrivateMethods
    private string CalculateTime()
    {
        minutes = Mathf.FloorToInt(chronometer / 60f);
        chronometer -= minutes * 60;
        seconds = Mathf.FloorToInt(chronometer);
        chronometer -= seconds;
        milliseconds = Mathf.FloorToInt(chronometer * 1000f);

        return minutes + "'" + seconds + "''" + milliseconds;
    }
    #endregion

    #region Coroutines
    private IEnumerator COInitialCountdown()
    {
        audioManager.PlayLightSound();

        float timer = 1;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        spriteRenderer.sprite = lights[0];
        audioManager.PlayLightSound();

        timer = 1;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        spriteRenderer.sprite = lights[1];
        audioManager.PlayLightSound();

        timer = 1;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        spriteRenderer.sprite = lights[2];
        audioManager.PlayStartSound();
        movement.ChangeCarStatus(true);
        StartCoroutine(Chronometer());

        timer = 1;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        spriteRenderer.gameObject.SetActive(false);
    }

    private IEnumerator Chronometer()
    {
        do
        {
            yield return null;
            chronometer += Time.deltaTime;
        } while (isRacing);
    }
    #endregion
}
