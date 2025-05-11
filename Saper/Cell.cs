namespace Saper
{
    public class Cell
    {
        public bool HasMine { get; set; } = false;
        public bool IsRevealed { get; set; } = false;
        public bool IsFlagged { get; set; } = false;
        public int NeighboringMines { get; set; } = 0;
    }
}