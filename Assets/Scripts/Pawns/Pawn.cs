using System;
using System.Collections;
using UnityEngine;

namespace Pawns
{

    /// <summary>
    /// Any controable character. Enemy, Player, etc...
    /// Handles Health and Death.
    /// </summary>
    public class Pawn : PhysicsObject
    {
        #region Protected Variables

        /// <summary>
        /// Can the player move, jump, etc.
        /// </summary>
        protected bool Controllable = true;

        /// <summary>
        /// The amount of Damage this pawn can do.
        /// </summary>
        protected int Damage = 20;

        /// <summary>
        /// Should the pawn be Destroyed when it dies.
        /// </summary>
        protected bool KillOnDied = false;

        /// <summary>
        /// Current hp a player has.
        /// </summary>
        protected float CurrentHealth = 100;

        #endregion

        #region Public Variables

        /// <summary>
        /// Total hp a player can have.
        /// </summary>
        public float MaxHealth = 100;

        #endregion

        //public Weapon weapon;

        /// <summary>
        /// Subtract amount to Helath.
        /// </summary>
        /// <param name="amount">dmg to do to player. opposite for healing.</param>
        public void ChangeHealthByAmount(int amount)
        {
            CurrentHealth -= amount;

            if (amount < 0)
            {
                //AudioManager.audioManager.PlaySoundEffect("Healed");
            }
            else if (amount > 0)
            {
                //GetComponent<AICharacterControl>().MyState = AICharacterControl.State.IsHit;
                Debug.Log("Hit");
            }
            if (CurrentHealth <= 0)
            {
                OnDied();
                Debug.Log("Died");
            }
        }

        public event EventHandler Died;

        /// <summary>
        /// Triggers the Died Event.
        /// </summary>
        protected virtual void OnDied()
        {
            StartCoroutine(Died_Implementation());
        }

        /// <summary>
        /// The implementation that will be used for every pawn. It will then trigger the Died event.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Died_Implementation()
        {
            Controllable = false;
            //var aiController = GetComponent<AICharacterControl>();
            //aiController.MyState = AICharacterControl.State.IsDead;
            //aiController.IsDead = true;

            //After some delay destroy enemy
            yield return new WaitForSeconds(2);
            EventHandler handler = Died;
            if (handler != null)
            {
                handler(this, null);
            }
            if (KillOnDied)
            {
                Destroy(gameObject);
            }
        }

    }
}
