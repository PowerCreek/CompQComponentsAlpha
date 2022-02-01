namespace CompQComponents.Lib.Utility
{

    public class DimensionTrack
    {
        public int TotalWidth { get; set; }
        public int TotalHeight { get; set; }

        public int GetWidthFromPercent(double percent) => (int)(TotalWidth * percent);
        public int GetHeightFromPercent(double percent) => (int)(TotalHeight * percent);
        
        public void Deconstruct(out int Width, out int Height)
        {
            Width = TotalWidth;
            Height = TotalHeight;
        }
    }
}