namespace AboutGround.GroundMap.Generator
{
    [System.Serializable]
    public struct SeedMapData
    {
        public int Width;
        public int Height;
        public int Seed;

        public SeedMapData(int Width, int Height, int Seed)
        {
            if (Width <= 0)
                Width = 1;
            if (Height <= 0)
                Height = 1;

            this.Width = Width;
            this.Height = Height;
            this.Seed = Seed;
        }

        public readonly int GetMapSize()
        {
            return Width * Height;
        }
    }
}
