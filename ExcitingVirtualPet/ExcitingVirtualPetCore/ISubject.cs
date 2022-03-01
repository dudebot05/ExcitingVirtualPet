using System;
using System.Collections.Generic;
using System.Text;

namespace ExcitingVirtualPetCore
{
    interface ISubject
    {
        void Register(IObserver o);
        void Unregister(IObserver o);
        void NotifyObservers(int hunger, int thirst, int boredom, int affection, int tiredness, int currentFood, int currentWater);
    }
}
