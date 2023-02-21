using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    #region Variables
    [SerializeField, Tooltip("La source dediee aux sons prioritaires du vehicule")]
    private AudioSource sfxChannel;
    [SerializeField, Tooltip("La source dediee uniquement aux sons du moteur")]
    private AudioSource engineChannel;

    [Space]

    [SerializeField, Tooltip("Tant qu'on est dans l'etat de slide")]
    private AudioClip slidingSFX;
    [SerializeField, Tooltip("Des qu'on bouge sur le cote")]
    private AudioClip steeringSFX;
    [SerializeField, Tooltip("Lorsqu'on appuie sur le frein")]
    private AudioClip brakingSFX;

    private PlayerMovement movement;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        movement = GetComponentInParent<PlayerMovement>();
    }

    private void Start()
    {
        StartCoroutine(CO_EngineSounds());
    }

    private void LateUpdate()
    {
        UpdateSoundChannels();
    }
    #endregion

    #region PrivateMethods
    /// <summary>
    /// Pour s'assurer qu'on joue le bon son
    /// </summary>
    private void UpdateSoundChannels()
    {
        //On regarde l'etat de la voiture, et quel son on doit jouer en consequence
        //On a qu'un son a la fois, donc il faut des priorites
        if (movement.GetSpeedRatio() > 0 && movement.currentState == movement.slidingState)
        {
            if (sfxChannel.clip != slidingSFX || !sfxChannel.isPlaying)
                ChangeMainAudio(slidingSFX);
        }
        else if(movement.GetSpeedRatio() > 0 && movement.steerInput != 0)
        {
            if (sfxChannel.clip != steeringSFX || !sfxChannel.isPlaying)
                ChangeMainAudio(steeringSFX);
        }
        /*else if(movement.GetSpeedRatio() > 0 && movement.pedalsInput < 0)
        {
            if (sfxChannel.clip != brakingSFX || !sfxChannel.isPlaying)
                ChangeMainAudio(brakingSFX);
        }*/

        //Si on a rien a jouer, on laisse le channel special moteur faire son travail
        else
        {
            if (engineChannel.mute)
            {
                sfxChannel.Stop();
                engineChannel.mute = false;
            }
        }
    }

    /// <summary>
    /// Permet de modifier automatiquement le clip en cours
    /// </summary>
    private void ChangeMainAudio(AudioClip currentClip)
    {
        sfxChannel.Stop();
        sfxChannel.clip = currentClip;
        engineChannel.mute = true;
        sfxChannel.Play();
    }
    #endregion

    #region Coroutines
    /// <summary>
    /// Ajuste les sons moteurs en temps reels pour coller a la realite
    /// </summary>
    private IEnumerator CO_EngineSounds()
    {
        while (true)
        {
            engineChannel.volume = movement.GetSpeedRatio() * .1f + .22f;
            engineChannel.pitch = movement.GetSpeedRatio() * .8f + .2f;
            yield return null;
        }
    }
    #endregion
}
