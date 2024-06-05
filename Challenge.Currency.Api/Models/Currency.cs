namespace Challenge.Bravo.Api.Models
{
    public class Currency : Entity
    {
        public string IdFormated => Id.ToString();

        public string Code { get; set; }

        public CurrencyValues CurrencyValues { get; set; }
    }
}
