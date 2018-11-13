using UnityEngine;

namespace Pawns
{
    
    public class Enemy : Movable
    {

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log(collision.collider.gameObject.tag);
            Debug.Log(collision.otherCollider.gameObject.tag);
            if (collision.otherCollider.gameObject.CompareTag("Weapon"))
            {
                if (!collision.collider.gameObject.CompareTag("Player"))
                {
                    return;
                }
                var player = collision.gameObject.GetComponent<Player>();
                if (!player)
                {
                    Debug.LogWarning("Player Tag on game object without Player Script.");
                }
                player.ChangeHealthByAmount(Damage);
            }
        }
    }

}