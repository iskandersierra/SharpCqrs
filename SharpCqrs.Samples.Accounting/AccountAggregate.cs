using System.Collections.Generic;

namespace SharpCqrs.Samples.Accounting
{
    public class Account
    {
        public IEnumerable<ValidationMessage> Requires(AccountCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.AccountId))
                yield return ValidationMessage.Error("Account id cannot be empty", nameof(command.AccountId));
        }
        public IEnumerable<ValidationMessage> Requires(AccountCommand command, AccountState before)
        {
            if (command.AccountId != before.AccountId)
                yield return ValidationMessage.Error("This is not the right account", nameof(command.AccountId), nameof(before.AccountId));
        }
        public IEnumerable<ValidationMessage> Requires(Create command)
        {
            if (string.IsNullOrWhiteSpace(command.Owner))
                yield return ValidationMessage.Error("Owner cannot be empty", nameof(command.Owner));
        }
        public IEnumerable<ValidationMessage> Requires(Deposit command)
        {
            if (command.Amount >= 0.01)
                yield return ValidationMessage.Error("Deposit amount must be positive", nameof(command.Amount));
        }
        public IEnumerable<ValidationMessage> Requires(Withdraw command)
        {
            if (command.Amount >= 0.01)
                yield return ValidationMessage.Error("Deposit amount must be positive", nameof(command.Amount));
        }
        public IEnumerable<ValidationMessage> Requires(Withdraw command, OpenState before)
        {
            if (command.Amount <= before.Balance)
                yield return ValidationMessage.Error("Deposit amount must be positive", nameof(command.Amount), nameof(before.Balance));
        }
        public IEnumerable<ValidationMessage> Requires(Close command, OpenState before)
        {
            if (before.Balance == 0.0)
                yield return ValidationMessage.Error("Cannot withdraw more than the available account balance", nameof(before.Balance));
        }

        public IEnumerable<ValidationMessage> Ensures(OpenState before, OpenState after)
        {
            if (after.AccountId != before.AccountId)
                yield return ValidationMessage.Error("AccountId cannot be changed while the account is open", nameof(after.AccountId));
            if (after.Owner != before.Owner)
                yield return ValidationMessage.Error("Owner cannot be changed while the account is open", nameof(after.Owner));
        }
        public IEnumerable<ValidationMessage> Ensures(OpenState after)
        {
            if (!string.IsNullOrWhiteSpace(after.AccountId))
                yield return ValidationMessage.Error("AccountId cannot be empty", nameof(after.AccountId));
            if (!string.IsNullOrWhiteSpace(after.Owner))
                yield return ValidationMessage.Error("Owner cannot be empty", nameof(after.Owner));
        }
        public IEnumerable<ValidationMessage> Ensures(Create command, OpenState after)
        {
            if (after.AccountId != command.AccountId)
                yield return ValidationMessage.Error("AccountId must match the command", nameof(after.AccountId));
            if (after.Owner != command.Owner)
                yield return ValidationMessage.Error("Owner must match the command", nameof(after.Owner));
            if (after.Balance != 0.0)
                yield return ValidationMessage.Error("Balance must be zero when an account is created", nameof(after.Balance));
        }
        public IEnumerable<ValidationMessage> Ensures(Deposit command, OpenState before, OpenState after)
        {
            if (after.Balance == before.Balance + command.Amount)
                yield return ValidationMessage.Error("Balance must be increased by given amount", nameof(after.Balance));
        }
        public IEnumerable<ValidationMessage> Ensures(Withdraw command, OpenState before, OpenState after)
        {
            if (after.Balance == before.Balance - command.Amount)
                yield return ValidationMessage.Error("Balance must be decreased by given amount", nameof(after.Balance));
        }

        public IEnumerable<object> When(Create command)
        {
            yield return new Created(command.AccountId, command.Owner);
        }
        public IEnumerable<object> When(Deposit command, OpenState state)
        {
            yield return new Deposited(command.AccountId, command.Amount);
        }
        public IEnumerable<object> When(Withdraw command, OpenState state)
        {
            yield return new Withdrawn(command.AccountId, command.Amount);
        }
        public IEnumerable<object> When(Closed command, OpenState state)
        {
            yield return new Closed(command.AccountId);
        }

        public OpenState On(Created e)
        {
            return new OpenState(e.AccountId, e.Owner, 0.0);
        }
        public OpenState On(Deposited e, OpenState state)
        {
            return new OpenState(state.AccountId, state.Owner, state.Balance + e.Amount);
        }
        public OpenState On(Withdrawn e, OpenState state)
        {
            return new OpenState(state.AccountId, state.Owner, state.Balance - e.Amount);
        }
        public ClosedState On(Closed e, OpenState state)
        {
            return new ClosedState(state.AccountId, state.Owner);
        }

    }

    #region [ States ]

    public abstract class AccountState
    {
        protected AccountState(string accountId, string owner)
        {
            AccountId = accountId;
            Owner = owner;
        }

        public string AccountId { get; }
        public string Owner { get; }
    }

    public class OpenState : AccountState
    {
        public OpenState(string accountId, string owner, double balance) : base(accountId, owner)
        {
            Balance = balance;
        }

        public double Balance { get; }
    }

    public class ClosedState : AccountState
    {
        public ClosedState(string accountId, string owner) : base(accountId, owner)
        {
        }
    }

    #endregion

    #region [ Commands ]

    public abstract class AccountCommand
    {
        protected AccountCommand(string accountId)
        {
            AccountId = accountId;
        }

        public string AccountId { get; }
    }

    public class Create : AccountCommand
    {
        public Create(string accountId, string owner) : base(accountId)
        {
            Owner = owner;
        }

        public string Owner { get; }
    }

    public class Deposit : AccountCommand
    {
        public Deposit(string accountId, double amount) : base(accountId)
        {
            Amount = amount;
        }

        public double Amount { get; set; }
    }

    public class Withdraw : AccountCommand
    {
        public Withdraw(string accountId, double amount) : base(accountId)
        {
            Amount = amount;
        }

        public double Amount { get; set; }
    }

    public class Close : AccountCommand
    {
        public Close(string accountId) : base(accountId)
        {
        }
    }

    #endregion

    #region [ Events ]

    public abstract class AccountEvent
    {
        protected AccountEvent(string accountId)
        {
            AccountId = accountId;
        }

        public string AccountId { get; }
    }

    public class Created : AccountEvent
    {
        public Created(string accountId, string owner) : base(accountId)
        {
            Owner = owner;
        }

        public string Owner { get; }
    }

    public class Deposited : AccountEvent
    {
        public Deposited(string accountId, double amount) : base(accountId)
        {
            Amount = amount;
        }

        public double Amount { get; set; }
    }

    public class Withdrawn : AccountEvent
    {
        public Withdrawn(string accountId, double amount) : base(accountId)
        {
            Amount = amount;
        }

        public double Amount { get; set; }
    }

    public class Closed : AccountEvent
    {
        public Closed(string accountId) : base(accountId)
        {
        }
    }

    #endregion
}
