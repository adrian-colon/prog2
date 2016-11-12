// Set -- Parse tree node strategy for printing the special form set!

using System;

namespace Tree
{
    public class Set : Special
    {
	public Set() { }
	
        public override void print(Node t, int n, bool p)
        {
            Printer.printSet(t, n, p);
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

            if (numArgs != 2)
            {
                Console.Error.WriteLine("Error: wrong number of arguments");
                return Nil.getInstance();
            }
            Node arg1 = exp.getCdr().getCar();
            Node arg2 = exp.getCdr().getCdr().getCar();
            env.assign(arg1, arg2.eval(env));
            return new StringLit("unspecified value");
        }
}
}

