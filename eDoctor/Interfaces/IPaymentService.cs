using eDoctor.Models.Dtos.Payment;
using eDoctor.Models.Dtos.Payment.Queries;
using eDoctor.Results;

namespace eDoctor.Interfaces;

public interface IPaymentService
{
    Task<Result<BillDto>> GetBillAsync(BillQueryDto dto);
    Task<Result<CreateOrderDto>> CreateOrderAsync(CreateOrderQueryDto dto);
    Task<Result> CaptureAsync(CaptureQueryDto dto);
    Task<Result<InvoiceDto>> GetInvoiceAsync(InvoiceQueryDto dto);
}
