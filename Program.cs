using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastConsole;
using static FastConsole.SystemCalls;

namespace FConsoleExample
{
    class Program
    {
        static void Main(string[] args)
        {
            FullHeightWidth();

            //Console.WriteLine("END OF PROGRAM");
            Console.ReadLine();
        }

        public static void FullHeightWidth()
        {
            FastConsoleInstance fci = new FastConsoleInstance(0, 0, (short)Console.BufferWidth, (short)Console.BufferHeight);
            fci.autoFlush = false;
            fci.processColours = true;

            //fci.Write("<blue>Hello, World!");
            Random random = new Random();
            //while (true)
            //{
            //    for (int y = 0; y < Console.WindowHeight; y++)
            //    {
            //        for (int x = 0; x < Console.WindowWidth; x++)
            //        {
            //            fci.WriteAt('⩯', x, y);
            //        }
            //    }
            //    fci.Flush();
            //}
            SystemCalls.CharInfo[] brData = BufferUtil.CreateBuffer("HelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorld", 100, new ColourPair(ConsoleColor.White));
            BufferRegion br = new BufferRegion(brData, 10, 10);

            short wWorld = 80;
            short hWorld = 80;

            SystemCalls.CharInfo[] br2Data = new CharInfo[wWorld * hWorld];
            BufferRegion br2 = new BufferRegion(br2Data, wWorld, hWorld);


            BufferRegionWriter brw = new BufferRegionWriter();
            brw.AddRegion(br);
            brw.AddRegion(br2);
            br2.layer = 1;


            float zoom = .0f;
            while (true)
            {
                zoom += 0.003f;
                for (int i = 0; i < brData.Length; i++)
                {
                    brData[i] = new SystemCalls.CharInfo() { Attributes = 15, Char = new SystemCalls.CharUnion() { UnicodeChar = (char)random.Next(33, 124) } };
                    //br2Data[i] = new SystemCalls.CharInfo() { Attributes = 67, Char = new SystemCalls.CharUnion() { UnicodeChar = (char)random.Next(33, 124) } };
                }
                //br2.offset.X = (short)((br2.offset.X + 1) % 10);
                RenderWorld(wWorld, hWorld, 0, 0, br2Data, zoom);
                brw.Render();

                //System.Threading.Thread.Sleep(100);
            }
        }

        public static void RenderWorld(int width, int height, int playerX, int playerY, CharInfo[] chars, float zoom)
        {
            CharInfo Generate(char c, ColourPair cp)
            {
                return new CharInfo() { Attributes = (short)((ushort)cp.Foreground | (ushort)cp.Background << 4), Char = new CharUnion() { UnicodeChar = c } };
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {

                    //double h = Perlin.OctavePerlin(((playerX + i) / (double)width) * zoom, ((playerY + j) / (double)height) * zoom, 0, 16, 0.6) / 2.1f;
                    double l0 = Perlin.perlin(((playerX + i) / (double)width) * zoom, ((playerY + j) / (double)height) * zoom, 0);
                    double l1 = Perlin.perlin(((playerX + i) / (double)width) * zoom * 16f, ((playerY + j) / (double)height) * zoom * 16f, 0);

                    double h = (l0 + l1) / 2f;
                    //Console.WriteLine(height);
                    CharInfo ci = Generate((char)(int)Math.Floor((123 - 32) * h + 32), new ColourPair(ConsoleColor.DarkGreen, ConsoleColor.Black));

                    if (h < .4)
                    {
                        ci = Generate('≈', new ColourPair(ConsoleColor.Blue, ConsoleColor.DarkBlue));
                    }
                    else if (h < .5)
                    {
                        ci = Generate('≈', new ColourPair(ConsoleColor.DarkBlue, ConsoleColor.Blue));
                    }
                    else if (h < .57)
                    {
                        ci = Generate('#', new ColourPair(ConsoleColor.DarkYellow, ConsoleColor.Yellow));
                    }
                    else if (h < .7)
                    {
                        ci = Generate('#', new ColourPair(ConsoleColor.Green, ConsoleColor.DarkGreen));
                    }
                    else if (h < .8)
                    {
                        ci = Generate('#', new ColourPair(ConsoleColor.White, ConsoleColor.DarkGray));
                    }
                    else if (h >= .8)
                    {
                        ci = Generate('^', new ColourPair(ConsoleColor.White, ConsoleColor.Gray));
                    }

                    chars[i + j * width] = ci;
                }
            }
        }
    }
}
