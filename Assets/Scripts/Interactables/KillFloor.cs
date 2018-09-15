using Pawns;
using UnityEngine;

namespace Interactables
{
    public class KillFloor : Interactable {
        public override bool Interact(Player player)
        {
            player.ChangeHealthByAmount(player.MaxHealth);
            return false;
        }

    }
}
