using System;
using System.Collections.Generic;
using System.Text;

namespace ExcitingVirtualPetCore
{
    abstract class IPetFactory
    {
        public abstract IAnimal CreateCat();
        public abstract IAnimal LoadCat(Load cat);
        public abstract IAnimal CreateDog();
        public abstract IAnimal LoadDog(Load dog);
        public abstract IAnimal CreateBird();
        public abstract IAnimal LoadBird(Load bird);
    }
}
