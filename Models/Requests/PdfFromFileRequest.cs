namespace pdf_generator.Request;
public class PdfFromFileRequest
{
    // Validação customizada para tipo de arquivo
    public required IFormFile File { get; set; }
}