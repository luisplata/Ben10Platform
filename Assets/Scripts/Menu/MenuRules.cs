using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MenuRules : MonoBehaviour
    {
        public void LoadScene(int index)
        {
            ServiceLocatorPath.ServiceLocator.Instance.GetService<ILoadScream>().Close(() =>
            {
                SceneManager.LoadScene(index);
            });
        }
    }
}
