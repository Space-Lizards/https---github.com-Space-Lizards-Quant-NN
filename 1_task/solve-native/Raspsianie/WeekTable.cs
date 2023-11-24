using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace RaspsianieGovna
{
   class Weektable
   {
        string[] Teachers = {"Иванов", "Петров", "Сидоров", "Карпов", "Соколов"};

        string[,] Subjects = { 
            {"Квантовая механика", "Квантовая теория информации" }, 
            {"Квантовые вычисления", "Сложность квантовых алгоритмов" }, 
            {"Квантовые алгоритмы в логистике", "Квантовое машинное обучение" }, 
            {"Моделирование квантовых систем", "Квантовые алгоритмы в химии" }, 
            {"Физическая реализация квантовых компьютеров", "Моделирование квантовых алгоритмов" } 
        };
        int[,] SubjectsHours = { { 0, 0}, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 } };

        int[] HoursPerDay = {0, 0, 0, 0, 0, 0};
        bool[,] FreeDays = { 
        {true, true, false, true, true, true },
        {false, true, true, true, true, true },
        {true, true, true, true, true, false },
        {true, false, true, true, true, true },
        {true, false, true, true, true, true }
        };

        public Weektable(string[,] table, int timespaces)
        {
            for(int teach = 0; teach < Teachers.Length; teach++)
            {
                for(int iter = 0; iter < 4; iter++)
                {
                    int day = DayGet(teach);
                    for (int time = 0; time < timespaces; time++)
                    {
                        if (table[day, time] == null)
                        {
                            if (HoursPerDay[day] < 6)
                            {
                                if (SubjectsHours[teach, 0] < 2)
                                {
                                    table[day, time] = Format(teach, 0);
                                    HoursPerDay[day]++;
                                    SubjectsHours[teach, 0]++;
                                    break;
                                }
                                else if (SubjectsHours[teach, 1] < 2)
                                {
                                    table[day, time] = Format(teach, 1);
                                    HoursPerDay[day]++;
                                    SubjectsHours[teach, 1]++;
                                    break;
                                }
                                else break;
                            }
                        }
                    }
                }
            }
        }

        private int DayGet(int index)
        {
            int x = HoursPerDay[0];
            int whatday = 0;
            for(int i = 0; i < HoursPerDay.Length; i++)
            {
                if (x > HoursPerDay[i] && FreeDays[index,i] == true)
                {
                    whatday = i;
                    x = HoursPerDay[i];
                }
            }
            return whatday;
        }
        private string Format(int teach, int subj)
        {
            return $"{Teachers[teach]} - {Subjects[teach, subj]}";
        }

   }
}
