// The goal of this app is to find the best 2 starting equations for Nerdle
// The "best" equations will use all of the available numbers and operators
// The "best" equations will have an equals in two different positions - the 6th and 7th spots

using System.Data;

// The System.Data.DataTable class has a Compute feature that we will leverage to check for validity
var computer = new DataTable();

bool IsValid(string equation)
{
    var splitEquation = equation.Split('=');
    var left = splitEquation[0];
    var right = splitEquation[1];
    var leftResult = computer.Compute(left, null).ToString();
    var isValid = leftResult == right;
    return isValid;
}

var numbers = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
var operators = new List<char> { '+', '-', '*', '/' };

List<List<T>> GetPermutations<T>(List<T> list, int length)
{
    return length == 1 ?
        list.Select(t => new List<T> { t }).ToList() :
        GetPermutations(list, length - 1).SelectMany(t => list.Where(e => !t.Contains(e)), (t1, t2) => t1.Concat(new T[] { t2 }).ToList()).ToList();
}

var numberPermutations = GetPermutations(numbers, numbers.Count);
var operatorPermutations = GetPermutations(operators, operators.Count);

var pairCount = 0;

void PairFound(string first, string second)
{
    pairCount++;
    Console.WriteLine($"Valid equation pair {pairCount} found! {first} | {second}");
}

var loopCount = 0;

foreach (var numberPermutation in numberPermutations)
{
    foreach (var operatorPermutation in operatorPermutations)
    {
        loopCount++;

        // Equals in 6th spot - 5 on left, 2 on right
        // 1 possible order
        // NONON=NN
        var first = new string(new[]
        {
            numberPermutation[0],
            operatorPermutation[0],
            numberPermutation[1],
            operatorPermutation[1],
            numberPermutation[2],
            '=',
            numberPermutation[3],
            numberPermutation[4]
        });

        if (!IsValid(first))
        {
            continue;
        }

        // Equals in 7th spot - 6 on left, 1 on right
        // 3 possible orders
        // NONONN=N
        // NNONON=N
        // NONNON=N
        var second1 = new string(new[]
        {
            numberPermutation[5],
            operatorPermutation[2],
            numberPermutation[6],
            operatorPermutation[3],
            numberPermutation[7],
            numberPermutation[8],
            '=',
            numberPermutation[9]
        });
        var second2 = new string(new[]
        {
            numberPermutation[5],
            numberPermutation[6],
            operatorPermutation[2],
            numberPermutation[7],
            operatorPermutation[3],
            numberPermutation[8],
            '=',
            numberPermutation[9]
        });
        var second3 = new string(new[]
        {
            numberPermutation[5],
            operatorPermutation[2],
            numberPermutation[6],
            numberPermutation[7],
            operatorPermutation[3],
            numberPermutation[8],
            '=',
            numberPermutation[9]
        });

        if (IsValid(second1))
        {
            PairFound(first, second1);
        }

        if (IsValid(second2))
        {
            PairFound(first, second2);
        }

        if (IsValid(second3))
        {
            PairFound(first, second3);
        }
    }
}

Console.WriteLine($"Found {pairCount} pairs after {loopCount} loops.");
