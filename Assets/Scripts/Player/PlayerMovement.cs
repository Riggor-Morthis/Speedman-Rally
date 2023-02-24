using UnityEngine;
using System.Collections;

/// <summary>
/// Main state machine
/// </summary>
[RequireComponent(typeof(PlayerInputs))]
public class PlayerMovement : MonoBehaviour
{
    #region Variables
    //State machine
    public StateBase currentState;
    public GrippingState grippingState { get; private set; }
    public SlidingState slidingState { get; private set; }
    private bool canAct = false;

    //Components
    private PlayerInputs inputs;

    //Facilitate inputs access
    public float pedalsInput { get; private set; }
    /// <summary>For 90deg turns</summary>
    public float turnInput { get; private set; }
    /// <summary>For 1 block slides</summary>
    public float steerInput { get; private set; }
    public bool menuConfirmInput { get; private set; }

    //save position for respawns
    private Vector3[] previousPositions = new Vector3[5];
    private Quaternion[] previousRotations = new Quaternion[5];
    public int offroadIndex { get; private set; }

    private AudioManager audioManager;
    private UIManager uiManager;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        InitializeVariables();
        InitializeStateMachine();
        StartCoroutine(COStockPosition());
    }

    private void Update()
    {
        GatherInputs();
        ExecuteState();
    }
    #endregion

    #region Public Methods
    public void ChangeCarStatus(bool status) => canAct = status;

    public void CrashTheCar()
    {
        StartCoroutine(CORespawn());
    }

    public void ChangeOffroadIndex(int step) => offroadIndex += step;
    public float GetSpeedRatio()
    {
        if (!canAct) return 0;
        return currentState.GetSpeedRatio();
    }
    public Vector3 GetDirection() => currentState.GetDirection();
    #endregion

    #region PrivateMethods
    /// <summary>
    /// Initialise variables
    /// </summary>
    private void InitializeVariables()
    {
        //Components
        inputs = GetComponent<PlayerInputs>();
        audioManager = GetComponentInChildren<AudioManager>();
        uiManager = GetComponent<UIManager>();
        offroadIndex = 0;
    }

    private void InitializeStateMachine()
    {
        //La state machine a proprement parler
        grippingState = new GrippingState(this);
        slidingState = new SlidingState(this);
        currentState = grippingState;

        //Mise en place du respawn
        for (int i = 0; i < 5; i++)
        {
            previousPositions[i] = transform.position;
            previousRotations[i] = transform.rotation;
        }

        //On fait tout de suite un update
        GatherInputs();
        ExecuteState();
    }

    /// <summary>
    /// Recuperation inputs preliminaires
    /// </summary>
    private void GatherInputs()
    {
        pedalsInput = inputs.pedalsInput;
        turnInput = inputs.GetTurnInput();
        steerInput = inputs.steerInput;
    }

    /// <summary>
    /// Actions
    /// </summary>
    private void ExecuteState()
    {
        if (canAct) currentState.ExecuteState();
    }
    #endregion

    #region Coroutines
    private IEnumerator COStockPosition()
    {
        float saveTimer;

        while (true)
        {
            saveTimer = .5f;

            while (saveTimer >= 0)
            {
                if (canAct && offroadIndex == 0) saveTimer -= Time.deltaTime;
                yield return null;
            }

            for (int i = 0; i < 4; i++)
            {
                previousPositions[i] = previousPositions[i + 1];
                previousRotations[i] = previousRotations[i + 1];
            }

            previousPositions[4] = transform.position;
            previousRotations[4] = transform.rotation;
        }
    }

    private IEnumerator CORespawn()
    {
        canAct = false;
        audioManager.PlayCrashSound();
        uiManager.HideEverything();

        yield return new WaitForSecondsRealtime(1);

        transform.position = previousPositions[0];
        transform.rotation = previousRotations[0];

        canAct = true;
        InitializeStateMachine();
    }
    #endregion
}
