namespace Brainfuck;

internal class Interpreter
{
    /// <summary>
    /// Runs a brainfuck program
    /// </summary>
    /// <param name="code">The brainfuck code to run</param>
    public static void Execute(string code)
    {
        var codeAsArray = code.ToCharArray();

        var currentCell = new DataCell(); // The cell currently pointed to by the brainfuck program. 
        var loopIndexStack = new Stack<int>(); // A stack of loop start position indexes, used to know where to jump back to when the end of the loop body is reached

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
                    bool foundClosingBrace = false;
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
                                foundClosingBrace = true;
                                break;
                            }
                            closingBracesToSkip--; // We've reached the end of a nested loop, so can remove that ] from the to-skip count
                        }
                    }
                    if (!foundClosingBrace)
                    {
                        throw new InvalidProgramException(
                            $"Loop start brace '[' at position {pc} has no matching closing brace.");
                    }
                }
            }
            else if (codeAsArray[pc] == ']')
            {
                if (!loopIndexStack.Any())
                {
                    throw new InvalidProgramException(
                        $"Loop closing brace ']' at position {pc} was found without a corresponding start loop brace.");
                }

                pc = loopIndexStack.Pop() - 1; // Subtract 1 because when we continue; the code parsing loop will increment pc by 1
                continue;
            }
        }

        if (loopIndexStack.Any())
        {
            throw new InvalidProgramException(
                $"Loop starting brace(s) '[' without corresponding closing brace(s) found at positions {string.Join(", ", loopIndexStack)}.");
        }
    }

    /// <summary>
    /// Represents one cell in the 1 dimensional list of characters navigated by the brainfuck
    /// program. Essentially represents one item in a doubly linked list.
    /// </summary>
    private class DataCell
    {
        public DataCell? Left;
        public DataCell? Right;
        public char Data;
    }
}
