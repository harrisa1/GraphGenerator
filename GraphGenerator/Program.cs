using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GraphGenerator
{
    class Program
    {
        private static string probtype;
        private static string directed;
        private static int numNodes;
        private static int numEdges;
        private static int maxWeight;
        private static int minWeight;
        private static string outputFile;
        private static List<string> exitCodes = new List<string> { "e", "y", "exit", "yes", "q", "quit", "done" };

        private static string GetLine(int[,] mat)
        {
            var line = "";
            int start = GetStart(), end = GetEnd(), weight = 0;
            bool notFound = true;
            while (notFound)
            {
                if (directed.ToLower().Equals("y"))
                {
                    if (start != end && mat[start, end] != 1)
                    {
                        notFound = false;
                        mat[start, end] = 1;
                        break;
                    }
                }
                else
                {
                    if (start != end && mat[start, end] != 1 && mat[end, start] != 1)
                    {
                        notFound = false;
                        mat[start, end] = 1;
                        break;
                    }
                }
                start = GetStart();
                end = GetEnd();
            }
            weight = GetWeight();
            if (directed.ToLower().Equals("y"))
            {
                line = "a " + start + " " + end + " " + weight;
            }
            else
            {
                line = "e " + start + " " + end + " " + weight;
            }
            return line;
        }

        private static int GetWeight()
        {
            Random rand = new Random();
            return rand.Next(minWeight,maxWeight+1);
        }

        private static int GetStart()
        {
            Random rand = new Random();
            return rand.Next(1, numNodes+1);
        }

        private static int GetEnd()
        {
            Random rand = new Random();
            return rand.Next(1, numNodes+1);
        }

        private static void GetInputs()
        {
            Console.WriteLine("What type of problem? (put whatever you want to parse here (sp, min, max, etc.))");
            probtype = Console.ReadLine();

            Console.WriteLine("Is it directed (y,n)?");
            directed = Console.ReadLine();

            Console.WriteLine("How many nodes?");
            numNodes = int.Parse(Console.ReadLine());

            Console.WriteLine("How many arcs/edges?");
            numEdges = int.Parse(Console.ReadLine());
            while (numEdges > numNodes && !directed.ToLower().Equals("y"))
            {
                Console.WriteLine("Too many edges for an undirected graph.");
                Console.WriteLine("How many edges?");
                numEdges = int.Parse(Console.ReadLine());
            }
            while (numEdges > numNodes*(numNodes-1) && directed.ToLower().Equals("y"))
            {
                Console.WriteLine("Too many arcs for a directed graph.");
                Console.WriteLine("How many arcs?");
                numEdges = int.Parse(Console.ReadLine());
            }

            Console.WriteLine("What is the maximum weight?");
            maxWeight = int.Parse(Console.ReadLine());
            Console.WriteLine("What is the minimum weight?");
            minWeight = int.Parse(Console.ReadLine());

            Console.WriteLine("Output file:");
            outputFile = Console.ReadLine();
        }

        static void Main(string[] args)
        {
            while (true)
            {
                GetInputs();

                int[,] mat = new int[numNodes + 1, numNodes + 1];

                using (StreamWriter file = new StreamWriter(outputFile))
                {
                    file.WriteLine("c " + probtype + " problem with " + numNodes + " nodes and " + numEdges);
                    file.WriteLine("c with maximum weight of " + maxWeight + " and minimum weight of " + minWeight);
                    file.WriteLine("p " + probtype + " " + numNodes + " " + numEdges);

                    for (int i = 0; i < numEdges; i++)
                    {
                        file.WriteLine(GetLine(mat));
                    }
                }
                Console.WriteLine("Done?");
                var done = Console.ReadLine();
                if (exitCodes.Contains(done)) 
                {
                    break;
                }
            }
        }
    }
}
