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
        private const int SIZE = 11000;
        private const int SIZE_CONVERSION = SIZE / 600;
        private const string IMAGE_PATH = @"C:\Users\chris\Documents\Projects\LargeBitMaps\Maps";
        private const string WHITE = "ffffffff";
        private const string BLACK = "ff000000";

        private static string IMAGE_NAME = IMAGE_PATH + SIZE_CONVERSION + "MSQFT_Generated.png";
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

            var width = SIZE;
            var height = SIZE;
            var bm = new Bitmap(width, height);
            var rand = new Random();

            Console.WriteLine("Creating map...");

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
            Console.WriteLine("Borders created...");

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
            Console.WriteLine("Lines created...");

            bm.Save(IMAGE_NAME, ImageFormat.Png);
            Console.WriteLine("Saved...");
        }

        private static void ImportMap()
        {
            Image image = Image.FromFile(IMAGE_NAME);
            Map = new Bitmap(image);

            Console.WriteLine("Imported Map...");
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
            Console.WriteLine($"Pixels in Dictionary: {MapDictionary.Count}");
            Console.WriteLine($"Total Time: {totalTime.TotalSeconds}");
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
