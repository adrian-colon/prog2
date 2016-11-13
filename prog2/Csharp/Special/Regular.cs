// Regular -- Parse tree node strategy for printing regular lists

using System;

namespace Tree
{
    public class Regular : Special
    {
        public Regular() { }

        public override void print(Node t, int n, bool p)
        {
            Printer.printRegular(t, n, p);
        }

        public Node makeCons(Node exp, Environment env)
        {
            if (exp.isNull())
                return Nil.getInstance();
            return new Cons(exp.getCar().eval(env), makeCons(exp.getCdr(), env));
        }

        public override Node eval(Node exp, Environment env)
        {
            if (!exp.isNull())
                return exp.getCar().eval(env).apply(makeCons(exp.getCdr(), env));
            Console.Error.WriteLine("Error: invalid expression");
            return Nil.getInstance();
        }
    }
}


