using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crud_Facade_Modelos.Web;
using Crud_Facade_Negocios.Base.Web.Interfaces;
using Crud_Facade_Negocios.Servicos.Web.Fachada;

namespace Crud_Facade_Negocios.Servicos.Web.Validador
{
    public class ValidaCamposCopiaDeAutorizacao : ValidadorAbstrato
    {
        public override string Executar(object entidade)
        {
            CopiaDeAutorizacao copia = entidade as CopiaDeAutorizacao;

            if (copia.AutorizacaoOrigem == null || copia.AutorizacaoOrigem.Usuario == null ||
                copia.AutorizacaoOrigem.Usuario.Codigo == null ||
                copia.AutorizacaoOrigem.Usuario.Codigo.Trim() == "")
            {
                return "O usuário de Origem (usuário a copiar) é origatório! Favor informá-lo";
            }


            if (copia.AutorizacaoDestino == null || copia.AutorizacaoDestino.Usuario == null ||
                copia.AutorizacaoDestino.Usuario.Codigo == null ||
                copia.AutorizacaoDestino.Usuario.Codigo.Trim() == "")
            {
                return "O usuário de destino (usuário que terá as permissões copiadas) " +
                    "é origatório! Favor informá-lo";
            }

            return null;

        }
    }
}
