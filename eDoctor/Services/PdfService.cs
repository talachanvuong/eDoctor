using eDoctor.Areas.Doctor.Models.Dtos.MedicalRecord;
using eDoctor.Helpers.ExtensionMethods;
using eDoctor.Interfaces;
using eDoctor.Models.Dtos.Payment;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace eDoctor.Services;

public class PdfService : IPdfService
{
    public byte[] GenerateInvoice(DetailInvoiceDto dto)
    {
        return Document.Create(container => container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(60);
            page.DefaultTextStyle(x => x.FontSize(11));

            page.Content().Column(col =>
            {
                col.Item()
                    .Text($"INVOICE #{dto.InvoiceId}")
                    .FontSize(20)
                    .Bold();

                col.Item()
                    .PaddingTop(4)
                    .PaddingBottom(16)
                    .Text($"{dto.CreatedAt:MMM dd, yyyy - HH:mm}")
                    .FontColor(Colors.Grey.Medium);

                col.Item()
                    .Border(1)
                    .BorderColor(Colors.Grey.Lighten2)
                    .Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn();
                            c.ConstantColumn(100);
                        });

                        table.Header(header =>
                        {
                            header.Cell()
                                .BorderRight(1)
                                .BorderBottom(1)
                                .BorderColor(Colors.Grey.Lighten2)
                                .Padding(8)
                                .Text("Service")
                                .SemiBold();

                            header.Cell()
                                .BorderBottom(1)
                                .BorderColor(Colors.Grey.Lighten2)
                                .Padding(8)
                                .AlignRight()
                                .Text("Price")
                                .SemiBold();
                        });

                        foreach (var item in dto.Services)
                        {
                            table.Cell()
                                .BorderRight(1)
                                .BorderBottom(1)
                                .BorderColor(Colors.Grey.Lighten2)
                                .Padding(8)
                                .Text(item.ServiceName);

                            table.Cell()
                                .BorderBottom(1)
                                .BorderColor(Colors.Grey.Lighten2)
                                .Padding(8)
                                .AlignRight()
                                .Text($"{item.Price:C}");
                        }
                    });

                col.Item()
                    .PaddingVertical(12)
                    .LineHorizontal(1)
                    .LineColor(Colors.Grey.Lighten2);

                col.Item().Row(row =>
                {
                    row.RelativeItem()
                        .Text("Total")
                        .Bold();

                    row.ConstantItem(100)
                        .AlignRight()
                        .Text($"{dto.Services.Sum(x => x.Price):C}")
                        .Bold();
                });

                col.Item().Row(row =>
                {
                    row.RelativeItem()
                        .Text("Method")
                        .Bold();

                    row.ConstantItem(100)
                        .AlignRight()
                        .Text("PayPal")
                        .Bold();
                });

