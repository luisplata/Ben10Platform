using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckPoint : MonoBehaviour
{
    private CheckPoint _lastCheckPoint;

    public void SetCheckPoint(CheckPoint check)
    {
        _lastCheckPoint = check;
    }

    public void LoadLastCheckPoint()
    {
        if (_lastCheckPoint != null)
        {
            transform.position = _lastCheckPoint.transform.position;
        }
    }
}
