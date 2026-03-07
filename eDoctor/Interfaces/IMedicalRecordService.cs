using eDoctor.Areas.Doctor.Models.Dtos.MedicalRecord.Queries;
using eDoctor.Results;

namespace eDoctor.Interfaces;

public interface IMedicalRecordService
{
    Task<Result> AddAsync(CreateMedicalRecordQueryDto dto);
}
