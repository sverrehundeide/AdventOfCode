var circularDigitsList = GetCircularLinkedListWitDigits1To100();
circularDigitsList.MoveToDigit(50);

const int digitToCount = 0;
var numberOfTimesAtZero = File.ReadAllLines("input.txt")
    .Select(instructionString => circularDigitsList.Move(instructionString))
    .Count(resultAfterMove => resultAfterMove == digitToCount);

Console.WriteLine($"The code is : {numberOfTimesAtZero}");
return;

static DialDigitsList GetCircularLinkedListWitDigits1To100()
{
    var circleWithDigits = new DialDigitsList();
    foreach (var index in Enumerable.Range(0, 100))
    {
        var digit = new Digit(index);
        if (circleWithDigits.Count > 0)
        {
            var lastDigit = circleWithDigits.Last();
            lastDigit.Next = digit;
            digit.Previous = lastDigit;
        }
        circleWithDigits.AddLast(digit);
    }

    circleWithDigits.Last().Next = circleWithDigits.First();
    circleWithDigits.First().Previous = circleWithDigits.Last();
    return circleWithDigits;
}


internal class Digit(int value)
{
    public int Value { get; } = value;
    public Digit? Next { get; set; }
    public Digit? Previous { get; set; }

    public static Digit Undefined = new(int.MaxValue);
}

internal class DialDigitsList : LinkedList<Digit>
{
    public Digit Current { get; private set; } = Digit.Undefined;

    public void MoveToDigit(int digit)
    {
        var matchingDigit = this.FirstOrDefault(node => node.Value == digit);
        Current = matchingDigit ?? throw new Exception($"Found no digit node with value {digit}");
    }

    public int MoveRight()
    {
        Current = Current.Next!;
        return Current.Value;
    }

    public int MoveLeft()
    {
        Current = Current.Previous!;
        return Current.Value;
    }

    public int Move(string movementInstructionString)
    {
        var movementInstruction = MovementInstruction.Parse(movementInstructionString);
        for (var i = 0; i < movementInstruction.Distance; i++)
        {
            if (movementInstruction.Direction == Direction.Right)
            {
                MoveRight();
            }
            else
            {
                MoveLeft();
            }
        }

        return Current.Value;
    }
}

internal class MovementInstruction
{
    private static readonly Regex InstructionParser = new("(?<direction>[LR])(?<distance>\\d{1,3})");

    public Direction Direction { get; init; }
    public int Distance { get; init; }

    private MovementInstruction(Direction direction, int distance)
    {
        Direction = direction;
        Distance = distance;
    }

    public static MovementInstruction Parse(string stringValue)
    {
        var parsed = InstructionParser.Match(stringValue);
        var parsedDirection = parsed.Groups["direction"].Value == "L" ? Direction.Left : Direction.Right;
        var parsedDistance = int.Parse(parsed.Groups["distance"].Value);
        return new MovementInstruction(parsedDirection, parsedDistance);
    }
}

internal enum Direction
{
    Left, Right
}