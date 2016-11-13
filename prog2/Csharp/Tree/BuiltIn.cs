// BuiltIn -- the data structure for built-in functions

// Class BuiltIn is used for representing the value of built-in functions
// such as +.  Populate the initial environment with
// (name, new BuiltIn(name)) pairs.

// The object-oriented style for implementing built-in functions would be
// to include the C# methods for implementing a Scheme built-in in the
// BuiltIn object.  This could be done by writing one subclass of class
// BuiltIn for each built-in function and implementing the method apply
// appropriately.  This requires a large number of classes, though.
// Another alternative is to program BuiltIn.apply() in a functional
// style by writing a large if-then-else chain that tests the name of
// the function symbol.

using System;
using Parse;

namespace Tree
{
    public class BuiltIn : Node
    {
        private Node symbol;            // the Ident for the built-in function

        public BuiltIn(Node s)		{ symbol = s; }

        public Node getSymbol()		{ return symbol; }

        // DONE :: The method isProcedure() should be defined in
        // class Node to return false.
        public override bool isProcedure()	{ return true; }

        public override void print(int n)
        {
            // there got to be a more efficient way to print n spaces
            for (int i = 0; i < n; i++)
                Console.Write(' ');
            Console.Write("#{Built-in Procedure ");
            if (!symbol.isNull())
                symbol.print(-Math.Abs(n));
            Console.Write('}');
            if (n >= 0)
                Console.WriteLine();
        }

        // DONE :: The method apply() should be defined in class Node
        // to report an error.  It should be overridden only in classes
        // BuiltIn and Closure.
        public override Node apply(Node args)
        {
            string name = symbol.getName();
            int numArgs = 0;
            Node arg1;
            Node arg2;
            Node tempArgs = args;
            while (!tempArgs.isNull())
            {
                numArgs++;
                tempArgs = tempArgs.getCdr();
            }

            if (numArgs == 0)
            {
                if (name.Equals("read"))
                {
                    Parser parser = new Parser(new Scanner(Console.In), new TreeBuilder());
                    Node exp = (Node)parser.parseExp();
                    if (!exp.isNull())
                        return exp;
                    return new StringLit("End of file");
                }
                else if (name.Equals("newline"))
                {
                    Console.WriteLine();
                    return new StringLit("#{Unspecific}");
                }
                else if (name.Equals("interaction-environment"))
                    return Scheme4101.env;
                Console.Error.WriteLine("Error: invalid input");
                return Nil.getInstance();
            }
            else if (numArgs == 1)
            {
                arg1 = args.getCar();
                if (name.Equals("symbol?"))
                    return BoolLit.getInstance(arg1.isSymbol());
                if (name.Equals("number?"))
                    return BoolLit.getInstance(arg1.isNumber());
                if (name.Equals("car"))
                    return arg1.getCar();
                if (name.Equals("cdr"))
                    return arg1.getCdr();
                if (name.Equals("null?"))
                    return BoolLit.getInstance(arg1.isNull());
                if (name.Equals("pair?"))
                    return BoolLit.getInstance(arg1.isPair());
                if (name.Equals("procedure?"))
                    return BoolLit.getInstance(arg1.isProcedure());
                if (name.Equals("write"))
                {
                    arg1.print(0); // 0 prints new line after. -1?
                    return new StringLit("#{Unspecific}");
                }
                if (name.Equals("display"))
                {
                    StringLit.printQuotes = false;
                    arg1.print(0); // -1?
                    StringLit.printQuotes = true;
                    return new StringLit("#{Unspecific}");
                }
                Console.Error.WriteLine("Error: invalid input");
                return Nil.getInstance();
            }
            else if (numArgs == 2)
            {
                arg1 = args.getCar();
                arg2 = args.getCdr().getCar();
                if (name.Equals("eq?"))
                {
                    if (arg1.isSymbol() && arg2.isSymbol())
                        return BoolLit.getInstance(arg1.getName().Equals(arg2.getName()));
                    return BoolLit.getInstance(arg1 == arg2);
                }
                if (name.Equals("cons"))
                    return new Cons(arg1, arg2);
                if (name.Equals("set-car!"))
                {
                    arg1.setCar(arg2);
                    return new StringLit("#{Unspecific}");
                }
                if (name.Equals("set-cdr!"))
                {
                    arg1.setCdr(arg2);
                    return new StringLit("#{Unspecific}");
                }
                if (name.Equals("eval"))
                    return arg1.eval((Environment)arg2);
                if (name.Equals("apply"))
                    return arg1.apply(arg2);

                int int1;
                int int2;
                if (arg1.isNumber() && arg2.isNumber())
                {
                    int1 = arg1.getValue();
                    int2 = arg2.getValue();
                }
                else
                {
                    Console.Error.WriteLine("Error: invalid argument types");
                    return Nil.getInstance();
                }
                if (name.Equals("b+"))
                    if (arg1.isNumber() && arg2.isNumber())
                        return new IntLit(int1 + int2);
                if (name.Equals("b-"))
                    return new IntLit(int1 - int2);
                if (name.Equals("b*"))
                    return new IntLit(int1 * int2);
                if (name.Equals("b/"))
                    return new IntLit(int1 / int2);
                if (name.Equals("b="))
                    return BoolLit.getInstance(int1 == int2);
                if (name.Equals("b<"))
                    return BoolLit.getInstance(int1 < int2);
                Console.Error.WriteLine("Error: invalid function");
                return Nil.getInstance();
            }
            else
            {
                Console.Error.WriteLine("Error: wrong number of args");
                return Nil.getInstance();
            }
    	}
    }    
}

