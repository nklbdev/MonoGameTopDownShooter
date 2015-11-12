using System;

namespace GameProject
{
    public interface IProcessor<out TItem>
    {
        void Process(Action<TItem> item);
    }
}