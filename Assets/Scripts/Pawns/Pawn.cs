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
        [SerializeField]
        protected int Damage = 20;

        /// <summary>
        /// Should the pawn be Destroyed when it dies.
        /// </summary>
        [SerializeField]
        protected bool KillOnDied = false;

        /// <summary>
        /// Current hp a player has.
        /// </summary>
        protected virtual float CurrentHealth { get; set; }

        /// <summary>
        /// refrence to animator.
        /// </summary>
        protected Animator animator;

        /// <summary>
        /// Has pawn used up all thier stamina.
        /// </summary>
        protected bool depletedStamina = false;

        protected float stamina = 100.0f;

        #endregion

        #region Public Variables

        /// <summary>
        /// Controls how long the pawn can run.
        /// </summary>
        public float Stamina
        {
            get { return stamina; }
            set
            {
                stamina = value;
                if(stamina > 100)
                {
                    stamina = 100;
                }
                else if (stamina < 0)
                {
                    stamina = 0;
                }
            }
        }

        /// <summary>
        /// Total hp a pawn can have.
        /// </summary>
        public int MaxHealth = 100;

        /// <summary>
        /// set true to stop pawn from moving. must be implemented.
        /// </summary>
        [NonSerialized]
        public bool Paused = false;

        #endregion

        // Use this for initialization
        protected void Awake()
        {
            animator = GetComponent<Animator>();
        }

        //public Weapon weapon;

        /// <summary>
        /// Subtract amount to Helath.
        /// </summary>
        /// <param name="amount">dmg to do to player. opposite for healing.</param>
        public void ChangeHealthByAmount(int amount)
        {
            //Currently getting hit.
            if (amount > 0 && animator.GetBool("isHit"))
            {
                return;
            }

            CurrentHealth -= amount;

            if (amount < 0)
            {
                //AudioManager.audioManager.PlaySoundEffect("Healed");
            }
            else if (amount > 0)
            {
                //GetComponent<AICharacterControl>().MyState = AICharacterControl.State.IsHit;
                OnHit(amount);
            }
            if (CurrentHealth <= 0)
            {
                OnDied();
            }
        }

        public event EventHandler<HitEventArgs> Hit;

        /// <summary>
        /// Triggers the Hit Event.
        /// </summary>
        protected virtual void OnHit(int amount)
        {
            Debug.Log("Hit");

            animator.SetBool("isHit", true);

            var args = new HitEventArgs();
            args.Amount = amount;
            Hit?.Invoke(this, args);
        }

        public event EventHandler Died;

        /// <summary>
        /// Triggers the Died Event.
        /// </summary>
        protected virtual void OnDied()
        {
            Debug.Log("Died");
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
            Died?.Invoke(this, null);
            if (KillOnDied)
            {
                Destroy(gameObject);
            }
        }

    }

    public class HitEventArgs : EventArgs
    {
        /// <summary>
        /// Amount hit for can be positive to heal.
        /// </summary>
        public int Amount { get; set; }
    }
}
