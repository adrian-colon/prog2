// If -- Parse tree node strategy for printing the special form if

using System;

namespace Tree
{
    public class If : Special
    {
	public If() { }

        public override void print(Node t, int n, bool p)
        {
            Printer.printIf(t, n, p);
        }

        public override Node eval(Node exp, Environment env)
        {
            int numArgs = 0;
            Node expCdr = exp.getCdr();
            while (!expCdr.isNull())
            {
                numArgs++;
                expCdr = expCdr.getCdr();
            }
            Node cnd;
            Node thn;
            Node els;
            if (numArgs < 2 || numArgs > 3)
            {
                Console.Error.WriteLine("Error: invalid expression");
                return Nil.getInstance();
            }       
            else
            {
                cnd = exp.getCdr().getCar();
                thn = exp.getCdr().getCdr().getCar();
                if (numArgs == 3)
                    els = exp.getCdr().getCdr().getCdr().getCar();
                else
                    els = new StringLit("unspecified value");
            }
            if (cnd.eval(env) != BoolLit.getInstance(false))
                return thn.eval(env);
            return els.eval(env);
        }
    }
}

