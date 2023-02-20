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
    protected Vector3 currentDirection = Vector3.forward;
    #endregion

    #region PublicMethods
    public StateBase(PlayerMovement pm)
    {
        movement = pm;
    }

    /// <summary>
    /// Initialisation correcte etat
    /// </summary>
    public abstract void EnterState();
    /// <summary>
    /// Calculs d'etat
    /// </summary>
    public abstract void ExecuteState();
    /// <summary>
    /// Conditions de sortie d'etat
    /// </summary>
    public abstract void ExitState();
    #endregion

    #region PrivateMethods
    //Translate player-friendly stats into maths-friendly stats
    protected float TranslateTopSpeed() =>
        17.5f * (statTopSpeed / 10f) + 10;
    protected float TranslateAcceleration() =>
        3 * (statAcceleration / 10f) + 3;
    protected float TranslateBraking() =>
        3 * (statBraking / 10f) + 5;

    /// <summary>
    /// Permet de s'assurer que notre position est toujours "un nombre 
    /// entier de pixels" relativement a notre resolution
    /// </summary>
    protected void SetCorrectPosition()
    {
        if(currentXPosition != Mathf.RoundToInt(trueXPosition * 6f) ||
            currentZPosition != Mathf.RoundToInt(trueZPosition * 6f))
        {
            currentXPosition = Mathf.RoundToInt(trueXPosition * 6f);
            currentZPosition = Mathf.RoundToInt(trueZPosition * 6f);

            movement.transform.position = new Vector3(currentXPosition / 6f,
                0f, currentZPosition / 6f);
        }

        /////////////////
        ///
        if (movement.transform.position.z >= 5)
        {
            trueXPosition = 0;
            trueZPosition = -5f;
        }
        /////////////////
        ///
    }
    #endregion
}
