using System;
using System.Collections.Generic;

namespace Saper
{

public class Board
{
    private int width;
    private int height;
    private int mineCount;
    private Cell[,] grid;
    private Random random = new Random();

    public Board(int width, int height, int mineCount)
    {
        this.width = width;
        this.height = height;
        this.mineCount = mineCount;
        grid = new Cell[width, height];
        
        InitializeCells();
        PlaceMines();
        CalculateNeighbors();
    }
    private string GetRowLabel(int index)
    {
        string label = "";
        index += 1; // bo chcemy zacząć od 1, a nie od 0

        while (index > 0)
        {
            index--;
            label = (char)('A' + (index % 26)) + label;
            index /= 26;
        }

        return label;
    }
    public int GetRowIndex(string label)
    {
        label = label.ToUpper();
        int result = 0;
        foreach (char c in label)
        {
            result = result * 26 + (c - 'A' + 1);
        }
        return result - 1;
    }
    
    private void InitializeCells()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                grid[x, y] = new Cell();
    }

    private void PlaceMines()
    {
        int placed = 0;
        while (placed < mineCount)
        {
            int x = random.Next(width);
            int y = random.Next(height);
            if (!grid[x, y].HasMine)
            {
                grid[x, y].HasMine = true;
                placed++;
            }
        }
    }

    private void CalculateNeighbors()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y].HasMine)
                {
                    grid[x, y].NeighboringMines = -1;
                    continue;
                }

                int count = 0;
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        int nx = x + dx;
                        int ny = y + dy;
                        if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                        {
                            if (grid[nx, ny].HasMine)
                                count++;
                        }
                    }
                }

                grid[x, y].NeighboringMines = count;
            }
        }
    }
    public bool RevealCell(int x, int y)
    {
        var cell = grid[x, y];
        if (cell.IsRevealed || cell.IsFlagged)
            return true; // nic nie robimy

        cell.IsRevealed = true;
        if (cell.HasMine)
            return false;

        if (cell.NeighboringMines == 0)
            FloodFillReveal(x, y);

        return true;
    }
    private void FloodFillReveal(int startX, int startY)
    {
        var stack = new Stack<(int x,int y)>();
        stack.Push((startX, startY));

        while (stack.Count > 0)
        {
            var (cx, cy) = stack.Pop();
            for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
            {
                int nx = cx + dx, ny = cy + dy;
                if (nx < 0 || nx >= width || ny < 0 || ny >= height)
                    continue;

                var neighbor = grid[nx, ny];
                if (!neighbor.IsRevealed && !neighbor.IsFlagged)
                {
                    neighbor.IsRevealed = true;
                    if (neighbor.NeighboringMines == 0)
                        stack.Push((nx, ny));
                }
            }
        }
    }
    public void ToggleFlag(int x, int y)
    {
        var cell = grid[x, y];
        if (!cell.IsRevealed)
            cell.IsFlagged = !cell.IsFlagged;
    }
    public void PrintBoard()
    {
        // Nagłówek kolumn
        Console.Write("    ");
        for (int x = 0; x < width; x++)
            if (x < 10)
            {
                Console.Write($"  {x + 1} ");
            }
            else
            {
                Console.Write($" {x + 1} ");
            }
        Console.WriteLine();

        // Górna linia siatki
        Console.Write("    ");
        for (int x = 0; x < width; x++)
            Console.Write("+---");
        Console.WriteLine("+");

        for (int y = 0; y < height; y++)
        {
            // Wiersz z kratkami
            string rowLabel = GetRowLabel(y).PadLeft(3);
            Console.Write($"{rowLabel} ");
            for (int x = 0; x < width; x++)
            {
                var c = grid[x, y];
                string symbol;
                if (c.IsRevealed)
                {
                    if (c.HasMine) symbol = " * ";
                    else if (c.NeighboringMines > 0) symbol = $" {c.NeighboringMines} ";
                    else symbol = "   ";
                }
                else if (c.IsFlagged)
                    symbol = " F ";
                else
                    symbol = " # ";

                Console.Write("|" + symbol);
            }
            Console.WriteLine("|");

            // Dolna linia siatki (po każdym wierszu)
            Console.Write("    ");
            for (int x = 0; x < width; x++)
                Console.Write("+---");
            Console.WriteLine("+");
        }
    }

    
    
}

}