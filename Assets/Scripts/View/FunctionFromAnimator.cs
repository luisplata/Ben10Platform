using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionFromAnimator : MonoBehaviour
{
    public delegate void OnActionEver();

    public OnActionEver finishAnimatorPunch;
    public void Configure()
    {
        Debug.Log($"Configure {name}");
    }

    public void FinishAnimatorPunch()
    {
        Debug.Log($"Finish punch animation {name}");
        finishAnimatorPunch?.Invoke();
    }
}
