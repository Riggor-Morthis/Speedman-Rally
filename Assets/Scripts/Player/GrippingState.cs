using UnityEngine;

/// <summary>
/// When going straigth, after turn stabilisation
/// </summary>
public class GrippingState : StateBase
{
    #region Variables
    /// <summary>Vitesse actuelle du vehicule</summary>
    private float velocity;
    /// <summary>Dans quel sens on va</summary>
    protected Vector3 currentDirection;
    #endregion

    #region PublicMethods
    public GrippingState(PlayerMovement pm) : base(pm)
    {
        if (movement.transform.rotation.eulerAngles.y == 90) currentDirection = Vector3.right;
        else if (movement.transform.rotation.eulerAngles.y == 270) currentDirection = Vector3.left;
        else if (movement.transform.rotation.eulerAngles.y == 180) currentDirection = Vector3.back;
        else currentDirection = Vector3.forward;
    }

    public override void EnterState(float vel, Vector3 oldDir, Vector3 newDir, float tzp, float txp)
    {
        velocity = vel;
        currentDirection = newDir;
        trueZPosition = tzp;
        trueXPosition = txp;
    }

    public override void ExecuteState()
    {
        ChangeVelocity();
        ChangeSteering();
        SetCorrectPosition();

        if (movement.turnInput != 0 && velocity > 0) ExitState();
    }

    public override void ExitState()
    {
        movement.currentState = movement.slidingState;
        movement.currentState.EnterState(velocity, currentDirection, new Vector3(
            currentDirection.z * movement.turnInput, 0f,
            currentDirection.x * -movement.turnInput),
            trueZPosition, trueXPosition);
    }

    public override float GetSpeedRatio() => velocity / TranslateTrueTopSpeed();
    public override Vector3 GetDirection() => currentDirection;
    #endregion

    #region PrivateMethods
    /// <summary>
    /// Change progressivement la velocite (acceleration et freins)
    /// Assure le capping de la velocite
    /// </summary>
    private void ChangeVelocity()
    {
        if (movement.pedalsInput > 0 && velocity < TranslateTopSpeed())
        {
            velocity += TranslateAcceleration() * Time.deltaTime;
            if (velocity > TranslateTopSpeed()) velocity = TranslateTopSpeed();
        }
        else if (velocity > 0)
        {
            if (movement.pedalsInput < 0)
                velocity -= TranslateBraking() * Time.deltaTime;
            else if (movement.pedalsInput == 0) velocity -= Time.deltaTime;
            if (velocity < 0) velocity = 0;
        }

        ApplyVelocity();
    }

    /// <summary>
    /// Applique la velocite actuelle a notre voiture
    /// </summary>
    private void ApplyVelocity()
    {
        //Deux time, on est sur une acceleration
        trueXPosition += velocity * Time.deltaTime * currentDirection.x;
        trueZPosition += velocity * Time.deltaTime * currentDirection.z;
    }

    /// <summary>
    /// Change lentement la position laterale du vehicule
    /// </summary>
    private void ChangeSteering()
    {
        if (movement.steerInput != 0 && velocity > 0)
        {
            trueXPosition += TranslateHandling() * Time.deltaTime
                * currentDirection.z * movement.steerInput;
            trueZPosition += TranslateHandling() * Time.deltaTime
                * currentDirection.x * -movement.steerInput;
        }
    }
    #endregion
}
