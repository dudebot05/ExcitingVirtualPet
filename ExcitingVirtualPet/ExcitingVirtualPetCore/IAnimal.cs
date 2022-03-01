using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ExcitingVirtualPetCore
{
    interface IAnimal : ISubject
    {
        void InitializeAnimal(
            Random generator,
            DispatcherTimer mainLoopTimer,
            Button petFeedButton,
            Button petWaterButton,
            Button petPlayButton,
            Button petPatButton,
            Image petImage
            );
        void InitializeFrames();
        void ConnectButtons(Random generator, DispatcherTimer mainLoopTimer, Button petFeedButton, Button petWaterButton, Button petPlayButton, Button petPatButton, Image petImage);
        void InitializeFood();
        void InitializeWater();
        void increaseNeedCounters();
        void increaseHunger();
        void increaseThirst();
        void increaseBoredom();
        void decreaseAffection();
        void increaseTiredness();
        void decreaseTiredness();
        void tryToDrink();
        void tryToEat();
        void maybeTakeAnimalAway();
        void Feed();
        void Water();
        void Play();
        void Pat();
        void MainLoopTimer_Tick(object sender, EventArgs e);
        void Save(SaveFileDialog saveDialog);
        void Dispose();
    }
}
