using eDoctor.Models.Dtos.Payment;
using eDoctor.Models.Dtos.Payment.Queries;
using eDoctor.Results;

namespace eDoctor.Interfaces;

public interface IPaymentService
{
    Task<Result<BillDto>> GetBillAsync(BillQueryDto dto);
}
