using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Pj configuration")]
public class PjConfiguration : ScriptableObject
{
    [SerializeField] private Pj[] pjs;
    private Dictionary<string, Pj> idToPjs;

    private void Awake()
    {
        idToPjs = new Dictionary<string, Pj>(pjs.Length);
        foreach (var powerUp in pjs)
        {
            idToPjs.Add(powerUp.Id, powerUp);
        }
    }

    public Pj GetPjPrefabById(string id)
    {
        if (!idToPjs.TryGetValue(id, out var pj))
        {
            throw new Exception($"Pj with id {id} does not exit");
        }
        return pj;
    }
}