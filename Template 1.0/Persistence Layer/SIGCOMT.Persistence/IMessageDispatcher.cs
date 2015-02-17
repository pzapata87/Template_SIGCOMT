using System;

namespace SIGCOMT.Persistence
{
    public interface IMessageDispatcher
    {
        bool RelaseContext { get; set; }
        void HandleCommand(Action action);
        T HandleQuery<T>(Func<T> action);
    }
}