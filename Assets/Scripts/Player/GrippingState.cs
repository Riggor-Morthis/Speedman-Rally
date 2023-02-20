using UnityEngine;

/// <summary>
/// When going straigth, after turn stabilisation
/// </summary>
public class GrippingState : StateBase
{
    #region Variables
    private float velocity;
    #endregion

    #region PublicMethods
    public GrippingState(PlayerMovement pm) : base(pm)
    {
    }

    public override void EnterState()
    {

    }

    public override void ExecuteState()
    {
        ChangeVelocity();
        ApplyVelocity();

        SetCorrectPosition();
    }

    public override void ExitState()
    {

    }
    #endregion

    #region PrivateMethods
    /// <summary>
    /// Change progressivement la velocite (acceleration et freins)
    /// Assure le capping de la velocite
    /// </summary>
    private void ChangeVelocity()
    {
        if (movement.pedalsInput > 0)
        {
            if(velocity < TranslateTopSpeed())
            {
                velocity += TranslateAcceleration() * Time.deltaTime;
                if (velocity > TranslateTopSpeed()) velocity = TranslateTopSpeed();
            }
        }
        else if(velocity > 0)
        {
            if (movement.pedalsInput < 0)
                velocity -= TranslateBraking() * Time.deltaTime;
            else velocity -= Time.deltaTime;
            if (velocity < 0) velocity = 0;
        }
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
    #endregion
}
