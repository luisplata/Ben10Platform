using UnityEngine;

public class PersonalCharacterFactory
{
    private readonly PjConfiguration pjConfiguration;

    public PersonalCharacterFactory(PjConfiguration pjConfiguration)
    {
        this.pjConfiguration = pjConfiguration;
    }
        
    public Pj Create(string id)
    {
        var prefab = pjConfiguration.GetPjPrefabById(id);

        return Object.Instantiate(prefab);
    }
}