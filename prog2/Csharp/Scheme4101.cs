// SPP -- The main program of the Scheme pretty printer.

using System;
using Parse;
using Tokens;
using Tree;
using Environment = Tree.Environment;

public class Scheme4101
{
    public static Environment env = null;
    public static int Main(string[] args)
    {
        // Create scanner that reads from standard input
        Scanner scanner = new Scanner(Console.In);
        
        if (args.Length > 1 ||
            (args.Length == 1 && ! args[0].Equals("-d")))
        {
            Console.Error.WriteLine("Usage: mono SPP [-d]");
            return 1;
        }
        
        // If command line option -d is provided, debug the scanner.
        if (args.Length == 1 && args[0].Equals("-d"))
        {
            // Console.Write("Scheme 4101> ");
            Token tok = scanner.getNextToken();
            while (tok != null)
            {
                TokenType tt = tok.getType();

                Console.Write(tt);
                if (tt == TokenType.INT)
                    Console.WriteLine(", intVal = " + tok.getIntVal());
                else if (tt == TokenType.STRING)
                    Console.WriteLine(", stringVal = " + tok.getStringVal());
                else if (tt == TokenType.IDENT)
                    Console.WriteLine(", name = " + tok.getName());
                else
                    Console.WriteLine();

                // Console.Write("Scheme 4101> ");
                tok = scanner.getNextToken();
            }
            return 0;
        }

        // Create parser
        TreeBuilder builder = new TreeBuilder();
        Parser parser = new Parser(scanner, builder);
        Node root;

        // DONE :: Create and populate the built-in environment and
        // create the top-level environment
        env = new Environment();
        Ident id = new Ident("symbol?");
        env.define(id, new BuiltIn(id));
        id = new Ident("symbol?");
        env.define(id, new BuiltIn(id));
        id = new Ident("number?");
        env.define(id, new BuiltIn(id));
        id = new Ident("b+");
        env.define(id, new BuiltIn(id));
        id = new Ident("b-");
        env.define(id, new BuiltIn(id));
        id = new Ident("b*");
        env.define(id, new BuiltIn(id));
        id = new Ident("b/");
        env.define(id, new BuiltIn(id));
        id = new Ident("b=");
        env.define(id, new BuiltIn(id));
        id = new Ident("b<");
        env.define(id, new BuiltIn(id));
        id = new Ident("car");
        env.define(id, new BuiltIn(id));
        id = new Ident("cdr");
        env.define(id, new BuiltIn(id));
        id = new Ident("cons");
        env.define(id, new BuiltIn(id));
        id = new Ident("set-car!");
        env.define(id, new BuiltIn(id));
        id = new Ident("set-cdr!");
        env.define(id, new BuiltIn(id));
        id = new Ident("null?");
        env.define(id, new BuiltIn(id));
        id = new Ident("pair?");
        env.define(id, new BuiltIn(id));
        id = new Ident("eq?");
        env.define(id, new BuiltIn(id));
        id = new Ident("procedure?");
        env.define(id, new BuiltIn(id));
        id = new Ident("read");
        env.define(id, new BuiltIn(id));
        id = new Ident("write");
        env.define(id, new BuiltIn(id));
        id = new Ident("display");
        env.define(id, new BuiltIn(id));
        id = new Ident("newline");
        env.define(id, new BuiltIn(id));
        id = new Ident("eval");
        env.define(id, new BuiltIn(id));
        id = new Ident("apply");
        env.define(id, new BuiltIn(id));
        id = new Ident("interaction-environment");
        env.define(id, new BuiltIn(id));
        env = new Environment(env);
        // Read-eval-print loop

        // DONE:: print prompt and evaluate the expression

        root = (Node) parser.parseExp();
        while (root != null) 
        {
            root.eval(env).print(0);
            Console.Write("> ");
            root = (Node) parser.parseExp();
        }

        return 0;
    }
}
