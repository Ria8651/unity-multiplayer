using System;
using System.IO;
using SkiaSharp;

public class MapLoader {
    public static TileType[,] LoadMap(string path) {
        TileType[,] tiles;
        using (var input = File.OpenRead(path)) {
            using (var inputStream = new SKManagedStream(input)) {
                using (var image = SKBitmap.Decode(inputStream)) {
                    int width, height;
                    height = image.Height;
                    width = image.Width;

                    tiles = new TileType[width, height];

                    SKColor[] pixels = image.Pixels;
                    for (int y = 0; y < height; y++) {
                        for (int x = 0; x < width; x++) {
                            SKColor c = pixels[y * width + x];
                            int num = (c.Red * 4 / 255) | (c.Green * 2 / 255) | (c.Blue / 255);

                            tiles[x, height - y - 1] = (TileType)num;
                        }
                    }
                }
            }
        }

        return tiles;
    }
}