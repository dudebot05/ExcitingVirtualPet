using System;
using System.Collections.Generic;
using System.Text;

namespace ExcitingVirtualPetCore
{
    interface IObserver
    {
        void Update(int hunger, int thirst, int boredom, int affection, int tiredness, int currentFood, int currentWater);
    }
}
