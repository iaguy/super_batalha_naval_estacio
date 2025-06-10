using System;
using System.Collections.Generic;

class Board
{
    private char[,] grid = new char[10, 10];
    private List<Ship> ships = new List<Ship>();
    private string owner;

    public Board(string owner)
    {
        this.owner = owner;
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
                grid[i, j] = '.';
    }

    public void Display(bool showShips)
    {
        Console.Write("  ");
        for (int i = 0; i < 10; i++) Console.Write((char)('A' + i) + " ");
        Console.WriteLine();

        for (int i = 0; i < 10; i++)
        {
            Console.Write(i + " ");
            for (int j = 0; j < 10; j++)
            {
                char cell = grid[i, j];
                if (cell == 'S' && !showShips) Console.Write(". ");
                else Console.Write(cell + " ");
            }
            Console.WriteLine();
        }
    }

    public void PlaceShipsManual()
    {
        var shipTypes = new[] {
            new { Name = "Porta-aviões", Size = 5 },
            new { Name = "Navio-tanque", Size = 4 },
            new { Name = "Contratorpedeiro", Size = 3 },
            new { Name = "Submarino", Size = 3 },
            new { Name = "Barco patrulha", Size = 2 },
        };

        foreach (var s in shipTypes)
        {
            while (true)
            {
                Console.Write($"Digite posição inicial para {s.Name} ({s.Size} casas): ");
                string input = Console.ReadLine().ToUpper();

                Console.Write("Direção (H=Horizontal, V=Vertical): ");
                string dir = Console.ReadLine().ToUpper();

                if (!TryParsePosition(input, out int row, out int col)) continue;

                if (PlaceShip(row, col, s.Size, dir == "V"))
                    break;
                else
                    Console.WriteLine("Não foi possível posicionar. Tente outra posição.");
            }
            Display(true);
        }
    }

    public void PlaceShipsRandom()
    {
        var rand = new Random();
        var shipSizes = new[] { 5, 4, 3, 3, 2 };

        foreach (int size in shipSizes)
        {
            bool placed = false;
            while (!placed)
            {
                int row = rand.Next(10);
                int col = rand.Next(10);
                bool vertical = rand.Next(2) == 0;
                placed = PlaceShip(row, col, size, vertical);
            }
        }
    }

    public bool PlaceShip(int row, int col, int size, bool vertical)
    {
        if (vertical)
        {
            if (row + size > 10) return false;
            for (int i = 0; i < size; i++)
                if (grid[row + i, col] != '.') return false;
            for (int i = 0; i < size; i++)
                grid[row + i, col] = 'S';
        }
        else
        {
            if (col + size > 10) return false;
            for (int i = 0; i < size; i++)
                if (grid[row, col + i] != '.') return false;
            for (int i = 0; i < size; i++)
                grid[row, col + i] = 'S';
        }

        ships.Add(new Ship(size));
        return true;
    }

    public bool Attack(int row, int col)
    {
        if (grid[row, col] == 'S')
        {
            grid[row, col] = 'X';
            foreach (var ship in ships)
                ship.Hit();
            return true;
        }
        if (grid[row, col] == '.')
        {
            grid[row, col] = 'O';
        }
        return false;
    }

    public bool AlreadyAttacked(int row, int col)
    {
        return grid[row, col] == 'X' || grid[row, col] == 'O';
    }

    public bool AllShipsSunk()
    {
        return ships.TrueForAll(s => s.IsSunk());
    }

    public static bool TryParsePosition(string pos, out int row, out int col)
    {
        row = col = -1;
        if (pos.Length < 2) return false;

        col = pos[0] - 'A';
        if (!int.TryParse(pos.Substring(1), out row)) return false;

        return row >= 0 && row < 10 && col >= 0 && col < 10;
    }

    public static string ToPosition(int row, int col)
    {
        return $"{(char)('A' + col)}{row}";
    }
}
