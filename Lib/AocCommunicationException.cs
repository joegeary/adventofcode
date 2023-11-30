using System.Net;
using System.Text;

namespace AdventOfCode;

public class AocCommuncationException(string Title, HttpStatusCode? Status = null, string Details = "") : Exception
{
    public override string Message
    {
        get
        {
            var sb = new StringBuilder();
            sb.AppendLine(Title);
            if (Status != null)
            {
                sb.Append($"[{Status}] ");
            }
            sb.AppendLine(Details);
            return sb.ToString();
        }
    }
}
