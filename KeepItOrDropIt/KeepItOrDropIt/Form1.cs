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

namespace KeepItOrDropIt
{
    public partial class Form1 : Form
    {
        // Globale Variablen
        List<Statement> statementliste = new List<Statement>();
        List<string> subjektliste = new List<string>();
        Random r = new Random();
        int chosenStatetementIndex = 0;
        Label[] labelscreens = new Label[20];

        // Liste zufällig anordnen
        // Quelle: http://www.vcskicks.com/randomize_array.php
        private List<E> ShuffleList<E>(List<E> inputList)
        {
            List<E> randomList = new List<E>();

            Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]); //add it to the new, random list
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            return randomList; //return the new random list
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Lädt Statements aus einer Datei
            int linecount = 1;
            string statementtext = "";
            bool[] screens = new bool[100];
            string filePath = System.IO.Path.GetFullPath("lösung\\statements\\videospiele.txt");
            StreamReader r = new StreamReader(filePath, Encoding.Default);
            while (!r.EndOfStream) {
                string zeile = r.ReadLine();
                if(linecount % 2 == 1)
                {
                    statementtext = zeile;
                }
                else
                {
                    // Binärstring in bool umwandeln
                    if(linecount == 2)
                    {
                        screens = new bool[zeile.Length];
                    }
                    char[] values = zeile.ToCharArray();
                    for(int zähler = 0; zähler < zeile.Length; zähler++)
                    {
                        screens[zähler] = Convert.ToBoolean(Convert.ToInt32(Convert.ToString(values[zähler])));
                    }
                    statementliste.Add(new Statement(statementtext, screens.ToArray()));
                }
                linecount++;
            }
            r.Close();

            // Lädt Antworten aus einer Datei
            filePath = System.IO.Path.GetFullPath("lösung\\screens\\videospiele.txt");
            r = new StreamReader(filePath, Encoding.Default);
            while (!r.EndOfStream)
            {
                string zeile = r.ReadLine();
                subjektliste.Add(zeile);
            }
            r.Close();

            // Durchmischt die ELemente einer Liste
            statementliste = ShuffleList(statementliste);
            List<string> screenliste = new List<string>(subjektliste);
            screenliste = ShuffleList(screenliste);

            // Erstellt die einzelenen Labels für die Antworten
            labelscreens = new Label[20];
            for (int zähler = 0; zähler < 20; zähler++)
            {
                labelscreens[zähler] = new Label();
                labelscreens[zähler].Text = screenliste[zähler];
                labelscreens[zähler].Left = 150 + zähler % 5 * 100;
                labelscreens[zähler].Top = 65 + zähler % 4 * 60;
                labelscreens[zähler].MaximumSize = new Size(90, 0);
                labelscreens[zähler].AutoSize = true;
                labelscreens[zähler].Font = new Font("Bahnschrift", 9);
                this.Controls.Add(labelscreens[zähler]);
            }

            // Zeigt ein Statement an
            label1.Text = statementliste[chosenStatetementIndex].Statementtext;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Antworten, die zum Statement passen, werden entfernt
            dropper(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Antworten, die nicht zum Statement passen, werden entfernt
            dropper(false);
        }

        private void dropper(bool dumped)
        {
            int activescreens = 0;

            // Prüft einzelne Antworten auf Übereinstimmung
            foreach (Label screenlabel in labelscreens)
            {
                int screenindex = -1;
                for (int zähler = 0; zähler < subjektliste.Count; zähler++)
                {
                    if (screenlabel.Text == subjektliste[zähler])
                    {
                        screenindex = zähler;
                        break;
                    }
                }

                if(statementliste[chosenStatetementIndex].Answers[screenindex] == dumped)
                {
                    screenlabel.Visible = false;
                }
                if(screenlabel.Visible == true)
                {
                    activescreens++;
                }
            }

            // Löst Runde auf
            if(activescreens == 0)
            {
                MessageBox.Show("Game Over! \nBestandene Runden: " + chosenStatetementIndex);
            }
            else if(chosenStatetementIndex == statementliste.Count - 1)
            {
                MessageBox.Show("Gratulation, du hast den Prototyp geschafft! ");
            }
            else
            {
                chosenStatetementIndex++;
                label1.Text = statementliste[chosenStatetementIndex].Statementtext;
            }
        }
    }
}
