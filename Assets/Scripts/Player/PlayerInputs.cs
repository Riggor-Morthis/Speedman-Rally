using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>Collecte inputs</summary>
public class PlayerInputs : MonoBehaviour
{
    #region Variables
    public float pedalsInput { get; private set; }
    /// <summary>For 90deg turns</summary>
    private float turnInput;
    /// <summary>For 1 block slides</summary>
    public float steerInput { get; private set; }
    private bool menuInput = false;

    private float turnBuffer;
    #endregion

    #region UnityMethods
    public void OnPedals(InputValue iv) =>
        pedalsInput = iv.Get<float>() != 0 ? Mathf.Sign(iv.Get<float>()) : 0;
    public void OnTurn(InputValue iv) =>
        turnInput = iv.Get<float>() != 0 ? Mathf.Sign(iv.Get<float>()) : 0;
    public void OnSteer(InputValue iv) =>
        steerInput = iv.Get<float>() != 0 ? Mathf.Sign(iv.Get<float>()) : 0;
    public void OnMenuConfirm(InputValue iv)
    {
        if (iv.Get<float>() >= .4f) menuInput = true;
    }
    #endregion

    #region PublicMethods
    public float GetTurnInput()
    {
        turnBuffer = turnInput;
        turnInput = 0f;
        return turnBuffer;
    }

    public bool GetMenuInput()
    {
        if (menuInput)
        {
            menuInput = false;
            return true;
        }
        return false;
    }
    #endregion
}
