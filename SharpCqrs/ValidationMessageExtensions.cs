using System.Collections.Generic;
using System.Linq;

namespace SharpCqrs
{
    public static class ValidationMessageExtensions
    {
        public static bool HasSucceeded(this IEnumerable<ValidationMessage> messages)
        {
            if (messages == null) return true;

            return messages.All(m => m.Success);
        }

        public static bool HasFailed(this IEnumerable<ValidationMessage> messages)
        {
            if (messages == null) return true;

            return messages.Any(m => m.Failed);
        }

        public static ValidationLevel GetLevel(this IEnumerable<ValidationMessage> messages)
        {
            if (messages == null) return ValidationLevel.Info;

            var level = ValidationLevel.Info;
            foreach (var message in messages)
            {
                if (message.Level < level)
                    level = message.Level;
            }
            return level;
        }
    }
}