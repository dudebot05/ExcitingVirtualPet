using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ExcitingVirtualPetCore
{
    class PetFactory : IPetFactory
    {
        public override IAnimal CreateCat()
        {
            return new Cat();
        }

        public override IAnimal LoadCat(Load cat)
        {
            return new Cat(cat);
        }

        public override IAnimal CreateDog()
        {
            return new Dog();
        }

        public override IAnimal LoadDog(Load dog)
        {
            return new Dog(dog);
        }

        public override IAnimal CreateBird()
        {
            return new Bird();
        }

        public override IAnimal LoadBird(Load bird)
        {
            return new Bird(bird);
        }
    }
}
