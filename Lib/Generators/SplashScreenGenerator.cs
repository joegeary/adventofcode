using System.Text;
using AdventOfCode.Models;

namespace AdventOfCode.Generators;

internal class SplashScreenGenerator
{
    public string Generate(Calendar calendar)
    {
        var calendarPrinter = CalendarPrinter(calendar);

        return $$"""
                namespace AdventOfCode.Y{{calendar.Year}};

                internal class SplashScreenImpl : ISplashScreen
                {

                    public void Show()
                    {
                        var color = Console.ForegroundColor;
                        {{calendarPrinter.Indent(12)}}
                        Console.ForegroundColor = color;
                        Console.WriteLine();
                    }

                    private static void Write(int rgb, bool bold, string text)
                    {
                        Console.Write($"\u001b[38;2;{(rgb>>16)&255};{(rgb>>8)&255};{rgb&255}{(bold ? ";1" : "")}m{text}");
                    }
                }
                """;
    }

    private string CalendarPrinter(Calendar calendar)
    {
        var lines = calendar.Lines.Select(line =>
            new[] { new CalendarToken { Text = "           " } }.Concat(line)).ToList();

        var bw = new BufferWriter();
        foreach (var line in lines)
        {
            foreach (var token in line)
            {
                bw.Write(token.ConsoleColor, token.Text, token.Bold);
            }

            bw.Write(-1, "\n", false);
        }
        return bw.GetContent();
    }

    private class BufferWriter
    {
        private readonly StringBuilder sb = new();
        private int bufferColor = -1;
        private string buffer = "";
        private bool bufferBold;

        public void Write(int color, string text, bool bold)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                if (!string.IsNullOrWhiteSpace(buffer) && (color != bufferColor || this.bufferBold != bold))
                {
                    Flush();
                }
                bufferColor = color;
                bufferBold = bold;
            }
            buffer += text;
        }

        private void Flush()
        {
            while (buffer.Length > 0)
            {
                var block = buffer.Substring(0, Math.Min(100, buffer.Length));
                buffer = buffer.Substring(block.Length);
                block = block.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n");
                sb.AppendLine($@"Write(0x{bufferColor.ToString("x")}, {bufferBold.ToString().ToLower()}, ""{block}"");");
            }
            buffer = "";
        }

        public string GetContent()
        {
            Flush();
            return sb.ToString();
        }
    }
}
