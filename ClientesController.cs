using exemplo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace exemplo.Controllers
{
    [RoutePrefix("api/clientes")]
    public class ClientesController : ApiController
    {
        private string ConnectionString = "Data Source=den1.mssql8.gear.host;User Id=softenterdb;Password=gustavo456@;Initial Catalog=softenterdb";
        private string Where;

        [HttpGet]
        [Route("listar/clientes")]
        public HttpResponseMessage Clientes(string Fk_empresa)
        {
            try
            {
                List<Clientes> lstclientes = new List<Clientes>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " select CONVERT(VARCHAR(25), data_nascimento, 103) as data, * from clientes where fk_empresa = " + Fk_empresa + " order by nome ";

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Clientes clientes = new Clientes()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Nome = reader["nome"] == DBNull.Value ? string.Empty : reader["nome"].ToString(),
                                Data_Nascimento = reader["data"] == DBNull.Value ? string.Empty : reader["data"].ToString(),
                                Cpf_cnpj = reader["cpf_cnpj"] == DBNull.Value ? string.Empty : reader["cpf_cnpj"].ToString(),
                                Celular = reader["celular"] == DBNull.Value ? string.Empty : reader["celular"].ToString(),
                                Endereco = reader["endereco"] == DBNull.Value ? string.Empty : reader["endereco"].ToString(),
                                Bairro = reader["bairro"] == DBNull.Value ? string.Empty : reader["bairro"].ToString(),
                                Cidade = reader["cidade"] == DBNull.Value ? string.Empty : reader["cidade"].ToString(),
                                Descricao = reader["descricao"] == DBNull.Value ? string.Empty : reader["descricao"].ToString(),
                                Fk_empresa = reader["fk_empresa"] == DBNull.Value ? string.Empty : reader["fk_empresa"].ToString(),
                            };

                            lstclientes.Add(clientes);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstclientes.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("listar/clientes_pesq")]
        public HttpResponseMessage Clientes_pesq(String Id, String Nome, String Cpf_cnpj, string Fk_empresa)
        {
           
            try
            {
                List<Clientes> lstclientes = new List<Clientes>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();



                    using (SqlCommand command = new SqlCommand())
                    {

                        if (Id != null)
                        {
                            Where = Where + " and id = " + Id;
                        }
                        if (Nome != null)
                        {
                            Where = Where + " and nome like '%" + Nome + "%' ";
                        }
                        if (Cpf_cnpj != null)
                        {
                            Where = Where + " and cpf_cnpj = '" + Cpf_cnpj + "'";
                        }

                        command.Connection = connection;
                        command.CommandText = " select CONVERT(VARCHAR(25), data_nascimento, 103) as data, * from clientes where fk_empresa = " + Fk_empresa + Where;

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Clientes clientes = new Clientes()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Nome = reader["nome"] == DBNull.Value ? string.Empty : reader["nome"].ToString(),
                                Data_Nascimento = reader["data"] == DBNull.Value ? string.Empty : reader["data"].ToString(),
                                Cpf_cnpj = reader["cpf_cnpj"] == DBNull.Value ? string.Empty : reader["cpf_cnpj"].ToString(),
                                Celular = reader["celular"] == DBNull.Value ? string.Empty : reader["celular"].ToString(),
                                Endereco = reader["endereco"] == DBNull.Value ? string.Empty : reader["endereco"].ToString(),
                                Bairro = reader["bairro"] == DBNull.Value ? string.Empty : reader["bairro"].ToString(),
                                Cidade = reader["cidade"] == DBNull.Value ? string.Empty : reader["cidade"].ToString(),
                                Descricao = reader["descricao"] == DBNull.Value ? string.Empty : reader["descricao"].ToString(),
                                Fk_empresa = reader["fk_empresa"] == DBNull.Value ? string.Empty : reader["fk_empresa"].ToString(),
                            };

                            lstclientes.Add(clientes);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstclientes.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        [Route("clientes/salvar")]
        public HttpResponseMessage Salvar(String Nome, String Data_Nascimento, String Cpf_cnpj, String Celular, String Endereco, String Bairro, String Cidade, String Descricao, string Fk_empresa)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " insert into clientes values ('" + Nome + "','" + Data_Nascimento + "', '" + Cpf_cnpj + "', '" + Celular + "','" + Endereco + "','" + Bairro + "','" + Cidade + "','" + Descricao + "', " + Fk_empresa + ") ";

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
        [Route("clientes/alterar")]
        public HttpResponseMessage Alterar(String Id, String Nome, String Data_Nascimento, String Cpf_cnpj, String Celular, String Endereco, String Bairro, String Cidade, String Descricao, string fk_empresa)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " update clientes set nome = '" + Nome + "', data_nascimento = '"+ Data_Nascimento +"', cpf_cnpj = '" + Cpf_cnpj + "', celular = '" + Celular + "', endereco = '" + Endereco + "', bairro = '" + Bairro + "', cidade = '" + Cidade  + "', descricao = '" + Descricao + "' where id = " + Id + " and fk_empresa = " + fk_empresa;

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
        [Route("clientes/deletar")]
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
                        command.CommandText = " delete from clientes where id = " + Id + " and fk_empresa = " + fk_empresa;

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
