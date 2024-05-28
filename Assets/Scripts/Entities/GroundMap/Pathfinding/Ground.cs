using UnityEngine;

namespace AboutGround.GroundMap
{
    public class Ground : IPrioritizable
    {
        public readonly GroundUniversalStatus GrobalStatus;
        public GroundLocalStatus LocalStatus;
        public AStarPathFindingData PathFindingData;

        public int QueueIndex
        {
            get
            {
                return PathFindingData.QueueIndex;
            }
            set
            {
                PathFindingData.QueueIndex = value;
            }
        }

        public int Priority
        {
            get
            {
                return PathFindingData.F;
            }
        }

        public Ground(Vector2Int pos)
        {
            LocalStatus.Pos = pos;
            ResetPriorityData();
        }

        public void ResetPriorityData()
        {
            PathFindingData = new();
        }
    }
}
