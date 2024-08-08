namespace PetProject.Domain
{
    public struct Requisite
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Requisite(string name, string description) 
        { 
            Name = name;
            Description = description;
        }
    }
}