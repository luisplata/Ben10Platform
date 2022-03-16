using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionFromAnimator : MonoBehaviour
{
    public delegate void OnActionEver();

    public OnActionEver finishAnimatorPunch;
    public void Configure()
    {
    }

    public void FinishAnimatorPunch()
    {
        finishAnimatorPunch?.Invoke();
    }
}
