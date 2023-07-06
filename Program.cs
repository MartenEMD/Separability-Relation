using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;

public class SeparabilityRelation
{
    public struct State
    {
        public string name;
        public bool isFinalState;
        public Dictionary<char, string> transitions;

        public State(string name, Dictionary<char, string> transitions)
        {
            this.name = name;
            isFinalState = false;
            this.transitions = transitions;
        }

        public State(string name, Dictionary<char, string> transitions, bool isFinalState)
        {
            this.name = name;
            this.isFinalState = isFinalState;
            this.transitions = transitions;
        }

        public State(string name, params Tuple<char, string>[] transitions) 
        {
            this.name = name;
            isFinalState= false;
            this.transitions = transitions.ToDictionary(x => x.Item1, x => x.Item2);
        }

        public State(string name, bool isFinalState, params Tuple<char, string>[] transitions)
        {
            this.name = name;
            this.isFinalState = isFinalState;
            this.transitions = transitions.ToDictionary(x => x.Item1, x => x.Item2);
        }

        public string GetTransition(char key)
        {
            return transitions[key];
        }
    }

    public static void Main(string[] args)
    {
        State[] states = new State[5];
        states[0] = new State("q0", new Tuple<char, string>('a', "q2"), new Tuple<char, string>('b', "q1"));
        states[1] = new State("q1", new Tuple<char, string>('a', "q2"), new Tuple<char, string>('b', "q1"));
        states[2] = new State("q2", new Tuple<char, string>('a', "q4"), new Tuple<char, string>('b', "q1"));
        states[3] = new State("q3", new Tuple<char, string>('a', "q4"), new Tuple<char, string>('b', "q3"));
        states[4] = new State("q4", true, new Tuple<char, string>('a', "q4"), new Tuple<char, string>('b', "q3"));
        Print2DArray(Minimize(new char[] { 'a', 'b'}, states));

        Console.Read();
    }

    public static bool?[,] Minimize(char[] alphabet, State[] states)
    {
        if (!Array.Exists(states, x => x.isFinalState))
            throw new ArgumentException("There must be at least one state wich is a final state.");

        bool?[,] table = new bool?[states.Length, states.Length];
        
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].isFinalState)
            {
                for (int x = 0; x < i; x++)
                    table[x, i] = true;
                for (int x = i + 1; x < states.Length; x++)
                    table[i, x] = true;
            }
            table[i, i] = null;
        }


        char currentLetter = (char) 0;
        bool crossSet = true;
        while (true)
        {
            for (int column = 0; column < states.Length - 1; column++)
            {
                for (int row = column + 1; row < states.Length; row++)
                {
                    // Initialise table
                    if (currentLetter == 0 && table[column, row] == null)
                    {
                        table[column, row] = false;
                        continue;
                    }

                    if(table[column, row] != false)
                    {
                        continue;
                    }

                    table[column, row] = false;

                    int first = Array.FindIndex(states, x => x.name == states[column].GetTransition(currentLetter));
                    int second = Array.FindIndex(states, x => x.name == states[row].GetTransition(currentLetter));

                    if (table[first, second] == true || table[second, first] == true)
                    {
                        table[column, row] = true;
                        crossSet = true;
                    }
                }
            }


            if (currentLetter == 0)
            {
                currentLetter = alphabet[0];
            }
            else if (currentLetter == alphabet[alphabet.Length - 1] && !crossSet)
            {
                break;
            }
            else if (currentLetter == alphabet[alphabet.Length - 1])
            {
                currentLetter = alphabet[0];
                crossSet = false;
            }
            else
            {
                int alphabetIndex = Array.FindIndex(alphabet, x => x == currentLetter);
                if (++alphabetIndex >= alphabet.Length)
                {
                    currentLetter = alphabet[0];
                }
                else
                {
                    currentLetter = alphabet[alphabetIndex];
                }
            }
        }

        return table;
    }


    public static void Print2DArray<T>(T[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                switch(matrix[i, j])
                {
                    case null:
                        Console.Write("-  ");
                        break;
                    case true:
                        Console.Write("x  ");
                        break;
                    case false:
                        Console.Write("   ");
                        break;
                }
            }
            Console.WriteLine();
        }
    }
}
