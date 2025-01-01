using System;
using System.Threading;
using Cosmos.Core;
using Cosmos.System.Graphics;
using System.Drawing;
using Cosmos.System;

namespace OS
{
    public class Kernel : Cosmos.System.Kernel
    {
        private Canvas _canvas;
        private Mode _mode;
        private bool _desktopStarted = false;
        private int _mouseX = 0;
        private int _mouseY = 0;
        private int _previousMouseX = 0;
        private int _previousMouseY = 0;
        private Color _cursorColor = Color.Black;

        protected override void BeforeRun()
        {
            _canvas.Clear(Color.Gray); // Заливка экрана серым цветом
            _canvas.Display(); // Применение изменений
            System.Console.WriteLine("Operating System");
            System.Console.WriteLine("----------------------");
        }

        protected override void Run()
        {
            while (true)
            {
                if (_desktopStarted)
                {
                    HandleMouse();
                    UpdateMouseCursor();

                    Thread.Sleep(10);
                }
                else
                {
                    System.Console.WriteLine(" ");
                    System.Console.WriteLine("Choose an action:");
                    System.Console.WriteLine("  (1) - Turn on");
                    System.Console.WriteLine("  (2) - Characteristics");
                    System.Console.WriteLine("  (3) - Turn off");
                    System.Console.WriteLine("  (4) - Reboot");
                    System.Console.WriteLine("  (5) - Clear screen");
                    System.Console.WriteLine("  (6) - System Info");
                    System.Console.Write(">");

                    string input = ReadLine();

                    switch (input)
                    {
                        case "1":
                            if (!_desktopStarted)
                            {
                                StartDesktop();
                                _desktopStarted = true;
                            }
                            else
                            {
                                System.Console.WriteLine("Desktop has already started!");
                            }
                            break;
                        case "2":
                            ShowCharacteristics();
                            break;
                        case "3":
                            Cosmos.System.Power.Shutdown();
                            break;
                        case "4":
                            Cosmos.System.Power.Reboot();
                            break;
                        case "5":
                            ClearScreen();
                            break;
                        case "6":
                            ShowSystemInfo();
                            break;
                        default:
                            System.Console.WriteLine("Invalid input. Please try again.");
                            break;
                    }
                }
            }
        }

        private void StartDesktop()
        {
            try
            {
                _mode = new Mode(1920, 1080, ColorDepth.ColorDepth32);

                _canvas = FullScreenCanvas.GetFullScreenCanvas(_mode);

                if (_canvas != null)
                {
                    ClearScreen();
                    _canvas.Display();
                    System.Console.WriteLine("Desktop started. Canvas initialized (1920x1080).");
                }
                else
                {
                    System.Console.WriteLine("Error: Canvas could not be created. Check display configuration.");
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine($"Error while creating canvas: {e}");
            }
        }

        private new void ClearScreen()
        {
            if (_canvas != null)
            {
                _canvas.Clear(Color.Gray);
                _canvas.Display();
            }
        }

        private void ShowCharacteristics()
        {
            System.Console.WriteLine("System characteristics:");
            System.Console.WriteLine(" - CPU: Example CPU");
            System.Console.WriteLine(" - RAM: 8GB");
            System.Console.WriteLine(" - Disk: 256GB SSD");
        }

        private void ShowSystemInfo()
        {
            System.Console.WriteLine("System Info:");
            System.Console.WriteLine($"  RAM Size: {CPU.GetAmountOfRAM()} MB");
            System.Console.WriteLine("----------------------");
        }

        private void HandleMouse()
        {
            _mouseX = (int)MouseManager.X;
            _mouseY = (int)MouseManager.Y;
        }

        private void UpdateMouseCursor()
        {
            if (_canvas != null)
            {
                _canvas.DrawFilledRectangle(new Pen(Color.Gray), _previousMouseX, _previousMouseY, 10, 10);
                _canvas.DrawFilledRectangle(new Pen(_cursorColor), _mouseX, _mouseY, 10, 10);
                _canvas.Display();

                _previousMouseX = _mouseX;
                _previousMouseY = _mouseY;
            }
        }

        private string ReadLine()
        {
            string input = "";
            while (true)
            {
                var key = System.Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    break;
                input += key.KeyChar;
            }
            return input;
        }
    }
}
