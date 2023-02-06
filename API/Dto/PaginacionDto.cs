namespace API.Dto
{
    public class PaginacionDto
    {
        private int _recordsPorPagina = 10;
        private readonly int _cantidadMaximaPorPagina = 50;

        public int Pagina { get; set; } = 1;
        public int RecordsPorPagina
        {
            get{ return _recordsPorPagina; }
            set{ _recordsPorPagina = (value > _cantidadMaximaPorPagina ? _cantidadMaximaPorPagina : value); }
        }
    }
}
