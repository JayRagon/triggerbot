using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace actual_tronk
{
    internal class MainClass
    {
        private static int Threshold = 30;

        private static bool Shot = false;

        private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, uint dwExtraInf);


        static void Main(string[] args)
        {
            Console.WriteLine("Hold t while standing still and don't move mouse for the triggerbot to work. For op/marshal disable center dot");
            CheckKeyDown();
        }

        static void CheckKeyDown()
        {
            for (; ; )
            {
                if (GetAsyncKeyState(Keys.T) < 0) // if t is down
                {
                    Shot = false;
                    SearchPixel();
                }
                while (Shot = false && GetAsyncKeyState(Keys.T) < 0)
                {
                    Thread.Sleep(1);
                }
                Thread.Sleep(1);
            }
        }

        private static void Click()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

            //mouse_event(0x02 | 0x04, 0, 0, 0, 0);
        }

        private static void SearchPixel()
        {
            //new bitmap
            Bitmap bitmap = new Bitmap(1, 1);
            //create something that can capture the screen
            Graphics graphics = Graphics.FromImage(bitmap);
            //screenshot screen to graphics obj
            graphics.CopyFromScreen(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2, 0, 0, new Size(1, 1), CopyPixelOperation.SourceCopy);
            //converts hex to a color obj
            Color startingPixelColor = bitmap.GetPixel(0, 0); // saves starting pixel

            while (GetAsyncKeyState(Keys.T) < 0)
            {
                graphics.CopyFromScreen(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2, 0, 0, new Size(1, 1), CopyPixelOperation.SourceCopy);
                Color currentPixelColor = bitmap.GetPixel(0, 0);

                if (startingPixelColor.R > (currentPixelColor.R + Threshold) || startingPixelColor.R < (currentPixelColor.R - Threshold) || startingPixelColor.G > (currentPixelColor.G + Threshold) || startingPixelColor.G < (currentPixelColor.G - Threshold) || startingPixelColor.B > (currentPixelColor.B + Threshold) || startingPixelColor.B < (currentPixelColor.B - Threshold))
                {
                    Click();

                    bitmap.Dispose();
                    graphics.Dispose();

                    Shot = true;

                    Thread.Sleep(500);
                    return;
                }
            }
            bitmap.Dispose();
            graphics.Dispose();
        }
    }
}
