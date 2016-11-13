// StringLit -- Parse tree node class for representing string literals

using System;

namespace Tree
{
    public class StringLit : Node
    {
        public static bool printQuotes = true;
        private string stringVal;

        public StringLit(string s)
        {
            stringVal = s;
        }

        public StringLit(string s, bool quotes)
        {
            stringVal = s;
            printQuotes = false;
        }

        public override void print(int n)
        {
            if (printQuotes)
                Printer.printStringLit(n, stringVal);
            else
            {
                for (int index = 0; index < n; ++index)
                    Console.Write(' ');
                Console.Write(stringVal);
                if (n >= 0)
                    Console.WriteLine();
            }
        }

        public override bool isString()
        {
            return true;
        }

        public override Node eval(Environment env)
        {
            return this;
        }
    }
}

