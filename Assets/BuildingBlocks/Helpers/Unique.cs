using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Its kind of a singleton in the means that there could be only one instance of this class present in the Scene
/// But all this class does is that the instance destroys intself if there is already other instance in the scene
/// </summary>
public class Unique : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _monobehaviour;

    private void Awake()
    {
        System.Type type = _monobehaviour.GetType();
        Object other = FindObjectOfType(type);

        if (other != null && other != this)
        {
            Destroy(this.gameObject);
        }
    }
}