using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SharpCqrs
{
    public class ValidationMessage
    {
        private static readonly IReadOnlyCollection<string> EmptyMembersCollection = new ReadOnlyCollection<string>(new string[0]);

        public static ValidationMessage Error(string message, params string[] members)
        {
            return new ValidationMessage(message, ValidationLevel.Error, members);
        }

        public static ValidationMessage Warning(string message, params string[] members)
        {
            return new ValidationMessage(message, ValidationLevel.Warning, members);
        }

        public static ValidationMessage Info(string message, params string[] members)
        {
            return new ValidationMessage(message, ValidationLevel.Info, members);
        }

        public ValidationMessage(string message, ValidationLevel level, params string[] members)
        {
            if (members == null || members.Length == 0)
                Members = EmptyMembersCollection;
            else
                Members = new ReadOnlyCollection<string>(members.ToArray());
            Message = message ?? level.ToString();
            Level = level;
        }

        public IReadOnlyCollection<string> Members { get; }
        public string Message { get; }
        public ValidationLevel Level { get; }
        public bool Success => Level > ValidationLevel.Error;
        public bool Failed => Level <= ValidationLevel.Error;
    }
}
