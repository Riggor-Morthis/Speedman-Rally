using UnityEngine;

/// <summary>
/// Etat de la state machine
/// </summary>
public abstract class StateBase
{
    #region Variables
    //Acces inputs
    protected PlayerMovement movement;

    //stats du vehicle
    protected int statTopSpeed = 3;
    protected int statAcceleration = 3;
    protected int statBraking = 3;
    protected int statHandling = 3;
    protected int statHealth = 100;

    //Stats de calcul
    protected float trueZPosition, trueXPosition;
    protected int currentZPosition, currentXPosition;
    #endregion

    #region PublicMethods
    /// <summary>
    /// Constructor
    /// </summary>
    public StateBase(PlayerMovement pm)
    {
        movement = pm;
        trueZPosition = movement.transform.position.z;
        trueXPosition = movement.transform.position.x;
        SetCorrectPosition();
    }

    /// <summary>
    /// Initialisation correcte etat
    /// </summary>
    public abstract void EnterState(float vel, Vector3 oldDir, Vector3 newDir, float tzp, float txp);
    /// <summary>
    /// Calculs d'etat
    /// </summary>
    public abstract void ExecuteState();
    /// <summary>
    /// Conditions de sortie d'etat
    /// </summary>
    public abstract void ExitState();
    /// <summary>
    /// Pourcentage entre la vitesse actuelle et la vitesse maximale
    /// </summary>
    public abstract float GetSpeedRatio();
    /// <summary>
    /// Recupere la direction actuelle de la voiture
    /// </summary>
    public abstract Vector3 GetDirection();
    #endregion

    #region PrivateMethods
    //Translate player-friendly stats into maths-friendly stats
    protected float TranslateTopSpeed()
    {
        if (movement.steerInput != 0 && movement.offroadIndex > 0)
            return (17.5f * (statTopSpeed / 10f) + 10) * .6f;
        else if (movement.steerInput != 0 || movement.offroadIndex > 0)
            return (17.5f * (statTopSpeed / 10f) + 10) * .8f;
        else return 17.5f * (statTopSpeed / 9f) + 10;
    }
    protected float TranslateTrueTopSpeed() =>
        17.5f * (statTopSpeed / 9f) + 10;
    protected float TranslateAcceleration()
    {
        if (movement.steerInput != 0 && movement.offroadIndex > 0)
            return (3 * (statAcceleration / 10f) + 3) * .5f;
        else if (movement.steerInput != 0 || movement.offroadIndex > 0)
            return (3 * (statAcceleration / 10f) + 3) * .75f;
        return 3 * (statAcceleration / 9f) + 3;
    }
    protected float TranslateBraking() =>
        4f * (statBraking / 9f) + 5f;
    protected float TranslateHandling() =>
        2.5f * (statHandling / 9f) + 2;
    //

    /// <summary>
    /// Permet de s'assurer que notre position est toujours "un nombre 
    /// entier de pixels" relativement a notre resolution
    /// </summary>
    protected void SetCorrectPosition()
    {
        if (currentXPosition != Mathf.RoundToInt(trueXPosition * 6f) ||
            currentZPosition != Mathf.RoundToInt(trueZPosition * 6f))
        {
            currentXPosition = Mathf.RoundToInt(trueXPosition * 6f);
            currentZPosition = Mathf.RoundToInt(trueZPosition * 6f);

            movement.transform.position = new Vector3(currentXPosition / 6f,
                0f, currentZPosition / 6f);
        }
    }
    #endregion
}
