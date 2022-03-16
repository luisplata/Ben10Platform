using UnityEngine;

namespace ServiceLocatorPath
{
    public class InstallerServiceLocator : MonoBehaviour
    {
        [SerializeField] private LoadSceneComponent loadSceneComponent;
        private void Awake()
        {
            if (FindObjectsOfType<InstallerServiceLocator>().Length > 1)
            {
                Destroy(gameObject);
                return;
            }
            ServiceLocator.Instance.RegisterService<ILoadScream>(loadSceneComponent);
            ServiceLocator.Instance.RegisterService<IPlayFabCustom>(new PlayFabCustom());
            
            DontDestroyOnLoad(gameObject);
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}