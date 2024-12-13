using PetProject.Core;
using PetProject.Core.ValueObjects;
using PetProject.Core.ValueObjects.IdValueObjects;
using PetProject.Volunteers.Domain.ValueObjects;

namespace PetProject.Volunteers.Domain
{
    public class Pet : SoftDeletableEntity<PetId>
    {
        private bool _isDeleted = false;

        public string Name { get; private set; } = default!;
        public Description Description { get; private set; } = default!;
        public string Color { get; private set; } = default!;
        public string HealthInfo { get; private set; } = default!;
        public Address Address { get; private set; } = default!;
        public TelephoneNumber TelephoneNumber { get; private set; } = default!;

        public PetTypeInfo PetTypeInfo { get; private set; } = default!;

        public SerialNumber SerialNumber { get; private set; } = default!;           

        public float Weight { get; private set; }
        public float Height { get; private set; }

        public bool IsCastrated { get; private set; }
        public bool IsVaccinated { get; private set; }

        public DateTime BirthdayTime { get; private set; }
        public DateTime DateOfCreation { get; private set; }

        private List<Requisite> _requisites = default!;
        public IReadOnlyList<Requisite> Requisites => _requisites;

        private List<PetPhoto> _photos = default!;
        public IReadOnlyList<PetPhoto> Photos => _photos;

        public HelpStatus HelpStatus { get; private set; }

        private Pet(PetId id) : base(id)
        {

        }

        public Pet(
            PetId id,
            string name, 
            Description description, 
            string color, 
            string healthInfo, 
            Address address, 
            TelephoneNumber telephoneNumber, 
            float weight, 
            float height, 
            bool isCastrated, 
            bool isVaccinated, 
            DateTime birthdayTime, 
            DateTime dateOfCreation,
            List<Requisite> requisites, 
            List<PetPhoto> photos,
            SpeciesId speciesId,
            BreedId breedId,
            HelpStatus helpStatus) : base(id)
        {
            Name = name;
            Description = description; 
            Color = color;
            HealthInfo = healthInfo;
            Address = address;
            TelephoneNumber = telephoneNumber;
            Weight = weight;
            Height = height;
            IsCastrated = isCastrated;
            IsVaccinated = isVaccinated;
            BirthdayTime = birthdayTime;
            DateOfCreation = dateOfCreation;
            _requisites = requisites;
            _photos = photos;
            PetTypeInfo = new PetTypeInfo(breedId, speciesId);
            HelpStatus = helpStatus;
        }

        internal void UpdateInfo(
            string name,
            Description description,
            string color,
            string healthInfo,
            Address address,
            TelephoneNumber telephoneNumber,
            float weight,
            float height,
            bool isCastrated,
            bool isVaccinated,
            DateTime birthdayTime,
            DateTime dateOfCreation,
            List<Requisite> requisites,
            SpeciesId speciesId,
            BreedId breedId,
            HelpStatus helpStatus)
        {
            Name = name;
            Description = description;
            Color = color; 
            HealthInfo = healthInfo;
            Address = address;
            TelephoneNumber = telephoneNumber;
            Weight = weight;
            Height = height;
            IsCastrated = isCastrated;
            IsVaccinated = isVaccinated;
            BirthdayTime = birthdayTime;
            DateOfCreation = dateOfCreation;
            _requisites = requisites;
            PetTypeInfo = new PetTypeInfo(breedId, speciesId);
            HelpStatus = helpStatus;

        }

        public void UpdatePhotos(IEnumerable<PetPhoto> petPhotos) =>
            _photos = petPhotos.ToList();

        internal void SetSerialNumber(SerialNumber serialNumber) =>
            SerialNumber = serialNumber;

        internal void MoveSerialNumberToForward() =>
            SerialNumber = SerialNumber.Create(SerialNumber.Value + 1).Value;

        internal void MoveSerialNumberToBackward() =>
            SerialNumber = SerialNumber.Create(SerialNumber.Value - 1).Value;
    }
}
