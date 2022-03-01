using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace ExcitingVirtualPetCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer mainLoopTimer;
        IAnimal currentPet;
        IPetFactory createPet;
        Random generator;
        IObserver observer;
        SaveFileDialog saveDialog;
        OpenFileDialog openDialog;

        public MainWindow()
        {
            InitializeComponent();

            //set up initial stuff
            generator = new Random();
            createPet = new PetFactory();
            observer = new Observer(HungerMeter, ThirstMeter, BoredomMeter, AffectionMeter, TirednessMeter, WaterAmountBar, FoodAmountBar);
            saveDialog = new SaveFileDialog();
            openDialog = new OpenFileDialog();

            SaveBird.IsEnabled = false;
            SaveCat.IsEnabled = false;
            SaveDog.IsEnabled = false;

            //set up loop
            mainLoopTimer = new DispatcherTimer();
            mainLoopTimer.Interval = new TimeSpan(17); //runs every 60th of a second
        }

        private void PetFeedButton_Click(object sender, RoutedEventArgs e)
        {
            currentPet.Feed();
        }

        private void PetWaterButton_Click(object sender, RoutedEventArgs e)
        {
            currentPet.Water();
        }

        private void PetPlayButton_Click(object sender, RoutedEventArgs e)
        {
            currentPet.Play();
        }

        private void PetPatButton_Click(object sender, RoutedEventArgs e)
        {
            currentPet.Pat();
        }

        private void CatButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPet != null)
            {
                mainLoopTimer.Stop();
                currentPet.Unregister(observer);
                currentPet.Dispose();
            }
            PetFeedButton.Content = "Feed Cat";
            PetWaterButton.Content = "Give Cat Water";
            PetPlayButton.Content = "Play With Cat";
            PetPatButton.Content = "Pat Cat";
            TirednessMeter.Maximum = 20;
            HungerMeter.Maximum = 10;
            BoredomMeter.Maximum = 10;
            AffectionMeter.Maximum = 10;

            currentPet = createPet.CreateCat();
            currentPet.InitializeAnimal(generator, mainLoopTimer, PetFeedButton, PetWaterButton, PetPlayButton, PetPatButton, PetImage);
            currentPet.Register(observer);
            currentPet.InitializeFood();
            currentPet.InitializeWater();
            currentPet.InitializeFrames();
            mainLoopTimer.Tick += currentPet.MainLoopTimer_Tick;
            mainLoopTimer.Start();

            CatButton.IsEnabled = false;
            DogButton.IsEnabled = true;
            BirdButton.IsEnabled = true;

            SaveBird.IsEnabled = false;
            SaveCat.IsEnabled = true;
            SaveDog.IsEnabled = false;
        }

        private void DogButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPet != null)
            {
                mainLoopTimer.Stop();
                currentPet.Unregister(observer);
                currentPet.Dispose();
            }
            PetFeedButton.Content = "Feed Dog";
            PetWaterButton.Content = "Give Dog Water";
            PetPlayButton.Content = "Play With Dog";
            PetPatButton.Content = "Pat Dog";
            TirednessMeter.Maximum = 10;
            HungerMeter.Maximum = 20;
            BoredomMeter.Maximum = 20;
            AffectionMeter.Maximum = 10;

            currentPet = createPet.CreateDog();
            currentPet.InitializeAnimal(generator, mainLoopTimer, PetFeedButton, PetWaterButton, PetPlayButton, PetPatButton, PetImage);
            currentPet.Register(observer);
            currentPet.InitializeFood();
            currentPet.InitializeWater();
            currentPet.InitializeFrames();
            mainLoopTimer.Tick += currentPet.MainLoopTimer_Tick;
            mainLoopTimer.Start();

            DogButton.IsEnabled = false;
            CatButton.IsEnabled = true;
            BirdButton.IsEnabled = true;

            SaveBird.IsEnabled = false;
            SaveCat.IsEnabled = false;
            SaveDog.IsEnabled = true;
        }

        private void BirdButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPet != null)
            {
                mainLoopTimer.Stop();
                currentPet.Unregister(observer);
                currentPet.Dispose();
            }
            PetFeedButton.Content = "Feed Bird";
            PetWaterButton.Content = "Give Bird Water";
            PetPlayButton.Content = "Play With Bird";
            PetPatButton.Content = "Pat Bird";
            TirednessMeter.Maximum = 10;
            HungerMeter.Maximum = 10;
            BoredomMeter.Maximum = 10;
            AffectionMeter.Maximum = 20;

            currentPet = createPet.CreateBird();
            currentPet.InitializeAnimal(generator, mainLoopTimer, PetFeedButton, PetWaterButton, PetPlayButton, PetPatButton, PetImage);
            currentPet.Register(observer);
            currentPet.InitializeFood();
            currentPet.InitializeWater();
            currentPet.InitializeFrames();
            mainLoopTimer.Tick += currentPet.MainLoopTimer_Tick;
            mainLoopTimer.Start();

            BirdButton.IsEnabled = false;
            CatButton.IsEnabled = true;
            DogButton.IsEnabled = true;

            SaveBird.IsEnabled = true;
            SaveCat.IsEnabled = false;
            SaveDog.IsEnabled = false;
        }

        private void SaveCat_Click(object sender, RoutedEventArgs e)
        {
            saveDialog.Filter = "Cat Files |*.cat";
            openDialog.Filter = "Cat Files |*.cat";
            saveDialog.DefaultExt = "Cat Files |*.cat";
            openDialog.DefaultExt = "Cat Files |*.cat";

            if (saveDialog.ShowDialog() == true)
            {
                currentPet.Save(saveDialog);
            }

            mainLoopTimer.Stop();
            currentPet.Unregister(observer);
            BirdButton.IsEnabled = true;
            CatButton.IsEnabled = true;
            DogButton.IsEnabled = true;
            PetImage.Source = null;

            TirednessMeter.Value = 0;
            HungerMeter.Value = 0;
            ThirstMeter.Value = 0;
            BoredomMeter.Value = 0;
            AffectionMeter.Value = 0;
            WaterAmountBar.Value = 0;
            FoodAmountBar.Value = 0;

            SaveBird.IsEnabled = false;
            SaveCat.IsEnabled = false;
            SaveDog.IsEnabled = false;
        }

        private void LoadCat_Click(object sender, RoutedEventArgs e)
        {
            if (currentPet != null)
            {
                mainLoopTimer.Stop();
                currentPet.Unregister(observer);
                currentPet.Dispose();
            }

            if (openDialog.ShowDialog() == true)
            {
                using (Stream input = File.OpenRead(openDialog.FileName))
                using (BinaryReader reader = new BinaryReader(input))
                {
                    Load cat = JsonSerializer.Deserialize<Load>(reader.ReadString());
                    Debug.WriteLine(cat);
                    currentPet = createPet.LoadCat(cat);
                    MessageBox.Show("Pet loaded from " + openDialog.FileName);
                }
            }

            PetFeedButton.Content = "Feed Cat";
            PetWaterButton.Content = "Give Cat Water";
            PetPlayButton.Content = "Play With Cat";
            PetPatButton.Content = "Pat Cat";
            TirednessMeter.Maximum = 20;
            HungerMeter.Maximum = 10;
            BoredomMeter.Maximum = 10;
            AffectionMeter.Maximum = 10;

            currentPet.ConnectButtons(generator, mainLoopTimer, PetFeedButton, PetWaterButton, PetPlayButton, PetPatButton, PetImage);
            currentPet.Register(observer);
            currentPet.InitializeFrames();
            mainLoopTimer.Tick += currentPet.MainLoopTimer_Tick;
            mainLoopTimer.Start();

            CatButton.IsEnabled = false;
            DogButton.IsEnabled = true;
            BirdButton.IsEnabled = true;

            SaveBird.IsEnabled = false;
            SaveCat.IsEnabled = true;
            SaveDog.IsEnabled = false;
        }

        private void SaveDog_Click(object sender, RoutedEventArgs e)
        {
            saveDialog.Filter = "Dog Files |*.dog";
            openDialog.Filter = "Dog Files |*.dog";
            saveDialog.DefaultExt = "Dog Files |*.dog";
            openDialog.DefaultExt = "Dog Files |*.dog";

            if (saveDialog.ShowDialog() == true)
            {
                currentPet.Save(saveDialog);
            }

            mainLoopTimer.Stop();
            currentPet.Unregister(observer);
            BirdButton.IsEnabled = true;
            CatButton.IsEnabled = true;
            DogButton.IsEnabled = true;
            PetImage.Source = null;

            TirednessMeter.Value = 0;
            HungerMeter.Value = 0;
            ThirstMeter.Value = 0;
            BoredomMeter.Value = 0;
            AffectionMeter.Value = 0;
            WaterAmountBar.Value = 0;
            FoodAmountBar.Value = 0;

            SaveBird.IsEnabled = false;
            SaveCat.IsEnabled = false;
            SaveDog.IsEnabled = false;
        }

        private void LoadDog_Click(object sender, RoutedEventArgs e)
        {
            if (currentPet != null)
            {
                mainLoopTimer.Stop();
                currentPet.Unregister(observer);
                currentPet.Dispose();
            }

            if (openDialog.ShowDialog() == true)
            {
                using (Stream input = File.OpenRead(openDialog.FileName))
                using (BinaryReader reader = new BinaryReader(input))
                {
                    Load dog = JsonSerializer.Deserialize<Load>(reader.ReadString());
                    Debug.WriteLine(dog);
                    currentPet = createPet.LoadDog(dog);
                    MessageBox.Show("Pet loaded from " + openDialog.FileName);
                }
            }

            PetFeedButton.Content = "Feed Dog";
            PetWaterButton.Content = "Give Dog Water";
            PetPlayButton.Content = "Play With Dog";
            PetPatButton.Content = "Pat Dog";
            TirednessMeter.Maximum = 10;
            HungerMeter.Maximum = 20;
            BoredomMeter.Maximum = 20;
            AffectionMeter.Maximum = 10;

            currentPet.ConnectButtons(generator, mainLoopTimer, PetFeedButton, PetWaterButton, PetPlayButton, PetPatButton, PetImage);
            currentPet.Register(observer);
            currentPet.InitializeFrames();
            mainLoopTimer.Tick += currentPet.MainLoopTimer_Tick;
            mainLoopTimer.Start();

            DogButton.IsEnabled = false;
            CatButton.IsEnabled = true;
            BirdButton.IsEnabled = true;

            SaveBird.IsEnabled = false;
            SaveCat.IsEnabled = false;
            SaveDog.IsEnabled = true;
        }

        private void SaveBird_Click(object sender, RoutedEventArgs e)
        {
            saveDialog.Filter = "Bird Files |*.brd";
            openDialog.Filter = "Bird Files |*.brd";
            saveDialog.DefaultExt = "Bird Files |*.brd";
            openDialog.DefaultExt = "Bird Files |*.brd";

            if (saveDialog.ShowDialog() == true)
            {
                currentPet.Save(saveDialog);
            }

            mainLoopTimer.Stop();
            currentPet.Unregister(observer);
            BirdButton.IsEnabled = true;
            CatButton.IsEnabled = true;
            DogButton.IsEnabled = true;
            PetImage.Source = null;

            TirednessMeter.Value = 0;
            HungerMeter.Value = 0;
            ThirstMeter.Value = 0;
            BoredomMeter.Value = 0;
            AffectionMeter.Value = 0;
            WaterAmountBar.Value = 0;
            FoodAmountBar.Value = 0;

            SaveBird.IsEnabled = false;
            SaveCat.IsEnabled = false;
            SaveDog.IsEnabled = false;
        }

        private void LoadBird_Click(object sender, RoutedEventArgs e)
        {
            if (currentPet != null)
            {
                mainLoopTimer.Stop();
                currentPet.Unregister(observer);
                currentPet.Dispose();
            }

            if (openDialog.ShowDialog() == true)
            {
                using (Stream input = File.OpenRead(openDialog.FileName))
                using (BinaryReader reader = new BinaryReader(input))
                {
                    Load bird = JsonSerializer.Deserialize<Load>(reader.ReadString());
                    Debug.WriteLine(bird);
                    currentPet = createPet.LoadBird(bird);
                    MessageBox.Show("Pet loaded from " + openDialog.FileName);
                }
            }

            PetFeedButton.Content = "Feed Bird";
            PetWaterButton.Content = "Give Bird Water";
            PetPlayButton.Content = "Play With Bird";
            PetPatButton.Content = "Pat Bird";
            TirednessMeter.Maximum = 10;
            HungerMeter.Maximum = 10;
            BoredomMeter.Maximum = 10;
            AffectionMeter.Maximum = 20;

            currentPet.ConnectButtons(generator, mainLoopTimer, PetFeedButton, PetWaterButton, PetPlayButton, PetPatButton, PetImage);
            currentPet.Register(observer);
            currentPet.InitializeFrames();
            mainLoopTimer.Tick += currentPet.MainLoopTimer_Tick;
            mainLoopTimer.Start();

            BirdButton.IsEnabled = false;
            CatButton.IsEnabled = true;
            DogButton.IsEnabled = true;

            SaveBird.IsEnabled = true;
            SaveCat.IsEnabled = false;
            SaveDog.IsEnabled = false;
        }
    }
}
