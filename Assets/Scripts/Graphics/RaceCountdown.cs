using UnityEngine;
using System.Collections;

public class RaceCountdown : MonoBehaviour
{
    #region Variables
    [SerializeField, Tooltip("La ou on affiche nos lights")]
    private SpriteRenderer startRenderer;
    [SerializeField, Tooltip("Les trois etats de la lumiere")]
    private Sprite[] lights = new Sprite[3];

    [Space]

    [SerializeField, Tooltip("La ou on affice la fin")]
    private GameObject endStage;
    [SerializeField, Tooltip("Pour ecrire clair")]
    private SpriteRenderer[] numberRenderers = new SpriteRenderer[8];
    [SerializeField, Tooltip("Pour ecrire clair")]
    private Sprite[] lightSprites = new Sprite[12];
    [SerializeField, Tooltip("Pour ecrire fonce")]
    private Sprite[] darkSprites = new Sprite[12];

    private PlayerMovement movement;
    private AudioManager audioManager;
    private bool isRacing;
    private float chronometer = 0f;
    private int time;
    private int[] timeString = new int[8];
    private bool isFLashing = false;
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
        endStage.SetActive(true);
        movement.ChangeCarStatus(false);
        audioManager.PlayEndSound();

        CalculateTime();
        StartCoroutine(COShowTime());
    }
    #endregion

    #region PrivateMethods
    private void CalculateTime()
    {
        //Pour commencer, les minutes
        time = Mathf.FloorToInt(chronometer / 600f);
        timeString[0] = time;
        chronometer -= time * 600f;

        time = Mathf.FloorToInt(chronometer / 60f);
        timeString[1] = time;
        chronometer -= time * 60f;

        Debug.Log(timeString[0] + timeString[1] + "'");
        timeString[2] = 10;

        time = Mathf.FloorToInt(chronometer / 10f);
        timeString[3] = time;
        chronometer -= time * 10f;

        time = Mathf.FloorToInt(chronometer);
        timeString[4] = time;
        chronometer -= time;

        Debug.Log(timeString[3] + timeString[4] + "''");
        timeString[5] = 11;
        chronometer *= 100f;

        time = Mathf.FloorToInt(chronometer / 10f);
        timeString[6] = time;
        chronometer -= time * 10f;

        time = Mathf.FloorToInt(chronometer);
        timeString[7] = time;
    }
    #endregion

    #region Coroutines
    private IEnumerator COInitialCountdown()
    {
        audioManager.PlayLightSound();

        float timer = 1;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        startRenderer.sprite = lights[0];
        audioManager.PlayLightSound();

        timer = 1;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        startRenderer.sprite = lights[1];
        audioManager.PlayLightSound();

        timer = 1;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        startRenderer.sprite = lights[2];
        audioManager.PlayStartSound();
        movement.ChangeCarStatus(true);
        StartCoroutine(Chronometer());

        timer = 1;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        startRenderer.gameObject.SetActive(false);
    }

    private IEnumerator Chronometer()
    {
        do
        {
            yield return null;
            chronometer += Time.deltaTime;
        } while (isRacing);
    }

    private IEnumerator COShowTime()
    {
        for(int i = 0; i < numberRenderers.Length; i++)
        {
            numberRenderers[i].sprite = darkSprites[timeString[i]];
        }

        while (true)
        {
            StartCoroutine(COFlashTime(lightSprites));
            while (isFLashing) yield return null;
            StartCoroutine(COFlashTime(darkSprites));
            while (isFLashing) yield return null;
        }
    }

    private IEnumerator COFlashTime(Sprite[] currentColor)
    {
        isFLashing = true;
        float timer;
        for (int i = 0; i < numberRenderers.Length; i++)
        {
            timer = .025f;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            numberRenderers[i].sprite = currentColor[timeString[i]];
        }
        isFLashing = false;
    }
    #endregion
}
