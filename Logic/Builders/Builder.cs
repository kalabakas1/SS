using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Interfaces;

namespace MPE.SS.Logic.Builders
{
    internal class Builder<T> : IBuilder<T>
    {
        private List<Action<T>> _actions;

        public Builder()
        {
            _actions = new List<Action<T>>();
        }

        public IBuilder<T> Where(Action<T> action)
        {
            _actions.Add(action);
            return this;
        }

        public T Build()
        {
            var obj = (T)Activator.CreateInstance(typeof(T), BindingFlags.Public | BindingFlags.Default | BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null);
            foreach (var action in _actions)
            {
                action.Invoke(obj);
            }
            _actions = new List<Action<T>>();
            return obj;
        }
    }
}
