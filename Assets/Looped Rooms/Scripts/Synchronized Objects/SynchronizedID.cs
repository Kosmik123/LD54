using System;
using System.Collections.Generic;

namespace Bipolar.LoopedRooms
{
    public class SynchronizedID : UniqueID
    {
        private readonly Dictionary<Type, Action> synchronizeActions = new Dictionary<Type, Action>();

        public void AddAction(Type type, Action action)
        {
            if (synchronizeActions.ContainsKey(type))
            {
                synchronizeActions[type] += action;
            }
            else
            {
                synchronizeActions.Add(type, action);
            }
        }

        public void RemoveAction(Type type, Action action)
        {
            synchronizeActions[type] -= action;
        }
    }
}
