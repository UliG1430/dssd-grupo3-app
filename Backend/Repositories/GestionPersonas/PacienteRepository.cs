using api.Data;
using api.Model;

namespace ApiACEAPP.Repositories
{
    public class PacienteRepository : GenericRepository<Paciente>
    {
        public PacienteRepository(ApiDbContext context) : base(context)
        {
        }

        public async Task<Paciente?> GetPacienteByDocumento(string tipoDocumento, string numeroDocumento, string sexo)
        {
            return (await FilterAsync(x => tipoDocumento.Contains(x.TipoDocumento) &&
                                          x.NumeroDocumento == numeroDocumento &&
                                          x.Sexo == sexo)).FirstOrDefault();
        }
    }
}