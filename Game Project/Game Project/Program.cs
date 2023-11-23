using System;

using System.IO;
using System.Reflection;

namespace Game_Project
{
    class Program
    {
        //to keep names and score from previous files 
        static string[] keepNames = new string[3];
        static double[] bestScores = new double[3];
        //to allow all of them to be called into different functions and changing the value
        static double firstScore = double.MaxValue;
        static double secondScore = double.MaxValue;
        static double thirdScore = double.MaxValue;
        static string firstName = "best score";
        static string secondName = "second best score";
        static string thirdName = "third best score";
        static int scoreCounter = 0;
        //set to random high number so its done until the user plays 5000 times 
        static double[] storeTimes= new double [5000];
        static string[] nameList = new string[5000];
        //to print different outcomes for gameover
        static bool winOrLost;
        //gameover appears in different locations so its better to have it as a global
        static bool gameover;
        //called global
        static double counter = 0;
        //mvemnts 
        static int horMov = 0;
        static int vertMov = 0;
        //starting spots 
        static double row = 6;
        static double col = 1;
        //movment  speed and lava speed
        static double speed = .3;
        const int frameInterval = 20;
        static int tickCount = 0;
        static int lavaSpawn = 0;
        //copy maze and replay as many times
        static string[] copyOrigin;
        //makes it so the actually maze doesnt change it just makes a copy and the copy gets played
        static System.Collections.ObjectModel.ReadOnlyCollection<string> theMaze = Array.AsReadOnly(new string[]{
                @"┌─────────────────────────────────────────────────────────────────────────────────────────────────┐",
                @"│          0        0           0    0                                   0        0               │",
                @"│    00000 0    00000   000000  0    0 000000000000000000000000000  0      0                      │",
                @"│        0 0            0  0    0    0 0                         0       0   0    0               │",
                @"│        0 0    0000000 0  000000    0 0                         0 0  0  0       0                │",
                @"│000000000 0000 0     0 0            0 0      0000000000000000   0     0 0      0                 │",
                @"│                       0            0 0      0              0   0   0   0 00000000000000000000000│",
                @"│ 000000000000000000000 0            0 00000000 0000000000   0   0 0     0 0                      │",
                @"│ 0                   0 0            0          0        0   0   0   0     0 0000000000000000000  │",
                @"│ 000000000000000000000 0            000000000  0        0   0000   0      0 0  0              0  │",
                @"│ 0                     0                    0  0        0      0   0  00000 0   0        000 0 0 │",
                @"│ 000000000000000000000 0000000000000000000000  0        000000 0     0    0 0     0     0     000│",
                @"│ 0                                             0             0 0 0  0     0 0     0     0   0 0  │",
                @"│0000  0000000000000000000000000000 0000000000000    0000000000 0  0       0 0    0  0     0 0    X",
                @"│   0  000  0 0           000000000 0                0          0  0 0 0   0 0        0  0    0   X",
                @"│ 0 0    0  0 0           0       0 0                0    0000000   0 0    0 0      0      0  0   X",
                @"│ 0 0000 0  0 0           0  0000 0 000000000000000000    0     0000000 0000 0000 0       0   0000│",
                @"│ 0      0  0 0           0  0  0                         0     0                0    0     0     │",
                @"│ 00000000  0 0000000000000  0  0 00000000000000000  000000     0   000 00000000 0        0       │",
                @"│ 0         0                0  0 0               0  0        00    0  0    0  0  0    0 0        │",
                @"│ 0 000     0000000000000000 0  0 0               0  0       00     0   0       00 0   0          │",
                @"│ 0 0  00000000000000000   0 0  0 0  00000000000000  0      00      0      0        0   0         │",
                @"│ 0 0  0               0   0 0  0 0  0               0     00       0   0   0   0 0 0    0        │",
                @"│ 0 0  0 0000000000000 0   0 0  0 0  0     00000000000    00        0 00  0   000     0   0       │",
                @"│ 0 0  0 0           0 0   0 0  0 0  0     0             00         0       00   00  0            │",
                @"│ 0 0  0 0     0000000 0   0 0  0 0  0     0            00          0  0 0 0       0      0       │",
                @"│ 0 0  0 0     0        0  0 0  0 0  0     00000000000000           0   0 0 0   0 0               │",
                @"│ 0 0  0 0     0 00000 0   0 0  0 0  00000000000000000000000000000000       0   0                 │",
                @"│ 0 0  0 0     0 0   0000000 0  0    0   0   0   00   0   0      00 000000 0000000000             │",
                @"│        0     0             0  0      0   0   0    0   0   0000                    0             │",
                @"└─────────────────────────────────────────────────────────────────────────────────────────────────┘",});

