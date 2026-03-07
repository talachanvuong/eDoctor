using eDoctor.Areas.Doctor.Models.Dtos.MedicalRecord.Queries;
using eDoctor.Data;
using eDoctor.Enums;
using eDoctor.Interfaces;
using eDoctor.Models;
using eDoctor.Results;
using Microsoft.EntityFrameworkCore;

namespace eDoctor.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly ApplicationDbContext _context;

    public MedicalRecordService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> AddAsync(CreateMedicalRecordQueryDto dto)
    {
        var schedule = _context.Schedules.FirstOrDefault(s => s.ScheduleId == dto.ScheduleId);

        if (schedule == null)
        {
            return Result.Failure("Schedule not found.");
        }

        if (schedule.DoctorId != dto.DoctorId)
        {
            return Result.Failure("Doctor does not belong to this schedule.");
        }

        if (await _context.MedicalRecords.AnyAsync(m => m.ScheduleId == dto.ScheduleId))
        {
            return Result.Failure("Schedule already has a medical record.");
        }

        if (schedule.Status != ScheduleStatus.ONGOING && schedule.Status != ScheduleStatus.COMPLETED)
        {
            return Result.Failure("Not an appropriate time to create a medical record.");
        }

        MedicalRecord medicalRecord = new MedicalRecord
        {
            Symptom = dto.Symptom,
            Diagnosis = dto.Diagnosis,
            Advice = dto.Advice
        };

        foreach (var item in dto.Prescription)
        {
            medicalRecord.DetailPrescriptions.Add(new DetailPrescription
            {
                DrugName = item.DrugName,
                Quantity = item.Quantity,
                Note = item.Note
            });
        }

        schedule.MedicalRecord = medicalRecord;

        await _context.SaveChangesAsync();

        return Result.Success();
    }
}
