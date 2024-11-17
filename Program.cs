using System.Reflection.Metadata.Ecma335;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            int logLine = 2, dethcount = 0;
            List<List<int>> map = new List<List<int>>
            {
                new List<int>{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new List<int>{ 0, 1, 1, 1, 1, 1, 1, 0, 1, 0 },
                new List<int>{ 0, 1, 1, 1, 1, 1, 1, 0, 1, 0 },
                new List<int>{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                new List<int>{ 0, 1, 1, 1, 2, 1, 1, 1, 1, 0 },
                new List<int>{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                new List<int>{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                new List<int>{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                new List<int>{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                new List<int>{ 0, 1, 1, 0, 0, 0, 0, 1, 1, 0 },
                new List<int>{ 0, 1, 1, 0, 6, 0, 1, 1, 1, 0 },
                new List<int>{ 0, 1, 1, 0, 1, 4, 5, 1, 1, 0 },
                new List<int>{ 0, 1, 1, 0, 0, 0, 1, 1, 1, 0 },
                new List<int>{ 0, 1, 1, 0, 1, 1, 2, 1, 1, 0 },
                new List<int>{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                new List<int>{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                new List<int>{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };
            Random rng = new Random();
            int liczba = rng.Next(10);


            Console.SetBufferSize(120, 30);



            List<Player> playerList = new List<Player>();

            Console.Write("How many players will enter dungeon? \n(solo raid or multi-player(max 3) raid) ");
            int playernumb = int.Parse(Console.ReadLine());
            if (playernumb <= 0)
            {
                playernumb = 1;
            }
            else if(playernumb>3)
            {
                playernumb = 3;
            }

            for (int i = 0; i < playernumb; i++)
            {
                Player p = CreatePlayer("player " + (i + 1), (i + 1).ToString());

                p.x += i;

                playerList.Add(p);
            }

            Console.Clear();


            Monster m = CreateMonster();


            DrawMap(map);
            AllPlayers(playerList);

            ClearLine(10);

            Console.SetCursorPosition(m.x, m.y);
            Console.Write(m.avatar);
            m.SetCombatArea(map, 0, 3);

            foreach (Player p in playerList)
            {
                Console.SetCursorPosition(p.x, p.y);
                Console.Write(p.avatar);
            }


            bool stillPlaying = true;

            

            while (stillPlaying)
            {
                foreach (Player p in playerList)
                {
                    if (p.alive == true)
                    {
                        if (playernumb == 1)
                        {
                            WhatsOnFlor(p, map, logLine, playerList, m, liczba, stillPlaying);
                        }
                        else
                        {
                            ClearLine(10);
                            Console.Write(p);

                            ClearLine(12);
                            ClearLine(11);
                            Console.WriteLine("Move or take action between players? (1 - move / 2 - take action) ");
                            Console.CursorVisible = true;
                            ConsoleKeyInfo decyzja = Console.ReadKey(true);

                            if (decyzja.Key == ConsoleKey.D1)
                            {
                                WhatsOnFlor(p, map, logLine, playerList, m, liczba, stillPlaying);


                            }
                            else if (decyzja.Key == ConsoleKey.D2)
                            {
                                //taking action

                                WhatAction(playerList, p);

                            }
                            else
                            {
                                ClearLine(11);
                                Console.Write(p.name + " wasted his turn");
                                ClearLine(12);
                            }
                        }
                    }
                    else
                    {
                        ClearLine(10);
                    }
                }
            }

            


            Console.SetCursorPosition(0, Console.BufferHeight - 5);
        }

        static void WhatsOnFlor(Player p, List<List<int>> map, int logLine, List<Player> playerList, Monster m, int liczba, bool stillPlaying)
        {
            Console.CursorVisible = false;

            ClearLine(11);
            Console.Write("W A S D to move....");
            ClearLine(12);

            int oldX = p.x, oldy = p.y;

            p.Movment(map);


            Console.SetCursorPosition(oldX, oldy);
            int cell = map[oldX][oldy];
            string cellView = GetCellView(cell);
            Console.Write(cellView);

            Console.SetCursorPosition(p.x, p.y);
            if (map[p.x][p.y] == 2)
            {
                Console.Write("+");
                p.hp = 0;
                ClearLine(10);
                Console.WriteLine(p);
                AllPlayers(playerList);
                Console.SetCursorPosition(20, logLine);
                logLine++;
                Console.Write(p.name + " step on the hole in the floor and fell into the abyss");
                logLine++;
                p.alive = false;

            }
            else if (map[p.x][p.y] == 3)
            {
                Console.Write(p.avatar);

                logLine = Combat(map, p, m, stillPlaying, liczba, playerList, logLine);
                Console.CursorVisible = false;
            }
            else if (map[p.x][p.y] == 5)
            {
                map[p.x][p.y] = 1;
                Console.Write(p.avatar);
                UnlockDoor(liczba);
                Console.CursorVisible = false;
                map[p.x][p.y - 1] = 1;
                cell = map[p.x][p.y - 1];
                cellView = GetCellView(cell);
                Console.SetCursorPosition(p.x, p.y - 1);
                Console.Write(cellView);
            }
            else if (map[p.x][p.y] == 6)
            {
                Console.SetCursorPosition(20, 7);
                Console.Write(p + " picked dangeon heart and obtained " + (p.points + 150) + " points");
                p.points += 150;
                logLine++;
                AllPlayers(playerList);
                stillPlaying=false;
            }
            else { Console.Write(p.avatar); }
        }

        private static void WhatAction(List<Player> playerList, Player p)
        {
            ClearLine(11);
            Console.WriteLine(p.name + ", what do you want to do? (atack/heal)");
            Console.CursorVisible = true;
            string action = Console.ReadLine();
            if (action == "atack" || action == "a")
            {
                ClearLine(11);
                Console.Write("Who do you want to atack? (player number)");
                ClearLine(12);
                int target = int.Parse(Console.ReadLine()) - 1;

                p.Atack(playerList[target]);
                AllPlayers(playerList);

            }
            else if (action == "heal" || action == "h")
            {
                ClearLine(11);
                Console.Write("Who do you want to heal? (player number)");
                ClearLine(12);
                int target = int.Parse(Console.ReadLine()) - 1;

                p.Heal(playerList[target]);
                AllPlayers(playerList);
            }
            else
            {
                ClearLine(11);
                Console.Write(p.name + " wasted his turn");
                ClearLine(12);
            }
        }

        static void AllPlayers(List<Player> playerList)
        {
            Console.SetCursorPosition(20, 0);
            foreach (Player p in playerList)
            {
                Console.Write(p + " | ");
            }
        }

        static void UnlockDoor(int liczba)
        {
            Console.CursorVisible = true;
            do
            {

                ClearLine(11);
                Console.WriteLine("To open door to treasure chest tray guessing randomly generated number in range 0 - 10");
                int zgadywana = int.Parse(Console.ReadLine());

                if (zgadywana == liczba)
                {
                    ClearLine(12);
                    ClearLine(11);
                    Console.WriteLine("WoW!!! You menadged to do it, congrats! Grab star to end game");
                    break;
                }
                else
                {
                    ClearLine(12);
                }

            } while (true);

        }

        static Player CreatePlayer(string text, string avatar)
        {
            Console.Write("Name yourself " + text + ": ");
            Player player = new Player();
            player.name = Console.ReadLine();
            if (string.IsNullOrEmpty(player.name))
            {
                player.name = text;
                player.avatar = avatar;
            }
            else
            {
                player.avatar = player.name.Substring(0, 1);
            }



            return player;
        }

        static Monster CreateMonster()
        {
            Monster monster = new Monster()
            {
                name = "Boss Monster",
                hp = 80,
                atk = 10,
                avatar = "B"
            };

            return monster;
        }

        static void DrawMap(List<List<int>> map)
        {
            for (int x = 0; x < map.Count; x++)
            {
                for (int y = 0; y < map[x].Count; y++)
                {
                    Console.SetCursorPosition(x, y);
                    int cell = map[x][y];
                    string cellView = GetCellView(cell);

                    Console.Write(cellView);
                }
            }
        }

        static string GetCellView(int cell)
        {
            string cellView = "!";

            if (cell == 0)
            {
                cellView = "#";
            }
            else if (cell == 1 || cell == 3 || cell == 5)
            {
                cellView = ".";
            }
            else if (cell == 2)
            {
                cellView = " ";
            }
            else if (cell == 4)
            {
                cellView = "$";
            }
            else if (cell == 6)
            {
                cellView = "*";
            }

            return cellView;
        }

        static void ClearLine(int linia)
        {
            Console.SetCursorPosition(0, linia);
            for (int i = 0; i < Console.BufferWidth - 1; i++)
            {
                Console.Write(" ");
            }
            Console.SetCursorPosition(0, linia);
        }
        static void PressToContinue(int line)
        {
            Console.CursorVisible = false;
            ClearLine(line);
            Console.Write("[...]");
            Console.ReadKey();
        }

        static int Combat(List<List<int>> map, Player p, Monster m, bool stillplay, int liczba, List<Player> playerList, int logLine)
        {
            bool alive = true;

            ClearLine(10);
            Console.Write(p + " vs " + m);
            Console.SetCursorPosition(20, logLine);
            Console.Write(p.name + " start battle with " + m.name);
            logLine++;
            ClearLine(11);
            Console.Write("Press any key to continue...");
            Console.ReadKey();

            do
            {
                Random rng = new Random();
                int result = rng.Next(2);
                string coinResult = "";

                ClearLine(13);
                ClearLine(12);
                ClearLine(11);

                Console.CursorVisible = true;
                Console.Write("head or tail? ");
                string coin = Console.ReadLine();

                coin = coin.ToLower();

                if (result == 0)
                {
                    coinResult = "head";
                    // ZerujCursor(11);
                    Console.Write("head");
                }
                else if (result == 1)
                {
                    coinResult = "tail";
                    //ZerujCursor(11);
                    Console.Write("tail");
                }

                static int AtackValue()
                {
                    Random rng = new Random();
                    int percent = rng.Next(10);
                    int AtackWalue;

                    if(percent<3)
                    {
                        AtackWalue = 0;
                    }
                    else
                    {
                        percent = rng.Next(10);
                        if(percent<3)
                        {
                            AtackWalue = 2;
                        }
                        else
                        {
                            AtackWalue = 1;
                        }
                    }

                    return AtackWalue;
                }

                if (coin == coinResult)
                {
                    int value = AtackValue();
                    ClearLine(10);
                    Console.Write(p + " vs " + m);

                    if (value==0)
                    {
                        ClearLine(12);
                        Console.Write(p.name + " atack mised " + m.name);

                        PressToContinue(13);
                    }
                    else
                    {
                        ClearLine(12);
                        Console.Write(p.name + " atack " + m.name+" for ["+ (value * p.finalAtk) +"]");

                        m.hp =m.hp - (value * p.finalAtk);

                        PressToContinue(13);
                    }

                    if (m.hp <= 0)
                    {
                        ClearLine(13);
                        ClearLine(12);
                        ClearLine(11);
                        ClearLine(10);
                        Console.SetCursorPosition(20, logLine);
                        Console.Write(m.name + " has been defeated");
                        logLine++;
                        Console.SetCursorPosition(20, logLine);
                        Console.Write(p + " get " + (p.points + 150) + " points");
                        logLine++;
                        p.points += 150;
                        m.SetCombatArea(map, 1, 1);
                        Console.SetCursorPosition(m.x, m.y);
                        Console.Write(liczba);
                        alive = false;
                    }
                }
                else
                {
                    ClearLine(10);
                    Console.Write(p + " vs " + m);

                    int value = AtackValue();

                    if(value==0)
                    {
                        ClearLine(12);
                        Console.Write(m.name + " atack mised " + p.name);

                        PressToContinue(13);
                    }
                    else
                    {
                        ClearLine(12);
                        Console.Write(m.name + " atack " + p.name+" for ["+ (value * p.finalAtk) +"]");

                        p.hp = p.hp - (value * m.atk);
                        PressToContinue(13);
                    }
                    
                    if (p.hp <= 0)
                    {
                        ClearLine(13);
                        ClearLine(12);
                        ClearLine(11);
                        ClearLine(10);
                        Console.SetCursorPosition(p.x, p.y);
                        Console.Write("+");
                        ClearLine(10);
                        Console.WriteLine(p);
                        AllPlayers(playerList);
                        Console.SetCursorPosition(20, logLine);
                        Console.Write(p.name + " was crushed to a bloody pulp by " + m.name);
                        logLine++;
                        p.alive = false;
                        alive = false;

                    }
                }
            } while (alive);

            Console.CursorVisible = true;
            return logLine;
        }
    }

    class Player
    {
        public string name = "CreativeName", avatar = "@";
        public int hp = 100, maxHp = 100, points = 0;
        public int baseAtk = 10, finalAtk = 10, healPover = 10;
        public int x = 5, y = 1, z = 0;
        int step = 1;
        public bool alive = true;

        public override string ToString()
        {
            return name + " " + "[" + hp + "/" + maxHp + "HP][" + points + " ptk]";
        }

        public void Movment(List<List<int>> map)
        {


            ConsoleKeyInfo moveKeyInfo = Console.ReadKey(true);

            if (moveKeyInfo.Key == ConsoleKey.W)
            {
                if (map[x][y - step] != 0)
                {
                    y -= step;
                }
                else
                {
                    ObstacleOnWay();
                }
            }
            else if (moveKeyInfo.Key == ConsoleKey.A)
            {
                if (map[x - step][y] != 0)
                {
                    x -= step;
                }
                else
                {
                    ObstacleOnWay();
                }
            }
            else if (moveKeyInfo.Key == ConsoleKey.S)
            {
                if (map[x][y + step] != 0)
                {
                    y += step;
                }
                else
                {
                    ObstacleOnWay();
                }
            }
            else if (moveKeyInfo.Key == ConsoleKey.D)
            {
                if (map[x + step][y] != 0)
                {
                    x += step;
                }
                else
                {
                    ObstacleOnWay();
                }
            }

        }

        static void ObstacleOnWay()
        {
            Console.SetCursorPosition(0, 12);
            Console.Write("There is somthing in your way");
            Task.Delay(500).Wait();
            ClearLine(12);
        }

        static void ClearLine(int linia)
        {
            Console.SetCursorPosition(0, linia);
            for (int i = 0; i < Console.BufferWidth - 1; i++)
            {
                Console.Write(" ");
            }
            Console.SetCursorPosition(0, linia);
        }

        public void Atack(Player target)
        {
            if (target.alive == false)
            {
                ClearLine(11);
                Console.Write(target.name + " is alredy dead, " + name + " It's not proper to kick a dead man");

                PressToContinue();
            }
            else
            {
                Random rng = new Random();
                int percent = rng.Next(10);

                if (percent > 6)
                {
                    ClearLine(11);
                    Console.Write(name + " mised "+target.name+" by hair's breadth");

                    PressToContinue();
                }
                else
                {
                    percent = rng.Next(10);

                    if (percent == 3)
                    {
                        ClearLine(11);
                        Console.Write(name + " menaged to atack " + target.name+"'s vital point and deal masive damage ["+(6*finalAtk)+" damage]");
                        target.hp = target.hp - 6*finalAtk;

                        PressToContinue();
                    }
                    else if(percent < 3)
                    {
                        ClearLine(11);
                        Console.Write(name + " atacked " + target.name+ " with doubled strang [" + (2 * finalAtk) + " damage]");
                        target.hp = target.hp - 2*finalAtk;

                        PressToContinue();
                    }
                    else
                    {
                        ClearLine(11);
                        Console.Write(name + " atack " + target.name+ " [" + (finalAtk) + " damage]");
                        target.hp = target.hp - finalAtk;

                        PressToContinue();
                    }
                        
                }
            }
        }

        public void Heal(Player target)
        {
            if (target.hp == target.maxHp)
            {
                ClearLine(11);
                Console.Write(target.name + " is completely healthy alredy, " + name + " you wasted turn");

                PressToContinue();
            }
            else
            {
                Random rng = new Random();
                int percent = rng.Next(10);

                if (percent > 6)
                {
                    ClearLine(11);
                    Console.Write(name + " mesed healing spell");

                    PressToContinue();
                }
                else
                {
                    percent = rng.Next(10);

                    if (percent > 3)
                    {
                        ClearLine(11);
                        Console.Write(name + " is healing " + target.name+" [+"+healPover+" Hp]");
                        target.hp = target.hp + healPover;

                        PressToContinue();
                    }
                    else 
                    {
                        ClearLine(11);
                        Console.Write(name + " fell magic inspiration and healing " + target.name + " with doubled power [+" + (2*healPover) + " Hp]");
                        if (target.hp + 2*healPover >= target.maxHp)
                        {
                            target.hp = target.maxHp;
                        }
                        else
                        {
                            target.hp = target.hp + (2*healPover);
                        }

                        PressToContinue();
                    }
                }
            }
        }

        static void PressToContinue()
        {
            ClearLine(12);
            Console.Write("[...]");
            Console.ReadKey();
        }
    }

    class Monster
    {
        public string name = "Dummy", avatar = "M";
        public int hp = 60, atk = 10, x = 5, y = 7;

        public override string ToString()
        {
            return name + " " + " [" + hp + "HP]";
        }

        public void SetCombatArea(List<List<int>> map, int monster, int area)
        {
            map[x][y] = monster;

            map[x + 1][y + 1] = area;
            map[x - 1][y - 1] = area;
            map[x - 1][y + 1] = area;
            map[x + 1][y - 1] = area;
            map[x][y + 1] = area;
            map[x][y - 1] = area;
            map[x + 1][y] = area;
            map[x - 1][y] = area;
        }


    }
}
