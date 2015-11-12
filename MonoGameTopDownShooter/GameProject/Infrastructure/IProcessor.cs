using System;

namespace GameProject.Infrastructure
{
    public interface IProcessor<out TItem>
    {
        void Process(Action<TItem> item);
    }
}