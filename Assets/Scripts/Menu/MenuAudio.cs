using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    #region Variables
    [SerializeField, Tooltip("Source sons")]
    private AudioSource sfxChannel;

    [SerializeField, Tooltip("Bouger")]
    private AudioClip lightSFX;
    [SerializeField, Tooltip("Valider")]
    private AudioClip startSFX;
    #endregion

    #region PublicMethods
    public void PlayLightSound() => ChangeMainAudio(lightSFX);
    public void PlayStartSound() => ChangeMainAudio(startSFX);
    #endregion

    #region PrivateMethods
    /// <summary>
    /// Permet de modifier automatiquement le clip en cours
    /// </summary>
    private void ChangeMainAudio(AudioClip currentClip)
    {
        sfxChannel.Stop();
        sfxChannel.clip = currentClip;
        sfxChannel.Play();
    }
    #endregion
}
