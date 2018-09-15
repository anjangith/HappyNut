using Pawns;
using UnityEngine;

namespace Interactables
{
    public class Dialog : Interactable
    {
        public GameObject DialogBig;
        public GameObject DialogIcon;

        public void Leave()
        {
            DialogIcon.SetActive(true);
            DialogBig.SetActive(false);
        }

        #region Overrides of Interactable

        public override bool Interact(Player player)
        {
            DialogIcon.SetActive(false);
            DialogBig.SetActive(true);
            return false;
        }

        #endregion
    }
}
