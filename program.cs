using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== BATALHA NAVAL ===\n");

        Game game = new Game();
        game.Setup();
        game.Play();
    }
}
