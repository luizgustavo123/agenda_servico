using exemplo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace exemplo.Controllers
{
    [RoutePrefix("api/funcionarios")]
    public class FuncionariosController : ApiController
    {
        private string ConnectionString = "Data Source=den1.mssql8.gear.host;User Id=softenterdb;Password=gustavo456@;Initial Catalog=softenterdb";
        private string Where;

        [HttpGet]
        [Route("listar/funcionarios")]
        public HttpResponseMessage Funcionarios(string fk_empresa)
        {
            try
            {
                List<Funcionarios> lstfuncionarios = new List<Funcionarios>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " select * from funcionarios where fk_empresa = " + fk_empresa + " order by usuario ";

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Funcionarios Funcionarios = new Funcionarios()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Usu_adm = reader["usu_adm"] == DBNull.Value ? string.Empty : reader["usu_adm"].ToString(),
                                Usuario = reader["usuario"] == DBNull.Value ? string.Empty : reader["usuario"].ToString(),
                                Senha = reader["senha"] == DBNull.Value ? string.Empty : reader["senha"].ToString(),
                                Descricao = reader["descricao"] == DBNull.Value ? string.Empty : reader["descricao"].ToString(),
                                Comissao = reader["comissao"] == DBNull.Value ? string.Empty : reader["comissao"].ToString(),
                                Nome = reader["nome"] == DBNull.Value ? string.Empty : reader["nome"].ToString(),
                                Fk_empresa = reader["fk_empresa"] == DBNull.Value ? string.Empty : reader["fk_empresa"].ToString(),
                            };

                            lstfuncionarios.Add(Funcionarios);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstfuncionarios.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("listar/funcionarios_pesq")]
        public HttpResponseMessage Funcionarios_pesq(String Usuario, string fk_empresa)
        {
           
            try
            {
                List<Funcionarios> lstFuncionarios = new List<Funcionarios>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();



                    using (SqlCommand command = new SqlCommand())
                    {


                        command.Connection = connection;
                        command.CommandText = " select * from funcionarios where fk_empresa = " + fk_empresa + "and usuario like '%" + Usuario + "%'";

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Funcionarios Funcionarios = new Funcionarios()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Usu_adm = reader["usu_adm"] == DBNull.Value ? string.Empty : reader["usu_adm"].ToString(),
                                Usuario = reader["usuario"] == DBNull.Value ? string.Empty : reader["usuario"].ToString(),
                                Senha = reader["senha"] == DBNull.Value ? string.Empty : reader["senha"].ToString(),
                                Descricao = reader["descricao"] == DBNull.Value ? string.Empty : reader["descricao"].ToString(),
                                Comissao = reader["comissao"] == DBNull.Value ? string.Empty : reader["comissao"].ToString(),
                                Fk_empresa = reader["fk_empresa"] == DBNull.Value ? string.Empty : reader["fk_empresa"].ToString(),
                                Nome = reader["nome"] == DBNull.Value ? string.Empty : reader["nome"].ToString(),
                            };

                            lstFuncionarios.Add(Funcionarios);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstFuncionarios.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("listar/funcionarios_usu")]
        public HttpResponseMessage Funcionarios_usu(String Usuario, String Senha)
        {

            try
            {
                List<Funcionarios> lstFuncionarios = new List<Funcionarios>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();



                    using (SqlCommand command = new SqlCommand())
                    {

                        command.Connection = connection;
                        command.CommandText = " select * from funcionarios where usuario = '" + Usuario + "' and senha = " + "'" + Senha + "'";

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Funcionarios Funcionarios = new Funcionarios()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Usu_adm = reader["usu_adm"] == DBNull.Value ? string.Empty : reader["usu_adm"].ToString(),
                                Usuario = reader["usuario"] == DBNull.Value ? string.Empty : reader["usuario"].ToString(),
                                Senha = reader["senha"] == DBNull.Value ? string.Empty : reader["senha"].ToString(),
                                Descricao = reader["descricao"] == DBNull.Value ? string.Empty : reader["descricao"].ToString(),
                                Comissao = reader["comissao"] == DBNull.Value ? string.Empty : reader["comissao"].ToString(),
                                Fk_empresa = reader["fk_empresa"] == DBNull.Value ? string.Empty : reader["fk_empresa"].ToString(),
                                Nome = reader["nome"] == DBNull.Value ? string.Empty : reader["nome"].ToString(),
                            };

                            lstFuncionarios.Add(Funcionarios);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstFuncionarios.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("listar/funcionarios_usunome")]
        public HttpResponseMessage Funcionarios_usunome(String Usuario)
        {

            try
            {
                List<Funcionarios> lstFuncionarios = new List<Funcionarios>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();



                    using (SqlCommand command = new SqlCommand())
                    {

                        command.Connection = connection;
                        command.CommandText = " select * from funcionarios where usuario = '" + Usuario + "'";

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Funcionarios Funcionarios = new Funcionarios()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Usu_adm = reader["usu_adm"] == DBNull.Value ? string.Empty : reader["usu_adm"].ToString(),
                                Usuario = reader["usuario"] == DBNull.Value ? string.Empty : reader["usuario"].ToString(),
                                Senha = reader["senha"] == DBNull.Value ? string.Empty : reader["senha"].ToString(),
                                Descricao = reader["descricao"] == DBNull.Value ? string.Empty : reader["descricao"].ToString(),
                                Comissao = reader["comissao"] == DBNull.Value ? string.Empty : reader["comissao"].ToString(),
                                Fk_empresa = reader["fk_empresa"] == DBNull.Value ? string.Empty : reader["fk_empresa"].ToString(),
                                Nome = reader["nome"] == DBNull.Value ? string.Empty : reader["nome"].ToString(),
                            };

                            lstFuncionarios.Add(Funcionarios);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstFuncionarios.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("listar/funcionarios_id")]
        public HttpResponseMessage Funcionarios_id(String Id, string fk_empresa)
        {

            try
            {
                List<Funcionarios> lstFuncionarios = new List<Funcionarios>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();



                    using (SqlCommand command = new SqlCommand())
                    {

                        command.Connection = connection;
                        command.CommandText = " select * from funcionarios where id = '" + Id + "' and fk_empresa = " + fk_empresa;

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Funcionarios Funcionarios = new Funcionarios()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Usu_adm = reader["usu_adm"] == DBNull.Value ? string.Empty : reader["usu_adm"].ToString(),
                                Usuario = reader["usuario"] == DBNull.Value ? string.Empty : reader["usuario"].ToString(),
                                Senha = reader["senha"] == DBNull.Value ? string.Empty : reader["senha"].ToString(),
                                Descricao = reader["descricao"] == DBNull.Value ? string.Empty : reader["descricao"].ToString(),
                                Comissao = reader["comissao"] == DBNull.Value ? string.Empty : reader["comissao"].ToString(),
                                Fk_empresa = reader["fk_empresa"] == DBNull.Value ? string.Empty : reader["fk_empresa"].ToString(),
                                Nome = reader["nome"] == DBNull.Value ? string.Empty : reader["nome"].ToString(),
                            };

                            lstFuncionarios.Add(Funcionarios);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstFuncionarios.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }


        [HttpPost]
        [Route("funcionarios/salvar")]
        public HttpResponseMessage Salvar(String Usu_adm, String Usuario, String Senha, String Descricao, String Comissao, string fk_empresa, string nome_func)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " insert into funcionarios values (" + Usu_adm + ",'" + Usuario + "','" + Senha + "','" + Descricao + "'," + Comissao + ", " + fk_empresa + ", '" + nome_func + "') ";

                        SqlDataReader reader = command.ExecuteReader();

                    }

                    connection.Close();
                }

                return Request.CreateResponse("Salvo com sucesso");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        [Route("funcionarios/alterar")]
        public HttpResponseMessage Alterar(String Id, String Usu_adm, String Usuario, String Senha, String Descricao, String Comissao, string fk_empresa, string nome_func)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " update funcionarios set usu_adm = " + Usu_adm +", usuario = '" + Usuario + "', senha = '" + Senha + "', descricao = '" + Descricao + "', comissao = " + Comissao + ", nome = '" + nome_func + "'  where id = " + Id + " and fk_empresa = " + fk_empresa;

                        SqlDataReader reader = command.ExecuteReader();

                    }

                    connection.Close();
                }

                return Request.CreateResponse("Alterado com sucesso");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpDelete]
        [Route("funcionarios/deletar")]
        public HttpResponseMessage Deletar(String Id, string fk_empresa)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " delete from funcionarios where id = " + Id + " and fk_empresa = " + fk_empresa;

                        SqlDataReader reader = command.ExecuteReader();

                    }

                    connection.Close();
                }

                return Request.CreateResponse("Deletado com sucesso");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }

}
