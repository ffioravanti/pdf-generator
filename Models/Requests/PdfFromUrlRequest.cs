namespace pdf_generator.Request;
public class PdfFromUrlRequest
{
    public required string Url { get; set; }
    public string? FooterText { get; set; }
}