using eDoctor.Areas.Doctor.Models.Dtos.MedicalRecord;
using eDoctor.Areas.Doctor.Models.Dtos.MedicalRecord.Queries;
using eDoctor.Results;

namespace eDoctor.Interfaces;

public interface IMedicalRecordService
{
    Task<Result> AddAsync(CreateMedicalRecordQueryDto dto);
    Task<Result<MedicalRecordDto>> GetDoctorMedicalRecordAsync(MedicalRecordQueryDto dto);
    Task<Result<MedicalRecordDto>> GetUserMedicalRecordAsync(MedicalRecordQueryDto dto);
}
