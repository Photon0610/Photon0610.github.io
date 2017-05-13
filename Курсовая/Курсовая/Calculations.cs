using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовая
{
    class Calculations
    {
        public double averenge { get; set; }
        public int max {get;set;}
        public int numberOfSenteses { get; set; }
        public double k = 0.14; //часть текста где начало и конец
        public Calculations (double av, int m, int nOS)
        {
            averenge = av;
            max = m;
            numberOfSenteses = nOS;
        }
        public double GetCountOfMeetings(int meets)
        {
            if (meets < averenge) return 1;
            else if (meets < max) return 1.5;
            else return 2;
        }
        public double GetLocation(int number)
        {
            if (number < numberOfSenteses * k) return 1.3;
            else if (number < numberOfSenteses * (1 - k)) return 1;
            else return 1.2;
        }
        public double TransformPartOfSpeech(string word)
        {
            switch (word)
            {
                case "A":// прилагательное
                    return 0.5;
                case "ADV"://наречие
                    return 0.5;
                case "ADVPRO"://местоименное наречие
                    return 0.2;
                case "APRO"://местоимение-прилагательное
                    return 0.2;
                case "COM"://часть композита -сложного слова
                    return 0.5;
                case "CONJ"://союз
                    return 0;
                case "INTJ"://междометие
                    return 0;
                case "NUM"://числительное
                    return 0.5;
                case "PART"://частица
                    return 0;
                case "PR"://предлог
                    return 0;
                case "S":// существительное
                    return 1.2;
                case "SPRO"://местоимение-существительное
                    return 0.2;
                case "V"://глагол
                    return 1;
                default:
                    return 0.5; // когда незнакомое слово
            } 
        }
    }
}
