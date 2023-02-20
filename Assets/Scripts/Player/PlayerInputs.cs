using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>Collecte inputs</summary>
public class PlayerInputs : MonoBehaviour
{
    #region Variables
    public float pedalsInput { get; private set; }
    /// <summary>For 90deg turns</summary>
    public float turnInput { get; private set; }
    /// <summary>For 1 block slides</summary>
    public float steerInput { get; private set; }
    public bool menuConfirmInput { get; private set; }

    private float deadZone = .4f;
    #endregion

    #region UnityMethods
    public void OnPedals(InputValue iv) =>
        pedalsInput = iv.Get<float>() != 0 ? Mathf.Sign(iv.Get<float>()) : 0;
    public void OnTurn(InputValue iv) =>
        turnInput = iv.Get<float>() != 0 ? Mathf.Sign(iv.Get<float>()) : 0;
    public void OnSteer(InputValue iv) =>
        steerInput = iv.Get<float>() != 0 ? Mathf.Sign(iv.Get<float>()) : 0;
    public void OnMenuConfirm(InputValue iv) =>
        menuConfirmInput = iv.Get<float>() > deadZone;
    #endregion
}
