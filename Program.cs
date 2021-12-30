/*
https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/get-started-speech-to-text?tabs=windowsinstall&pivots=programming-language-csharp
https://github.com/TChatzigiannakis/InputSimulatorPlus   
 */
using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using WindowsInput;
using WindowsInput.Native;
using System.Configuration;
using System.Collections.Specialized;

namespace MinecraftVoiceControl
{
    internal class Program
    {
        async static Task GetInput(SpeechConfig speechConfig)
        {
            InputSimulator simulator = new();
            bool end = false;
            string input = "";
            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);

            while (!end)
            {
                Console.WriteLine("Můžete začít mluvit.");
                var result = await recognizer.RecognizeOnceAsync();
                //Get input to lowercase and remove special characters
                if (result.Text.Contains(".")) input = result.Text.Replace(".", "").ToLower();
                if (result.Text.Contains("?")) input = result.Text.Replace("?", "").ToLower();
                if (result.Text.Contains("!")) input = result.Text.Replace("!", "").ToLower();
                Console.Write("Rozpoznaný text: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(input);
                Console.ForegroundColor = ConsoleColor.Gray;
                switch (input)
                {
                    //Movement
                    case "dopředu":
                        simulator.Keyboard.KeyUp(VirtualKeyCode.VK_S);
                        simulator.Keyboard.KeyDown(VirtualKeyCode.VK_W);
                        break;
                    case "dozadu":
                        simulator.Keyboard.KeyUp(VirtualKeyCode.VK_W);
                        simulator.Keyboard.KeyDown(VirtualKeyCode.VK_S);
                        break;
                    case "sprint":
                        simulator.Keyboard.KeyUp(VirtualKeyCode.SHIFT);
                        simulator.Keyboard.KeyDown(VirtualKeyCode.LCONTROL);
                        break;
                    case "krčit":
                        simulator.Keyboard.KeyUp(VirtualKeyCode.LCONTROL);
                        simulator.Keyboard.KeyDown(VirtualKeyCode.SHIFT);
                        break;
                    case "stát":
                        simulator.Keyboard.KeyUp(VirtualKeyCode.SHIFT);
                        simulator.Keyboard.KeyUp(VirtualKeyCode.LCONTROL);
                        simulator.Keyboard.KeyUp(VirtualKeyCode.VK_W);
                        simulator.Keyboard.KeyUp(VirtualKeyCode.VK_S);
                        break;
                    case "skok":
                        simulator.Keyboard.KeyDown(VirtualKeyCode.SPACE);
                        System.Threading.Thread.Sleep(100);
                        simulator.Keyboard.KeyUp(VirtualKeyCode.SPACE);
                        break;
                    case "velký skok":
                        simulator.Keyboard.KeyDown(VirtualKeyCode.SPACE);
                        System.Threading.Thread.Sleep(500);
                        simulator.Keyboard.KeyUp(VirtualKeyCode.SPACE);
                        break;
                    //Interacting with UI
                    case "inventář":
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_E);
                        break;
                    case "menu":
                        simulator.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);
                        break;
                    case "ukončit":
                        simulator.Mouse.MoveMouseToPositionOnVirtualDesktop(917 * 37, 500 * 63);
                        simulator.Mouse.LeftButtonClick();
                        break;
                    //Hotbar
                    case "1":
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_1);
                        break;
                    case "2":
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_2);
                        break;
                    case "3":
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_3);
                        break;
                    case "4":
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_4);
                        break;
                    case "5":
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_5);
                        break;
                    case "6":
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_6);
                        break;
                    case "7":
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_7);
                        break;
                    case "8":
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_8);
                        break;
                    case "9":
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_9);
                        break;
                    //Mouse control
                    case "zničit":
                        if (simulator.InputDeviceState.IsKeyDown(VirtualKeyCode.LBUTTON))
                            simulator.Mouse.LeftButtonUp();
                        else
                            simulator.Mouse.LeftButtonDown();
                        break;
                    case "položit":
                        simulator.Mouse.RightButtonClick();
                        break;
                    case "otevřít":
                        simulator.Mouse.RightButtonClick();
                        break;
                    case "doleva":
                        simulator.Mouse.MoveMouseBy(-200, 0);
                        break;
                    case "doprava":
                        simulator.Mouse.MoveMouseBy(200, 0);
                        break;
                    case "nahoru":
                        simulator.Mouse.MoveMouseBy(0, -200);
                        break;
                    case "dolů":
                        simulator.Mouse.MoveMouseBy(0, 200);
                        break;
                    case "mírně doleva":
                        simulator.Mouse.MoveMouseBy(-25, 0);
                        break;
                    case "mírně doprava":
                        simulator.Mouse.MoveMouseBy(25, 0);
                        break;
                    case "mírně nahoru":
                        simulator.Mouse.MoveMouseBy(0, -25);
                        break;
                    case "mírně dolů":
                        simulator.Mouse.MoveMouseBy(0, 25);
                        break;
                    case "otočit":
                        simulator.Mouse.MoveMouseBy(1200, 0);
                        break;
                    //End application
                    case "ukončit aplikaci":
                        end = true;
                        Environment.Exit(0);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Tento příkaz nedokážu rozpoznat");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                }
                Console.WriteLine();
            }
        }

        async static Task Main()
        {

            
            var speechConfig = SpeechConfig.FromSubscription(ConfigurationManager.AppSettings.Get("subscription"), ConfigurationManager.AppSettings.Get("location"));
            speechConfig.SpeechRecognitionLanguage = "cs-CZ";
            await GetInput(speechConfig);
        }
    }
}
