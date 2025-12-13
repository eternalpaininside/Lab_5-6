using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Lab_5_6
{
    internal class Var1
    {
        #region ввод чисел и интерфейс
        static Random random = new Random();

        static void PrintMenu(string[] menuText)
        {
            for (int i = 0; i < menuText.Length; i++)
            {
                Console.WriteLine( $"{i + 1}) {menuText[i]}");
            }
        }

        
        static void WriteCenterMessage(string message)
        {
            string borders = new string('=', message.Length + 2);
            string result = $@"{borders}
 {message}
{borders}";
            ColoringMessage(result, 10);
            Console.WriteLine();
        }

        static int ReadInt(string[] message, int min = int.MinValue, int max = int.MaxValue)
        {
            bool isConvert;
            int n;
            do
            {
                Console.Write(message[0]);
                Console.ForegroundColor = ConsoleColor.Green;
                isConvert = int.TryParse(Console.ReadLine(), out n);
                if (!isConvert)
                {
                    ColoringMessage(message[1], 4);
                    Console.WriteLine();
                }
                else if (n < min || n > max)
                {
                    ColoringMessage(message[2], 12);
                    Console.WriteLine();
                }
            } while (!isConvert || n < min || n > max);
            Console.ForegroundColor = ConsoleColor.White;
            return n;
        }

        static void CreateBordersRandoming(out int start, out int end)
        {
            string[] messageStart = {"Введите нижнюю границу генерирования чисел: ", "То что вы ввели не похоже на целое число! ", "Данное число слишком большое/малое! "};
            string[] messageEnd = {"Введите верхнюю границу генерирования чисел: ", "То что вы ввели не похоже на целое число! ", "Данное число слишком большое/малое! "};
            start = ReadInt(messageStart, -1000000, 1000000);
            end = ReadInt(messageEnd);
            if (start > end)
            {
                Console.WriteLine("Вверхняя граница станет нижней, а нижняя верхней.");
                (start, end) = (end, start);
            }
        }

        static void ColoringConsole(int colorBackground, int colorForeground)
        {
            Console.ForegroundColor = (ConsoleColor)colorForeground;
            Console.BackgroundColor = (ConsoleColor)colorBackground;
        }

        static void ColoringMessage(int number, int color)
        {
            Console.ForegroundColor = (ConsoleColor)color;
            Console.Write(number + " ");
            ColoringConsole(1, 15);
        }

        static void ColoringMessage(string message, int color)
        {
            Console.ForegroundColor = (ConsoleColor)color;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void PrintNumberColumn(int[,] matr)
        {
            int lengthmatr = matr.GetLength(0);
            Console.Write(new string(' ', 12) + "|");
            for (int i = 0; i < lengthmatr; i++)
            {
                ColoringMessage($"{i + 1} столбец |", 14);
            }
            Console.WriteLine();
        }
        
        static void PrintNumberColumn(int[][] ragArr)
        {
            Console.Write(new string(' ', 9) + '|');
            int longestLine = FindLongestLine(ragArr);
            for (int i = 0; i < ragArr[longestLine].Length; i++)
            {
                Console.Write($" {i + 1} столбец|");
            }
            Console.WriteLine();
        }
        #endregion

        #region Работа с двумерными массивами
        static void CreateSizeMatrix(out int width, out int height)
        {
            string[] messages1 = { "Введите ширину матрицы: ", "Ширина не может быть такой! ", "Данное число некорректно для ширины! " };
            string[] messages2 = { "Введите высоту матрицы: ", "Высота не может быть такой! ", "Данное число некорректно для высоты! " };
            width = ReadInt(messages1, 1, 10);
            height = ReadInt(messages2, 1, 256);
        }

        static void CreateArtificialMatrix(out int[,] matr)
        {
            string[] messages = { "", "То что вы ввели не похоже на целое число! ", "Данное число слишком большое/малое! " };
            CreateSizeMatrix(out int width, out int height);
            matr = new int[width, height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    ColoringMessage($"Вводите число в {i + 1}-ой строке в {j + 1}-ом столбце: ", 15);
                    matr[j, i] = ReadInt(messages, -1000000, 1000000);
                }
            }
        }

        static void CreateRandomMatrix(out int[,] matr)
        {
            CreateBordersRandoming(out int start, out int end);
            CreateSizeMatrix(out int width, out int height);
            matr = new int[width, height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    matr[j, i] = random.Next(start, end);
                }
            }
        }

        static void PrintMatrix(int[,] matr)
        {
            PrintNumberColumn(matr);
            int numberLine = 1;
            for (int i = 0; i < matr.GetLength(1); i++)
            {
                for (int j = 0; j < matr.GetLength(0); j++)
                {
                    string mes = $"{matr[j, i],8}";
                    if (j == 0)
                        Console.Write($"{numberLine}-ая строка | ");
                    ColoringMessage(mes, 14);
                    ColoringMessage(" | ", 14);
                }
                numberLine++;
                Console.WriteLine();
            }
        }

        static void InputLineInMatrix(ref int[,] matr, int lineNumber)
        {

            int width = matr.GetLength(0);
            int height = matr.GetLength(1);
            int[,] temp = new int[width, height + 1];
            int k = 0;
            for (int i = 0; i <= height; i++)
            {
                for (int j = 0;j < width; j++)
                {
                    if (i == lineNumber - 1)
                    {
                        string[] mes = { $"Введите {j + 1}-ый элемент: ", "Ошибка ввода элемента!", "Число слишком большое/маленькое" };
                        temp[j,lineNumber-1] = ReadInt(mes);
                    }
                    else
                    {
                        (temp[j, i], matr[j, k]) = (matr[j, k], temp[j, i]);
                    }
                }
                if (i != lineNumber - 1)
                    k++;
            }
            matr = temp;
        }

        static void InputRandomLineInMatrix(ref int[,] matr, int lineNumber)
        {

            int width = matr.GetLength(0);
            int height = matr.GetLength(1);
            int[,] temp = new int[width, height + 1];
            int k = 0;
            CreateBordersRandoming(out int start, out int end);
            for (int i = 0; i <= height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == lineNumber - 1)
                    {
                        temp[j, lineNumber - 1] = random.Next(start,end);
                    }
                    else
                    {
                        (temp[j, i], matr[j, k]) = (matr[j, k], temp[j, i]);
                    }
                }
                if (i != lineNumber - 1)
                    k++;
            }
            matr = temp;
        }
        #endregion

        #region Работа с рванными массивами
        static int[][] CreateSizeRaggedArray(out int height)
        {
            string[] messagesHeight = { "Введите количество строк: ", "Ошибка ввода целого числа!", "Данное число некорректно для высоты!" };
            height = ReadInt(messagesHeight, 1, 256);
            int[][] ragArr = new int[height][];   
            for (int i = 0; i < height; i++)
            {
                string[] messagesWidth = { $"Введите количество чисел в {i + 1}-ой строке: ", "Ошибка ввода целого числа!", "Данное число некорректно для количества чисел!" };
                int width = ReadInt(messagesWidth, 1, 10);
                ragArr[i] = new int[width];
            }
            return ragArr; 
        }

        static int[][] CreateArtificialRaggedArray()
        {
            int n = 1;
            string[] messages = {"", "Ошибка ввода целого числа!", "Данное число слишком большое/маленькое" };
            int[][] ragArr = CreateSizeRaggedArray(out int height);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < ragArr[i].Length; j++)
                {   
                    ColoringMessage($"Введите число в {i + 1}-ой строке в {j + 1}-ом столбце: ", 15);
                    ragArr[i][j] = ReadInt(messages, -1000000, 1000000);
                }
                n++;
            }
            return ragArr;
        }

        static int[][] CreateRandomRaggedArray()
        {
            int[][] ragArr = CreateSizeRaggedArray(out int height);
            CreateBordersRandoming(out int start, out int end);
            for (int i = 0;i < height; i++)
            {
                for (int j = 0; j < ragArr[i].Length; j++)
                {
                    ragArr[i][j] = random.Next(start, end);
                }
            }
            return ragArr;
        }

        static void PrintRaggedArray(int[][] ragArr)
        {
            PrintNumberColumn(ragArr);
            int numberLine = 1;
            for (int i = 0;i < ragArr.GetLength(0); i++)
            {
                for (int j = 0; j < ragArr[i].Length; j++)
                {
                    string mes = $"{ragArr[i][j], 8}";
                    if (j == 0)
                        Console.Write($"{numberLine} строка | ");
                    ColoringMessage(mes, 14);
                    ColoringMessage(" | ", 14);
                }
                numberLine++;
                Console.WriteLine();
            }
        }

        static int FindLongestLine(int[][] ragArr)
        {
            int longestLine = 0;
            int maxLong = int.MinValue;
            for (int i = 0; i < ragArr.GetLength(0); i++)
            {
                if (ragArr[i].Length > maxLong)
                {
                    maxLong = ragArr[i].Length;
                    longestLine = i;
                }
            }
            return longestLine;
        }

        static void CopyingRagArray(ref int[][] ragArr, ref int[][] temp, int numberLine)
        {
            int k = 0;
            for (int i = 0; i < ragArr.GetLength(0); i++)
            {
                for (int j = 0; j < ragArr[i].Length; j++)
                {
                    if (i == numberLine)
                        break;
                    else
                        temp[k][j] = ragArr[i][j];
                }
                if (i != numberLine)
                    k++;
            }
            ragArr = temp;
        }

        static void DeleteLongestLine(ref int[][] ragArr)
        {
            int[][] temp = new int[ragArr.GetLength(0) - 1][];
            int longestLineNumber = FindLongestLine(ragArr);
            int k = 0;
            for (int i = 0; i < ragArr.GetLength(0); i++)
            {
                for (int j = 0;j < ragArr[i].Length; j++)
                {
                    if (i == longestLineNumber)
                        break;
                    else
                    {
                        temp[k] = new int[ragArr[i].Length];
                    }
                }
                if (i != longestLineNumber)
                    k++;
            }
            CopyingRagArray(ref ragArr, ref temp, longestLineNumber);
        }
        #endregion

        #region Работа со строками
        static string WriteString()
        {
            Console.Write("Вводите вашу строку: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string str = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            string pattern = @"\s+";
            string target = "";
            Regex regex = new Regex(pattern);
            string test = regex.Replace(str, target);
            if (test.Length == 0)
            {
                return "";
            }
            return str;
        }

        static string CreateRandomString()
        {
            string[] selection = { "Я памятник себе воздвиг   нерукотворный, К нему не зарастет народная тропа, " +
                    "Вознесся выше он главою непокорной Александрийского столпа.. Нет, весь я не умру — душа в заветной лире Мой прах переживет и тленья убежит — " +
                    "И славен буду я, доколь в подлунном мире Жив будет хоть один пиит. Слух обо мне пройдет по всей Руси великой, И назовет меня всяк сущий в ней язык," +
                    " И гордый внук славян, и финн, и  ныне дикой Тунгус,  и друг степей калмык. И долго буду тем любезен я народу, Что чувства добрые я лирой пробуждал," +
                    "Что в мой жестокий век восславил?> я Свободу И милость к падшим призывал. Веленью божию, о муза, будь послушна, Обиды не страшась, не требуя венца, Хвалу и клевету приемли равнодушно И не оспоривай глупца. ",
            "Старый маяк стоял на скале, вечно глядя в морскую даль. Каждую ночь он  зажигал  свой луч, указывая путь заблудшим кораблям. Иногда казалось, что он тоскует по давно ушедшим морякам. Его свет был единственным другом в бескрайней тьме.",
            "Пианино в углу комнаты было покрыто пылью.!.. Много лет назад  его  клавиши рождали прекрасные мелодии. Однажды, когда прозвучала старая запись, инструмент вдруг тихо вздохнул. Казалось, он вспомнил свою забытую песню.",
            "Садовник всю жизнь выращивал цветы,,, но мечтал о звёздах. Он посадил особые семена, купленные у странного торговца. Ночью из земли проросли стебли, усыпанные светящимися бутонами. Утром над его домом сиял собственный маленький космос.",
            "Блестящая монетка выпала из  кармана и покатилась по улице. Её подхватил уличный музыкант, потом она купила хлеб. Затем её уронил ребёнок в фонтан, где она покоилась, мечтая о новых приключениях. Её путь был полон случайных встреч.",
            "Старик дрожащей рукой наносил последний мазок. Его картина изображала мир,  который он знал, но который исчезал. Каждый цвет был пропитан ностальгией и грустью. Когда холст был закончен, мир за окном изменился навсегда.",
            "Попугай Кеша жил у бабушки и повторял  ,.странные фразы. 'Где мой  ключ?' — кричал он, когда никого не было дома. Однажды, во время уборки, бабушка нашла старинный сундук. Ключ от него лежал на дне клетки Кеши.",
            "Девочка нашла старый, потрёпанный   зонт ?на чердаке. Когда она открыла его, вокруг закружились'! радужные бабочки. Он защищал не только от дождя, но и от скуки. Под этим зонтом каждый день становился приключением.",
            "За книжной  полкой в старинной библиотеке ''скрывалась маленькая дверца. Никто не знал, куда она ведёт. Один смельчак отважился открыть её и попал в мир, где книги разговаривали. Это было начало его великого исследования.",
            "Старый сервисный робот всю ночь видел сны о @#зелёных полях. Он мечтал почувствовать траву под своими!. металлическими ступнями. Утром он решился покинуть городскую суету. В его программе не было такого пункта, но он всё равно пошёл.",
            "Внутри старинной шкатулки лежал   маленький, пыльный свёрток. Когда его развернули,! там оказался рисунок<! ребёнка. Это был давно забытый подарок, сделанный с большой любовью. Он принёс тепло и улыбки спустя многие годы.",
            "В центре   города открылась кофейня, где часы ?!шли назад. Каждый, кто пил там кофе, чувствовал себя моложе. Но время не обманешь полностью. Старики вспоминали юность, а молодые люди не спешили взрослеть.",
            "Тень от старой груши танцевала на стене дома. В лунном свете она принимала причудливые формы. Дети придумывали истории про её приключения. Для них она была живым существом, каждую ночь приносившим сказки.",
            "Осень почти  закончилась, но один  листочек упрямо!. держался!, на ветке. Он был золотым и слегка подрагивал от ветра. Зима приближалась, но он не сдавался. Его стойкость вдохновляла всех, кто проходил мимо.",
            "На берегу нашли древний корабль, закопанный,, в песок.  На его борту не было никаких сокровищ, только странные символы. Учёные ломали голову над их значением. Корабль хранил свою тайну, унося её через века.",
            "Был город, где никто никогда не спешил. Люди гуляли медленно,  беседовали часами.  Казалось, время там останавливалось, чтобы каждый мог насладиться моментом. Жизнь текла спокойно и размеренно, без суеты и тревог.",
            "я видел пьяниц с мудрыми глазами. и падших женщин с ликом чистоты. я знаю сильных, что взыхлеб рыдали, и слабых, что несут кресты. не осуждай за то в чем неуверен. не говори, если решил солгать. не проверяй, если уже доверил. и не дари, планируя отнять."};
            return selection[random.Next(0, selection.Length-1)];
        }

        static void ReverseNotEvenSentences(ref string inputString)
        {
            inputString = CorrectingString(inputString);

            if (string.IsNullOrEmpty(inputString))
                return;

            char[] sentenceEndingDelimiters = { '.', '!', '?' };
            string resultString = "";
            int sentenceCount = 0;
            int lastEndIndex = 0;

            for (int i = 0; i < inputString.Length; i++)
            {
                char currentChar = inputString[i];

                if (currentChar == '.' || currentChar == '!' || currentChar == '?')
                {
                    if (i >= 1 && (inputString[i - 1] == '.' || inputString[i - 1] == '!' || inputString[i - 1] == '?'))
                    {
                        sentenceCount++;
                    }
                    sentenceCount++;
                    // Нашли конец предложения включая разделитель
                    string currentSentenceContent = inputString.Substring(lastEndIndex, i - lastEndIndex + 1);

                    
                    int nextCharIndex = i + 1;
                    while (nextCharIndex < inputString.Length && char.IsWhiteSpace(inputString[nextCharIndex]))
                    {
                        nextCharIndex++;
                    }
                    string trailingWhitespace = inputString.Substring(i + 1, nextCharIndex - (i + 1));

                    string sentenceText = currentSentenceContent.TrimEnd(sentenceEndingDelimiters).Trim(); // получаем только текст предложения
                    char delimiter = currentSentenceContent.TrimStart()[currentSentenceContent.Length - 1]; // получаем сам разделитель

                    if (sentenceCount % 2 != 0) 
                    {
                        string[] words = sentenceText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < words.Length; j++)
                        {
                            for (int k = 0; k < words[j].Length; k++)
                            {
                                if (words[j][k] == ',')
                                {
                                    ReverseComma(ref words[j]);
                                    words[j] = words[j].Substring(1, words[j].Length - 1);
                                }
                            }

                        }
                        Array.Reverse(words);
                        string reversedWords = string.Join(" ", words);
                        resultString += reversedWords;
                    }
                    else // Четное предложение
                    {
                        resultString += sentenceText;
                    }

                    resultString += delimiter; 
                    resultString += trailingWhitespace; 

                    lastEndIndex = nextCharIndex; 
                }
            }
           
            if (lastEndIndex < inputString.Length)
            {
                string remainingText = inputString.Substring(lastEndIndex).Trim();
                if (!string.IsNullOrEmpty(remainingText))
                {
                    sentenceCount++;
                    if (sentenceCount % 2 != 0)
                    {
                        string[] words = remainingText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        Array.Reverse(words);
                        string reversedWords = string.Join(" ", words);
                        resultString += reversedWords;
                    }
                    else
                    {
                        resultString += remainingText;
                    }
                }
            }
            resultString = CorrectingString(resultString);
            inputString = resultString;
        }

        static void ReverseComma(ref string str)
        {
            string temp = "";
            string[] strings = str.Split(',');
            char[] deleteEnd = { ' ' };
            for (int i = strings.Length - 1; i > -1; i--)
            {
                if (strings.Length == 0)
                    temp += ',';
                else
                {
                    temp = temp.TrimEnd(deleteEnd);
                    temp += ", " + strings[i];
                }
            }
            str = temp;
        }

        static string CorrectingString(string inputString)
        {
            inputString = ReplaceComma(inputString);
            inputString = ReplaceDot(inputString);
            inputString = ReplaceSpaces(inputString);
            inputString = ReplaceQuestions(inputString);
            inputString = ReplaceExclamation(inputString);
            inputString = DeleteSpaceBeforeComma(inputString);
            inputString = DeleteSpaceBeforeDot(inputString);
            inputString = DeleteSpaceBeforeQuestion(inputString);
            inputString = DeleteSpaceBeforeExclamation(inputString);
            return inputString;
        }

        static string ValidateText(string inputString)
        {
            inputString = CorrectingString(inputString);
            if (ContainsInvalidCharacters(inputString))
            {
                return "ОШИБКА: Строка содержит запрещенные символы.";
            }

            if (HasAdjacentPunctuation(inputString))
            {
                return "ОШИБКА: Знаки препинания стоят рядом друг с другом.";
            }

            if (!EndsCorrectly(inputString))
            {
                return "ОШИБКА: Предложение должно заканчиваться точкой, восклицательным или вопросительным знаком.";
            }

            if (!HasIncorrectSpacing(inputString))
            {
                return "ОШИБКА: Перед знаками препинания не должно быть пробелов.";
            }

            return "";
        }

        static bool ContainsInvalidCharacters(string inputString)
        {
            string invalidPattern = @"[^а-яёА-ЯЁa-zA-Z0-9\s.,!'?:;-]";
            return Regex.IsMatch(inputString, invalidPattern);
        }

        static bool HasAdjacentPunctuation(string inputString)
        {
            string adjacentPattern = @"[.,!?'-:;]{2,}";  // Два и более знака препинания подряд

            if (Regex.IsMatch(inputString, adjacentPattern))
            {
                if (!Regex.IsMatch(inputString, @"[!?][?!]") && !Regex.IsMatch(inputString, @"[.]{3}"))
                    return true;
            }
            return false;
        }

        static bool EndsCorrectly(string inputString)
        {
            string trimmed = inputString.TrimEnd();
            if (string.IsNullOrEmpty(trimmed))
                return false;

            return Regex.IsMatch(trimmed, @"[.!?]$");
        }

        private static bool HasIncorrectSpacing(string inputString)
        {
            string spaceBeforePunctuation = @"\s[.,!?:;]";
            return Regex.IsMatch(inputString, spaceBeforePunctuation);
        }

        static string ReplaceSpaces(string inputString)
        {
            string pattern = @"\s+";
            string target = " ";
            Regex regex = new Regex(pattern);
            string result = regex.Replace(inputString, target);
            return result;
        }

        static string ReplaceQuestions(string inputString)
        {
            string pattern = @"[?]+";
            string target = "?";
            Regex regex = new Regex(pattern);
            string result = regex.Replace(inputString, target);
            return result;
        }

        static string ReplaceExclamation(string inputString)
        {
            string pattern = @"[!]+";
            string target = "!";
            Regex regex = new Regex(pattern);
            string result = regex.Replace(inputString, target);
            return result;
        }

        static string ReplaceDot(string inputString)
        {
            string pattern = @"[.]+";
            string target = ".";
            Regex regex = new Regex(pattern);
            string result = regex.Replace(inputString, target);
            return result;
        }
        static string ReplaceComma(string inputString)
        {
            string pattern = @"[,]+";
            string target = ",";
            Regex regex = new Regex(pattern);
            string result = regex.Replace(inputString, target);
            return result;
        }

        static string DeleteSpaceBeforeComma(string inputString)
        {
            string[] puncts = { ",", ".", "!", "?" };
            string result = "";
            foreach (string item in puncts)
            {
                string pattern = $@"\s+[{item}]";
                string target = $"{item}";
                Regex regex = new Regex(pattern);
                result = regex.Replace(inputString, target);
            }
            return result;
        }
        static string DeleteSpaceBeforeDot(string inputString)
        {
            string pattern = @"\s+[.]";
            string target = ".";
            Regex regex = new Regex(pattern);
            string result = regex.Replace(inputString, target);
            return result;
        }

        static string DeleteSpaceBeforeQuestion(string inputString)
        {
            string pattern = @"\s+[?]";
            string target = "?";
            Regex regex = new Regex(pattern);
            string result = regex.Replace(inputString, target);
            return result;
        }

        static string DeleteSpaceBeforeExclamation(string inputString)
        {
            string pattern = @"\s+[!]";
            string target = "!";
            Regex regex = new Regex(pattern);
            string result = regex.Replace(inputString, target);
            return result;
        }
        #endregion

        static void Main(string[] args)
        {
            
            int choose;
            int[,] matrix = new int[0, 0];
            int[][] ragArray = new int[0][];
            string sentences = new string(' ',0);
            int choice = 0;
            do
            {
                ColoringConsole(1, 15);
                Console.Clear();
                bool isNullMatr = (matrix.Length == 0), isNullRag = (ragArray.Length == 0), isNullString = (sentences.Length == 0);
                switch (choice)
                {
                    case 0:
                        break;
                    case 1:
                        if (!isNullMatr)
                        {
                            WriteCenterMessage("Вот ваш двумерный массив: ");
                            PrintMatrix(matrix);
                            Console.WriteLine();
                        }
                        break;
                    case 2:
                        if (!isNullRag)
                        {
                            WriteCenterMessage("Вот ваш рванный массив: ");
                            PrintRaggedArray(ragArray);
                            Console.WriteLine();
                        }
                        break;
                    case 3:
                        if (!isNullString)
                        {
                            WriteCenterMessage("Вот ваша строка:");
                            ColoringMessage(sentences, 14);
                            Console.WriteLine();
                        }
                        break;
                }
                string[] menu = { "Работа с двумерным массивом", "Работа с рванным массивом", "Работа со строкой", "Выход" };
                WriteCenterMessage("Лабораторная работа №5");
                PrintMenu(menu);
                string[] messages = { "Введите номер команды: ", "Ошибка ввода числа!", "Такой команды нет!" };
                choose = ReadInt(messages, 1, menu.Length);
                switch (choose)
                {
                    case 1:
                        Console.Clear();
                        string[] case1InsideMenu = { "Создание двумерного массива", "Добавление строки в двумерный массив", "Выход" };
                        WriteCenterMessage(menu[0]);
                        PrintMenu(case1InsideMenu);
                        int choose1 = ReadInt(messages, 1, case1InsideMenu.Length);
                        switch (choose1)
                        {
                            case 1:
                                Console.Clear();
                                WriteCenterMessage(menu[0]);
                                string[] firstInsideMenu = {"Создание случайного двумерного массива", "Создание двумерного массива вручную", "Выход"};
                                PrintMenu(firstInsideMenu);
                                int choose2 = ReadInt(messages, 1, firstInsideMenu[0].Length);
                                switch (choose2)
                                {
                                    case 1:
                                        Console.Clear();
                                        WriteCenterMessage(menu[0]);
                                        CreateRandomMatrix(out matrix);
                                        choice = 1;
                                        break;
                                    case 2:
                                        Console.Clear();
                                        WriteCenterMessage(menu[0]);
                                        CreateArtificialMatrix(out matrix);
                                        choice = 1;
                                        break;
                                    case 3:
                                        Console.Clear();
                                        break;
                                }
                                break;
                            case 2:
                                if (isNullMatr)
                                {
                                    ColoringMessage("Сначала создайте массив!", 4);
                                    Console.WriteLine();
                                    break;
                                }
                                else
                                {
                                    Console.Clear();
                                    WriteCenterMessage(menu[0]);
                                    string[] secondInsideMenu = {"Добавление случайной строки", "Добавление строки вручную", "Выход" };
                                    PrintMenu(secondInsideMenu);
                                    int choice3 = ReadInt(messages, 1, secondInsideMenu.Length);
                                    Console.Clear();
                                    string[] messageInputLine = {"Введите номер добавляемой строки: ", "Ошибка ввода номера строки!", "Такой строки в массива нет!" };
                                    int lineNumber;
                                    switch (choice3)
                                    {
                                        case 1:
                                            WriteCenterMessage(menu[0]);
                                            lineNumber = ReadInt(messageInputLine, 1, matrix.GetLength(1) + 1);
                                            InputRandomLineInMatrix(ref matrix, lineNumber);
                                            break;
                                        case 2:
                                            WriteCenterMessage(menu[0]);
                                            lineNumber = ReadInt(messageInputLine, 1, matrix.GetLength(1) + 1);
                                            InputLineInMatrix(ref matrix, lineNumber);
                                            break;
                                        case 3:
                                            Console.Clear();
                                            break;
                                    }
                                }
                                break;

                        }
                        break;
                    case 2:
                        Console.Clear();
                        string[] case2InsideMenu = { "Создание рваного массива", "Удаление самой длинной строки из рваного массива", "Выход" };
                        WriteCenterMessage(menu[1]);
                        PrintMenu(case2InsideMenu);
                        int choose3 = ReadInt(messages, 1,case2InsideMenu.Length);
                        switch (choose3)
                        {
                            case 1:
                                Console.Clear();
                                WriteCenterMessage(menu[1]);
                                string[] secondInsideMenu = { "Создание случайного рваного массива", "Создание массива вручную", "Выход" };
                                PrintMenu(secondInsideMenu);
                                int insideCommand1 = ReadInt(messages, 1, 3);
                                switch (insideCommand1)
                                {
                                    case 1:
                                        Console.Clear();
                                        WriteCenterMessage(menu[1]);
                                        ragArray = CreateRandomRaggedArray();
                                        choice = 2;
                                        isNullRag = false;
                                        break;
                                    case 2:
                                        Console.Clear();
                                        WriteCenterMessage(menu[1]);
                                        ragArray = CreateArtificialRaggedArray();
                                        choice = 2;
                                        isNullRag = false;
                                        break;
                                    case 3:
                                        Console.Clear();
                                        break;
                                }
                                break;
                            case 2:
                                if (isNullRag)
                                {
                                    ColoringMessage("Сначала создайте рванный массив!", 4);
                                }
                                else
                                {
                                    DeleteLongestLine(ref ragArray);
                                    ColoringMessage("Длиннейшая строка была удалена.", 4);
                                }
                                break;
                            case 3:
                                Console.Clear();
                                break;
                        }
                        break;
                    case 3:
                        Console.Clear();
                        string[] case3InsideMenu = { "Создание строки", "Переворот нечетных предложений в строке", "Выход" };
                        WriteCenterMessage(menu[2]);
                        PrintMenu(case3InsideMenu);
                        int choos3 = ReadInt(messages, 1, case3InsideMenu.Length); 
                        switch (choos3)
                        {
                            case 1:
                                Console.Clear();
                                WriteCenterMessage(menu[2]);
                                string[] thirdInsideMenu = {"Создание случайной строки", "Создание строки вручную", "Выход"};
                                PrintMenu(thirdInsideMenu);
                                int insideCommand2 = ReadInt(messages, 1, thirdInsideMenu.Length);
                                switch (insideCommand2)
                                {
                                    case 1: 
                                        Console.Clear();
                                        WriteCenterMessage(menu[2]);
                                        sentences = CreateRandomString();
                                        ColoringMessage("Строка успешно была создана!", 6);
                                        choice = 3;
                                        break;
                                    case 2:
                                        Console.Clear();
                                        WriteCenterMessage(menu[2]);
                                        sentences = WriteString();
                                        choice = 3;
                                        break;
                                    case 3:
                                        Console.Clear();
                                        break;
                                }
                                break;
                            case 2:
                                if (isNullString)
                                {
                                    ColoringMessage("Сначала создайте строку!", 4);
                                }
                                else
                                {
                                    string result = ValidateText(sentences);
                                    if (result.Length == 0)
                                    {
                                        ReverseNotEvenSentences(ref sentences);
                                        ColoringMessage("Нечетные предложения были перевернуты успешно!", 6);
                                    }
                                    else
                                    {
                                        ColoringMessage(result, 6);
                                    }
                                }
                                break;
                            case 3:
                                Console.Clear();
                                break;
                        }
                        break;
                    case 4:
                        Console.WriteLine();
                        ColoringMessage("Завершение работы программы...", 6);
                        break;
                }
                Console.WriteLine();
                Console.Write("Нажмите enter, чтобы продолжить...");
                Console.ReadLine();
            } while (choose != 4);
        }
    }
}
