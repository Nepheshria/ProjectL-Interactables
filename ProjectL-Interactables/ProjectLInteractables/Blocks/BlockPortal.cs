//Here are the imports for this script. Most of these will add automatically.
using System;
using ProjectLInteractables.Items;

using Vintagestory.API.Common.Entities;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

/*
* The namespace the class will be in. This is essentially the folder the script is found in.
* If you need to use the BlockBumper class in any other script, you will have to add 'using ProjectLInteractables.Blocks' to that script.
*/
namespace ProjectLInteractables.Blocks {
    /*
    * The class definition. Here, you define BlockBumper as a child of Block, which
    * means you can 'override' many of the functions within the general Block class.
    */
    internal class BlockPortal : Block {

        public static Vec3d portal_location = null;


        public override void OnEntityCollide(IWorldAccessor world, Entity entity, BlockPos pos, BlockFacing facing, Vec3d collideSpeed, bool isImpact) {

            if (portal_location == null) {
                entity.World.Logger.Warning("Portal location not set!");
                return;
            }

            if (entity.World.Side == EnumAppSide.Server) {
                if (entity is not EntityPlayer) return;

                AssetLocation sound = new("linteractables:sounds/effect/Pop");
                entity.World.PlaySoundAt(sound, entity, null, 0.5F, 5);

                entity.TeleportTo(portal_location.XInt, portal_location.YInt, portal_location.ZInt);
                return;
            }

        }
    }
}
