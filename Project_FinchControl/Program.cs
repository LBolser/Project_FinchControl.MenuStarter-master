﻿using System;
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

    /// <summary>
    /// User Commands
    /// </summary>
    public enum Command
    {
        NONE,
        MOVEFORWARD,
        MOVEBACKWARD,
        STOPMOTORS,
        WAIT,
        TURNRIGHT,
        TURNLEFT,
        LEDON,
        LEDOFF,
        GETTEMP,
        DONE
    }

    class Program
    {
        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            DisplayReadAndSetTheme();
            DisplaySetNewTheme();

            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }

        #region THEME
        static void DisplayReadAndSetTheme()
        {
            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) themeColors;
            string fileIOStatusMessage;

            themeColors = ReadThemeDataExceptions(out fileIOStatusMessage);
            
            

            if (fileIOStatusMessage == "Complete")
            {
                Console.ForegroundColor = themeColors.foregroundColor;
                Console.BackgroundColor = themeColors.backgroundColor;
                Console.Clear();

                DisplayScreenHeader("Fetching Color Theme");
                Console.WriteLine("\n\tTheme successfully read from data file.\n");
            }
            else
            {
                DisplayScreenHeader("Fetching Color Theme");
                Console.WriteLine("\n\tTheme NOT read from data file.");
                Console.WriteLine($"\t\t{fileIOStatusMessage} \n");
            }

            DisplayContinuePrompt();
        }

         static (ConsoleColor foregroundColor, ConsoleColor backgroundColor) ReadThemeDataExceptions(out string fileIOStatusMessage)
        {
            string dataPath = @"Data/Theme.txt";
            string[] themeColors;

            ConsoleColor foregroundColor = ConsoleColor.Green;
            ConsoleColor backgroundColor = ConsoleColor.Black;

            try
            {
                themeColors = File.ReadAllLines(dataPath);
                if (Enum.TryParse(themeColors[0], true, out foregroundColor) &&
                    Enum.TryParse(themeColors[1], true, out backgroundColor))
                {
                    fileIOStatusMessage = "Complete";
                }
                else
                {
                    fileIOStatusMessage = "\tERROR: Data file incorrectly formated.";
                }
            }
            catch (DirectoryNotFoundException)
            {
                fileIOStatusMessage = "\tERROR: Unable to locate the folder for the data file.";
            }
            catch (Exception)
            {
                fileIOStatusMessage = "\tERROR: Unable to read data file.";
            }

            return (foregroundColor, backgroundColor);
        }

        static void DisplaySetNewTheme()
        {
            Console.CursorVisible = true;

            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) themeColors;
            bool themeChosen = false;
            string fileIOStatusMessage;

            DisplayScreenHeader("Set New Theme");

            Console.WriteLine($"\tCurrent foreground color: {Console.ForegroundColor}");
            Console.WriteLine($"\tCurrent background color: {Console.BackgroundColor}");
            Console.WriteLine();

            Console.Write("\tWould you like to contiue using this theme? [ yes | no ]? ");

            if (Console.ReadLine().ToLower() == "no")
            {
                do
                {
                    themeColors.foregroundColor = GetConsoleColorFromUser("foreground");
                    themeColors.backgroundColor = GetConsoleColorFromUser("background");

                    Console.ForegroundColor = themeColors.foregroundColor;
                    Console.BackgroundColor = themeColors.backgroundColor;

                    Console.Clear();

                    DisplayScreenHeader("Set Application Theme");

                    Console.WriteLine($"\tNew foreground color: {Console.ForegroundColor}");
                    Console.WriteLine($"\tNew background color: {Console.BackgroundColor}");

                    Console.WriteLine();
                    Console.Write("\tWould you like to save the current theme? [ yes | no ]? ");

                    if (Console.ReadLine().ToLower() == "yes")
                    {
                        themeChosen = true;
                        fileIOStatusMessage = WriteThemeDataExceptions(themeColors.foregroundColor, themeColors.backgroundColor);
                        if (fileIOStatusMessage == "Complete")
                        {
                            Console.WriteLine("\n\tNew theme saved in Theme data file.\n");
                        }
                        else
                        {
                            Console.WriteLine("\n\tNew theme not saved to Theme data file.");
                            Console.WriteLine($"\t\tERROR: {fileIOStatusMessage} \n");
                        }
                    }
                } while (!themeChosen);
            }
            DisplayContinuePrompt();
        }

        static string WriteThemeDataExceptions(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            string dataPath = @"Data/Theme.txt";
            string fileIOStatusMessage = "";

            try
            {
                File.WriteAllText(dataPath, foregroundColor.ToString() + "\n");
                File.AppendAllText(dataPath, backgroundColor.ToString());
                fileIOStatusMessage = "Complete";
            }
            catch (DirectoryNotFoundException)
            {
                fileIOStatusMessage = "Unable to locate the folder for the data file.";
            }
            catch (Exception)
            {
                fileIOStatusMessage = "Unable to write to data file.";
            }

            return fileIOStatusMessage;
        }

        static ConsoleColor GetConsoleColorFromUser(string property)
        {
            ConsoleColor consoleColor;
            bool validConsoleColor;

            ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
            ConsoleColor currentBackground = Console.BackgroundColor;
            ConsoleColor currentForeground = Console.ForegroundColor;

            do
            {
                Console.WriteLine();
                Console.WriteLine("\tAvailable Console Colors:");
                Console.WriteLine();
                foreach (var color in colors)
                {
                    if (color == currentBackground)
                    {
                        Console.ForegroundColor = color;
                        Console.Write("\t ");
                        Console.Write('\u2588');
                        Console.Write('\u2588');
                        Console.Write('\u2588');
                        Console.ForegroundColor = currentForeground;
                        Console.WriteLine(" {0} - See Background Color", color);
                        continue;
                    }
                    Console.ForegroundColor = color;
                    Console.Write("\t ");
                    Console.Write('\u2588');
                    Console.Write('\u2588');
                    Console.Write('\u2588');
                    Console.ForegroundColor = currentForeground;
                    Console.Write(" {0} - ", color);
                    Console.ForegroundColor = color;
                    Console.WriteLine("SAMPLE ");

                }

                Console.ForegroundColor = currentForeground;
                Console.WriteLine();

                Console.Write($"\tEnter a value for the {property}:");
                
                validConsoleColor = Enum.TryParse<ConsoleColor>(Console.ReadLine(), true, out consoleColor);

                Console.Clear();

                if (!validConsoleColor)
                {
                    Console.WriteLine("\n\tERROR: Please provide a valid console color. \n");
                }
                else
                {
                    validConsoleColor = true;
                }
                
            } while (!validConsoleColor);

            return consoleColor;

        }

        /// <summary>
        /// setup the console theme
        /// </summary>
        static void SetTheme()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;
        }
        #endregion

        #region MAIN MENU
        /// <summary>
        /// MAIN MENU
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
                        DataRecorderDisplayMenuScreen(finchRobot);
                        break;

                    case "d":
                        LightAlarmDisplayMenuScreen(finchRobot);
                        break;

                    case "e":
                        UserProgrammingDisplayMenuScreen(finchRobot);
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
        #endregion

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
            finchRobot.setLED(0, 0, 0);

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

            while (string.IsNullOrEmpty(userName))
            {
                Console.Write("\tUh oh! You didn't enter anything for your name. Please input your name: ");
                userName = Console.ReadLine();
            }

            Console.Write("\t{0}, Please enter a number 0 to 255: ", userName);
            userResponse = Console.ReadLine();
            while (!int.TryParse(userResponse, out userNumber))
            {
                Console.Write("\tThis is not a number! Enter again: ");
                userResponse = Console.ReadLine();
            }


            Console.WriteLine("\t{0} You entered {1}", userName, userNumber);
            Console.Write("Finch will now dance for you!");

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

            while (string.IsNullOrEmpty(userName))
            {
                Console.Write("\tUh oh! You didn't enter anything for your name. Please input your name: ");
                userName = Console.ReadLine();
            }

            Console.Write("\t{0}, Please enter a number 0 to 255: ", userName);
            userResponse = Console.ReadLine();
            while (!int.TryParse(userResponse, out userNumber))
            {
                Console.Write("\tThis is not a number! Enter again: ");
                userResponse = Console.ReadLine();
            }


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

            finchRobot.setLED(0, 0, 0);
            DisplayContinuePrompt();
        }



        #endregion

        #region DATA RECORDER

        /// <summary>
        /// displays the Data Recorder Menu
        /// </summary>
        static void DataRecorderDisplayMenuScreen(Finch finchRobot)
        {
            int numberOfDataPoints = 0;
            double dataPointFrequency = 0;
            double[] temperatures = null;

            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Data Recorder Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Number of Data Points");
                Console.WriteLine("\tb) Frequency of Data Points");
                Console.WriteLine("\tc) Get Data");
                Console.WriteLine("\td) Show Data");
                Console.WriteLine("\te) Show Data in Fahrenheit");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        numberOfDataPoints = DataRecorderDisplayGetNumberOfDataPoints();
                        break;

                    case "b":
                        dataPointFrequency = DataRecorderDisplayGetDataPointFrequency();
                        break;

                    case "c":
                        temperatures = DataRecorderDisplayGetData(numberOfDataPoints, dataPointFrequency, finchRobot);
                        break;

                    case "d":
                        DataRecorderDisplayData(temperatures);
                        break;

                    case "e":
                        DataRecorderDisplayDataFarenheit(temperatures);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }

        /// <summary>
        /// gets the sum of the data points
        /// </summary>
        static double Sum(params double[] temperatures)
        {
            double tempSum = 0;

            for (int i = 0; i < temperatures.Length; i++)
            {
                tempSum += temperatures[i];
            }

            return tempSum;
        }

        /// <summary>
        /// gets the average of the data
        /// </summary>
        static decimal Average(params double[] temperatures)
        {
            double sum = Sum(temperatures);
            decimal tempAvg = (decimal)sum / temperatures.Length;

            Console.WriteLine();
            Console.WriteLine("\tAverage of the Data Points: {0}", tempAvg.ToString("n2"));

            return tempAvg;
        }

        /// <summary>
        /// shows the data in fahrenheit
        /// </summary>
        static void DataRecorderDisplayDataFarenheit(double[] temperatures)
        {
            DisplayScreenHeader("Show Data in Fahrenheit");
            
            //
            // display table headers
            //
            Console.WriteLine(
                "Recording #".PadLeft(15) +
                "Temp".PadLeft(15)
                );
            Console.WriteLine(
                "-----------".PadLeft(15) +
                "-----------".PadLeft(15)
                );
            //
            // display table data
            //
            for (int index = 0; index < temperatures.Length; index++)
            {
                Console.WriteLine(
                    (index + 1).ToString().PadLeft(15) +
                    ((((temperatures[index])/5)*9)+32).ToString("n2").PadLeft(15)
                    );
            }
            DisplayContinuePrompt();
        }

        /// <summary>
        /// shows the data
        /// </summary>
        static void DataRecorderDisplayData(double[] temperatures)
        {
            DisplayScreenHeader("Show Data");

            DataRecorderDisplayTable(temperatures);
            DisplayContinuePrompt();

        }
    
        /// <summary>
        /// shows a table of the data points
        /// </summary>
        static void DataRecorderDisplayTable(double[] temperatures)
        {
            //
            // display table headers
            //
            Console.WriteLine(
                "Recording #".PadLeft(15) +
                "Temp".PadLeft(15)
                );
            Console.WriteLine(
                "-----------".PadLeft(15) +
                "-----------".PadLeft(15)
                );
            //
            // display table data
            //
            for (int index = 0; index < temperatures.Length; index++)
            {
                Console.WriteLine(
                    (index + 1).ToString().PadLeft(15) +
                    temperatures[index].ToString("n2").PadLeft(15)
                    );
            }
            Average(temperatures);


        }

        /// <summary>
        /// get the data from the finch robot
        /// </summary>
        static double[] DataRecorderDisplayGetData(int numberOfDataPoints, double dataPointFrequency, Finch finchRobot)
        {
            double[] temperatures = new double[numberOfDataPoints];

            DisplayScreenHeader("Get Data");

            Console.WriteLine($"\tNumber of Data Points: {numberOfDataPoints}");
            Console.WriteLine($"\tData Point Freqency: {dataPointFrequency}");
            Console.WriteLine();
            Console.WriteLine("\tThe Finch Robot is ready to begin recording the tempurature data");
            DisplayContinuePrompt();
            Console.WriteLine();
            
            for (int index = 0; index < numberOfDataPoints; index++)
            {
                temperatures[index] = finchRobot.getTemperature();
                Console.WriteLine($"\tReading {index + 1}  :  {temperatures[index].ToString("n2")}");
                int waitInSeconds = (int)(dataPointFrequency * 1000);
                finchRobot.wait(waitInSeconds);
            }

            DisplayContinuePrompt();
            DisplayScreenHeader("Get Data");

            Console.WriteLine();
            Console.WriteLine("\tTable of Temperatures");
            Console.WriteLine();
            DataRecorderDisplayTable(temperatures);


            DisplayContinuePrompt();
            return temperatures;
        }

        /// <summary>
        /// get the frequency of data points from the user
        /// </summary>
        /// <returns>frequency of data points</returns>
        static double DataRecorderDisplayGetDataPointFrequency()
        {
            double dataPointFrequency;

            DisplayScreenHeader("Data Point Frequency");

            Console.Write("\tPlease enter the frequency of Data Points: ");

            double.TryParse(Console.ReadLine(), out dataPointFrequency);

            DisplayContinuePrompt();

            return dataPointFrequency;
        }

        /// <summary>
        /// get the number of data points from user
        /// </summary>
        /// <returns>number of data points</returns>
        static int DataRecorderDisplayGetNumberOfDataPoints()
        {
            int numberOfDataPoints;
            string userResponse;

            DisplayScreenHeader("Number of Data Points");

            Console.Write("\tPlease enter the number of Data Points: ");
            userResponse = Console.ReadLine();
            Console.WriteLine("\tYou entered {0}", userResponse);

            int.TryParse(userResponse, out numberOfDataPoints);
            
            DisplayContinuePrompt();

            return numberOfDataPoints;
        }





        #endregion

        #region ALARM SYSTEM

        /// <summary>
        /// displays the LightAlarm Menu
        /// </summary>
        static void LightAlarmDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            string sensorsToMonitor = "";
            string rangeType = "";

            int minMaxThresholdValue = 0;
            int timeToMonitor = 0;

            do
            {
                DisplayScreenHeader("Light Alarm Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Sensors to Monitor");
                Console.WriteLine("\tb) Set Range Type");
                Console.WriteLine("\tc) Set Minimum/Maximum Threshold Value");
                Console.WriteLine("\td) Set Time To Monitor");
                Console.WriteLine("\te) Set Alarm");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        sensorsToMonitor = LightAlarmDisplaySetSensorsToMonitor();
                        break;

                    case "b":
                        rangeType = LightAlarmDisplaySetRangeType();
                        break;

                    case "c":
                        minMaxThresholdValue = LightAlarmSetMinMaxThresholdValue(rangeType, finchRobot);
                        break;

                    case "d":
                        timeToMonitor =  LightAlarmSetTimeToMonitor();
                        break;

                    case "e":
                        LightAlarmSetAlarm(finchRobot, sensorsToMonitor, rangeType, minMaxThresholdValue, timeToMonitor);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }

        static void LightAlarmSetAlarm(
            Finch finchRobot, 
            string sensorsToMonitor, 
            string rangeType, 
            int minMaxThresholdValue, 
            int timeToMonitor)
        {
            int secondsElapsed = 0;
            bool thresholdExceeded = false;
            int currentLightSensorValue = 0;

            DisplayScreenHeader("Set Alarm");

            Console.WriteLine("\tSensors to Monitor: {0}", sensorsToMonitor);
            Console.WriteLine("\tRange Type: {0}", rangeType);
            Console.WriteLine("\tMin/Max Threshold Value: {0}", minMaxThresholdValue);
            Console.WriteLine("\tTime To Monitor: {0}", timeToMonitor);
            Console.WriteLine();

            Console.WriteLine("\tPress any key to begin monitoring.");
            Console.ReadKey();
            Console.WriteLine();

            while ((secondsElapsed < timeToMonitor) && (!thresholdExceeded))
            {
                switch (sensorsToMonitor)
                {
                    case "left":
                        currentLightSensorValue = finchRobot.getLeftLightSensor();
                        break;

                    case "right":
                        currentLightSensorValue = finchRobot.getRightLightSensor();
                        break;

                    case "both":
                        currentLightSensorValue = (finchRobot.getLeftLightSensor() + finchRobot.getRightLightSensor()) / 2;
                        break;
                }
                switch(rangeType)
                {
                    case "minimum":
                        if (currentLightSensorValue < minMaxThresholdValue)
                        {
                            thresholdExceeded = true;
                        }
                        break;

                    case "maximum":
                        if (currentLightSensorValue > minMaxThresholdValue)
                        {
                            thresholdExceeded = true;
                        }
                        break;
                }
                finchRobot.wait(1000);
                secondsElapsed++;
            
            }
            if (thresholdExceeded)
            {
                Console.WriteLine("\tThe {0} threshold value of {1} was exceeded by the current light sensor value of {2}", rangeType, minMaxThresholdValue, currentLightSensorValue);
            }
            else
            {
                Console.WriteLine("\tThe {0} threshold value of {1} was not exceeded by the current light sensor value of {2}", rangeType, minMaxThresholdValue, currentLightSensorValue);
            }
            DisplayMenuPrompt("Light Alarm");
        }

        static int LightAlarmSetTimeToMonitor()
        {
            int timeToMonitor;

            DisplayScreenHeader("Time To Monitor");
            
            Console.Write("\tEnter the amount of time to monitor in seconds: ");
            int.TryParse(Console.ReadLine(), out timeToMonitor);
            while (timeToMonitor == 0)
            {
                Console.Write("\tYour value must be a non zero integer. Please try again: ");
                int.TryParse(Console.ReadLine(), out timeToMonitor);
            }
            Console.WriteLine();

            Console.WriteLine("\tYou entered the following amount of time to monitor in seconds: {0}", timeToMonitor);


            DisplayMenuPrompt("Light Alarm");

            return timeToMonitor;
        }

        static int LightAlarmSetMinMaxThresholdValue(string rangeType, Finch finchRobot)
        {
            int minMaxThresholdValue;

            DisplayScreenHeader("Minimum/Maximum Threshold Value");

            Console.WriteLine($"\tLeft Light Sensor Ambient Light: {finchRobot.getLeftLightSensor()}");
            Console.WriteLine($"\tRight Light Sensor Ambient Light: {finchRobot.getRightLightSensor()}");
            Console.WriteLine();

            Console.Write("\tEnter the {0} light sensor threshold value: ", rangeType);
            int.TryParse(Console.ReadLine(), out minMaxThresholdValue);
            while(minMaxThresholdValue == 0)
            {
                Console.Write("\tYour value must be a non zero integer. Please try again: ");
                int.TryParse(Console.ReadLine(), out minMaxThresholdValue);
            }
            
            Console.WriteLine();
            Console.WriteLine("\tYour {0} light sensor threshold value is: {1}", rangeType, minMaxThresholdValue);


            DisplayMenuPrompt("Light Alarm");

            return minMaxThresholdValue;
        }

        static string LightAlarmDisplaySetRangeType()
        {
            string rangeType;

            DisplayScreenHeader("Range Type");

            Console.Write("\tRange Type [minimum, maximum]: ");
            rangeType = Console.ReadLine();

            while(rangeType.Equals("minimum") == false && rangeType.Equals("maximum") == false)
            {
                Console.Write("\tPlease enter the all lowercase word \"minimum\" or \"maximum\": ");
                rangeType = Console.ReadLine();
            }
            Console.WriteLine();
            Console.WriteLine("\tYou have entered the following range type: {0}", rangeType);

            DisplayMenuPrompt("Light Alarm");

            return rangeType;
        }

        static string LightAlarmDisplaySetSensorsToMonitor()
        {
            string sensorsToMonitor;

            DisplayScreenHeader("Sensors to Monitor");

            Console.Write("\tSensors to Monitor [left, right, both]: ");
            sensorsToMonitor = Console.ReadLine();

            while (sensorsToMonitor.Equals("left") == false && sensorsToMonitor.Equals("right") == false && sensorsToMonitor.Equals("both") == false)
            {
                Console.Write("\tPlease enter the all lowercase word \"left\" \"right\" or \"both\": ");
                sensorsToMonitor = Console.ReadLine();
            }
            Console.WriteLine();
            Console.WriteLine("\tYou have entered the following sensor(s) to monitor: {0}", sensorsToMonitor);
            
            DisplayMenuPrompt("Light Alarm");

            return sensorsToMonitor;
        }

        #endregion

        #region USER PROGRAMMING
        /// <summary>
        /// displays the User Programming Menu
        /// </summary>
        
        static void UserProgrammingDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            //
            // tuple to store all three command parameters
            //
            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;

            List<Command> commands = new List<Command>();

            do
            {
                DisplayScreenHeader("User Programming Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Command Parameters");
                Console.WriteLine("\tb) Add Commands");
                Console.WriteLine("\tc) View Commands");
                Console.WriteLine("\td) Execute Commands");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        commandParameters = UserProgrammingDisplayGetCommandParameters();
                        break;

                    case "b":
                        UserProgrammingGetFinchCommands(commands);
                        break;

                    case "c":
                        UserProgrammingDisplayFinchCommands(commands);
                        break;

                    case "d":
                        UserProgrammingDisplayExecuteFinchCommands(finchRobot, commands, commandParameters);
                        break;
                        
                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }

        static void UserProgrammingDisplayExecuteFinchCommands(Finch finchRobot, List<Command> commands, (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters)
        {
            int motorSpeed = commandParameters.motorSpeed;
            int ledBrightness = commandParameters.ledBrightness;
            int waitMilliSeconds = (int)(commandParameters.waitSeconds * 1000);
            string commandFeedback = "";
            const int TURNING_MOTOR_SPEED = 100;

            DisplayScreenHeader("Execute Finch Commands");

            Console.WriteLine("\tThe Finch Robot is ready to execute the list of commands. ");
            DisplayContinuePrompt();
            Console.WriteLine();

            foreach (Command command in commands)
            {
                switch (command)
                {
                    case Command.NONE:
                        break;

                    case Command.MOVEFORWARD:
                        finchRobot.setMotors(motorSpeed, motorSpeed);
                        commandFeedback = Command.MOVEFORWARD.ToString();
                        break;

                    case Command.MOVEBACKWARD:
                        finchRobot.setMotors(-motorSpeed, -motorSpeed);
                        commandFeedback = Command.MOVEBACKWARD.ToString();
                        break;

                    case Command.STOPMOTORS:
                        finchRobot.setMotors(0, 0);
                        commandFeedback = Command.STOPMOTORS.ToString();
                        break;

                    case Command.WAIT:
                        finchRobot.wait(waitMilliSeconds);
                        commandFeedback = Command.WAIT.ToString();
                        break;

                    case Command.TURNRIGHT:
                        finchRobot.setMotors(TURNING_MOTOR_SPEED, -TURNING_MOTOR_SPEED);
                        commandFeedback = Command.TURNRIGHT.ToString();
                        break;

                    case Command.TURNLEFT:
                        finchRobot.setMotors(-TURNING_MOTOR_SPEED, TURNING_MOTOR_SPEED);
                        commandFeedback = Command.TURNLEFT.ToString();
                        break;

                    case Command.LEDON:
                        finchRobot.setLED(ledBrightness, ledBrightness, ledBrightness);
                        commandFeedback = Command.LEDON.ToString();
                        break;

                    case Command.LEDOFF:
                        finchRobot.setLED(0, 0, 0);
                        commandFeedback = Command.LEDOFF.ToString();
                        break;

                    case Command.GETTEMP:
                        commandFeedback = $"GETTEMP: {finchRobot.getTemperature().ToString("n2")}° Celsius";
                        break;

                    case Command.DONE:
                        commandFeedback = Command.DONE.ToString();
                        break;

                    default:
                        break;
                }

                Console.WriteLine($"\t{commandFeedback}");
               
            }
            finchRobot.setMotors(0, 0);
            finchRobot.setLED(0, 0, 0);
            DisplayContinuePrompt();
        }

        static void UserProgrammingDisplayFinchCommands(List<Command> commands)
        {
            DisplayScreenHeader("Finch Robot Commands");

            foreach (Command command in commands)
            {
                Console.WriteLine($"\t{command}");
            }

            DisplayMenuPrompt("User Programming");
        }

        static void UserProgrammingGetFinchCommands(List<Command> commands)
        {
            Command command = Command.NONE;

            DisplayScreenHeader("Finch Robot Commands");

            int commandCount = 1;
            Console.WriteLine("\tList of Available Commands");
            Console.WriteLine();
            foreach (string commandName in Enum.GetNames(typeof(Command)))
            {
                Console.WriteLine($"\t-- {commandName.ToLower()}  ");
                commandCount++;
            }
            Console.WriteLine();

            while (command != Command.DONE)
            {
                Console.Write("\tEnter Command:");

                if (Enum.TryParse(Console.ReadLine().ToUpper(), out command))
                {
                    commands.Add(command);
                }
                else
                {
                    Console.WriteLine("\t***INVALID ENTRY*** Please enter a command from the above list.");
                }
            }


        }

        static (int motorSpeed, int ledBrightness, double waitSeconds)  UserProgrammingDisplayGetCommandParameters()
        {
            DisplayScreenHeader("User Command Parameters");

            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;

            GetValidInteger("\tEnter Motor Speed [1 to 255]: ",1,255, out commandParameters.motorSpeed);
            GetValidInteger("\tEnter LED Brightness [1 to 255]: ", 1, 255, out commandParameters.ledBrightness);
            GetValidDouble("\tEnter Wait in Seconds [0 to 10]: ", 0, 10, out commandParameters.waitSeconds);

            Console.WriteLine();
            Console.WriteLine("\tMotor Speed: {0}", commandParameters.motorSpeed);
            Console.WriteLine("\tLED Brightness: {0}", commandParameters.ledBrightness);
            Console.WriteLine("\tWait Command Duration: {0}", commandParameters.waitSeconds);
            
            DisplayMenuPrompt("User Programming");

            return commandParameters;
        }

        static bool GetValidInteger(string prompt, int minValue, int maxValue, out int validInteger)
        {
            bool validResponse = false;

            do
            {
                Console.Write(prompt);

                if (int.TryParse(Console.ReadLine(), out validInteger))
                {
                    if (validInteger >= minValue && validInteger <= maxValue)
                    {
                        validResponse = true;
                    }
                    else
                    {
                        Console.Write("\tYou must enter an integer value between {0} and {1}. Please try again: ", minValue, maxValue);
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.Write("\tYou must enter an integer value. Please try again: ");
                    Console.WriteLine();
                }

            }
            while (!validResponse);
            return validResponse;
        }
        static bool GetValidDouble(string prompt, int minValue, int maxValue, out double validDouble)
        {
            bool validResponse = false;

            do
            {
                Console.Write(prompt);

                if (double.TryParse(Console.ReadLine(), out validDouble))
                {
                    if (validDouble >= minValue && validDouble <= maxValue)
                    {
                        validResponse = true;
                    }
                    else
                    {
                        Console.Write("\tYou must enter a double value between {0} and {1}. Please try again: ", minValue, maxValue);
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.Write("\tYou must enter a double value. Please try again: ");
                    Console.WriteLine();
                }

            }
            while (!validResponse);
            return validResponse;
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

            Console.WriteLine();
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
