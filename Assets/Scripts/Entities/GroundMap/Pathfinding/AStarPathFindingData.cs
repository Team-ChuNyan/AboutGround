namespace AboutGround.GroundMap
{
    public struct AStarPathFindingData
    {
        public Ground BeforeNode;
        public int QueueIndex;

        public int G;
        public int H;
        public int F;

        public AStarPathFindingData(int g, int h, int f)
        {
            BeforeNode = null;
            QueueIndex = 0;
            G = g;
            H = h;
            F = f;
        }

        public AStarPathFindingData(Ground beforeNode, int queueIndex, int g, int h, int f)
        {
            BeforeNode = beforeNode;
            QueueIndex = queueIndex;
            G = g;
            H = h;
            F = f;
        }
    }
}
