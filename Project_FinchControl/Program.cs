using System;
using System.Collections.Generic;
using System.IO;
using FinchAPI;

namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: Finch Control
    // Description: Starter solution with the helper methods,
    //              opening and closing screens, and the menu
    // Application Type: Console
    // Author: Bolser, Lindsey
    // Date Created:        2/29/2020
    // Date Revised:        3/2/2020
    //
    // **************************************************

    class Program
    {
        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            SetTheme();

            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }

        /// <summary>
        /// setup the console theme
        /// </summary>
        static void SetTheme()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Main Menu                                 *
        /// *****************************************************************
        /// </summary>
        static void DisplayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            Finch finchRobot = new Finch();

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Talent Show");
                Console.WriteLine("\tc) Data Recorder");
                Console.WriteLine("\td) Alarm System");
                Console.WriteLine("\te) User Programming");
                Console.WriteLine("\tf) Disconnect Finch Robot");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayConnectFinchRobot(finchRobot);
                        break;

                    case "b":
                        DisplayTalentShowMenuScreen(finchRobot);
                        break;

                    case "c":

                        break;

                    case "d":

                        break;

                    case "e":

                        break;

                    case "f":
                        DisplayDisconnectFinchRobot(finchRobot);
                        break;

                    case "q":
                        DisplayDisconnectFinchRobot(finchRobot);
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        #region TALENT SHOW

        /// <summary>
        /// *****************************************************************
        /// *                     Talent Show Menu                          *
        /// *****************************************************************
        /// </summary>
        static void DisplayTalentShowMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitTalentShowMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Talent Show Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Light and Sound");
                Console.WriteLine("\tb) Dance Party");
                Console.WriteLine("\tc) Mixing It Up");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayLightAndSound(finchRobot);
                        break;

                    case "b":
                        DisplayDance(finchRobot);
                        break;

                    case "c":
                        DisplayMixingItUp(finchRobot);
                        break;

                    case "q":
                        quitTalentShowMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitTalentShowMenu);
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Light and Sound                   *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayLightAndSound(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Light and Sound");

            Console.WriteLine("\tThe Finch robot will now show off its glowing talent!");
            DisplayContinuePrompt();

            for (int lightSoundLevel = 0; lightSoundLevel < 255; lightSoundLevel++)
            {
                finchRobot.setLED(lightSoundLevel, lightSoundLevel, lightSoundLevel);
                finchRobot.noteOn(lightSoundLevel * 100);
            }

            DisplayMenuPrompt("Talent Show Menu");
        }
        static void DisplayDance(Finch finchRobot)
        {
            string userName;
            string userResponse;
            int userNumber;

            DisplayScreenHeader("Talent Show - Dance");
            Console.Write("\tWhat is your name? ");
            userName = Console.ReadLine();

            Console.Write("\t{0}, Please enter a number 0 to 255: ", userName);
            userResponse = Console.ReadLine();
            userNumber = Convert.ToInt32(userResponse);
            Console.WriteLine("\t{0} You entered {1}", userName, userNumber);

            finchRobot.setMotors(userNumber, userNumber);
            finchRobot.wait(200);
            finchRobot.setMotors(0, 0);

            finchRobot.setMotors(-userNumber, -userNumber);
            finchRobot.wait(200);
            finchRobot.setMotors(0, 0);

            finchRobot.setMotors(-userNumber, userNumber);
            finchRobot.wait(200);
            finchRobot.setMotors(0, 0);

            finchRobot.setMotors(userNumber, -userNumber);
            finchRobot.wait(200);
            finchRobot.setMotors(0, 0);

            finchRobot.setMotors(userNumber, -userNumber);
            finchRobot.wait(200);
            finchRobot.setMotors(0, 0);

            finchRobot.setMotors(-userNumber, userNumber);
            finchRobot.wait(200);
            finchRobot.setMotors(0, 0);

            finchRobot.setMotors(-userNumber, -userNumber);
            finchRobot.wait(200);
            finchRobot.setMotors(0, 0);

            finchRobot.setMotors(userNumber, userNumber);
            finchRobot.wait(200);
            finchRobot.setMotors(0, 0);
            DisplayContinuePrompt();
        }
        static void DisplayMixingItUp(Finch finchRobot)
        {
            string userName;
            string userResponse;
            int userNumber;
            
            DisplayScreenHeader("Talent Show - Mixing It Up");
            Console.Write("\tWhat is your name? ");
            userName = Console.ReadLine();

            Console.Write("\t{0}, Please enter a number 0 to 255: ", userName);
            userResponse = Console.ReadLine();
            userNumber = Convert.ToInt32(userResponse);
            Console.WriteLine("\t{0} You entered {1}", userName, userNumber);
            Console.WriteLine();
            Console.WriteLine("\tFinch will now play \"We Will Rock You\" by Queen.");

            // We Will Rock You: 
            //C(1047) B(988) A(880) G(784) A(880) A(880) 
            //Quarter(740) Eighth(370)

            finchRobot.setLED(255, 0, 0);
            finchRobot.noteOn(1047);
            finchRobot.wait(740);
            finchRobot.noteOff();

            finchRobot.setLED(255, 255, 0);
            finchRobot.noteOn(988);
            finchRobot.wait(740);
            finchRobot.noteOff();

            finchRobot.setLED(0, 255, 0);
            finchRobot.noteOn(880);
            finchRobot.wait(740);
            finchRobot.noteOff();

            finchRobot.setLED(0, 255, 255);
            finchRobot.noteOn(784);
            finchRobot.wait(740);
            finchRobot.noteOff();

            finchRobot.setLED(0, 0, 255);
            finchRobot.noteOn(880);
            finchRobot.wait(370);
            finchRobot.noteOff();

            finchRobot.setLED(0, 0, 255);
            finchRobot.noteOn(880);
            finchRobot.wait(370);
            finchRobot.noteOff();

            finchRobot.setMotors(-userNumber, userNumber);
            finchRobot.wait(200);
            finchRobot.setMotors(userNumber, -userNumber);
            finchRobot.wait(400);
            finchRobot.setMotors(-userNumber, userNumber);
            finchRobot.wait(200);
            finchRobot.setMotors(0, 0);

            DisplayContinuePrompt();
        }



        #endregion

        #region FINCH ROBOT MANAGEMENT

        /// <summary>
        /// *****************************************************************
        /// *               Disconnect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("\tAbout to disconnect from the Finch robot.");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.WriteLine("\tThe Finch robot is now disconnected.");

            DisplayMenuPrompt("Main Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *                  Connect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            bool robotConnected;

            DisplayScreenHeader("Connect Finch Robot");

            Console.WriteLine("\tAbout to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
            DisplayContinuePrompt();

            robotConnected = finchRobot.connect();

            // TODO test connection and provide user feedback - text, lights, sounds

            DisplayMenuPrompt("Main Menu");

            //
            // reset finch robot
            //
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();

            return robotConnected;
        }

        #endregion

        #region USER INTERFACE

        /// <summary>
        /// *****************************************************************
        /// *                     Welcome Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Closing Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPlease press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display menu prompt
        /// </summary>
        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion
    }
}
