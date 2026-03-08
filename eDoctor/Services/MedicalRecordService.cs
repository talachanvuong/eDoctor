using eDoctor.Areas.Doctor.Models.Dtos.MedicalRecord;
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
    private readonly IPdfService _pdfService;

    public MedicalRecordService(ApplicationDbContext context, IPdfService pdfService)
    {
        _context = context;
        _pdfService = pdfService;
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

    public async Task<Result<MedicalRecordDto>> GetDoctorMedicalRecordAsync(MedicalRecordQueryDto dto)
    {
        if (!await _context.MedicalRecords.AnyAsync(m => m.ScheduleId == dto.ScheduleId))
        {
            return Result<MedicalRecordDto>.Failure("Medical record not found.");
        }

        var schedule = await _context.Schedules.FirstAsync(s => s.ScheduleId == dto.ScheduleId);

        if (!await _context.Schedules.AnyAsync(s => s.DoctorId == dto.Id && s.UserId == schedule.UserId))
        {
            return Result<MedicalRecordDto>.Failure("You haven't had an appointment with this patient.");
        }

        DetailMedicalRecordDto detailMedicalRecord = await _context.MedicalRecords
            .Where(dmr => dmr.ScheduleId == dto.ScheduleId)
            .Select(dmr => new DetailMedicalRecordDto
            {
                MedicalRecordId = dmr.MedicalRecordId,
                Symptom = dmr.Symptom,
                Diagnosis = dmr.Diagnosis,
                Advice = dmr.Advice,
                Prescription = dmr.DetailPrescriptions.Select(dp => new DetailPrescriptionDto
                {
                    DrugName = dp.DrugName,
                    Quantity = dp.Quantity,
                    Note = dp.Note
                }),
                PatientFullName = dmr.Schedule.User!.FullName,
                BirthDate = dmr.Schedule.User.BirthDate,
                Sex = dmr.Schedule.User.Sex,
                DoctorFullName = dmr.Schedule.Doctor.FullName,
                RankCode = dmr.Schedule.Doctor.RankCode
            })
            .FirstAsync();

        MedicalRecordDto value = new MedicalRecordDto
        {
            Pdf = _pdfService.GenerateMedicalRecord(detailMedicalRecord)
        };

        return Result<MedicalRecordDto>.Success(value);
    }

    public async Task<Result<MedicalRecordDto>> GetUserMedicalRecordAsync(MedicalRecordQueryDto dto)
    {
        if (!await _context.MedicalRecords.AnyAsync(m => m.ScheduleId == dto.ScheduleId))
        {
            return Result<MedicalRecordDto>.Failure("Medical record not found.");
        }

        if (!await _context.Schedules.AnyAsync(s => s.ScheduleId == dto.ScheduleId && s.UserId == dto.Id))
        {
            return Result<MedicalRecordDto>.Failure("You are not allowed to access this appointment.");
        }

        DetailMedicalRecordDto detailMedicalRecord = await _context.MedicalRecords
            .Where(dmr => dmr.ScheduleId == dto.ScheduleId)
            .Select(dmr => new DetailMedicalRecordDto
            {
                MedicalRecordId = dmr.MedicalRecordId,
                Symptom = dmr.Symptom,
                Diagnosis = dmr.Diagnosis,
                Advice = dmr.Advice,
                Prescription = dmr.DetailPrescriptions.Select(dp => new DetailPrescriptionDto
                {
                    DrugName = dp.DrugName,
                    Quantity = dp.Quantity,
                    Note = dp.Note
                }),
                PatientFullName = dmr.Schedule.User!.FullName,
                BirthDate = dmr.Schedule.User.BirthDate,
                Sex = dmr.Schedule.User.Sex,
                DoctorFullName = dmr.Schedule.Doctor.FullName,
                RankCode = dmr.Schedule.Doctor.RankCode
            })
            .FirstAsync();

        MedicalRecordDto value = new MedicalRecordDto
        {
            Pdf = _pdfService.GenerateMedicalRecord(detailMedicalRecord)
        };

        return Result<MedicalRecordDto>.Success(value);
    }
}
