namespace Brainfuck;

/// <summary>
/// Exception thrown when a brainfuck program is invalid
/// </summary>
internal class InvalidProgramException : Exception
{
    public InvalidProgramException(string message) : base(message) { }
}
