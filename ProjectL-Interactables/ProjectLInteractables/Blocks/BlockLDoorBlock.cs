
using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;


namespace ProjectLInteractables.Blocks {
    internal class BlockLDoorBlock : Block {

        public string color = "unkown";

        public override void OnLoaded(ICoreAPI api) {
            if (this.Attributes != null && this.Attributes.Exists && this.Attributes["color"].Exists) {
                color = this.Attributes["color"].AsString("unkown");
            }
        }

        public List<BlockPos> OppenDoor(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, List<BlockPos> list) {
            list.Add(pos);
            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    for (int z = -1; z <= 1; z++) {
                        BlockPos n_pos = new BlockPos(pos.X + x, pos.Y + y, pos.Z + z);
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
            return list;
        }
    }
}
