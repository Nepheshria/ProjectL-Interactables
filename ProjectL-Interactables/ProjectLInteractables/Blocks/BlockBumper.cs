//Here are the imports for this script. Most of these will add automatically.
using System;
using ProjectLInteractables.Items;

using Vintagestory.API.Common.Entities;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

/*
* The namespace the class will be in. This is essentially the folder the script is found in.
* If you need to use the BlockBumper class in any other script, you will have to add 'using ProjectLInteractables.Blocks' to that script.
*/
namespace ProjectLInteractables.Blocks {
    /*
    * The class definition. Here, you define BlockBumper as a child of Block, which
    * means you can 'override' many of the functions within the general Block class.
    */
    internal class BlockBumper : Block {

        private float bump_multiplier = 1f;

        public override void OnLoaded(ICoreAPI api) {
            if (this.Attributes != null && this.Attributes.Exists && this.Attributes["color"].Exists) {
                string color = this.Attributes["color"].AsString("unkown");
                // TODO make different level
                switch (color.Trim().ToLower()) {
                    case "black":
                        bump_multiplier = 10f;
                        break;
                    case "red":
                        bump_multiplier = 5f;
                        break;
                    default:
                        bump_multiplier = 1f;
                        break;
                }
            }
        }

        //Any code within this 'override' function will be called when a trampoline block is placed.
        public override void OnBlockPlaced(IWorldAccessor world, BlockPos blockPos, ItemStack byItemStack = null) {
            //Log a message to the console.
            //Perform any default logic when our block is placed.
            if (this.Attributes != null && this.Attributes.Exists && this.Attributes["color"].Exists) {
                string color = this.Attributes["color"].AsString("unkown");
                world.Api.Logger.Event("Bumper color: " + color);
            }
            base.OnBlockPlaced(world, blockPos, byItemStack);
        }

        //Any code within this 'override' function will be called when a trampoline block is broken.
        public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1) {
            //Log a message to the console.
            //Perform any default logic when our block is broken (e.g., dropping the block as an item.)
            base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier);
        }

        public override void OnEntityCollide(IWorldAccessor world, Entity entity, BlockPos pos, BlockFacing facing, Vec3d collideSpeed, bool isImpact) {
            if (isImpact) {
                if (facing.IsVertical) {
                    //TODO Remove fall damage
                    //TODO Make sneak bounce less EntityControls would help

                    entity.Pos.Motion.Y *= -bump_multiplier;
                }

                if (facing.IsHorizontal) {
                    if (facing.IsAxisNS) {
                        entity.Pos.Motion.Z *= -bump_multiplier;
                    }
                    else {
                        entity.Pos.Motion.X *= -bump_multiplier;
                    }
                }

                if (this.Attributes != null && this.Attributes.Exists && this.Attributes["color"].Exists) {
                    string color = this.Attributes["color"].AsString("unkown");
                    world.Api.Logger.Event($"Bumper color: {color}, multiplier: {bump_multiplier}");
                }

            }
        }
    }
}
