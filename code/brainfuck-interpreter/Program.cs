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

        try
        {
            Brainfuck.Interpreter.Execute(testStr);
        }
        catch (Brainfuck.InvalidProgramException ex)
        {
            Console.WriteLine(
                $"The specified program code is invalid:\n" +
                $"{ex.Message}");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"An unexpected error occurred:\n" +
                $"Message: {ex.Message}\n" +
                $"Stack trace: {ex.StackTrace}");
            Console.ReadKey();
        }
    }
}