using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesOfMainGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ServiceLocatorPath.ServiceLocator.Instance.GetService<ILoadScream>().Open(() => { });
    }
}
