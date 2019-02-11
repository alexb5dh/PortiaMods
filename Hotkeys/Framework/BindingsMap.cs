using System;
using System.Linq.Expressions;
using InControl;

namespace Hotkeys
{
    public class BindingsMap<TInput>: Multimap<Expression<Func<TInput, PlayerAction>>, BindingSource>
        where TInput: PlayerActionSet
    {
        public void Add(Expression<Func<TInput, PlayerAction>> actionReference, params Key[] key) => Add(actionReference, new KeyBindingSource(key));

        public void Apply(TInput input)
        {
            foreach (var pair in this)
            {
                if (pair.Key.TryGetPropertyInfo()?.GetValue(input, null) is PlayerAction action)
                {
                    foreach (var binding in pair.Value) action.AddBinding(binding);
                }
            }
        }
    }
}