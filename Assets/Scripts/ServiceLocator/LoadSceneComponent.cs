using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LoadSceneComponent : MonoBehaviour, ILoadScream
{
    [SerializeField] private Animator anim;
    private bool finishAnimation;

    public void Finished()
    {
        finishAnimation = true;
    }

    public void NotFinished()
    {
        finishAnimation = false;
    }
    public void Open()
    {
        anim.SetBool("open", false);
    }

    public async UniTaskVoid Open(Action a)
    {
        Open();
        while (!finishAnimation)
        {
            await UniTask.NextFrame();
        }
        a?.Invoke();
        NotFinished();
    }

    public void Close()
    {
        anim.SetBool("open", true);
    }
    public async UniTaskVoid Close(Action a)
    {
        Close();
        while (!finishAnimation)
        {
            await UniTask.NextFrame();
        }
        a?.Invoke();
        NotFinished();
    }
}