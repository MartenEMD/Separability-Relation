# Separability-Relation
This program can be used to minimize a Deterministic Finite Automaton (DFA).  
The program takes an DFA as input and outputs a tabulated equivalence relation.

## How to use
To use the program, you need a C# compiler, because the DFA has to be defined inside the source code.
1. Inside the Main method, set the amount of states your DFA has and write this amount into the array
2. Define each state by following this syntax    
   states[*index*] = new State(*stateName*, new Tuple<char, string>(*transitionLetter*, *transitionState*), *other connections*);  
   The end state needs to be declard with *true* as second parameter
4. Set all transition letters in the char array in the Print2DArray method call
5. Compile and run the program

### Example
![DFA](https://github.com/MartenEMD/Separability-Relation/blob/master/Documentation/DFA.png)  
This DFA gets described by this code:  
```C#
State[] states = new State[5];
states[0] = new State("q0", new Tuple<char, string>('a', "q2"), new Tuple<char, string>('b', "q1"));
states[1] = new State("q1", new Tuple<char, string>('a', "q2"), new Tuple<char, string>('b', "q1"));
states[2] = new State("q2", new Tuple<char, string>('a', "q4"), new Tuple<char, string>('b', "q1"));
states[3] = new State("q3", new Tuple<char, string>('a', "q4"), new Tuple<char, string>('b', "q3"));
states[4] = new State("q4", true, new Tuple<char, string>('a', "q4"), new Tuple<char, string>('b', "q3"));
Print2DArray(Minimize(new char[] { 'a', 'b'}, states));
```
That results in this tabulated equivalence relation:  
![Tabulated-Equivalence-Relation](https://github.com/MartenEMD/Separability-Relation/blob/master/Documentation/Tabulated-Equivalence-Relation.png)
