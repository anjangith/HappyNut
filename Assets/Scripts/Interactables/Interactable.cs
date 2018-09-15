using Pawns;
using UnityEngine;

namespace Interactables
{
    /// <summary>
    /// Triggerable object.
    /// </summary>
    public abstract class Interactable : MonoBehaviour
    {
        /// <summary>
        /// Does this object get interacted with immediatly on overlap.
        /// </summary>
        [Tooltip("Does this object get interacted with immediatly on overlap.")]
        public bool Immediate;

        //public abstract bool Interact();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The player who interacted with you.</param>
        /// <returns>Wether to slow down the player after interaction.</returns>
        public abstract bool Interact(Player player);

    }
}
