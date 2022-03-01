using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ExcitingVirtualPetCore
{
    class Bird : IAnimal
    {
        private const int MAX_HUNGER = 10;
        private const int MIN_HUNGER = 0;
        private const int MAX_AFFECTION = 20;
        private const int MIN_AFFECTION = 0;
        private const int MAX_THIRST = 10;
        private const int MIN_THIRST = 0;
        private const int MAX_BOREDOM = 10;
        private const int MIN_BOREDOM = 0;
        private const int MAX_TIREDNESS = 10;
        private const int MIN_TIREDNESS = 0;

        private const int MAX_FOOD = 10;
        private const int MIN_FOOD = 0;
        private const int MAX_WATER = 10;
        private const int MIN_WATER = 0;

        private int hunger;
        private int affection;
        private int thirst;
        private int boredom;
        private int tiredness;
        private int currentFood;
        private int currentWater;
        private int startEating;
        private int startDrinking;
        private bool currentlyEating = false;
        private bool currentlyDrinking = false;
        private bool currentlySleeping = false;

        private DispatcherTimer mainLoopTimer;
        private Random generator;
        private int hungerFrame;
        private int thirstFrame;
        private int boredomFrame;
        private int affectionFrame;
        private int tirednessFrame;
        private int sleepFrame;
        private int eatFrame;
        private int drinkFrame;
        private int hungerCounter;
        private int thirstCounter;
        private int boredomCounter;
        private int affectionCounter;
        private int tirednessCounter;
        private int sleepCounter;
        private int eatCounter;
        private int drinkCounter;

        private BitmapImage normalImage = new BitmapImage(new Uri("Resources/basic_bird.jpg", UriKind.Relative));
        private BitmapImage leavingImage = new BitmapImage(new Uri("Resources/bird_leaving.jpg", UriKind.Relative));
        private BitmapImage sleepingImage = new BitmapImage(new Uri("Resources/bird_sleeping.jpg", UriKind.Relative));

        private Button petFeedButton;
        private Button petWaterButton;
        private Button petPlayButton;
        private Button petPatButton;
        private Image petImage;

        List<IObserver> observerList = new List<IObserver>();

        public Bird(Load bird)
        {
            hunger = bird.hunger;
            thirst = bird.thirst;
            boredom = bird.boredom;
            affection = bird.affection;
            tiredness = bird.tiredness;
            currentFood = bird.currentFood;
            currentWater = bird.currentWater;
        }

        public Bird() { }

        public void InitializeAnimal(Random generator, DispatcherTimer mainLoopTimer, Button petFeedButton, Button petWaterButton, Button petPlayButton, Button petPatButton, Image petImage)
        {
            this.generator = generator;
            this.mainLoopTimer = mainLoopTimer;
            this.petFeedButton = petFeedButton;
            this.petWaterButton = petWaterButton;
            this.petPlayButton = petPlayButton;
            this.petPatButton = petPatButton;
            this.petImage = petImage;

            this.petImage.Source = normalImage;

            hunger = 5;
            affection = 0;
            thirst = 5;
            boredom = 5;
            tiredness = 0;

            startEating = 6;
            startDrinking = 6;



            currentlySleeping = false;
            currentlyEating = false;
            currentlyDrinking = false;
        }

        public void ConnectButtons(Random generator, DispatcherTimer mainLoopTimer, Button petFeedButton, Button petWaterButton, Button petPlayButton, Button petPatButton, Image petImage)
        {
            this.generator = generator;
            this.mainLoopTimer = mainLoopTimer;
            this.petFeedButton = petFeedButton;
            this.petWaterButton = petWaterButton;
            this.petPlayButton = petPlayButton;
            this.petPatButton = petPatButton;
            this.petImage = petImage;

            this.petImage.Source = normalImage;
        }

        public void InitializeFrames()
        {
            hungerFrame = generator.Next(120, 600);
            thirstFrame = generator.Next(120, 600);
            boredomFrame = generator.Next(120, 600);
            affectionFrame = generator.Next(120, 600);

            tirednessFrame = 60;
            sleepFrame = 30;
            eatFrame = 60;
            drinkFrame = 60;

            hungerCounter = 0;
            thirstCounter = 0;
            boredomCounter = 0;
            affectionCounter = 0;
            tirednessCounter = 0;
            sleepCounter = 0;
            eatCounter = 0;
            drinkCounter = 0;
        }

        public void InitializeFood()
        {
            currentFood = 1;
        }

        public void InitializeWater()
        {
            currentWater = 1;
        }

        public void increaseNeedCounters()
        {
            hungerCounter++;
            thirstCounter++;
            affectionCounter++;
            boredomCounter++;
        }

        public void increaseHunger()
        {
            if (hunger < MAX_HUNGER) hunger++;
            if (hunger > startEating) currentlyEating = true;

            hungerCounter = 0;
        }

        public void increaseThirst()
        {
            if (thirst < MAX_THIRST) thirst++;
            if (thirst > startDrinking) currentlyDrinking = true;

            thirstCounter = 0;
        }

        public void increaseBoredom()
        {
            if (boredom < MAX_BOREDOM) boredom++;

            boredomCounter = 0;
        }

        public void decreaseAffection()
        {
            if (affection > MIN_AFFECTION) affection--;

            affectionCounter = 0;
        }

        public void increaseTiredness()
        {
            if (tiredness < MAX_TIREDNESS) tiredness++;
            if (tiredness == MAX_TIREDNESS) currentlySleeping = true;

            tirednessCounter = 0;
        }

        public void decreaseTiredness()
        {
            if (tiredness > MIN_TIREDNESS) tiredness--;
            if (tiredness == MIN_TIREDNESS) currentlySleeping = false;

            sleepCounter = 0;
        }

        public void tryToDrink()
        {
            if (currentWater > MIN_WATER)
            {
                currentWater--;
                thirst--;
            }

            if (thirst == MIN_THIRST || currentWater == MIN_WATER || currentlyEating == true) currentlyDrinking = false;

            drinkCounter = 0;
        }

        public void tryToEat()
        {
            if (currentFood > MIN_FOOD)
            {
                currentFood--;
                hunger--;
            }

            if (hunger == MIN_HUNGER || currentFood == MIN_FOOD || currentlyDrinking == true) currentlyEating = false;

            eatCounter = 0;
        }

        public void maybeTakeAnimalAway()
        {
            if (hunger == MAX_HUNGER && thirst == MAX_THIRST && boredom == MAX_BOREDOM && affection == MIN_AFFECTION)
            {
                if (petImage != null) petImage.Source = leavingImage;

                if (petFeedButton != null) petFeedButton.IsEnabled = false;
                if (petWaterButton != null) petWaterButton.IsEnabled = false;
                if (petPlayButton != null) petPlayButton.IsEnabled = false;
                if (petPatButton != null) petPatButton.IsEnabled = false;

                if (mainLoopTimer != null) mainLoopTimer.Stop();
            }
        }

        public void Feed()
        {
            if (currentFood < MAX_FOOD) currentFood++;
        }

        public void Water()
        {
            if (currentWater < MAX_WATER) currentWater++;
        }

        public void Play()
        {
            if (boredom > MIN_BOREDOM) boredom--;
        }

        public void Pat()
        {
            if (affection < MAX_AFFECTION) affection++;
        }

        public void MainLoopTimer_Tick(object sender, EventArgs e)
        {
            increaseNeedCounters();

            if (hungerCounter >= hungerFrame) increaseHunger();
            if (thirstCounter >= thirstFrame) increaseThirst();
            if (affectionCounter >= affectionFrame) decreaseAffection();
            if (boredomCounter >= boredomFrame) increaseBoredom();

            if (currentlySleeping == true)
            {
                sleepCounter++;

                if (petImage != null) petImage.Source = sleepingImage;

                if (petFeedButton != null) petFeedButton.IsEnabled = false;
                if (petWaterButton != null) petWaterButton.IsEnabled = false;
                if (petPlayButton != null) petPlayButton.IsEnabled = false;
                if (petPatButton != null) petPatButton.IsEnabled = false;

                currentlyDrinking = false;
                currentlyEating = false;

                if (sleepCounter >= sleepFrame) decreaseTiredness();
            }

            if (currentlySleeping == false)
            {
                tirednessCounter++;

                if (petImage != null) petImage.Source = normalImage;

                if (petFeedButton != null) petFeedButton.IsEnabled = true;
                if (petWaterButton != null) petWaterButton.IsEnabled = true;
                if (petPlayButton != null) petPlayButton.IsEnabled = true;
                if (petPatButton != null) petPatButton.IsEnabled = true;

                if (tirednessCounter >= tirednessFrame) increaseTiredness();
            }

            if (currentlyDrinking == true)
            {
                if (petPlayButton != null) petPlayButton.IsEnabled = false;
                drinkCounter++;
                if (drinkCounter >= drinkFrame) tryToDrink();
            }

            if (currentlyEating == true)
            {
                if (petPlayButton != null) petPlayButton.IsEnabled = false;
                eatCounter++;
                if (eatCounter >= eatFrame) tryToEat();
            }

            if (currentlyEating == false && currentlyDrinking == false && currentlySleeping == false && petPlayButton != null) petPlayButton.IsEnabled = true;

            maybeTakeAnimalAway();

            NotifyObservers(hunger, thirst, boredom, affection, tiredness, currentFood, currentWater);
        }

        public void Register(IObserver observer)
        {
            observerList.Add(observer);
        }

        public void Unregister(IObserver observer)
        {
            observerList.Remove(observer);
        }

        public void NotifyObservers(int hunger, int thirst, int boredom, int affection, int tiredness, int currentFood, int currentWater)
        {
            foreach (IObserver observer in observerList)
            {
                observer.Update(hunger, thirst, boredom, affection, tiredness, currentFood, currentWater);
            }
        }

        public void Save(SaveFileDialog saveDialog)
        {
            Load save = new Load
            {
                hunger = hunger,
                thirst = thirst,
                boredom = boredom,
                affection = affection,
                tiredness = tiredness,
                currentFood = currentFood,
                currentWater = currentWater
            };
            using (Stream output = File.Create(saveDialog.FileName))
            using (BinaryWriter writer = new BinaryWriter(output))
            {
                var jsonPet = JsonSerializer.Serialize<Load>(save);
                Debug.WriteLine(jsonPet);
                writer.Write(jsonPet);
                MessageBox.Show("Pet saved at " + saveDialog.FileName);
            }
        }

        public void Dispose()
        {
            this.generator = null;
            this.mainLoopTimer = null;
            this.petFeedButton = null;
            this.petWaterButton = null;
            this.petPlayButton = null;
            this.petPatButton = null;
            this.petImage = null;
        }
    }
}
