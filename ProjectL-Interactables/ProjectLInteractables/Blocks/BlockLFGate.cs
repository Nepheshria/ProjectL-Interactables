
using System;
using System.Collections.Generic;
using ProjectLInteractables.Items;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace ProjectLInteractables.Blocks {
    internal class BlockLFGate : Block {
        public override bool OnBlockInteractStart(
            IWorldAccessor world,
            IPlayer byPlayer,
            BlockSelection blockSel) {
            IPlayerInventoryManager inventoryManager = byPlayer.InventoryManager;
            ItemSlot activeSlot = inventoryManager?.ActiveHotbarSlot;

            // Require the LF key.
            if (activeSlot != null &&
                !activeSlot.Empty &&
                activeSlot.Itemstack.Collectible is ItemLFKey) {
                return true;
            }

            return false;
        }

        public override bool OnBlockInteractStep(
            float secondsUsed,
            IWorldAccessor world,
            IPlayer byPlayer,
            BlockSelection blockSel) {
            // Continue while the player holds the interaction.
            // No duration limit.
            return true;
        }

        public override void OnBlockInteractStop(
            float secondsUsed,
            IWorldAccessor world,
            IPlayer byPlayer,
            BlockSelection blockSel) {
            // Discover the connected gate structure.
            List<BlockPos> positions = new List<BlockPos>();

            CollectConnectedGates(
                world,
                blockSel.Position,
                positions
            );

            if (positions.Count == 0)
                return;

            DestroyBlocksWithDelay(world, positions, byPlayer);
        }

        private void CollectConnectedGates(
            IWorldAccessor world,
            BlockPos startPos,
            List<BlockPos> positions) {
            // Only process each position once.
            if (ContainsPosition(positions, startPos))
                return;

            Block block = world.BulkBlockAccessor.GetBlock(startPos);

            if (!(block is BlockLFGate))
                return;

            positions.Add(startPos.Copy());

            // Search all 26 neighboring positions.
            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    for (int z = -1; z <= 1; z++) {
                        if (x == 0 && y == 0 && z == 0)
                            continue;

                        BlockPos neighborPos = new BlockPos(
                            startPos.X + x,
                            startPos.Y + y,
                            startPos.Z + z
                        );

                        if (ContainsPosition(positions, neighborPos))
                            continue;

                        Block neighbor =
                            world.BulkBlockAccessor.GetBlock(neighborPos);

                        if (neighbor is BlockLFGate) {
                            CollectConnectedGates(
                                world,
                                neighborPos,
                                positions
                            );
                        }
                    }
                }
            }
        }

        private bool ContainsPosition(
            List<BlockPos> positions,
            BlockPos pos) {
            return positions.Exists(p =>
                p.X == pos.X &&
                p.Y == pos.Y &&
                p.Z == pos.Z
            );
        }

        private void DestroyBlocksWithDelay(
            IWorldAccessor world,
            List<BlockPos> positions,
            IPlayer player) {
            int index = 0;

            Action<float> destroyNext = null;

            destroyNext = (dt) => {
                if (index >= positions.Count)
                    return;

                BlockPos pos = positions[index];

                // Recheck the block in case it changed since discovery.
                Block block = world.BlockAccessor.GetBlock(pos);

                if (block is BlockLFGate) {
                    if (block.Sounds?.Break != null) {
                        world.PlaySoundAt(
                            block.Sounds.Break.Location,
                            pos.X,
                            pos.Y,
                            pos.Z,
                            player
                        );
                    }

                    world.BlockAccessor.BreakBlock(
                        pos,
                        player,
                        0.0f
                    );
                }

                index++;

                if (index < positions.Count) {
                    world.Api.Event.RegisterCallback(destroyNext, 100);
                }
            };

            // Begin after 100 ms.
            world.Api.Event.RegisterCallback(destroyNext, 100);
        }
    }
}
