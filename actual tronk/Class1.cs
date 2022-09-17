using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using WindowsInput.Native;
using WindowsInput;
using WindowsInput.Events;
using AutoItX3Lib;

namespace actual_tronk
{
    internal class MainClass
    {
        AutoItX3 au3 = new AutoItX3();

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
                if (GetAsyncKeyState(Keys.T) < 0)
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
        }

        private static void SearchPixel()
        {
            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            //create something that can capture the screen
            Graphics graphics = Graphics.FromImage(bitmap);
            //screenshot screen to graphics obj
            graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            //converts hex to a color obj
            Color startingPixelColor = bitmap.GetPixel(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);

            while (GetAsyncKeyState(Keys.T) < 0)
            {
                graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
                Color currentPixelColor = bitmap.GetPixel(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);
                
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
