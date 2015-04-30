using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace WebApplication1
{
    public class WindDirection
    {
        public static int[] WindDirections = { 1, 2, 4, 8 }; // bitmap
        public static String[] WindNames = { "North", "South", "East", "West" };

        public static String GetName (int Winddirection)
        {
            String direction = "";

            for (int i = 0; i < WindDirections.Length; i++)
                if ((WindDirections[i] & Winddirection) == WindDirections[i])
                    direction += WindNames[i];

            return direction;
        }

        public static bool WindDoesNotExist(int Winddirection)
        {
            bool isWindFake = true;
            int counter = 0;

            foreach (int value in WindDirections)
                if ((Winddirection & value) == value && counter == 1) // checks if value exists in bitmap and whether prior to this one there was a match (south cannot be consecutive to north, nor west to east)
                    return true;
                else if ((Winddirection & value) == value)
                {
                    counter++;
                    isWindFake = false; // wind exists, therefore not fake
                }
                else
                    counter = 0; // values not consecutive

            return isWindFake;
        }
    }
}