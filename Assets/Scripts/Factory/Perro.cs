using UnityEngine;

class Perro : Pj
{
    [SerializeField] private float _smoothTime;
    public override void Action()
    {
        base.Action();
        Debug.Log("Perro atacando");
        _playerController.MoveForward(_smoothTime);
    }
}