                col.Item()
                    .PaddingTop(28)
                    .AlignRight()
                    .Column(right =>
                    {
                        right.Item()
                            .Text("Payer")
                            .FontColor(Colors.Grey.Medium)
                            .AlignCenter();

                        right.Item()
                            .PaddingTop(6)
                            .AlignCenter()
                            .Shrink()
                            .Border(3)
                            .BorderColor(Colors.Red.Medium)
                            .Padding(12)
                            .Text("PAID")
                            .FontSize(24)
                            .Bold()
                            .FontColor(Colors.Red.Medium);

                        right.Item()
                            .PaddingTop(8)
                            .Text(dto.Payer)
                            .Bold()
                            .AlignCenter();
                    });
            });
        })).GeneratePdf();
    }

    public byte[] GenerateMedicalRecord(DetailMedicalRecordDto dto)
    {
        return Document.Create(container => container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(60);
            page.DefaultTextStyle(x => x.FontSize(11));

            page.Content().Column(col =>
            {
                col.Item()
                    .AlignCenter()
                    .Text("MEDICAL RECORD")
                    .FontSize(18)
                    .Bold();

                col.Item()
                    .PaddingTop(2)
                    .AlignCenter()
                    .Text($"#{dto.MedicalRecordId}")
                    .FontSize(10)
                    .FontColor(Colors.Grey.Medium);

                col.Item()
                    .PaddingTop(16)
                    .LineHorizontal(1)
                    .LineColor(Colors.Grey.Darken1);

                // Patient info
                col.Item()
                    .PaddingVertical(12)
                    .Column(info =>
                    {
                        info.Item()
                            .Text("I. PATIENT INFORMATION")
                            .FontSize(10)
                            .SemiBold()
                            .FontColor(Colors.Grey.Darken1);

                        info.Item().PaddingTop(8).Row(row =>
                        {
                            row.RelativeItem().Text($"Full name: {dto.PatientFullName}");
                            row.RelativeItem().AlignRight().Text($"Date of birth: {dto.BirthDate:MM/dd/yyyy}");
                        });

                        info.Item().PaddingTop(4).Text($"Sex: {(dto.Sex ? "Female" : "Male")}");
                    });

                col.Item()
                    .LineHorizontal(1)
                    .LineColor(Colors.Grey.Lighten2);

                // Clinical info
                col.Item()
                    .PaddingVertical(12)
                    .Column(clinical =>
                    {
                        clinical.Item()
                            .Text("II. CLINICAL INFORMATION")
                            .FontSize(10)
                            .SemiBold()
                            .FontColor(Colors.Grey.Darken1);

                        clinical.Item().PaddingTop(8).Column(c =>
                        {
                            c.Item().Text("Symptom").FontSize(10).FontColor(Colors.Grey.Medium);
                            c.Item().PaddingTop(2).Text(dto.Symptom);
                        });

                        clinical.Item().PaddingTop(8).Column(c =>
                        {
                            c.Item().Text("Diagnosis").FontSize(10).FontColor(Colors.Grey.Medium);
                            c.Item().PaddingTop(2).Text(dto.Diagnosis);
                        });

                        clinical.Item().PaddingTop(8).Column(c =>
                        {
                            c.Item().Text("Advice").FontSize(10).FontColor(Colors.Grey.Medium);
                            c.Item().PaddingTop(2).Text(dto.Advice);
                        });
                    });

                col.Item()
                    .LineHorizontal(1)
                    .LineColor(Colors.Grey.Lighten2);

                // Prescription
                col.Item()
                    .PaddingTop(12)
                    .Text("III. PRESCRIPTION")
                    .FontSize(10)
                    .SemiBold()
                    .FontColor(Colors.Grey.Darken1);

                col.Item()
                    .PaddingTop(8)
                    .Border(1)
                    .BorderColor(Colors.Grey.Lighten2)
                    .Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(3);
                            c.ConstantColumn(60);
                            c.RelativeColumn(4);
                        });

                        table.Header(header =>
                        {
                            header.Cell().BorderRight(1).BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text("Drug name").SemiBold();
                            header.Cell().BorderRight(1).BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(8).AlignCenter().Text("Qty").SemiBold();
                            header.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text("Note").SemiBold();
                        });

                        foreach (var item in dto.Prescription)
                        {
                            table.Cell().BorderRight(1).BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text(item.DrugName);
                            table.Cell().BorderRight(1).BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(8).AlignCenter().Text(item.Quantity.ToString());
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text(item.Note ?? "-").FontColor(Colors.Grey.Medium);
                        }
                    });

                // Doctor signature
                col.Item()
                    .PaddingTop(40)
                    .AlignRight()
                    .Column(right =>
                    {
                        right.Item().AlignCenter().Text("Attending Doctor").FontSize(10).FontColor(Colors.Grey.Medium);
                        right.Item().PaddingTop(4).AlignCenter().Text("(Signature and seal)").FontSize(9).FontColor(Colors.Grey.Lighten1);
                        right.Item().PaddingTop(36).AlignCenter().Text($"{dto.RankCode.ConvertToString()} {dto.DoctorFullName}");
                    });
            });
        })).GeneratePdf();
    }
}
