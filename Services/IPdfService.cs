using Microsoft.Extensions.Caching.Distributed;
using pdf_generator.Models.Requests;

namespace pdf_generator.Services;

public interface IPdfService
{
    Task<byte[]> GenerateFromHtmlAsync(PdfFromHtmlRequest request, IDistributedCache cache);
    // Task<byte[]> GenerateFromUrlAsync(PdfFromUrlRequest request);
    // Task<byte[]> GenerateFromFileAsync(PdfFromFileRequest request);
}