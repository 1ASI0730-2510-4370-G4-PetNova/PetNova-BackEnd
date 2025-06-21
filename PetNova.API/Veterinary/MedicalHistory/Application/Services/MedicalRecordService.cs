using PetNova.API.Shared.Domain.Repository;
using PetNova.API.Veterinary.ClientAndPetManagement.Domain.Model.Aggregate;
using PetNova.API.Veterinary.MedicalHistory.Domain.Model.Aggregate;

namespace PetNova.API.Veterinary.MedicalHistory.Application.Services;

public class MedicalRecordService(
    IRepository<MedicalRecord, Guid> medicalRecordRepository,
    IRepository<Pet, Guid> petRepository,
    IRepository<Doctor, Guid> doctorRepository,
    IUnitOfWork unitOfWork)
{
    private readonly IRepository<Doctor, Guid> _doctorRepository = doctorRepository;

    public async Task<IEnumerable<MedicalRecord>> ListAsync()
    {
        return await medicalRecordRepository.ListAsync();
    }

    public async Task<MedicalRecord?> GetByIdAsync(Guid id)
    {
        return await medicalRecordRepository.FindByIdAsync(id);
    }

    public async Task<IEnumerable<MedicalRecord>> GetByPetIdAsync(Guid petId)
    {
        var pet = await petRepository.FindByIdAsync(petId);
        return pet?.MedicalRecords ?? Enumerable.Empty<MedicalRecord>();
    }

    public async Task<MedicalRecord> CreateAsync(MedicalRecordDto medicalRecordDto)
    {
        var medicalRecord = new MedicalRecord
        {
            RecordDate = medicalRecordDto.RecordDate,
            Diagnosis = medicalRecordDto.Diagnosis,
            Treatment = medicalRecordDto.Treatment,
            Notes = medicalRecordDto.Notes,
            PetId = medicalRecordDto.PetId,
            DoctorId = medicalRecordDto.DoctorId
        };
    
        await medicalRecordRepository.AddAsync(medicalRecord);
        await unitOfWork.CompleteAsync();
    
        return medicalRecord;
    }
} 

public class MedicalRecordDto
{
    public DateTime RecordDate { get; set; }
    public string Diagnosis { get; set; }
    public Guid DoctorId { get; set; }
    public required string Treatment { get; set; }
    public required string Notes { get; set; }
    public Guid PetId { get; set; }

    public MedicalRecordDto(string diagnosis)
    {
        Diagnosis = diagnosis;
    }
}
