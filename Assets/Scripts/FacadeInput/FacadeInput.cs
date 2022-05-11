using UnityEngine;
using UnityEngine.InputSystem;

public class FacadeInput : MonoBehaviour
{
    public delegate void OnTransformation();
    public OnTransformation OnTransform;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private float timeToCanTransform, timeToCanFire;
    private FunctionFromAnimator _fromAnimator;
    private bool pressJumpButton;
    private float velocity;
    private bool fire;
    private bool canTransforms;
    private bool canFire;
    private float timeToCanTransformTemp, timeToCanFireTemp;

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

        if (!canTransforms) timeToCanTransformTemp += Time.deltaTime;
        timeToCanFireTemp += Time.deltaTime;
        
        if (timeToCanTransformTemp > timeToCanTransform)
        {
            canTransforms = true;
        }

        if (timeToCanFireTemp > timeToCanFire)
        {
            timeToCanFireTemp = 0;
            canFire = true;
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
        OnTransform?.Invoke();
    }
    
    public void Fire()
    {
        fire = true;
    }
    
    public void TransformOmni(InputAction.CallbackContext context)
    {
        Debug.Log("Action");
        if (context.started && !context.performed && !context.canceled && CanTransform())
        {
            Transform();
            canTransforms = false;
            timeToCanTransformTemp = 0;
        }
    }

    private bool CanTransform()
    {
        return canTransforms;
    }


    public void ActionPower(InputAction.CallbackContext context)
    {
        if (context.started && !context.performed && !context.canceled && CanActionPower())
        {
            Fire();
            canFire = false;
        }
    }

    private bool CanActionPower()
    {
        return canFire;
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

    public void Configure(Pj getAnimator)
    {
        _fromAnimator = getAnimator.GetComponent<FunctionFromAnimator>();
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

    public void DestroyPj(Pj pj)
    {
        if (pj == null) return;
        OnDestroy();
        Destroy(pj.gameObject);
    }

    public float GetPorcentToTransformInAlien()
    {
        var aux = timeToCanTransformTemp / timeToCanTransform;
        //Debug.Log($"{aux}");
        return aux;
    }
}
