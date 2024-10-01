using api.Data;
using api.Model;

namespace ApiACEAPP.Repositories
{
    public class AfiliacionRepository : GenericRepository<Afiliacion>
    {
        public AfiliacionRepository(ApiDbContext context) : base(context)
        {
        }

        public async Task<Paciente?> GetPacienteByAfiliacion(string numeroAfiliado, string obraSocial)
        {
            Afiliacion? afiliacion = (await FilterAsync(x => x.NroAfiliado == numeroAfiliado &&
                                          x.ObraSocial.Nombre == obraSocial, includes: "Paciente")).FirstOrDefault();
            if (afiliacion != null)
            {
                return afiliacion.Paciente;
            }
            return null;
        }
    }
}