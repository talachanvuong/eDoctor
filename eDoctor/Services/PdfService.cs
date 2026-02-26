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
}
