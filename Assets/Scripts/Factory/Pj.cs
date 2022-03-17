using UnityEditor.Animations;
using UnityEngine;

public abstract class Pj : MonoBehaviour
{
    [SerializeField] protected string id;
    [SerializeField] protected AnimatorController _controller;
    [SerializeField] protected Animator _animator;

    public string Id => id;
    public AnimatorController Controller => _controller;

    public Animator GetAnimator()
    {
        return _animator;
    }
}