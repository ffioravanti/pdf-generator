using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using pdf_generator.Models.Requests;
using pdf_generator.Services;

namespace pdf_generator.Endpoints;
public static class PdfEndpoints
{
    public static void MapPdfEndpoints(this IEndpointRouteBuilder app)
    {
        // define um authorization e mapeamento de rota padrão para as rotas
        var group = app.MapGroup("v1/pdf")
            .WithTags("PDF");
        // .RequireAuthorization();

        group.MapPost("from-html", async (
            [FromBody] PdfFromHtmlRequest request,
            [FromServices] IPdfService pdfService,
            [FromServices] IDistributedCache cache) =>
        {
            if (string.IsNullOrWhiteSpace(request.HtmlContent))
                return Results.BadRequest("HTML content is required");

            try
            {
                var pdfBytes = await pdfService.GenerateFromHtmlAsync(request, cache);
                return Results.File(pdfBytes, "application/pdf", "document.pdf");
            }
            catch (Exception ex)
            {
                // Logging via injected service would be better
                return Results.Problem("Internal server error", statusCode: 500);
            }
        })
        .WithName("FromHtml");

        // Similar para from-url e from-file
    }
}
