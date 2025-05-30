using System;
using System.Windows.Media;

namespace MistycPawCraftCore.Utils
{
    public class ColorsHelper
    {
        static string SerializeBrush(SolidColorBrush brush)
        {
            // Get the color from the SolidColorBrush and convert it to a string
            return brush.Color.ToString();
        }

        static SolidColorBrush DeserializeBrush(string colorString)
        {
            try
            {
                // Convert the string back to a Color
                Color color = (Color)ColorConverter.ConvertFromString(colorString);

                // Create a new SolidColorBrush using the color
                return new SolidColorBrush(color);
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., invalid color string)
                Console.WriteLine($"Error deserializing color: {ex.Message}");
                return new SolidColorBrush(Colors.Black); // Default to black on error
            }
        }

    }
}
