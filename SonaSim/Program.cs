using System.Text.RegularExpressions;
class program
{
    // quick method definitions
    static public Dictionary<string, Dictionary<string, string>> characterProperties = new Dictionary<string, Dictionary<string, string>>();
    static public Dictionary<string, string> storyProperties = new Dictionary<string, string>();
    static public Dictionary<string, string> playerProperties = new Dictionary<string, string>();
    static public void resetConsoleColor() { Console.ForegroundColor = ConsoleColor.White; }
    static public void diagFormatter(string input = null, MatchCollection matchCollection = null, string playerName = null, Array sourceFile = null)
    {
        if (sourceFile != null)
        {
            try
            {
                string working = System.Convert.ToString(sourceFile.GetValue(sourceFile.Length - 1));
                Regex characterParser = new Regex(@"(?<=\{)(.*?)(?=\})", RegexOptions.Compiled | RegexOptions.Multiline);
                MatchCollection characters = characterParser.Matches(working);
                foreach (Match character in characters)
                {
                    string[] working2 = character.Value.ToString().Split(":");
                    characterProperties.Add(working2[0].ToString().ToLower(), new Dictionary<string, string> { 
                        { "Name", working2[1].ToString() }, 
                        { "Color", working2[2].ToString() }, 
                        { "Pro1", working2[3].ToString() }, 
                        { "Pro2", working2[4].ToString() }, 
                        { "Pro3", working2[5].ToString() } }); ;
                }
                Regex infoParser = new Regex(@"(?<=\?)(.*?)(?=\?)", RegexOptions.Compiled | RegexOptions.Multiline);
                MatchCollection info = infoParser.Matches(working);
                string[] working3 = info[0].Value.ToString().Split(",");
                foreach (string item in working3)
                {
                    string[] working4 = item.Split(":");
                    storyProperties.Add(working4[0].ToLower(), working4[1]);
                }
                return;
            } catch
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("That file is not correctly formatted.\nShutting Down...");
                System.Environment.Exit(3);
            }
        }
        input = input.Replace(storyProperties["name"], playerName);
        foreach (Match match in matchCollection)
        {
            if (match.Value.GetType() == typeof(string))
            {
                try
                {
                    string color = characterProperties[match.Value.ToLower()]["Color"];
                    string nameReplacement = characterProperties[match.Value.ToLower()]["Name"];
                    input = input.Replace(storyProperties["cpronoun1"].ToString(), characterProperties[match.Value.ToLower()]["Pro1"].ToString());
                    input = input.Replace(storyProperties["cpronoun2"].ToString(), characterProperties[match.Value.ToLower()]["Pro2"].ToString());
                    input = input.Replace(storyProperties["cpronoun3"].ToString(), characterProperties[match.Value.ToLower()]["Pro3"].ToString());
                    input = input.Replace(storyProperties["ppronoun1"].ToString(), playerProperties["Pro1"].ToString());
                    input = input.Replace(storyProperties["ppronoun2"].ToString(), playerProperties["Pro2"].ToString());
                    input = input.Replace(storyProperties["ppronoun3"].ToString(), playerProperties["Pro3"].ToString());
                    Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color, true);
                    Console.WriteLine($"{nameReplacement}: {input}");
                    resetConsoleColor();
                    return;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{input}");
                    resetConsoleColor();
                    return;
                }
            }
        }
    }
    // end quick method definitons
    static void Main(string[] args)
    {
        string name = "y/n";
        var breadcrumbs = new List<int>();
        int nextDiagIndex = 0;
        bool gameActive = true;
        string sourceFile = $@"";
        if (args.Length == 0)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"To begin, please provide the path of your story file (.txt/.story): ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            sourceFile = @$"{Console.ReadLine()}";
            resetConsoleColor();
        }
        else { sourceFile = @$"args[1]"; }
        if (File.Exists(sourceFile))
        {
            string[] lines = File.ReadAllLines(sourceFile);
            diagFormatter(sourceFile: lines);
            Console.WriteLine($"───────────────────────\n" +
                $"File Sucessfully Loaded" +
                $"\n───────────────────────\n" +
                $"{storyProperties["title"]}\n" +
                $"By: {storyProperties["author"]}" +
                $"\n───────────────────────\n" +
                $"Instructions:\n" +
                $"If a choice is provided to you, respond with the number in its brackets to select it.\n" +
                $"Press [ENTER] to advance dialouge\n" +
                $"───────────────────────\n" +
                $"Press [ENTER] to begin...");
            Console.ReadKey();
            bool playerSetupComplete = false;
            string[] pronouns = { "they", "them", "theirs" };
            while (playerSetupComplete == false){
                Console.Clear();
                Console.WriteLine("What is your name?\nName >");
                Console.SetCursorPosition(7, Console.CursorTop - 1);
                name = Console.ReadLine();
                Console.WriteLine($"Hello {name}! Now, what pronouns do you go by? You may provide up to three like this:\nSubject Pronoun/Object Pronoun/Posessive Pronoun\nPronouns >");
                Console.SetCursorPosition(11, Console.CursorTop - 1);
                pronouns = Console.ReadLine().Split("/");
                Console.Clear();
                if (name == "") { name = "y/n"; };
                Console.WriteLine($"Great! Does this information look correct to you?\nName: {name}\nPronouns: {String.Join("/", pronouns)}\nSubject Pronoun: {pronouns[0]}\nObject Pronoun: {pronouns[1]}\nPossesive Pronoun: {pronouns[2]}\n(Y/n) >");
                Console.SetCursorPosition(8, Console.CursorTop - 1);
                if (Console.ReadLine().ToLower() == "y") { playerSetupComplete = true; }
                else if (Console.ReadLine().ToLower() == "n") { continue; }
            }
            playerProperties.Add("Name", name);
            playerProperties.Add("Pro1", pronouns[0]);
            playerProperties.Add("Pro2", pronouns[1]);
            playerProperties.Add("Pro3", pronouns[2]);
            Console.Clear();
            Regex rx = new Regex(@"(?<=\[)(.*?)(?=\])", RegexOptions.Compiled | RegexOptions.Multiline);
            Regex choices = new Regex(@"(?<=;=)(.*?)(?=;;)", RegexOptions.Compiled | RegexOptions.Multiline);
            while (gameActive)
            {
                breadcrumbs.Add(nextDiagIndex);
                string current = lines[nextDiagIndex];
                if (current.StartsWith("!!&&CLEAR&&!!"))
                {
                    Console.Clear();
                    current = current.Replace("!!&&CLEAR&&!!", "");
                }
                MatchCollection choiceParsed = choices.Matches(current);
                if (choiceParsed.Count > 0)
                {
                    MatchCollection actions = rx.Matches(current);
                    var dest = new Dictionary<int, int>();
                    int destIndex = 0;
                    foreach (Match i in actions)
                    {
                        try
                        {
                            destIndex++;
                            dest.Add(destIndex, System.Convert.ToInt32(i.Value));
                        }
                        catch (FormatException)
                        {
                            destIndex--;
                            continue;
                        }

                    }
                    destIndex = 0;
                    string display = Regex.Replace(current, @"\[.*?\]", "", RegexOptions.Compiled | RegexOptions.Multiline).Trim();
                    display = Regex.Replace(display, @";=.*?;;", "", RegexOptions.Compiled | RegexOptions.Multiline).Trim();
                    diagFormatter(display, actions, name);
                    string displayChoices = "───────────────────────";
                    foreach (Match match in choiceParsed)
                    {
                        destIndex++;
                        displayChoices += $"\n[{destIndex}] {Regex.Replace(match.Value, @"\[.*?\]", "", RegexOptions.Compiled | RegexOptions.Multiline).Trim()}";
                    }
                    Console.WriteLine(displayChoices + "\nChoose >");
                    try
                    {
                        Console.SetCursorPosition(9, Console.CursorTop - 1);
                        nextDiagIndex = dest[int.Parse(Console.ReadLine())];

                    }
                    catch (KeyNotFoundException)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("That is not a valid choice. Please Select again:");
                        resetConsoleColor();

                        nextDiagIndex = dest[int.Parse(Console.ReadLine())];
                    }
                }
                else
                {
                    MatchCollection actions = rx.Matches(current);
                    nextDiagIndex = int.Parse(actions[1].Value);
                    string display = Regex.Replace(current, @"\[.*?\]", "", RegexOptions.Compiled | RegexOptions.Multiline).Trim();
                    diagFormatter(display, actions, name);
                    Console.ReadKey();
                }
                if (nextDiagIndex == -1)
                {
                    gameActive = false;
                }
            }
            Console.WriteLine($"Story Completed! Thanks For Playing {name}!!\nYour Path: [ {String.Join(" > ", breadcrumbs.ToArray())} ]");
        }
    }
}