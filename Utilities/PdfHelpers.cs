using System.Security.Cryptography;
using System.Text;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace pdf_generator.Utilities;

public static class PdfHelpers
{
    public static string ComputeSHA256Hash(string input)
    {
        byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(hashBytes);
    }

    public static byte[] GeneratePdfInternal(string htmlContent, string? headerText, string? footerText)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10));

                // Cabeçalho
                page.Header().Column(col =>
                {
                    col.Item().AlignCenter().Text("SUPLEMENTAÇÃO DE APOSENTADORIA/PENSÃO").Bold().FontSize(14);
                    col.Item().AlignCenter().Text("Demonstrativo de Pagamento").Bold().FontSize(12);
                    col.Item().PaddingBottom(10).BorderBottom(1).BorderColor(Colors.Grey.Lighten1);
                });

                // Conteúdo Principal
                page.Content().Column(contentColumn =>
                {
                    contentColumn.Item().Background(Colors.Red.Medium);
                    // Seção de Dados
                    contentColumn.Item().Column(column =>
                    {
                        column.Spacing(5);
                        column.Item().Text("Dados").Bold().FontSize(11);
                        AddDataRow(column, "Nome:", "DILERMANDO HERMINIO BISPO");
                        AddDataRow(column, "Banco:", "33 - BANCO SANTANDER BRASIL S.A.");
                        AddDataRow(column, "Plano:", "Lei nº 4819 - Fazenda do Estado");
                        AddDataRow(column, "Agência:", "00105 - AVENIDAS");
                        AddDataRow(column, "Matrícula:", "3064");
                        AddDataRow(column, "Conta:", "01 / 0211720");
                        AddDataRow(column, "Nome da Empresa:", "ISA ENERGIA");
                        column.Item().PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten1);
                        column.Item().Background(Colors.Red.Lighten5);
                    });

                    // Seção de Pagamento
                    contentColumn.Item().Column(column =>
                    {
                        column.Spacing(5);
                        column.Item().Text("Aviso de Pagamento - Benefício Mensal").Bold().FontSize(11);
                        AddPaymentRow(column, "Mês/Ano:", "Janeiro/2025");
                        AddPaymentRow(column, "Líquido a receber:", "R$ 14.956,20");
                        AddPaymentRow(column, "Data de Crédito:", "30/01/2025");
                        AddPaymentRow(column, "Adiantamento previsto para o próximo mês:", "R$ 0,00");
                        column.Item().PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten1);
                    });

                    // Tabela de Históricos
                    contentColumn.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(1.5f);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("HISTÓRICOS").Bold().FontSize(10);
                            header.Cell().Text("VENCIMENTOS").Bold().FontSize(10);
                            header.Cell().Text("DESCONTOS").Bold().FontSize(10);
                            header.Cell().ColumnSpan(3).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten1);
                        });

                        AddTableRow(table, "Benefício - Fazenda do Estado - Lei nº 4819, de 1958", "41.549,37", "0,00");
                        AddTableRow(table, "Imposto de Renda", "0,00", "7.416,93");
                        AddTableRow(table, "Utilização PES A", "0,00", "206,48");
                        AddTableRow(table, "Mensalidade Planos de Saúde Pré", "0,00", "6.241,94");
                        AddTableRow(table, "Pensão Alimentícia", "0,00", "9.226,98");
                        AddTableRow(table, "Associação dos Aposentados da Funcesp", "0,00", "207,75");
                        AddTableRow(table, "Mensalidade PES A", "0,00", "3.293,09");

                        table.Footer(footer =>
                        {
                            footer.Cell().ColumnSpan(3).PaddingTop(10).BorderTop(1).BorderColor(Colors.Grey.Lighten1);
                            footer.Cell().ColumnSpan(3).Text("TOTAIS").Bold().FontSize(10);
                            footer.Cell().Text("R$ 41.549,37").Bold().FontSize(10);
                            footer.Cell().Text("R$ 26.593,17").Bold().FontSize(10);
                        });
                    });

                    // Seção Final
                    contentColumn.Item().Column(column =>
                    {
                        column.Item().AlignCenter().Text("LÍQUIDO A RECEBER").Bold().FontSize(12);
                        column.Item().AlignCenter().Text("R$ 14.956,20").Bold().FontSize(14);
                        column.Item().PaddingTop(15).AlignCenter().Text("A Funcesp agora é Vivest. Para maiores informações, por favor acesse: www.vivest.com.br").FontSize(8);
                    });
                });
            });
        }).GeneratePdf();
    }

    // Métodos auxiliares dentro da mesma classe
    private static void AddDataRow(ColumnDescriptor column, string label, string value)
    {
        column.Item().Row(row =>
        {
            row.RelativeItem(2).Text(label).SemiBold();
            row.RelativeItem(5).Text(value);
        });
    }

    private static void AddPaymentRow(ColumnDescriptor column, string label, string value)
    {
        column.Item().Row(row =>
        {
            row.RelativeItem(4).Text(label).SemiBold();
            row.RelativeItem(2).Text(value).AlignRight();
        });
    }

    private static void AddTableRow(TableDescriptor table, string historico, string vencimento, string desconto)
    {
        table.Cell().Text(historico).FontSize(9);
        table.Cell().Text(vencimento).AlignRight().FontSize(9);
        table.Cell().Text(desconto).AlignRight().FontSize(9);
    }
}