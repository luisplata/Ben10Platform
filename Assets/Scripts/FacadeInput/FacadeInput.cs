using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class FacadeInput : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private FunctionFromAnimator _fromAnimator;
    private bool pressJumpButton;
    private float velocity;
    private bool fire;
    private bool transforms;

    private void Update()
    {
        if (_joystick.Horizontal != 0)
        {
            velocity = _joystick.Horizontal > 0 ? 1 : -1;
        }
        else
        {
            velocity = 0;
        }
    }

    public float GetHorizontal()
    {
        return velocity;
    }

    public bool IsJumpPress()
    {
        var aux = pressJumpButton;
        pressJumpButton = false;
        return aux;
    }

    public bool IsAction()
    {
        return fire;
    }
    
    public void Transform()
    {
        
        Jump();
    }
    
    public void Fire()
    {
        fire = true;
    }
    
    public void TransformOmni(InputAction.CallbackContext context)
    {
        Debug.Log("Action");
        if (context.started && !context.performed && !context.canceled)
        {
            Transform();
        }
    }

    
    public void ActionPower(InputAction.CallbackContext context)
    {
        if (context.started && !context.performed && !context.canceled)
        {
            Fire();
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && !context.performed && !context.canceled)
        {
            Jump();
        }
    }
    
    //Jump
    public void Jump()
    {
        pressJumpButton = true;
    }

    public void Configure()
    {
        _fromAnimator.Configure();
        _fromAnimator.finishAnimatorPunch += FinishPunch;
    }

    private void OnDestroy()
    {
        _fromAnimator.finishAnimatorPunch -= FinishPunch;
    }

    public void FinishPunch()
    {
        fire = false;
    }
}
