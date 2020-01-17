﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using static System.Console;

namespace Hamlet
{
    class Program
    {
        static readonly string intro = @"Hamlet - Text Adventure
=======================

This program is what happens when you tell a programmer to summarize
Hamlet in 45 seconds as an English assignment.  Please note that
it took way more than 45 seconds to write this and will probably take
more than 45 seconds to play through this.

I wanted to do a full text adventure similar to the Zork series where
you are able to freely type anything but that'd be way too much work even
for me.  So I decided to approach this more like the text adventures
seen in Saints Row 4.  It's more like a multiple choice, but still,
a text adventure nonetheless.  Also, this wouldn't be my program if there
weren't a few bugs so be careful.  And it wouldn't be my program if it
didn't have my sense of humor embedded straight into the code, so you're
going to KNOW when you've chosen a wrong path in the game.

     [Press any key to continue.]";

        static GameData gameData = null;
        static bool inGame = false;
        static int currentState = 0;
        static int currentChoice = 0;
        static bool success = false;
        static string result = "";

        static void Main(string[] args)
        {
            WriteLine(intro);
            ReadKey(true);

            inGame = true;
            currentState = 0;
            gameData = JsonConvert.DeserializeObject<GameData>(Properties.Resources.gamedata);

            while(inGame)
            {
                var state = gameData.SceneStates[currentState];

                Clear();
                WriteLine("HAMLET - ACT {0}, SCENE {1}", state.Act + 1, state.Scene + 1);
                WriteLine("============");
                if(!string.IsNullOrWhiteSpace(result))
                {
                    WriteLine();
                    WriteLine(result);
                }
                WriteLine();
                WriteLine(state.Prompt);
                WriteLine();
                for(int i = 0; i < state.Choices.Length; i++)
                {
                    if(i == currentChoice)
                    {
                        BackgroundColor = ConsoleColor.White;
                        ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        BackgroundColor = ConsoleColor.Black;
                        ForegroundColor = ConsoleColor.White;
                    }

                    WriteLine(" {0}. {1}", i + 1, state.Choices[i]);
                }

                BackgroundColor = ConsoleColor.Black;
                ForegroundColor = ConsoleColor.Gray;

                WriteLine();
                WriteLine();
                WriteLine("[ENTER] Choose   [UP/DOWN] Select choice");

                var kinf = ReadKey(true);

                switch(kinf.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentChoice > 0) currentChoice--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (currentChoice < state.Choices.Length - 1) currentChoice++;
                        break;
                    case ConsoleKey.Enter:
                        result = state.Responses[currentChoice];
                        if(currentChoice == state.CorrectChoice)
                        {
                            currentState++;
                            if(currentState >= gameData.SceneStates.Length)
                            {
                                inGame = false;
                                success = true;
                            }
                        } 
                        else
                        {
                            inGame = false;
                            success = false;
                        }


                        currentChoice = 0;
                        break;
                }
            }

            Clear();

            if(success)
            {
                WriteLine("You have completed HAMLET.");
            }
            else
            {
                WriteLine("GAME OVER");
            }
            if (!string.IsNullOrWhiteSpace(result))
            {
                WriteLine();
                WriteLine(result);
            }
            WriteLine();
            WriteLine("Thanks for playing.");
            WriteLine();
            WriteLine("Press any key to exit.");
            ReadKey(true);
        }
    }
}
