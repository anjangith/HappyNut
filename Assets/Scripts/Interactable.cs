using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    /// <summary>
    /// Does this object get interacted with immediatly on overlap.
    /// </summary>
    [Tooltip("Does this object get interacted with immediatly on overlap.")]
    public bool Immediate;

    //public abstract bool Interact();
    public abstract bool Interact(Player player);

}
