using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud_Facade_Negocios.Base.Web.Interfaces
{
    public interface IValidador
    {
        string Executar(object entidade);
        void SalvaConexao(IDbConnection conexao);
        void SalvaTransacao(IDbTransaction transacao);

    }
}