        //=========================================
        //=========================================
        //write highscores onto a file 
        //=========================================
        //=========================================

        static void writeHighScores()
        {
            //making topscore.txt into gamelogs so i can just call gamelogs
            string gamelogs = @"topScore.txt";
            //if the file exist
            if (File.Exists(gamelogs))
            {
            //reads the file 
            string readtext = File.ReadAllText(gamelogs);
            string[] besttimes = readtext.Split("\n");
            string name, number;
              //will read the lines of the text split up the name and the score 
              //parse the score and save score into array and save names into array
                for (int i = 0; i < besttimes.Length; i++)
                {
                    string line = besttimes[i];

                    string[] fields = line.Split(": ");
                    if (fields.Length >= 2)
                    {
                        name = fields[0];
                        number = fields[1];
                        keepNames[i] = name;
                        bestScores[i] = double.Parse(number);
                    }
                }
                //to ensure empty array/0 doesnt get but in so the score can be outputted properly
                if (bestScores[0] != 0)
                {


                    firstScore = bestScores[0];
                    firstName = keepNames[0];
                }
                //to ensure empty array/0 doesnt get but in so the score can be outputted properly
                if (bestScores[1] != 0)
                {
                    secondScore = bestScores[1];
                    secondName = keepNames[1];

                }
                //to ensure empty array/0 doesnt get but in so the score can be outputted properly
                if (bestScores[2]!= 0)
                {
                    thirdScore = bestScores[2];
                    thirdName = keepNames[2];
                }
               
            }
            //if they try to read file before any scores are logged
            else if (!File.Exists(gamelogs))
            {

                {
                    using (StreamWriter sw = File.CreateText(gamelogs))
                    {

                        sw.WriteLine("No scores yet");
                    }
                }

            }
            //again to ensure no 0 has been entered to ruin the scores
            if (storeTimes[scoreCounter] >= 0)
            {

                using (StreamWriter sw = File.CreateText(gamelogs))
                {
                    //storing the best score
                    if (storeTimes[scoreCounter] < firstScore)
                    {
                        
                    thirdScore = secondScore;
                    thirdName = secondName;
                    secondScore = firstScore;
                    secondName = firstName;
                    firstScore = storeTimes[scoreCounter];
                    firstName = nameList[scoreCounter];
                        
                    }
                    //storing the second best score
                    else if (storeTimes[scoreCounter] > firstScore & storeTimes[scoreCounter] <secondScore) 
                    {
                    thirdScore = secondScore;
                    thirdName = secondName;
                    secondScore = storeTimes[scoreCounter];
                    secondName = nameList[scoreCounter];
                    }
                    //storing the third best score
                    else if(storeTimes[scoreCounter] > secondScore & storeTimes[scoreCounter] < thirdScore)
                    {
                    thirdScore = storeTimes[scoreCounter];
                    thirdName = nameList[scoreCounter];
                    }
                    // to only display scores that have been entered and not a messy value
                    if (firstScore != double.MaxValue)
                    {
                        sw.WriteLine(firstName + ": " + firstScore );
                       
                    }
                    // to only display scores that have been entered and not a messy value
                    if (secondScore != double.MaxValue)
                    {
                        sw.WriteLine(secondName + ": " + secondScore);

                       
                    }
                    // to only display scores that have been entered and not a messy value
                    if (thirdScore != double.MaxValue)
                    {
                        sw.WriteLine(thirdName + ": " + thirdScore);
                       
                        
                    }
                }
            }
        }

        //=========================================
        //=========================================
        //drawing lava
        //=========================================
        //=========================================

