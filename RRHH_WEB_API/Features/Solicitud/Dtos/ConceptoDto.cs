namespace RRHH_WEB_API.Features.Solicitud.Dtos
{
    public class ConceptoDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public decimal Valor { get; set; }
        public int Moneda { get; set; }
    }

    public enum Moneda
    {
        Lempiras = 1,
        Dolares = 2
    }
}
