using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Курсовая
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> Sentences = new List<string>();
            List<int> CountOfWords = new List<int>();
            List<string> PrimaryDictionary = new List<string>();
            List<int> CountOfDictionary = new List<int>();
            List<string> PartOfSpeech = new List<string>();
            double weightOfWord;
            List<double> WeightOfSentences = new List<double>();
            List<double> AdditionalWeight = new List<double>();

            char c, c1;
            int k = 0, i, j, wordAmount = 0;
            bool b = false;
            string str = "";
            StreamReader sr = new StreamReader("C:\\Users\\Андрей\\Desktop\\Курсовая\\input.txt", Encoding.GetEncoding(1251)); //для чтения файла
            while (!sr.EndOfStream)
            {
                c = (char)sr.Read();
                if (c != ' ') str += c;
                do
                {
                    c1 = c;
                    c = (char)sr.Read();
                    if (c1 == ' ')
                        while (c == ' ' || c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9'|| c == ',' || c == ';' || c == ':')
                        {
                            str += c;
                            c = (char)sr.Read();
                        }
                    str += c;
                    if (c == ' ')
                    {
                        /*str += '!'*/; k++;
                    }
                }
                while (c != '.');
                ++k;
                CountOfWords.Add(k);
                wordAmount += k;
                k = 0;
                Sentences.Add(str);
                str = "";

            }
            sr.Close();
            for (i = 0; i < Sentences.Count; ++i)
            {
                Console.WriteLine(Sentences[i] + "  " + CountOfWords[i]);
                Console.WriteLine("");
            }
            Process mystem = new Process();
            mystem.StartInfo.FileName = "C:\\Users\\Андрей\\Desktop\\Курсовая\\mystem.exe";
            mystem.StartInfo.Arguments = "-i C:\\Users\\Андрей\\Desktop\\Курсовая\\input.txt C:\\Users\\Андрей\\Desktop\\Курсовая\\output.txt";
            mystem.StartInfo.UseShellExecute = false;
            mystem.StartInfo.RedirectStandardInput = true;
            mystem.StartInfo.RedirectStandardOutput = true;
            mystem.Start();
            mystem.WaitForExit();
            mystem.Close();

            sr = new StreamReader("C:\\Users\\Андрей\\Desktop\\Курсовая\\output.txt", Encoding.GetEncoding(1251));
            while (!sr.EndOfStream)
            {
                b = false;
                do c = (char)sr.Read(); while (c != '{');
                str = "";
                c = (char)sr.Read();
                while (c != '=' && c != '?')
                {
                    str += c;
                    c = (char)sr.Read();
                }

                if (PrimaryDictionary.Count > 0)
                {
                    b = false;
                    for (i = 0; i < PrimaryDictionary.Count; ++i)
                    {
                        if (PrimaryDictionary[i] == str)
                        {
                            CountOfDictionary[i]++;
                            b = true;
                        }
                    }
                    if (!b)
                    {
                        PrimaryDictionary.Add(str);
                        CountOfDictionary.Add(1);
                        AdditionalWeight.Add(1);
                    }
                }
                else
                {
                    PrimaryDictionary.Add(str);
                    CountOfDictionary.Add(1);
                }
                if (c == '=')
                {
                    str = "";
                    c = (char)sr.Read();
                    while (c != ',' && c != '=')
                    {
                        str += c;
                        c = (char)sr.Read();
                    }
                    if (!b) PartOfSpeech.Add(str);
                }
                else
                {
                    c = (char)sr.Read();
                    if (c == '=')
                    {
                        str = "";
                        c = (char)sr.Read();
                        while (c != ',' && c != '=')
                        {
                            str += c;
                            c = (char)sr.Read();
                        }
                    }
                    else  str = "UNKNOWN";
                    if (!b) PartOfSpeech.Add(str);
                }
                while (c != '}') c = (char)sr.Read();
            }
            for (i = 0; i < PrimaryDictionary.Count; ++i)
                Console.WriteLine(PrimaryDictionary[i] + "  " + CountOfDictionary[i] + "  " + PartOfSpeech[i]);
            sr.Close();
            //////////////  2 проход по тексту///////////////
            sr = new StreamReader("C:\\Users\\Андрей\\Desktop\\Курсовая\\output.txt", Encoding.GetEncoding(1251));
            int sum = 0, max = 0, w = 0;
            double aver;
            for (i = 0; i < CountOfWords.Count; ++i)
                if (PartOfSpeech[i] != "CONJ" && PartOfSpeech[i] != "INTJ" && PartOfSpeech[i] != "PART" && PartOfSpeech[i] != "PR")
                {
                    w++;
                    sum += CountOfDictionary[i];
                    if (CountOfDictionary[i] > max) max = CountOfDictionary[i];
                }
            aver = Convert.ToDouble(sum) / w;
            Calculations Calculate = new Calculations(aver, max, Sentences.Count);
            Console.WriteLine(Calculate.averenge + "  " + Calculate.max + "  " + Calculate.numberOfSenteses);

            c = ' ';
            for (i = 0; i < Sentences.Count; ++i) // по каждому i предложению
            {
                weightOfWord = 0;
                int numberOfWords = 0;
                for (j = 0; j < CountOfWords[i]; ++j) //по каждому j слову в предложении
                {                       
                    while (c != '{') c = (char)sr.Read();
                    str = "";
                    c = (char)sr.Read();
                    while (c != '=' && c != '?')
                    {
                        str += c;
                        c = (char)sr.Read();
                    }
                    for (int g = 0; g < PrimaryDictionary.Count; ++g) // g это по всем словам словаря
                    {
                        if (str == PrimaryDictionary[g])
                        {
                            if (Calculate.TransformPartOfSpeech(PartOfSpeech[g]) != 0)
                            {
                                if (Calculate.GetLocation(i) == 1.3 && PartOfSpeech[g] == "S") AdditionalWeight[g] = 1.5;
                                weightOfWord += Calculate.TransformPartOfSpeech(PartOfSpeech[g]) * Calculate.GetLocation(i) * Calculate.GetCountOfMeetings(CountOfDictionary[g]) * AdditionalWeight[g];
                                numberOfWords++;
                                //Console.WriteLine(str);
                            }
                            break;
                        }
                    }
                }
                if (numberOfWords != 0) weightOfWord /= numberOfWords; // не считаем пустые слова
                else
                    weightOfWord = 0;
                WeightOfSentences.Add(weightOfWord);
                if (sr.EndOfStream) break;
            }
            //Console.WriteLine("!!");
            sr.Close();

            ////////////// найдем подходящие предложения...\\\\\\\\
            double[] SortWeight = new double[WeightOfSentences.Count];
            for (i = 0; i < WeightOfSentences.Count; ++i)
                SortWeight[i] = WeightOfSentences[i];
            Array.Sort(SortWeight);
            Console.WriteLine("Введите процент текста, который нужно реферировать");
            int part;
            part = int.Parse(Console.ReadLine());
            while (part <= 0 || part > 100)
            {
                Console.WriteLine("Неверные введены данные, попробуйте еще раз");
                part = Console.Read();
            }           
            double num1 = (WeightOfSentences.Count *(1 - Convert.ToDouble(part) / 100.0));
            int num = Convert.ToInt32(num1);
            Console.WriteLine(num);
            double board = SortWeight[num];
            for (i = 0; i < WeightOfSentences.Count; ++i)
                if (WeightOfSentences[i] > board) Console.WriteLine(Sentences[i]);
            Console.ReadKey();
        }
    }
}