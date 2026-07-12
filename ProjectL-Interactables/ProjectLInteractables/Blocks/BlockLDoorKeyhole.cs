using Vintagestory.API.Common;
using Vintagestory.API.MathTools;



namespace ProjectLInteractables.Blocks {
    internal class BlockLDoorKeyhole : Block {

        BlockPos[] neighbors = new BlockPos[6];

        public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos) {
            // TODO Store the good neighboor and with variant check

            base.OnNeighbourBlockChange(world, pos, neibpos);
        }

    }
}
