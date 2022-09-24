internal class Program
{
    static void Main(string[] args)
    {
        // hello world with a wait for key press on the end
        var testStr = "++++++++[>++++[>++>+++>+++>+<<<<-]>+>+>->>+[<]<-]>>.>---.+++++++..+++.>>.<-.<.+++.------.--------.>>+.>++.,";

        //if (args.Length == 0)
        //{
        //    Console.WriteLine("No code specified. Supply the brainfuck code as the sole command line parameter to run it.");
        //    return;
        //}

        Execute(testStr);
    }

    static void Execute(string code)
    {
        var codeAsArray = code.ToCharArray();

        var currentCell = new DataCell();
        var loopIndexStack = new Stack<int>();

        for (int pc = 0; pc < codeAsArray.Length; pc++)
        {
            switch (codeAsArray[pc])
            {
                case '+':
                    currentCell.Data++;
                    break;
                case '-':
                    currentCell.Data--;
                    break;
                case '>':
                    currentCell.Right ??= new DataCell() { Left = currentCell };
                    currentCell = currentCell.Right;
                    break;
                case '<':
                    currentCell.Left ??= new DataCell() { Right = currentCell };
                    currentCell = currentCell.Left;
                    break;
                case '.':
                    Console.Write(currentCell.Data);
                    break;
                case ',':
                    currentCell.Data = (char)Console.Read();
                    break;
            }

            if (codeAsArray[pc] == '[')
            {
                loopIndexStack.Push(pc);
                if (currentCell.Data == 0) // Current cell of 0 at loop start [ means skip loop
                {
                    // If there are nested loops, we can't just find the next ] so we need to know how many we need to skip by counting them
                    var closingBracesToSkip = 0;

                    // Find closing loop brace to jump to
                    for (int i = pc + 1; i < codeAsArray.Length; i++) // Add 1 to pc for starting point to skip initial [
                    {
                        if (codeAsArray[i] == '[')
                        {
                            closingBracesToSkip++; // Found the start of a nested loop, so we will need to skip a ]
                        }
                        else if (codeAsArray[i] == ']')
                        {
                            if (closingBracesToSkip == 0) // We don't have a ] to skip and we've found a ] so that must be the end of our loop
                            {
                                pc = i;
                                loopIndexStack.Pop();
                                break;
                            }
                            closingBracesToSkip--; // We've reached the end of a nested loop, so can remove that ] from the to-skip count
                        }
                    }
                }
            }
            else if (codeAsArray[pc] == ']')
            {
                pc = loopIndexStack.Pop() - 1; // Subtract 1 because when we continue; the code parsing loop will increment pc by 1
                continue;
            }
        }
    }
}

internal record DataCell
{
    public DataCell? Left;
    public DataCell? Right;
    public char Data;
}