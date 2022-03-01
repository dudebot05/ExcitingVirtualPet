using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ExcitingVirtualPetCore
{
    class Observer : IObserver
    {
        private ProgressBar hungerMeter;
        private ProgressBar thirstMeter;
        private ProgressBar boredomMeter;
        private ProgressBar affectionMeter;
        private ProgressBar tirednessMeter;
        private ProgressBar waterAmountBar;
        private ProgressBar foodAmountBar;

        public Observer(ProgressBar hungerMeter, ProgressBar thirstMeter, ProgressBar boredomMeter, ProgressBar affectionMeter, ProgressBar tirednessMeter, ProgressBar waterAmountBar,
            ProgressBar foodAmountBar)
        {
            this.hungerMeter = hungerMeter;
            this.thirstMeter = thirstMeter;
            this.boredomMeter = boredomMeter;
            this.affectionMeter = affectionMeter;
            this.tirednessMeter = tirednessMeter;
            this.waterAmountBar = waterAmountBar;
            this.foodAmountBar = foodAmountBar;
        }

        public void Update(int hunger, int thirst, int boredom, int affection, int tiredness, int currentFood, int currentWater)
        {
            hungerMeter.Value = hunger;
            thirstMeter.Value = thirst;
            boredomMeter.Value = boredom;
            affectionMeter.Value = affection;
            tirednessMeter.Value = tiredness;
            foodAmountBar.Value = currentFood;
            waterAmountBar.Value = currentWater;
        }
    }
}
