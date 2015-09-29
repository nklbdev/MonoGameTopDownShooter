using System;
using System.Collections.Generic;

namespace SimplestIocContainer
{
    public class Container
    {
        private class Binding
        {
            public Func<Container, object> Resolver { get; set; }
            public bool IsSingleInstance { get; set; }
            public object ResolvedSingleInstance { get; set; }
        }

        private readonly Dictionary<object, Binding> _bindings = new Dictionary<object, Binding>();

        public Container Bind(object key, Func<Container, object> resolver, bool isSingleInstance = true)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (resolver == null)
                throw new ArgumentNullException("resolver");
            if (_bindings.ContainsKey(key))
                throw new InvalidOperationException(string.Format("IoC: Key \"{0}\" is already present", key));
            _bindings[key] = new Binding
            {
                Resolver = resolver,
                IsSingleInstance = isSingleInstance
            };
            return this;
        }

        public Container Bind(object key, Func<object> resolver, bool isSingleInstance = true)
        {
            return Bind(key, c => resolver(), isSingleInstance);
        }

        public Container Bind<T>(Func<Container, T> resolver, bool isSingleInstance = true)
        {
            return Bind(typeof(T), c => resolver(c), isSingleInstance);
        }

        public Container Bind<T>(Func<T> resolver, bool isSingleInstance = true)
        {
            return Bind(c => resolver(), isSingleInstance);
        }

        public T Resolve<T>(object key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            Binding binding;
            if (!_bindings.TryGetValue(key, out binding))
                throw new InvalidOperationException(string.Format("IoC: Cannot resolve key \"{0}\"", key));

            if (!binding.IsSingleInstance)
                return (T)binding.Resolver(this);

            if (binding.ResolvedSingleInstance == null)
                binding.ResolvedSingleInstance = binding.Resolver(this);

            return (T)binding.ResolvedSingleInstance;
        }

        public T Resolve<T>()
        {
            return Resolve<T>(typeof(T));
        }
    }

}
