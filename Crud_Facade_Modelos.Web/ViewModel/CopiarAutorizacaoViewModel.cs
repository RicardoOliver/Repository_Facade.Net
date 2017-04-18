using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud_Facade_Modelos.Web.ViewModel
{
    public class CopiarAutorizacaoViewModel : IViewModel<CopiaDeAutorizacao>
    {
        public string UsuarioCopia { get; set; }

        public string NomeUsuario { get; set; }

        public Profissional UsuarioLiberacao { get; set; }

        public void PreencherDadosView(CopiaDeAutorizacao dados)
        {
            throw new NotImplementedException("A cópia de Autorização utiliza a mesma tela que a " +
                "autorização, portanto, o que é retornado para a tela é um model do tipo " +
                "AutorizarUsuarioViewModel");
        }

        public CopiaDeAutorizacao CriarModelo()
        {
            CopiaDeAutorizacao retorno = new CopiaDeAutorizacao();

            retorno.AutorizacaoDestino = new Autorizacao
            {
                Usuario = new Profissional { Codigo = NomeUsuario },
                UsuarioAutorizando = UsuarioLiberacao,
            };

            retorno.AutorizacaoOrigem = new Autorizacao
            {
                Usuario = new Profissional { Codigo = UsuarioCopia },
                UsuarioAutorizando = UsuarioLiberacao,
            };

            return retorno;
        }
    }
}


