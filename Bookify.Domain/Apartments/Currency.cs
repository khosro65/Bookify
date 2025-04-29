namespace Bookify.Domain.Apartments;

public record Currency
{
    /// <summary>
    /// we don't want expose non currency out of domain
    /// </summary>
    internal static readonly Currency None = new("");
    public static readonly Currency Usd = new("USD");
    public static readonly Currency Eur = new("EUR");

    public string Code { get; init; }
    private Currency(string code) => Code = code;


    public static Currency FromCode(string code)
    {
        return All.FirstOrDefault(c => c.Code == code) ??
            throw new ApplicationException("The currency code is invalid");
    }

    public static readonly IReadOnlyCollection<Currency> All =
        [
        Usd,
        Eur,
        ];
}