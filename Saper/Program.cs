using System;


namespace Saper
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            
            Console.Write("Podaj szerokość planszy: ");
            int width = int.Parse(Console.ReadLine());

            Console.Write("Podaj wysokość planszy: ");
            int height = int.Parse(Console.ReadLine());

            Console.Write("Podaj liczbę bomb: ");
            int bombs = int.Parse(Console.ReadLine());
            

            
            var board = new Board(width, height, bombs);
            
            bool running = true;
            while (running)
            {
                Console.Clear();
                board.PrintBoard();
                Console.Write("Ruch (np. A5 lub A5 F, lub EXIT aby wyjść): ");
                var input = Console.ReadLine().Trim();

                // jeśli wpisano exit → koniec gry
                if (input.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                    break;

                if (string.IsNullOrEmpty(input)) continue;
                var parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var coord = parts[0].ToUpper();

                // rozdzielenie liter i cyfr
                int split = 0;
                while (split < coord.Length && char.IsLetter(coord[split])) split++;
                var rowLabel = coord.Substring(0, split);
                var colLabel = coord.Substring(split);

                if (!int.TryParse(colLabel, out int col) ||
                    rowLabel.Length == 0)
                {
                    Console.WriteLine("❌ Niepoprawny format. Wciśnij ENTER i spróbuj ponownie.");
                    Console.ReadLine();
                    continue;
                }

                int row = board.GetRowIndex(rowLabel);
                col = col - 1; // na 0-based

                if (row < 0 || row >= height || col < 0 || col >= width)
                {
                    Console.WriteLine("❌ Poza Planszą. ENTER i ponów.");
                    Console.ReadLine();
                    continue;
                }

                bool isFlag = parts.Length > 1 && parts[1].Equals("F", StringComparison.OrdinalIgnoreCase);

                if (isFlag)
                    board.ToggleFlag(col, row);
                else
                {
                    bool alive = board.RevealCell(col, row);
                    if (!alive)
                    {
                        Console.Clear();
                        board.PrintBoard();
                        Console.WriteLine("💥 Trafiłeś na minę! Koniec gry.");
                        break;
                    }
                }
                if (board.IsVictory())
                {
                    Console.Clear();
                    board.PrintBoard();
                    Console.WriteLine("🎉 Brawo! Znalazłeś/łaś wszystkie miny! Wygrałeś/łaś grę.");
                    break;
                }
            }
        }
    }
}