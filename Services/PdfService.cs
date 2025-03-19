using Microsoft.Extensions.Caching.Distributed;
using pdf_generator.Models.Requests;
using pdf_generator.Request;
using pdf_generator.Utilities;

namespace pdf_generator.Services;

public class PdfService(
    IHttpClientFactory _httpClientFactory,
    ILogger<PdfService> _logger) : IPdfService
{

    public Task<byte[]> GenerateFromFileAsync(PdfFromFileRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<byte[]> GenerateFromHtmlAsync(PdfFromHtmlRequest request, IDistributedCache cache)
    {
        var cacheKey = $"pdf_{PdfHelpers.ComputeSHA256Hash(request.HtmlContent)}";
        // var cachedPdf = await cache.GetAsync(cacheKey);

        // if (cachedPdf != null)
        //     return cachedPdf;

        var pdfBytes = PdfHelpers.GeneratePdfInternal(
            request.HtmlContent,
            request.HeaderText,
            request.FooterText
        );

        await cache.SetAsync(cacheKey, pdfBytes, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        });

        return pdfBytes;
    }

    // public Task<byte[]> GenerateFromHtmlAsync(PdfFromHtmlRequest request, IDistributedCache cache)
    // {
    //     throw new NotImplementedException();
    // }

    // public Task<byte[]> GenerateFromUrlAsync(PdfFromUrlRequest request)
    // {
    //     throw new NotImplementedException();
    // }
}