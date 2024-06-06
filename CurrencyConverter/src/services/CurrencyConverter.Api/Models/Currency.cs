namespace CurrencyConverter.Api.Models
{
    public class Currency : Entity
    {
        public string Code { get; set; }

        public CurrencyValues CurrencyValues { get; set; }
    }
}
