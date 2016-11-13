// Define -- Parse tree node strategy for printing the special form define

using System;

namespace Tree
{
    public class Define : Special
    {
	public Define() { }

        public override void print(Node t, int n, bool p)
        {
            Printer.printDefine(t, n, p);
        }

        private bool verifyArgs(Node args)
        {
            return args.isSymbol() || args.isNull() || args.isPair() && args.getCar().isSymbol() && verifyArgs(args.getCdr());
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

            if (numArgs < 2)
            {
                Console.Error.WriteLine("Error: invalid expression");
                return Nil.getInstance();
            }
            Node arg1 = exp.getCdr().getCar();
            if (numArgs == 2 && arg1.isSymbol())
            {
                Node val = exp.getCdr().getCdr().getCar();
                env.define(arg1, val.eval(env));
                return new StringLit("; no values returned", false);
            }
            if (arg1.isPair())
            {
                Node fName = arg1.getCar();
                Node fArgs = arg1.getCdr();
                Node fBody = exp.getCdr().getCdr();
                bool validArgs = verifyArgs(fArgs);
                if (fName.isSymbol() && validArgs)
                {
                    Node node = new Cons(new Ident("lambda"), new Cons(fArgs, fBody));
                    env.define(fName, node.eval(env));
                    return new StringLit("; no values returned", false);
                }
                Console.Error.WriteLine("Error: ill-formed definition");
                return Nil.getInstance();
            }
            Console.Error.WriteLine("Error: invalid expression");
            return Nil.getInstance();
        }
    }
}


