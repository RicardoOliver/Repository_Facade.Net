using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud_Facade_Modelos.Web
{
    [Serializable()]
    public class Profissional
    {


        /// <summary>
        /// Login do usuário. Ex: RicardoOliver
        /// </summary>
        public string Codigo { get; set; }

        /// <summary>
        /// Nome do usuário. Ex: Ricardo de Mello Oliveira
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Órgão escolhido pelo usuário no WEB.
        /// </summary>
        public Orgao OrgaoAtual { get; set; }

        /// <summary>
        /// Órgão Padrão do usuário.
        /// </summary>
        public Orgao OrgaoPadrao { get; set; }

        /// <summary>
        /// Todos os órgão que o usuário trabalha.
        /// </summary>
        public IList<Orgao> OrgaosDoUsuario { get; set; }

        /// <summary>
        /// Dados de Liberacao do Usuario
        /// </summary>
        public Aplicativo AplicativoAtual { get; set; }

        /// <summary>
        /// Perfil do Usuário
        /// </summary>
        public Perfil Perfil { get; set; }

    }
}


