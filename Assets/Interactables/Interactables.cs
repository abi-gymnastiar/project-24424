using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Interactables : MonoBehaviour
{
    public virtual void Interact()
    {
        Debug.Log("Interacting with " + gameObject.name);
    }
}
