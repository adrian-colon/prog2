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
        private Node evalClauses(Node clauses, Environment env)
        {
            // if end of clauses and nothing has been returned, return unspecific
            if (clauses.isNull())
                return new StringLit("#{Unspecific}");
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
                    Node car;
                    if (exprs.getCar().isPair())
                    {
                        // placeholder assignment to avoid unassigned variable error
                        // the loop will always run at least once so this value wont matter
                        car = exprs;
                        // iterate exprs, evaluate each, then return the last one
                        while (!exprs.isNull())
                        {
                            //get an expression out of the list of else expressions
                            expr = exprs.getCar();
                            //evaluate the extracted expression
                            Node cdr = expr.getCdr();
                            while (!cdr.isNull())
                            {
                                car = cdr.getCar().eval(env);
                                cdr = cdr.getCdr();
                            }
                            //get rest of expressions
                            exprs = exprs.getCdr();
                        }
                    }
                    else
                        car = exprs.getCar().eval(env);
                    return car;
                }
            }
            Node testEval = test.eval(env);
            // if clause is a test with no expressions
            if (exprs.isNull())
                return testEval;
            // if test failed go to next clause
            if (testEval == BoolLit.getInstance(false))
                return evalClauses(clauses.getCdr(), env);
            // if alternate form
            if (exprs.getCar().getName().Equals("=>"))
                return exprs.getCdr().getCar().eval(env).apply(testEval);
            //normal form: iterate exprs, evaluate each expr, then return last expr
            else
            {
                Node car;
                if (exprs.getCar().isPair())
                {
                    car = exprs;
                    while (!exprs.isNull())
                    {
                        expr = exprs.getCar();
                        Node cdr = expr.getCdr();
                        while (!cdr.isNull())
                        {
                            car = cdr.getCar().eval(env);
                            cdr = cdr.getCdr();
                        }
                        exprs = exprs.getCdr();
                    }
                }
                else
                    car = exprs.getCar().eval(env);
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
            Node cnd = exp.getCdr().getCar();
            Node test = cnd.getCar();
            return evalClauses(exp.getCdr(), env);
        }
    }
}


