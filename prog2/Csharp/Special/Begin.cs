// Begin -- Parse tree node strategy for printing the special form begin

using System;

namespace Tree
{
    public class Begin : Special
    {
	public Begin() { }

        public override void print(Node t, int n, bool p)
        {
            Printer.printBegin(t, n, p);
        }
        
        public override Node eval(Node exp, Environment env)
        {
            Node expCdr = exp.getCdr();
            if (!expCdr.isNull())
            {
                Node car = expCdr.getCar().eval(env);
                Node cdr = expCdr.getCdr();
                while (!cdr.isNull())
                {
                    car = cdr.getCar().eval(env);
                    cdr = cdr.getCdr();
                }
                return car;
            }
            Console.Error.WriteLine("Error: invalid expression");
            return Nil.getInstance();
        }
    }
}

