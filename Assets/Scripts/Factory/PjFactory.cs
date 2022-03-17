using UnityEngine;

namespace Factory
{
    public class PjFactory : MonoBehaviour, IPjFactory
    {
        [SerializeField] private PjConfiguration config;
        private PersonalCharacterFactory pjsFactory;
        public void Configure()
        {
            pjsFactory = new PersonalCharacterFactory(Instantiate(config));
        }
        
        // Logic

        public Pj SpawnPj(string id)
        {
            return pjsFactory.Create(id);
        }
    }
}