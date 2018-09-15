using UnityEngine;

namespace Interactables
{
    public class DialogBounds : MonoBehaviour
    {

        private Dialog dialog;

        public void Start()
        {
            dialog = transform.parent.GetComponentInChildren<Dialog>();
        }

        public void OnTriggerExit2D(Collider2D collider)
        {
            dialog.Leave();
        }
    }
}
