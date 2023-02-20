using UnityEngine;

/// <summary>
/// During turn stabilization
/// </summary>
public class SlidingState : StateBase
{
    #region Variables
    private float rightVelocity, wrongVelocity;
    private Vector3 rightDirection, wrongDirection;
    #endregion

    #region PublicMethods
    public SlidingState(PlayerMovement pm) : base(pm)
    {
    }

    public override void EnterState(float vel, Vector3 oldDir, Vector3 newDir, float tzp, float txp)
    {
        rightVelocity = wrongVelocity = vel * .5f;
        wrongDirection = oldDir;
        rightDirection = newDir;
        trueZPosition = tzp;
        trueXPosition = txp;

        //On en profite pour changer l'angle de camera
        if (rightDirection.x != 0)
        {
            if (rightDirection.x > 0) movement.transform.rotation =
                    Quaternion.Euler(0, 90, 0);
            else movement.transform.rotation =
                    Quaternion.Euler(0, 270, 0);
        }
        else
        {
            if (rightDirection.z > 0) movement.transform.rotation =
                    Quaternion.Euler(0, 0, 0);
            else movement.transform.rotation =
                    Quaternion.Euler(0, 180, 0);
        }
    }

    public override void ExecuteState()
    {
        ChangeVelocity();
        ChangeSteering();
        SetCorrectPosition();

        if (wrongVelocity == 0) ExitState();
    }

    public override void ExitState()
    {
        movement.currentState = movement.grippingState;
        movement.currentState.EnterState(rightVelocity, wrongDirection,
            rightDirection, trueZPosition, trueXPosition);
    }
    #endregion

    #region PrivateMethods
    /// <summary>
    /// Modifie la velocite du vehicule
    /// </summary>
    private void ChangeVelocity()
    {
        DecreaseSliding();

        if (movement.pedalsInput > 0)
        {
            if (rightVelocity < TranslateTopSpeed())
            {
                rightVelocity += (TranslateAcceleration() / 2f) * Time.deltaTime;
                if (rightVelocity > TranslateTopSpeed()) rightVelocity = TranslateTopSpeed();
            }
        }
        else if (rightVelocity > 0)
        {
            if (movement.pedalsInput < 0)
                rightVelocity -= TranslateBraking() * Time.deltaTime;
            else rightVelocity -= Time.deltaTime;
            if (rightVelocity < 0) rightVelocity = 0;
        }

        ApplyVelocity();
    }

    /// <summary>
    /// Diminue lentement notre etat actuel
    /// </summary>
    private void DecreaseSliding()
    {
        if (wrongVelocity > TranslateHandling() * 2f * Time.deltaTime)
        {
            wrongVelocity -= TranslateHandling() * 2f * Time.deltaTime;
            rightVelocity += TranslateHandling() * 2f * Time.deltaTime;
        }
        else
        {
            rightVelocity += wrongVelocity;
            wrongVelocity = 0;
        }
    }

    /// <summary>
    /// Applique nos calculs sur le vehicule
    /// </summary>
    private void ApplyVelocity()
    {
        trueXPosition += rightVelocity * Time.deltaTime * rightDirection.x +
            wrongVelocity * Time.deltaTime * wrongDirection.x;
        trueZPosition += rightVelocity * Time.deltaTime * rightDirection.z +
            wrongVelocity * Time.deltaTime * wrongDirection.z;
    }

    /// <summary>
    /// Change lentement la position laterale du vehicule
    /// </summary>
    private void ChangeSteering()
    {
        if (movement.steerInput != 0 && rightVelocity > 0)
        {
            trueXPosition += (TranslateHandling() / 2f) * Time.deltaTime
                * rightDirection.z * movement.steerInput;
            trueZPosition += (TranslateHandling() / 2f) * Time.deltaTime
                * rightDirection.x * -movement.steerInput;
        }
    }
    #endregion
}
