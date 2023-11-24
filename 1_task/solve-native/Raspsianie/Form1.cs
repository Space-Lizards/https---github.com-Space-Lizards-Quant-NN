using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RaspsianieGovna
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string [,] g1 = CreateTable(8);
            
            Weektable w1 = new Weektable(g1, 8);
            GridShow(g1, 8, tableG1);
            using (StreamWriter sw = new StreamWriter("timtetable.txt"))
            {
                string[] lsn;
                for(int i = 0; i < 6; i++)
                {
                    for(int j = 1; j < 8; j++)
                    {
                        if (g1[i,j] != null)
                        {
                            lsn = g1[i, j].Split('-');
                            sw.WriteLine($"Группа1 | Неделя 1 | {g1[i, 0]} | {8 + j}:00 - {9 + j}:00 | {lsn[1]} | {lsn[0]} ");
                            sw.WriteLine($"Группа2 | Неделя 1 | {g1[i, 0]} | {8 + j + 1}:00 - {9 + j + 1}:00 | {lsn[1]} | {lsn[0]} ");

                            sw.WriteLine($"Группа2 | Неделя 2 | {g1[i, 0]} | {8 + j}:00 - {9 + j}:00 | {lsn[1]} | {lsn[0]} ");
                            sw.WriteLine($"Группа1 | Неделя 2 | {g1[i, 0]} | {8 + j + 1}:00 - {9 + j + 1}:00 | {lsn[1]} | {lsn[0]} ");
                        }
                    }
                }
                sw.Dispose();
            }
        }

        public string[,] CreateTable(int promejytki_vremeni)
        {
            string[,] table = new string[6, promejytki_vremeni+1];
            table[0, 0] = "Пн";
            table[1, 0] = "Вт";
            table[2, 0] = "Ср";
            table[3, 0] = "Чт";
            table[4, 0] = "Пт";
            table[5, 0] = "Сб";
            return table;

        }
        public void GridShow(string[,] table, int timespaces, DataGridView grid)
        {
            grid.RowCount = timespaces;
            grid.ColumnCount = 6;
            for(int i = 0; i < grid.ColumnCount; i++)
            {
                for(int j = 0; j < grid.RowCount; j++)
                {
                    grid.Rows[j].Cells[i].Value = table[i,j];
                }
            }
        }
    }
}
