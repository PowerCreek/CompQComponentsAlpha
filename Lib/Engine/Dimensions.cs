namespace CompQComponents.Lib.Engine
{
    public class Dimensions
    {
        public int? PercentWidth { get; set; }
        public int? PercentHeight { get; set; }

        public int? PxW { get; set; }
        public int? PxH { get; set; }

        public string W => PercentWidth.HasValue ? $"{PercentWidth}%" : $"{PxW}px";
        public string H => PercentHeight.HasValue ? $"{PercentHeight}%" : $"{PxH}px";

        public Dimensions()
        {
            
        }

        public Dimensions(int x, int y, bool percent = false)
        {
            if (percent)
            {
                PercentWidth = x;
                PercentHeight = y;
            }
            else
            {
                PxW = x;
                PxH = y;
            }
        }
    }
}