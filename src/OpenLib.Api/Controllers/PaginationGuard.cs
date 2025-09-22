using Microsoft.AspNetCore.Mvc;

namespace OpenLib.Api.Controllers;

internal static class PaginationGuard
{
    public static IActionResult? Validate(ControllerBase controller, int pagina, int tamanho)
    {
        if (pagina < 1 || tamanho < 1)
        {
            return controller.BadRequest(new { erro = "Os parâmetros de paginação devem ser maiores ou iguais a 1." });
        }

        return null;
    }
}
