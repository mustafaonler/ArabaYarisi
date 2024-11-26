using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;


class Program
{
    static int carPosX = 10; // Arabanın başlangıç konumu (X)
    static int carPosY = 18; // Arabanın başlangıç konumu (Y)
    static int roadWidth = 30; // Yarış yolunun genişliği
    static int roadHeight = 20; // Yarış yolunun yüksekliği
    static int speed = 1; // Araba hızı
    static bool gameRunning = true; // Oyun durumu
    static List<int> obstaclesX = new List<int>(); // Engellerin X konumu
    static List<int> obstaclesY = new List<int>(); // Engellerin Y konumu
    static Random rand = new Random(); // Rastgele engel yaratmak için

    static void Main()
    {
        Console.CursorVisible = false; // İmleci gizle
        Console.SetWindowSize(roadWidth + 2, roadHeight + 2); // Konsol penceresini boyutlandırıyoruz

        while (gameRunning)
        {
            DrawRoad(); // Yolu çiz
            DrawCar(); // Arabayı çiz
            HandleInput(); // Kullanıcı girdilerini al
            MoveObstacles(); // Engelleri hareket ettir
            CheckCollisions(); // Çarpışmaları kontrol et
            Thread.Sleep(100); // Bir sonraki kareye geçmek için bekle
        }

        Console.SetCursorPosition(0, roadHeight + 1);
        Console.WriteLine("Oyun bitti!");
    }

    static void DrawRoad()
    {
        Console.Clear(); // Ekranı temizle

        // Yarış yolunu çiz
        for (int y = 0; y < roadHeight; y++)
        {
            for (int x = 0; x < roadWidth; x++)
            {
                if (x == 0 || x == roadWidth - 1)
                {
                    Console.Write("|"); // Yolun kenarlarını çiz
                }
                else
                {
                    Console.Write(" "); // Yolun iç kısmı boş
                }
            }
            Console.WriteLine();
        }
    }

    static void DrawCar()
    {
        // Araba çiz
        Console.SetCursorPosition(carPosX, carPosY);
        Console.Write("A"); // Araba karakteri "A"
    }

    static void HandleInput()
    {
        if (Console.KeyAvailable) // Klavye girdisi var mı?
        {
            var key = Console.ReadKey(true).Key;

            // Ok tuşları ile araba hareketi
            if (key == ConsoleKey.LeftArrow && carPosX > 1)
            {
                carPosX--; // Araba sola hareket eder
            }
            if (key == ConsoleKey.RightArrow && carPosX < roadWidth - 2)
            {
                carPosX++; // Araba sağa hareket eder
            }
        }
    }

    static void MoveObstacles()
    {
        // Engelleri aşağıya hareket ettir
        for (int i = 0; i < obstaclesY.Count; i++)
        {
            obstaclesY[i]++;
        }

        // Ekranın altına inen engelleri sil
        for (int i = obstaclesY.Count - 1; i >= 0; i--)
        {
            if (obstaclesY[i] >= roadHeight)
            {
                obstaclesX.RemoveAt(i);
                obstaclesY.RemoveAt(i);
            }
        }

        // Yeni engeller ekle
        if (rand.Next(0, 10) < 2) // %20 ihtimalle yeni engel ekle
        {
            obstaclesX.Add(rand.Next(1, roadWidth - 1)); // Engelin X konumu
            obstaclesY.Add(0); // Engelin Y konumu
        }

        // Engelleri çiz
        for (int i = 0; i < obstaclesX.Count; i++)
        {
            Console.SetCursorPosition(obstaclesX[i], obstaclesY[i]);
            Console.Write("X"); // Engel karakteri "X"
        }
    }

    static void CheckCollisions()
    {
        // Araba ile engellerin çarpışmasını kontrol et
        for (int i = 0; i < obstaclesY.Count; i++)
        {
            if (obstaclesX[i] == carPosX && obstaclesY[i] == carPosY)
            {
                gameRunning = false; // Çarpışma olduğunda oyun sona erer
            }
        }
    }
}