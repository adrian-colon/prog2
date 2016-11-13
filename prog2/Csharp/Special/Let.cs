// Let -- Parse tree node strategy for printing the special form let

using System;

namespace Tree
{
    public class Let : Special
    {
	public Let() { }

        public override void print(Node t, int n, bool p)
        {
            Printer.printLet(t, n, p);
        }

        private static int define(Node bindings, Environment env, Environment letEnv)
        {
            //no bindings left, exit with 0
            if (bindings.isNull())
                return 0;
            Node binding = bindings.getCar();
            int numArgs = 1;
            Node expCdr = binding.getCdr();
            while (!expCdr.isNull())
            {
                numArgs++;
                expCdr = expCdr.getCdr();
            }
            //binding has wrong num args, exit with -1
            if (numArgs != 2)
                return -1;
            Node var = binding.getCar();
            Node val = binding.getCdr().getCar().eval(env);
            letEnv.define(var, val);
            return Let.define(bindings.getCdr(), env, letEnv);
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

            if(numArgs < 2)
            {
                Console.Error.WriteLine("Error: invalid expression");
                return Nil.getInstance();
            }
            Node bindings = exp.getCdr().getCar(); //bindings car
            Node body = exp.getCdr().getCdr(); //body cdr
            Node cdr = exp.getCdr().getCar();
            Node car;
            int numBindings = 0;
            while (!cdr.isNull())
            {
                car = cdr.getCar();
                //bindings have proper format
                if (!car.isPair())
                {
                    Console.Error.WriteLine("Error: invalid expression");
                    return Nil.getInstance();
                }
                numBindings++;
                cdr = cdr.getCdr();
            }
            //at least one binding
            if (numBindings == 0)
            {
                Console.Error.WriteLine("Error: invalid expression");
                return Nil.getInstance();
            }
            Environment letEnv = new Environment(env);
            if (Let.define(bindings, env, letEnv) == 0) // >= 0?
            {
                Node bcar = body.getCar().eval(letEnv);
                Node bcdr = body.getCdr();
                while (!bcdr.isNull())
                {
                    bcar = bcdr.getCar().eval(letEnv);
                    bcdr = bcdr.getCdr();
                }
                return bcar;
            }
            Console.Error.WriteLine("Error: invalid expression");
            return Nil.getInstance();
        }
    }
}