        static void drawLava(int lavCol)
        {
           //counts the elements contained
            for (int i = 0; i < theMaze.Count; i++)
            {
                //to print lava over 0 any space and $
                if (copyOrigin[i][lavCol] == ' ' || copyOrigin[i][lavCol] == '0' || copyOrigin[i][lavCol] == '$')
                {
                    //changing lava colour to red
                    Console.ForegroundColor = ConsoleColor.Red;
                    //to spawn lava
                    Console.SetCursorPosition(lavCol, i);
                    Console.Write('#');
                    //to replace whatever is in the location and to change it with lava
                    copyOrigin[i] = copyOrigin[i].Remove(lavCol, 1).Insert(lavCol, "#");
                }
            }
        }
        //=========================================
        //=========================================
        //display the stopwatch
        //=========================================
        //=========================================
        static void stopwatch()
        {

            counter++;

            Console.SetCursorPosition(110, 15);
            Console.WriteLine(counter);

        }

        //=========================================
        //=========================================
        //Draw function
        //=========================================
        //=========================================
        static void Draw()
        {
            //sets cursor postion to the start of the maze
            Console.SetCursorPosition((int)col, (int)row);
            Console.Write(" ");

            if (row < 0) row = Console.WindowHeight - 1;
            if (row > Console.WindowHeight - 1) row = 0;
            if (col < 0) col = Console.WindowWidth - 1;
            if (col > Console.WindowWidth - 1) col = 0;
            //to get percentage of maze completion for calculations to spped up lava
            int completionPercentage = (int)((col / copyOrigin[0].Length) * 100);
            //original lava speed
            int lavaSpeed = 180;
            //speed when they pass 70%
            if (completionPercentage > 70)
            {
                lavaSpeed = 8;
            }
            //speed if they pass 50%
            else if (completionPercentage > 50)
            {
                lavaSpeed = 15;
            }
            //speed if they pass 30%
            else if (completionPercentage > 30)
            {
                lavaSpeed = 120;
            }
            if (++tickCount % lavaSpeed == 0)
            {
                drawLava(lavaSpawn++);
            }
          //to see if current loction is lava and to see if i get hit by lava
            int lavaHor = (int)col;
            int lavaVer = (int)row;
            string lavarow = copyOrigin[lavaVer];
            char currentLoc = lavarow[lavaHor];
            //calculations to see if next spot is availiable 
            //hori
            int x;
            //vert
            int y;
            x = (int)col + horMov;
            y = (int)row + vertMov;
            string rowstring = copyOrigin[y];
            char iswall = rowstring[x];
            //move if its a space
            if (iswall == ' ')
            {

                row = row + vertMov * speed;

            }
            //move if its a space
            if (iswall == ' ')
            {
                col = col + horMov * speed;
            }

            //if on x u win and set winorlost to true and game is over
            if (iswall == 'X')
            {

                winOrLost = true;
                gameover = true;
            }
            //if hit by a # u win and set winorlost to false and game is over
            if (iswall == '#')
            {
                winOrLost = false;
                gameover = true;
            }
            if (currentLoc == '#')
            {
                winOrLost = false;
                gameover = true;
            }
            //character 
            Console.SetCursorPosition((int)col, (int)row);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("$");
            

        }
        //=========================================
        //=========================================
        //Get valid int
        //=========================================
        //=========================================

        static int validInt()
        {

            bool test = true;
            // to store number
            int answer;
            do
            {
                Console.WriteLine("What would you like to do");
                test = int.TryParse(Console.ReadLine(), out answer);

            } while (!test);

            return answer;

        }
        //=========================================
        //=========================================
        //The title 
        //=========================================
        //=========================================

        static void theTitle()
        {

            string[] Title = {
                 @"            ##                 /\       \          /       /\                #########   |       |    |\     |  ",
                 @"            ##                /  \       \        /       /  \               ##     ##   |       |    | \    |  ",
                 @"            ##               /    \       \      /       /    \              ##     ##   |       |    |  \   |  ",
                 @"            ##              / ---- \       \    /       / ---- \             #########   |       |    |   \  |  ",
                 @"            ##             /        \       \  /       /        \            ##    ##    |       |    |    \ |  ",
                 @"            #########     /          \       \/       /          \           ##     ##   |_______|    |     \|  "};
            foreach (string i in Title)
            {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(i);
            }
        }

        //=========================================
        //=========================================
        //CASE 1 The game
        //=========================================
        //=========================================

