using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.API.MathTools;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;

namespace ProjectLInteractables.Items {
    internal class ItemLLocator : Item {
        public override void OnHeldInteractStart(
            ItemSlot slot,
            EntityAgent byEntity,
            BlockSelection blockSel,
            EntitySelection entitySel,
            bool firstEvent,
            ref EnumHandHandling handling) {

            // Request server-side execution
            handling = EnumHandHandling.PreventDefault;

            // Get locations
            Vec3d cur_pos = byEntity.Pos.XYZ.Add(0, byEntity.LocalEyePos.Y / 2, 0);

            // Read attributes from the actual itemstack
            ITreeAttribute target = slot.Itemstack.Attributes.GetTreeAttribute("target");

            if (target == null) {
                byEntity.World.Logger.Warning("Locator has no configured target!");
                return;
            }

            int x = target.GetInt("x");
            int y = target.GetInt("y");
            int z = target.GetInt("z");

            Vec3d target_pos = new(x + 0.5, y + 0.5, z + 0.5);

            Vec3d target_dir = target_pos - cur_pos;
            double distance = target_dir.Length();

            target_dir.Normalize();

            if (byEntity.World.Side == EnumAppSide.Client) {
                // Direction laser
                for (int i = 20; i < 75; i++) {
                    Vec3d realPos = cur_pos + target_dir * i * 0.05;

                    byEntity.World.SpawnParticles(1, ColorUtil.ToRgba(255, 255, 0, 0), realPos, realPos, Vec3f.Zero, Vec3f.Zero, 0.5F, 0F, 1, EnumParticleModel.Cube);
                }
                return;
            }

            AssetLocation sound = new("linteractables:sounds/effect/BipBoup");
            byEntity.World.PlaySoundAt(sound, byEntity, null, 5F);

            // Get player
            if (byEntity is not EntityPlayer entityPlayer) return;

            IServerPlayer player = entityPlayer.Player as IServerPlayer;

            if (player == null) return;

            player.SendMessage(GlobalConstants.GeneralChatGroup, $"Distance {distance:F2}m", EnumChatType.OwnMessage);

        }
    }
}
