using Crypto.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.Web.Controllers;

public class QuoteController : ControllerBase
{
    private readonly QuoteCalculator _calculator;
    private readonly ILogger<QuoteController> _logger;

    public QuoteController(QuoteCalculator calculator, ILogger<QuoteController> logger)
    {
        _calculator = calculator;
        _logger = logger;
    }

    [HttpGet("{cryptoCode}")]
    public async Task<ActionResult> Get(string cryptoCode)
    {

        var result = await _calculator.GetQuoteAsync(cryptoCode);
        if (result == null)
            return NotFound("CryptoCode not found or API error");

        return Ok(result);
    }
}