        static void game()
        {
            //resets everything when game gets restarted 
            vertMov = 0;
            horMov = 0;
            counter = 0;
            theMaze.CopyTo(copyOrigin, 0);
            lavaSpawn = 0;
            row = 6;
            col = 1;
            
            //Colour change so u can see the spacing better

            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach (string i in copyOrigin)
            {

                Console.WriteLine(i);
            }
            //so yoy cant see cursor
            Console.CursorVisible = false;

            
            //movements
            gameover = false;
            while (!gameover)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {

                        case ConsoleKey.UpArrow:

                            vertMov = -1;
                            horMov = 0;

                            break;
                        case ConsoleKey.DownArrow:
                            vertMov = 1;
                            horMov = 0;
                            break;
                        case ConsoleKey.RightArrow:

                            vertMov = 0;
                            horMov = 1;
                            break;
                        case ConsoleKey.LeftArrow:
                            vertMov = 0;
                            horMov = -1;
                            break;
                        
                        
                    }
                }
                stopwatch();
                Draw();
                System.Threading.Thread.Sleep(frameInterval);

                if (gameover)
                {
                    //if you won
                    if (winOrLost == true)
                    {
                        Console.Clear();
                        Console.SetCursorPosition(30, 16);
                        Console.Write("What is the name of the legend that has just won: ");
                        string name = Console.ReadLine();

                        Console.SetCursorPosition(30, 17);
                        Console.WriteLine("Congrats you won in " + counter + " game ticks");
                                                
                        Console.SetCursorPosition(30, 18);
                        Console.WriteLine("Hit any key to go back to menu");

                        //displays scores from that round to arrays to save scores
                        nameList[scoreCounter] = name;
                        storeTimes[scoreCounter] = counter;
                        writeHighScores();
                        scoreCounter++;

                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                    {
                        Console.Clear();
                        Console.SetCursorPosition(30, 15);
                        Console.WriteLine("Haha imagine losing in " + counter + " game ticks" );
                        Console.SetCursorPosition(30, 16);
                        Console.WriteLine("Hit any key to go back to menu");
                        Console.ReadKey();
                        Console.Clear();
                    }


                }


            }

        }



        //=========================================
        //=========================================
        //CASE 2 Instructions
        //=========================================
        //=========================================
        static void instructions()
        {
            

            

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You are the $");
            Console.WriteLine("Get to the X before you get hit by Lava ");
            Console.WriteLine("The red # are the lava");
            Console.WriteLine("Lava will be following behind you so choose your path wisely ");



            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" ");

            Console.WriteLine("Use arrow keys to play  ");
            Console.WriteLine(" ");

            Console.WriteLine("Up arrow key to move up ^");
            Console.WriteLine(" ");

            Console.WriteLine("down arrow key to move down v");
            Console.WriteLine(" ");

            Console.WriteLine("left arrow key to move left <");
            Console.WriteLine(" ");

            Console.WriteLine("Right arrow key to move right >");
            Console.WriteLine(" ");

            Console.WriteLine("Hit any key to return to menu");
            Console.ReadKey();
            Console.Clear();
        }
        //=========================================
        //=========================================
        //Case 3 rview best times
        //=========================================
        //=========================================
        static void viewBestTimes()
        {

            Console.Clear();
            
            Console.WriteLine("The Best of the Best");
            //if the file exists
            if (File.Exists("topScore.txt"))
            {
            //reads everything from the file    
            string viewScores = File.ReadAllText("topScore.txt");
            Console.WriteLine(viewScores);
            }

            //if it doesnt
            else
            {
            Console.WriteLine("no scores have been logged yet");
            }

            Console.WriteLine("hit any key to return to menu");
            Console.ReadKey();
            Console.Clear();
        }



        static void Main(string[] args)
        {
            //sets window size
            Console.SetWindowSize(120, 40);
            //copies maze
            copyOrigin = new string[theMaze.Count];
            // prints the title
            theTitle();

            int choice = 0;
            do
            {
                //Menu
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("1. Start ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("2. Instructions ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("3. View best times  ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("4. Quit ");
                Console.ForegroundColor = ConsoleColor.White;

                do
                {
                    choice = validInt();
                    switch (choice)
                    {
                        case 1:
                            Console.Clear();
                            game();
                            break;
                        case 2:
                            Console.Clear();
                            instructions();
                            break;
                        case 3:
                            viewBestTimes();
                            break;
                        case 4:
                            break;
                    }
                } while (choice > 4 || choice < 1);
            } while (choice != 4);

        }

    }

}