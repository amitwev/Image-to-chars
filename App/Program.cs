using System; 
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

Console.WriteLine("-------------------------");
Console.WriteLine("Image to dice app");
Console.WriteLine("-------------------------");
string imagePath = "/Users/amitsalim/Dev Projects/Image to dice/App/Images/riders.jpg";

using (Image<Rgba32> image = Image.Load<Rgba32>(imagePath))
{
    int newWidth = 150;
    var imageRatio = (double)image.Height / image.Width;
    var newHeight = (int)Math.Round(newWidth * imageRatio);

    image.Mutate(x =>
    {
        x.Resize(newWidth, newHeight);
        x.Grayscale();
    });
    
    // string output = "/Users/amitsalim/Dev Projects/Image to dice/App/Images/new image1.png";
    // image.Save(output);
    
    var imageArray = new int[newWidth, newHeight]; 
    
    for (int y = 0; y < newHeight; y++)
    {
        for (int x = 0; x < newWidth; x++)
        {
            var lum = GetPixelLuminance(image[x, y]);
            var pixelValue = GetPixelValue(lum);
            imageArray[x, y] = pixelValue;
        }
    }
    
    // char[] diceCharacters = { '⚀', '⚁', '⚂', '⚃', '⚄', '⚅' };
    char[] diceCharacters = { ' ', '.', '-', '\\', '=', '#'};
    Console.WriteLine("-------image save--------");
    var filePath = "/Users/amitsalim/Dev Projects/Image to dice/App/Images/file.txt";
    using (StreamWriter writer = new StreamWriter(filePath))
    {
        for (int i = 0; i < imageArray.GetLength(1); i++)
        {
            for (int j = 0; j < imageArray.GetLength(0); j++)
            {
                // diceCharacters[imageArray[i, j] - 1]
                Console.Write(diceCharacters[imageArray[j, i] - 1]);
                writer.Write(diceCharacters[imageArray[j, i] - 1]);
            }

            Console.WriteLine();
            writer.WriteLine();
        }
    }

}

static double GetPixelLuminance(Rgba32 pixel)
{
    double r = (double)pixel.R / 255;
    double g = (double)pixel.G / 255;
    double b = (double)pixel.B / 255;
    return 0.2126 * r + 0.7152 * g + 0.0722 * b;
}

static int GetPixelValue(double value)
{
    int group = (int)Math.Ceiling(6 * value);
    return Math.Clamp(group, 1, 6);
}