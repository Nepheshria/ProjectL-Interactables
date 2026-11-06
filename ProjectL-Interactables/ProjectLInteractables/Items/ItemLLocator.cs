using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.API.MathTools;
using Vintagestory.API.Config;

namespace ProjectLInteractables.Items {
    internal class ItemLLocator : Item {
        public override void OnHeldInteractStart(
            ItemSlot slot,
            EntityAgent byEntity,
            BlockSelection blockSel,
            EntitySelection entitySel,
            bool firstEvent,
            ref EnumHandHandling handling) {
            // TODO Send message to player
            //
            // Save location in attribute
            //
            // Cook mesage to be direction toward location
        }
    }
}
