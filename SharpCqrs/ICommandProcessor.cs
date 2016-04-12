using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharpCqrs
{
    public interface ICommandProcessor
    {
        Task ProcessAsync(ProcessCommandContext context, CancellationToken ct);
    }

    public class ProcessCommandContext
    {
        public ProcessCommandContext(object command, string commandType, ProcessCommandStep step = ProcessCommandStep.Start)
        {
            Command = command;
            CommandType = commandType;
            Step = step;
        }

        public object Command { get; }
        public string CommandType { get; }

        public bool HasFailed { get; }

        public ProcessCommandStep Step { get; }

        public IEnumerable<ValidationMessage> Messages { get; }

        public void NextStep()
        {
            throw new NotImplementedException();
        }

        public void AddMessages(IEnumerable<ValidationMessage> messages)
        {
            throw new NotImplementedException();
        }
    }

    public enum ProcessCommandStep
    {
        Start,
        CommandValidation,
        Precondition,
        Execution,
        PostCondition,
        Persistence,
        End,
    }
}
