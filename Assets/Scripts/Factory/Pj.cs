using Mechanics;
using UnityEngine;

public abstract class Pj : MonoBehaviour
{
    [SerializeField] protected string id;
    [SerializeField] protected Animator _animator;
    protected IPlayerController _playerController;

    public string Id => id;

    public Animator GetAnimator()
    {
        return _animator;
    }

    public virtual void Action(){}

    public void ConfigurePj(IPlayerController playerController)
    {
        _playerController = playerController;
    }
}