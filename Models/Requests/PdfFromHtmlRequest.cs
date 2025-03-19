namespace pdf_generator.Models.Requests;
public class PdfFromHtmlRequest
{
    public required string HtmlContent { get; set; }
    public string? HeaderText { get; set; }
    public string? FooterText { get; set; }
}