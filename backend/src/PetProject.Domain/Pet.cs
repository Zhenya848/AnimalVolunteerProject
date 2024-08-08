namespace PetProject.Domain
{
    public class Pet
    {
        public IPet _pet { get; private set; }

        public Pet(IPet pet) => _pet = pet;
    }
}
