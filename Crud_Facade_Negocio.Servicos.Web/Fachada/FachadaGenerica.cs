using Crud_Facade_Negocios.Base.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Crud_Facade_Acesso.Dados.Web.Contexto;
using Crud_Facade_Modelos.Web;

namespace Crud_Facade_Negocios.Servicos.Web.Fachada
{
    public abstract class FachadaGenerica<Tipo> : IFachada<Tipo>
    {
        #region Atributos
        protected Dictionary<string, IContextoDAO> Daos;
        protected Dictionary<string, IList<IValidador>> ValidadoresSalvar;
        protected Dictionary<string, IList<IValidador>> ValidadoresAlterar;
        protected Dictionary<string, IList<IValidador>> ValidadoresExcluir;
        protected Dictionary<string, IList<IValidador>> ValidadoresSalvarTodos;
        protected Dictionary<string, IList<IValidador>> ValidadoresConsultar;
        protected Dictionary<string, IList<IValidador>> ValidadoresAlterarTodos;
        protected Dictionary<string, IList<IValidador>> ValidadoresExcluirTodos;

        protected IDbConnection ConexaoAtiva;
        protected IDbTransaction TransacaoAtiva;
        protected bool TemQueFechar;
        #endregion


        public FachadaGenerica()
        {
            Daos = new Dictionary<string, IContextoDAO>();
            ValidadoresSalvar = new Dictionary<string, IList<IValidador>>();
            ValidadoresAlterar = new Dictionary<string, IList<IValidador>>();
            ValidadoresExcluir = new Dictionary<string, IList<IValidador>>();
            ValidadoresSalvarTodos = new Dictionary<string, IList<IValidador>>();
            ValidadoresAlterarTodos = new Dictionary<string, IList<IValidador>>();
            ValidadoresExcluirTodos = new Dictionary<string, IList<IValidador>>();
            ValidadoresConsultar = new Dictionary<string, IList<IValidador>>();
            ConexaoAtiva = null;
            TransacaoAtiva = null;
            TemQueFechar = true; // Por padrão, tem que fechar
        }


        public string Salvar(Tipo entidade)
        {
            return RealizaModificacao(entidade, // Entidade contendo os valores para serem salvos
                ValidadoresSalvar, // Validadores pelo qual a entidade deve passar antes do banco
                dao => dao.Salvar(entidade)); // Operação que será feita

        }


        public string Alterar(Tipo entidade)
        {
            return RealizaModificacao(entidade, // Entidade contendo os valores para serem salvos
                ValidadoresAlterar, // Validadores pelo qual a entidade deve passar antes do banco
                dao => dao.Alterar(entidade)); // Operação que será feita
        }

        public string Excluir(Tipo entidade)
        {
            return RealizaModificacao(entidade, // Entidade contendo os valores para serem salvos
                ValidadoresExcluir, // Validadores pelo qual a entidade deve passar antes do banco
                dao => dao.Excluir(entidade)); // Operação que será feita
        }

        public IList<Tipo> Consultar(Tipo entidade)
        {
            return RealizaConsulta(entidade); // Nesse caso, não há validação

        }

        public string SalvarTodos(IList<Tipo> entidades)
        {
            return RealizaModificacao(entidades, // Lista contendo os valores para serem salvos
                ValidadoresSalvarTodos, // Validadores pelo qual a entidade deve passar antes do banco
                (dao, valor) => dao.Salvar(valor)); // Operação que será feita        

        } // SalvarTodos

        public string AlterarTodos(IList<Tipo> entidades)
        {
            return RealizaModificacao(entidades, // Lista contendo os valores para serem salvos
                ValidadoresAlterarTodos, // Validadores pelo qual a entidade deve passar antes do banco
                (dao, entidade) => dao.Alterar(entidade)); // Operação que será feita 
        } // AlterarTodos


        public string ExcluirTodos(IList<Tipo> entidades)
        {
            return RealizaModificacao(entidades, // Lista contendo os valores para serem salvos
                ValidadoresExcluirTodos, // Validadores pelo qual a entidade deve passar antes do banco
                (dao, entidade) => dao.Excluir(entidade)); // Operação que será feita 
        } // ExcluirTodos


        public string ConsultarComValidacao(Tipo entidade, IList<Tipo> retorno)
        {
            return RealizaConsulta(entidade, // Entidade contendo os valores para serem salvos
                ValidadoresConsultar, // Validadores pelo qual a entidade deve passar antes do banco
                retorno); // Mensagem de retorno

        } // ConsultarComValidaçao

        public IDbConnection RetornaConexaoAtiva()
        {
            return ConexaoAtiva;
        }

        public IDbTransaction RetornaTransacaoAtiva()
        {
            return TransacaoAtiva;
        }

        public void DefineTemQueFecharConexao(bool temQueFechar)
        {
            this.TemQueFechar = temQueFechar;
        }

        public void SalvaConexaoAtiva(IDbConnection conexao)
        {
            this.ConexaoAtiva = conexao;
        }

        public void SalvaTransacaoAtiva(IDbTransaction transacao)
        {
            this.TransacaoAtiva = transacao;
        }

