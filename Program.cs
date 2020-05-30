using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace lab14
{
    class Program
    {
        static void Main(string[] args)
        {
            string expr;
            try
            {             
                StreamReader file = new StreamReader(args[0]);
                StreamWriter outfile = new StreamWriter(args[1]);

                while ((expr = file.ReadLine()) != null)
                {
                    //getting rpn
                    var rpn = ExprCalc.GetRPN(expr).ToArray();

                    Console.WriteLine("rpn:");
                    Console.WriteLine(rpn);

                    outfile.WriteLine("rpn:");
                    outfile.WriteLine(rpn);

                    LinkedList<String> sdnf = new LinkedList<string>();
                    LinkedList<String> sknf = new LinkedList<string>();
                    var varValues = new Dictionary<char, int>();
                    var headerShown = false;

                    Console.WriteLine("truth table:");
                    foreach (var combination in ExprCalc.GetAllCombinations(ExprCalc.GetVariables(expr), 0, varValues))
                    {
                        //calculating expression
                        var res = ExprCalc.Calculate(rpn, varValues);

                        string str = "";

                        if (res)//sdnf
                        {
                            foreach (var val in varValues)
                            {
                                if (val.Value == 0) str += "¬" + val.Key + " + ";

                                else str += val.Key + " + ";
                            }
                            if (str.EndsWith(' '))
                            {
                                str = str.Substring(0, str.Length - 3);
                            }
                            sdnf.AddLast(str);
                        }
                        else//sknf
                        {
                            foreach (var val in varValues)
                            {
                                if (val.Value == 1) str += "¬" + val.Key + " * ";

                                else str += val.Key + " * ";
                            }
                            if (str.EndsWith(' '))
                            {
                                str = str.Substring(0, str.Length - 3);
                            }
                            sknf.AddLast(str);
                        }

                        //showing header
                        if (!headerShown)
                        {
                            foreach (var var in varValues.Keys)
                            {
                                Console.Write(var + "\t");
                                outfile.Write(var + "\t");
                            }
                            Console.WriteLine(expr);
                            outfile.WriteLine(expr);
                            headerShown = true;
                        }

                        //showing row
                        foreach (var var in varValues.Values)
                        {
                            Console.Write(var + "\t");
                            outfile.Write(var + "\t");
                        }
                        if (res)
                        {
                            Console.WriteLine('1');
                            outfile.WriteLine('1');
                        }
                        else
                        {
                            Console.WriteLine('0');
                            outfile.WriteLine('0');
                        }
                    }

                    //output of sdnf
                    Console.WriteLine("sdnf:");
                    outfile.WriteLine("sdnf:");

                    uint cnt = 0;
                    foreach (var val in sdnf)
                    {
                        if (cnt++ == 0)
                        {
                            Console.Write("({0})", val);
                            outfile.Write("({0})", val);
                        }
                        else
                        {
                            Console.Write(" + ({0})", val);
                            outfile.Write(" + ({0})", val);
                        }
                    }
                    Console.Write(" = 1");
                    outfile.Write(" = 1");

                    Console.WriteLine();
                    outfile.WriteLine();

                    //output of sknf
                    Console.WriteLine("sknf: ");
                    outfile.WriteLine("sknf: ");

                    cnt = 0;
                    foreach (var val in sknf)
                    {
                        if (cnt++ == 0)
                        {
                            Console.Write("({0})", val);
                            outfile.Write("({0})", val);
                        }
                        else
                        {
                            Console.Write(" * ({0})", val);
                            outfile.Write(" * ({0})", val);
                        }
                    }
                    Console.Write(" = 0\n\n");
                    outfile.Write(" = 0\n\n");
                }
                file.Close();
                outfile.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
