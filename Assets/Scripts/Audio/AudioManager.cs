using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    #region Variables
    [SerializeField, Tooltip("La source dediee uniquement aux sons d'une slide")]
    private AudioSource slideChannel;
    [SerializeField, Tooltip("La source dediee uniquement aux sons du moteur")]
    private AudioSource engineChannel;

    [Space]

    [SerializeField, Tooltip("La source pour les sons prioritaires")]
    private AudioSource sfxChannel;
    [SerializeField, Tooltip("Lorsqu'on rentre dans un arbre")]
    private AudioClip crashSFX;
    [SerializeField, Tooltip("Compte a rebours")]
    private AudioClip lightSFX;
    [SerializeField, Tooltip("Depart")]
    private AudioClip startSFX;
    [SerializeField, Tooltip("Arrivee")]
    private AudioClip endSFX;

    private PlayerMovement movement;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        movement = GetComponentInParent<PlayerMovement>();
    }

    private void Start()
    {
        StartCoroutine(CO_ProportionalSounds());
    }

    private void LateUpdate()
    {
        UpdateSoundChannels();
    }
    #endregion

    #region PublicMethods
    public void PlayCrashSound() => ChangeMainAudio(crashSFX);
    public void PlayLightSound() => ChangeMainAudio(lightSFX);
    public void PlayStartSound() => ChangeMainAudio(startSFX);
    public void PlayEndSound()
    {
        ChangeMainAudio(endSFX);
        engineChannel.Stop();
        slideChannel.Stop();
    }
    #endregion

    #region PrivateMethods
    /// <summary>
    /// Pour s'assurer qu'on joue le bon son
    /// </summary>
    private void UpdateSoundChannels()
    {
        //On agit que si on a pas de son prioritaire
        if (!sfxChannel.isPlaying)
        {
            //Lancer une slide
            if (movement.GetSpeedRatio() > 0 && movement.currentState == movement.slidingState)
            {
                if (slideChannel.mute)
                {
                    engineChannel.mute = true;
                    slideChannel.mute = false;
                }
            }
            else
            {
                if (engineChannel.mute)
                {
                    slideChannel.mute = true;
                    engineChannel.mute = false;
                }
            }
        }
    }

    /// <summary>
    /// Permet de modifier automatiquement le clip en cours
    /// </summary>
    private void ChangeMainAudio(AudioClip currentClip)
    {
        slideChannel.mute = true;
        engineChannel.mute = true;
        sfxChannel.Stop();

        sfxChannel.clip = currentClip;
        sfxChannel.Play();
    }
    #endregion

    #region Coroutines
    /// <summary>
    /// Ajuste les sons moteurs en temps reels pour coller a la realite
    /// </summary>
    private IEnumerator CO_ProportionalSounds()
    {
        while (true)
        {
            engineChannel.volume = movement.GetSpeedRatio() * .1f + .22f;
            engineChannel.pitch = movement.GetSpeedRatio() * .8f + .2f;
            slideChannel.volume = movement.GetSpeedRatio() * .1f + .2f;
            slideChannel.pitch = movement.GetSpeedRatio() * .5f + .5f;
            yield return null;
        }
    }
    #endregion
}
