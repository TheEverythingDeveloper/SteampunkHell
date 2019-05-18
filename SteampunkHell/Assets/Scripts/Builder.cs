using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public string[] names;

    public Character prefab;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            BuilderCharacter bc = new BuilderCharacter();

            float rnd = Random.Range(0, 1f);

            if (rnd < 0.5f)
            {
                bc.SetLife(Random.Range(0, 101))
                    .SetName(names[Random.Range(0, names.Length)])
                    .SetProf((Prof)Random.Range(0, 3));
            }
            else
            {
                bc.SetLife(Random.Range(0, 101))
                    .SetProf((Prof)Random.Range(0, 3))
                    .SetColor(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
            }

            var c = Instantiate(prefab);
            c.SetBuilder(bc);
        }
    }
}
