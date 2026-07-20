using System;
using System.Collections.Generic;
using ProjectLInteractables.Items;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;



namespace ProjectLInteractables.Blocks {
    internal class BlockLDoorKeyhole : Block {
        string color = "unkown";

        public override void OnLoaded(ICoreAPI api) {
            if (this.Attributes != null && this.Attributes.Exists && this.Attributes["color"].Exists) {
                color = this.Attributes["color"].AsString("unkown");
            }
        }

        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel) {
            if (color == "unkown") return false;  // Early return if color not set

            // TODO make the dame source
            DamageSource damageSource = new DamageSource();

            IPlayerInventoryManager inventoryManager = byPlayer.InventoryManager;
            if (inventoryManager.ActiveHotbarSlot != null) {
                // Hotbar selected
                ItemSlot active_slot = inventoryManager.ActiveHotbarSlot;

                if (active_slot.Empty) {
                    byPlayer.Entity.ReceiveDamage(damageSource, 1.0f);
                }
                else {

                    if (active_slot.Itemstack.Collectible is ItemLDoorKey) {
                        Item key = world.Items[active_slot.Itemstack.Id];
                        string key_color = "unknown";
                        if (key.Attributes != null && key.Attributes.Exists && key.Attributes["color"].Exists) {
                            key_color = key.Attributes["color"].AsString("unkown");
                        }
                        world.Api.Logger.Event("key " + key_color);
                        if (key_color == color) {
                            return true;
                        }
                    }
                }
            }
            else {
                // Error
                byPlayer.Entity.ReceiveDamage(damageSource, 1.0f);
            }

            // Check if player has the right key
            //
            //
            // If doesn't

            return false;
        }

        public override bool OnBlockInteractStep(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel) {
            if (secondsUsed > 1.0f) return false;
            return true;
        }

        public override void OnBlockInteractStop(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel) {
            world.Api.Logger.Event("Stop");

            List<BlockPos> list = new List<BlockPos>();

            list.Add(blockSel.Position);

            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    for (int z = -1; z <= 1; z++) {
                        BlockPos n_pos = new BlockPos(blockSel.Position.X + x, blockSel.Position.Y + y, blockSel.Position.Z + z);
                        Block n_block = world.BulkBlockAccessor.GetBlock(n_pos);
                        if (n_block is BlockLDoorBlock &&
                            !list.Exists(p => p.X == n_pos.X && p.Y == n_pos.Y && p.Z == n_pos.Z) &&
                            ((BlockLDoorBlock)n_block).color == this.color
                        ) {
                            list = ((BlockLDoorBlock)n_block).OppenDoor(world, n_pos, byPlayer, list);
                        }
                    }
                }
            }

            DestroyBlocksWithDelay(world, list, byPlayer);
        }

        private void DestroyBlocksWithDelay(IWorldAccessor world, List<BlockPos> positions, IPlayer player) {
            int index = 0;

            Action<float> destroyNext = null;
            destroyNext = (dt) => {
                if (index >= positions.Count) return;

                world.PlaySoundAt(
                    world.BlockAccessor.GetBlock(positions[index]).Sounds.Break.Location,
                    positions[index].X, positions[index].Y, positions[index].Z
                );

                world.BlockAccessor.BreakBlock(positions[index], player, 0.0f);
                index++;

                // Run again in 0.1 seconds
                world.Api.Event.RegisterCallback(destroyNext, 100);
            };

            // Start
            world.Api.Event.RegisterCallback(destroyNext, 100);
        }

    }
}
