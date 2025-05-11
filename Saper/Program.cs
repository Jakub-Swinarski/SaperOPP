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
            
            int consoleWidth = Console.WindowWidth;
            int maxWidth = (consoleWidth - 5) / 4;

            if (width > maxWidth)
            {
                Console.WriteLine($"⚠️ Szerokość planszy ({width}) przekracza możliwości konsoli ({maxWidth} pól).");
                width = maxWidth;
                Console.WriteLine($"✅ Ustawiono szerokość na maksymalne {width}.");
            }

            Board board = new Board(width, height, bombs);
            board.PrintBoard(); 
        }
    }
}