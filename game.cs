using System;
using System.Collections.Generic;

class Game
{
    private Board playerBoard;
    private Board computerBoard;
    private Random rand = new Random();

    public void Setup()
    {
        playerBoard = new Board("Jogador");
        computerBoard = new Board("Computador");

        Console.WriteLine("Posicionando navios do jogador...\n");
        playerBoard.PlaceShipsManual();

        Console.WriteLine("\nPosicionando navios do computador...\n");
        computerBoard.PlaceShipsRandom();
    }

    public void Play()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Seu tabuleiro:");
            playerBoard.Display(true);
            Console.WriteLine("\nTabuleiro do computador:");
            computerBoard.Display(false);

            Console.Write("\nDigite a posição para atacar (ex: B4): ");
            string input = Console.ReadLine().ToUpper();
            if (!Board.TryParsePosition(input, out int row, out int col))
            {
                Console.WriteLine("Posição inválida!");
                Console.ReadKey();
                continue;
            }

            bool hit = computerBoard.Attack(row, col);
            Console.WriteLine(hit ? "💥 Acertou!" : "💦 Errou!");
            Console.ReadKey();

            if (computerBoard.AllShipsSunk())
            {
                Console.WriteLine("🎉 Você venceu!");
                break;
            }

            // Turno do computador
            int cpuRow, cpuCol;
            do
            {
                cpuRow = rand.Next(10);
                cpuCol = rand.Next(10);
            } while (playerBoard.AlreadyAttacked(cpuRow, cpuCol));

            bool cpuHit = playerBoard.Attack(cpuRow, cpuCol);
            Console.WriteLine($"\nO computador atacou {Board.ToPosition(cpuRow, cpuCol)} e {(cpuHit ? "acertou!" : "errou!")}");
            Console.ReadKey();

            if (playerBoard.AllShipsSunk())
            {
                Console.WriteLine("☠️ O computador venceu!");
                break;
            }
        }
    }
}
