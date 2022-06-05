using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargePngFramework
{
    internal class Program
    {
        // Configs
        private const int SIZE = 4;
        private const int SIZE_IN_PIXELS = SIZE*600; // 1px = 50cm = 600 pixels = 1m SQFT
        private const string IMAGE_PATH = @"C:\Users\chris\Documents\Projects\LargeMapsInDictionaries\Maps\";
        
        private const string WHITE = "ffffffff";
        private const string BLACK = "ff000000";

        private static string IMAGE_NAME = IMAGE_PATH + SIZE + "MSQFT_Generated.png";
        private static Dictionary<string, Coordinate> MapDictionary = new Dictionary<string, Coordinate>();
        private static Bitmap Map;

        static void Main(string[] args)
        {
            CreateAMap();

            ImportMap();

            InitDictionary();

            Console.ReadLine();
        }

        private static void CreateAMap()
        {
            if (File.Exists(IMAGE_NAME)) return;

            var width = SIZE_IN_PIXELS;
            var height = SIZE_IN_PIXELS;
            var bm = new Bitmap(width, height);
            var rand = new Random();

            Console.WriteLine("Starting map generation...");

            Console.WriteLine("Creating Borders...");
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                    {
                        bm.SetPixel(x, y, Color.Black);
                    }
                    else
                    {
                        bm.SetPixel(x, y, Color.White);
                    }
                }
            }

            Console.WriteLine("Creating Lines...");
            for (var x = 0; x < width; x++)
            {
                if (rand.NextDouble() < 0.2)
                {
                    var stopY = rand.Next(height);
                    for (var y = 0; y <= stopY; y++)
                    {
                        bm.SetPixel(x, y, Color.Black);
                    }

                    var direction = rand.NextDouble() < 0.5;

                    for (var j = 0; j < width; j++)
                    {
                        if ((j < x && direction) || (j > x && !direction))
                        {
                            bm.SetPixel(j, stopY, Color.Black);
                        }
                    }
                }
            }

            Console.WriteLine("Saving...");
            bm.Save(IMAGE_NAME, ImageFormat.Png);
        }

        private static void ImportMap()
        {
            Console.WriteLine("Importing Map...");
            Image image = Image.FromFile(IMAGE_NAME);
            Map = new Bitmap(image);
        }

        private static void InitDictionary()
        {
            Console.WriteLine("Creating Dictionary...");
            var startTime = DateTime.Now;

            for (var x = 0; x < Map.Width; x++)
            {
                for (var y = 0; y < Map.Height; y++)
                {
                    if (Map.GetPixel(x, y).Name == BLACK)
                    {
                        MapDictionary.Add($"{x},{y}", new Coordinate(x, y));
                    }
                }
            }

            var endTime = DateTime.Now;
            var totalTime = endTime - startTime;

            Console.WriteLine("Created Dictionary...");
            Console.WriteLine("DICTIONARY INFO");
            Console.WriteLine($"Pixels in Dictionary: {MapDictionary.Count}");
            Console.WriteLine($"Create Time: {totalTime.TotalSeconds} seconds");
        }
    }

    public class Coordinate
    {
        public int X;
        public int Y;
        
        public Coordinate(int x, int y)
        {
            X = x; 
            Y = y; 
        }
    }
}
