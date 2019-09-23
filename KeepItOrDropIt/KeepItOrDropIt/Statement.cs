using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepItOrDropIt
{
    class Statement
    {
        string myStatementtext;
        bool[] myAnswers;

        public Statement(string statementtext, bool[] answers)
        {
            myStatementtext = statementtext;
            myAnswers = answers;
        }

        public string Statementtext
        {
            get { return myStatementtext; }
        }

        public bool[] Answers
        {
            get { return myAnswers; }
        }
    }
}
