using System;
using System.Collections.Concurrent;
using System.Data.Entity;
using StructureMap;

namespace OSSE.Persistence
{
    public class MessageDispatcher
    {
        public static BlockingCollection<string> QueriesQueue;
        public bool RelaseContext { get; set; }

        public MessageDispatcher()
        {
            QueriesQueue = new BlockingCollection<string>(new ConcurrentQueue<string>());
            RelaseContext = true;
        }

        public void HandleCommand(Action action)
        {
            var instance = ObjectFactory.GetInstance<DbContext>();

            if (RelaseContext)
            {
                using (instance)
                {
                    ActionSaveChanges(action, instance);
                }
            }
            else
            {
                ActionSaveChanges(action, instance);
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