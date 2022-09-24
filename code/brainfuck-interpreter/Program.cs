internal class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Invalid command line arguments. Supply the brainfuck code as the sole command line parameter to run it.");
            return;
        }

        var code = string.Join(string.Empty, args);

        try
        {
            Brainfuck.Interpreter.Execute(code);
        }
        catch (Brainfuck.InvalidProgramException ex)
        {
            Console.WriteLine(
                $"\nThe specified program code is invalid:\n" +
                $"{ex.Message}");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"\nAn unexpected error occurred:\n" +
                $"Message: {ex.Message}\n" +
                $"Stack trace: {ex.StackTrace}");
            Console.ReadKey();
        }
    }
}