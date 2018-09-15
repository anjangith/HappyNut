using Managers;
using Pawns;
using UnityEngine;

namespace Interactables
{
    /// <summary>
    /// Object that on interaction give the player money.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Money : Interactable
    {
        /// <summary>
        /// How much the play will earn on interact.
        /// </summary>
        [Tooltip("How much the play will earn on interaction.")]
        public int Value;

        #region Overrides of Interactable

        public override bool Interact(Player player)
        {
            GameManager.GameManagerInst.Money += Value;
            Destroy(gameObject);
            //GetComponent<ParticleSystem>().Play();
            return false;
        }

        #endregion
    }
}