        #region Métodos Private
        private IContextoDAO InicializaDAO(Tipo entidade)
        {
            IContextoDAO dao = Daos[entidade.GetType().Name];

            if (dao == null)
                return null;

            if (ConexaoAtiva == null)
            {
                dao.AbrirConexao();
                dao.ComecaTransacao();
                ConexaoAtiva = dao.RetornaConexao();
                TransacaoAtiva = dao.RetornaTransacao();
            }
            else
            {
                dao.CompartilhaConexao(ConexaoAtiva);
                dao.CompartilharTransacao(TransacaoAtiva);
            }

            return dao;
        }


        private string Validar(Tipo entidade, IDictionary<string, IList<IValidador>> Validadores)
        {
            if (Validadores != null && Validadores.ContainsKey(entidade.GetType().Name))
            {
                foreach (IValidador val in Validadores[entidade.GetType().Name])
                {
                    val.SalvaConexao(this.ConexaoAtiva);
                    val.SalvaTransacao(this.TransacaoAtiva);
                    string msg = val.Executar(entidade);

                    if (msg == null)
                        continue;

                    IContextoDAO dao = Daos[entidade.GetType().Name];
                    dao.Rollback();
                    dao.FecharConexao();

                    return (msg);

                }
            }

            return null;
        }

        private string RealizaModificacaoNoBD(Tipo entidade, IContextoDAO dao, Action<IContextoDAO> expressao)
        {
            try
            {

                expressao.Invoke(dao); // salva os dados parametrizados
                DarCommit(dao);
                return null;
            }
            catch (Exception e)
            {
                dao.Rollback();
                dao.FecharConexao();
                return e.Message;
            }
        }

        private IList<Tipo> RealizaConsultaNoBD(Tipo entidade, IContextoDAO dao)
        {
            try
            {
                IList<object> retorno = dao.Consultar(entidade); // salva os dados parametrizados
                DarCommit(dao);
                if (retorno == null || retorno.Count == 0)
                    return null;
                IList<Tipo> retornoFinal = retorno.OfType<Tipo>().ToList();

                return retornoFinal;
            }
            catch (Exception e)
            {
                dao.Rollback();
                dao.FecharConexao();
                throw;
            }
        }

        private void DarCommit(IContextoDAO dao)
        {
            if (TemQueFechar == true) // tem que manter a conexão e a transação??
            {
                dao.Commit();
                dao.FecharConexao();
            }
        }


        private string RealizaModificacao(Tipo entidade,
            IDictionary<string, IList<IValidador>> Validadores, Action<IContextoDAO> expressao)
        {

            IContextoDAO dao = this.InicializaDAO(entidade); // Pegar contexto correspondente

            if (dao == null) // Não tem contexto??
                return "Operação Inválida";

            string msg = Validar(entidade, Validadores); // validar entidade

            if (msg != null) // Deu erro??
                return msg;

            return RealizaModificacaoNoBD(entidade, dao, expressao);

        }

        private string RealizaModificacao(IList<Tipo> entidades,
            IDictionary<string, IList<IValidador>> Validadores, Action<IContextoDAO, Tipo> expressao)
        {
            if (entidades == null || entidades.Count == 0)
            {
                Daos[typeof(Tipo).Name].Rollback();
                Daos[typeof(Tipo).Name].FecharConexao();
                return "Nenhum valor informado";
            }

            IContextoDAO dao = this.InicializaDAO(entidades[0]); // Pegar contexto correspondente

            if (dao == null) // Não tem contexto??
                return "Operação Inválida";

            foreach (Tipo valor in entidades)
            {
                string msg = Validar(valor, Validadores); // validar entidade

                if (msg != null) // Deu erro??
                    return msg;
            }


            foreach (Tipo valor in entidades) // realizar Operações
            {
                try
                {
                    expressao.Invoke(dao, valor);
                }
                catch (Exception e)
                {
                    dao.Rollback();
                    dao.FecharConexao();
                    return e.Message;
                }
            }

            DarCommit(dao);

            return null;
        }

        private string RealizaConsulta(Tipo entidade,
            IDictionary<string, IList<IValidador>> Validadores,
            IList<Tipo> retorno)
        {
            IContextoDAO dao = this.InicializaDAO(entidade); // Pegar contexto correspondente

            if (dao == null) // Não tem contexto??
                throw new Exception("Operação Inválida");

            if (Validadores != null)
            {
                string msg = Validar(entidade, Validadores);

                if (msg != null)
                {
                    retorno = null;
                    return msg;
                }
            }

            try
            {

                retorno.Clear();
                IList<Tipo> auxiliar = RealizaConsultaNoBD(entidade, dao);

                if (auxiliar != null)
                {
                    foreach (Tipo valor in auxiliar)
                    {
                        retorno.Add(valor); // salvando valor retornado
                    } // foreach
                } // if

                return null;
            }
            catch (Exception e)
            {
                retorno = null;
                return e.Message;

            }
        }

        private IList<Tipo> RealizaConsulta(Tipo entidade)
        {
            IContextoDAO dao = this.InicializaDAO(entidade); // Pegar contexto correspondente

            if (dao == null) // Não tem contexto??
                throw new Exception("Operação Inválida");

            return RealizaConsultaNoBD(entidade, dao);
        }

        
        #endregion




    } // Definição da classe
}

