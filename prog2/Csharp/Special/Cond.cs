// Cond -- Parse tree node strategy for printing the special form cond

using System;

namespace Tree
{
    public class Cond : Special
    {
	public Cond() { }

        public override void print(Node t, int n, bool p)
        { 
            Printer.printCond(t, n, p);
        }

        // check if null/end of last clause:
        //    return unspecific
        // check if else clause:
        //    yes: eval exps and return value of last exp
        // check if clause has exp
        //    if no exp: return value of test
        //    if test failed: go to next clause
        //    check alt or normal form
        //        eval expression(s) appropriately
        private Node evalConds(Node clauses, Environment env)
        {
            // if end of clauses and nothing has been returned, return unspecific
            if (clauses.isNull())
                return new StringLit("#{Unspecific}", false);
            Node cnd = clauses.getCar();
            // make sure clause isnt empty
            if (cnd.isNull())
            {
                Console.Error.WriteLine("Error: invalid expression");
                return Nil.getInstance();
            }
            Node test = cnd.getCar();
            Node exprs = cnd.getCdr();
            Node expr;
            if (test.isSymbol() && test.getName().Equals("else"))
            {
                //erorr if no expression or else is not last clause
                if (exprs.isNull() || !clauses.getCdr().isNull())
                {
                    Console.Error.WriteLine("Error: invalid expression");
                    return Nil.getInstance();
                }
                else
                {
                    // evaluate each expr in exprs then return the last one
                    Node car = Nil.getInstance();
                    while (!exprs.isNull())
                    {
                        expr = exprs.getCar();
                        car = expr.eval(env);
                        if (expr.isPair())
                        {
                            Node cdr = expr.getCdr();
                            while (!cdr.isNull())
                            {
                                car = cdr.getCar().eval(env);
                                cdr = cdr.getCdr();
                            }
                        }
                        exprs = exprs.getCdr();
                    }
                    return car;
                }
            }
            Node testEval = test.eval(env);
            // if clause is a test with no expressions
            if (exprs.isNull())
                return testEval;
            // if test failed go to next clause
            if (testEval == BoolLit.getInstance(false))
                return evalConds(clauses.getCdr(), env);
            // if alternate form
            if (exprs.getCar().getName().Equals("=>"))
                return exprs.getCdr().getCar().eval(env).apply(testEval);
            //normal form: iterate exprs, evaluate each expr, then return last expr
            else
            {
                Node car = Nil.getInstance();
                while (!exprs.isNull())
                {
                    expr = exprs.getCar();
                    car = expr.eval(env);
                    if (expr.isPair())
                    {
                        Node cdr = expr.getCdr();
                        while (!cdr.isNull())
                        {
                            car = cdr.getCar().eval(env);
                            cdr = cdr.getCdr();
                        }
                    }
                    exprs = exprs.getCdr();
                }
                return car;
            }
        }

        public override Node eval(Node exp, Environment env)
        {
            int numConds = 0;
            Node expCdr = exp.getCdr();
            while (!expCdr.isNull())
            {
                numConds++;
                expCdr = expCdr.getCdr();
            }
            if (numConds < 1)
            {
                Console.Error.WriteLine("Error: invalid expression");
                return Nil.getInstance();
            }
            return evalConds(exp.getCdr(), env);
        }
    }
}


