using System;
using System.Collections.Concurrent;
using System.Data.Entity;

namespace SIGCOMT.Persistence
{
    public class MessageDispatcher : IMessageDispatcher
    {
        public static BlockingCollection<string> QueriesQueue;

        private readonly DbContext _instanceDbContext;

        public MessageDispatcher(DbContext instanceDbContext)
        {
            QueriesQueue = new BlockingCollection<string>(new ConcurrentQueue<string>());
            RelaseContext = true;
            _instanceDbContext = instanceDbContext;
        }

        public bool RelaseContext { get; set; }

        public void HandleCommand(Action action)
        {
            if (RelaseContext)
            {
                using (_instanceDbContext)
                {
                    ActionSaveChanges(action, _instanceDbContext);
                }
            }
            else
            {
                ActionSaveChanges(action, _instanceDbContext);
            }
        }

        public T HandleQuery<T>(Func<T> action)
        {
            T local2 = action();
            return local2;
        }

        private static void ActionSaveChanges(Action action, DbContext instance)
        {
            action();
            instance.SaveChanges();
        }
    }
}