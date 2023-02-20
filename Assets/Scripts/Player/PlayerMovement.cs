using UnityEngine;

/// <summary>
/// Main state machine
/// </summary>
[RequireComponent(typeof(PlayerInputs))]
public class PlayerMovement : MonoBehaviour
{
    #region Variables
    //State machine
    private StateBase currentState;
    public GrippingState grippingState { get; private set; }
    public DriftingState driftingState { get; private set; }

    //Components
    private PlayerInputs inputs;

    //Facilitate inputs access
    public float pedalsInput { get; private set; }
    /// <summary>For 90deg turns</summary>
    public float turnInput { get; private set; }
    /// <summary>For 1 block slides</summary>
    public float steerInput { get; private set; }
    public bool menuConfirmInput { get; private set; }
    #endregion

    #region UnityMethods
    private void Awake()
    {
        InitializeVariables();
    }

    private void Update()
    {
        GatherInputs();
        ExecuteState();
    }
    #endregion

    #region PrivateMethods
    /// <summary>
    /// Initialise variables
    /// </summary>
    private void InitializeVariables()
    {
        inputs = GetComponent<PlayerInputs>();

        grippingState = new GrippingState(this);
        driftingState = new DriftingState(this);
        currentState = grippingState;
    }

    /// <summary>
    /// Recuperation inputs preliminaires
    /// </summary>
    private void GatherInputs()
    {
        pedalsInput = inputs.pedalsInput;
        turnInput = inputs.turnInput;
        steerInput = inputs.steerInput;
    }

    /// <summary>
    /// Actions
    /// </summary>
    private void ExecuteState()
    {
        currentState.ExecuteState();
    }
    #endregion
}
