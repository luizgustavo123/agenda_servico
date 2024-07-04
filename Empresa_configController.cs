using exemplo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace exemplo.Controllers
{
    [RoutePrefix("api/empresa_config")]
    public class Empresa_configController : ApiController
    {
        private string ConnectionString = "Data Source=den1.mssql8.gear.host;User Id=softenterdb;Password=gustavo456@;Initial Catalog=softenterdb";
        private string Where;

        [HttpGet]
        [Route("listar/empresa_config")]
        public HttpResponseMessage Empresa_config(string Id_empresa)
        {
            try
            {
                List<Empresa_config> lstempresa_config = new List<Empresa_config>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " select taxa_entrega, logo, CONVERT(VARCHAR(25), data_licenca, 103) as data_licenca, id, nome, endereco, telefone, cnpj, nome_link from empresa where id = " + Id_empresa;

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Empresa_config empresa_config = new Empresa_config()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Nome = reader["nome"] == DBNull.Value ? string.Empty : reader["nome"].ToString(),
                                Endereco = reader["endereco"] == DBNull.Value ? string.Empty : reader["endereco"].ToString(),
                                Telefone = reader["telefone"] == DBNull.Value ? string.Empty : reader["telefone"].ToString(),
                                Cnpj = reader["cnpj"] == DBNull.Value ? string.Empty : reader["cnpj"].ToString(),
                                Data_licenca = reader["data_licenca"] == DBNull.Value ? string.Empty : reader["data_licenca"].ToString(),
                                Nome_link = reader["nome_link"] == DBNull.Value ? string.Empty : reader["nome_link"].ToString(),
                                Logo = reader["logo"] == DBNull.Value ? string.Empty : reader["logo"].ToString(),
                                Taxa_entrega = reader["taxa_entrega"] == DBNull.Value ? string.Empty : reader["taxa_entrega"].ToString(),
                            };

                            lstempresa_config.Add(empresa_config);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstempresa_config.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("listar/empresa_nome_link")]
        public HttpResponseMessage Empresa_nome_link(string nome_link)
        {
            try
            {
                List<Empresa_config> lstempresa_config = new List<Empresa_config>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " select taxa_entrega, logo, CONVERT(VARCHAR(25), data_licenca, 103) as data_licenca, id, nome, endereco, telefone, cnpj, nome_link from empresa where nome_link = '" + nome_link + "'";

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Empresa_config empresa_config = new Empresa_config()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Nome = reader["nome"] == DBNull.Value ? string.Empty : reader["nome"].ToString(),
                                Endereco = reader["endereco"] == DBNull.Value ? string.Empty : reader["endereco"].ToString(),
                                Telefone = reader["telefone"] == DBNull.Value ? string.Empty : reader["telefone"].ToString(),
                                Cnpj = reader["cnpj"] == DBNull.Value ? string.Empty : reader["cnpj"].ToString(),
                                Data_licenca = reader["data_licenca"] == DBNull.Value ? string.Empty : reader["data_licenca"].ToString(),
                                Nome_link = reader["nome_link"] == DBNull.Value ? string.Empty : reader["nome_link"].ToString(),
                                Logo = reader["logo"] == DBNull.Value ? string.Empty : reader["logo"].ToString(),
                                Taxa_entrega = reader["taxa_entrega"] == DBNull.Value ? string.Empty : reader["taxa_entrega"].ToString(),
                            };

                            lstempresa_config.Add(empresa_config);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstempresa_config.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        [Route("empresa_config/salvar")]
        public HttpResponseMessage Salvar(string Nome, string Endereco, string Telefone, String Cnpj, string Data_licenca, string Id_empresa, string Nome_link, string Taxa_entrega)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " insert into empresa values (" + Id_empresa + ", '" + Nome + "','" + Endereco + "', '" + Telefone + "', '" + Cnpj + "','" + Data_licenca + "', '" + Nome_link + ", " + Taxa_entrega + "') ";

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
        [Route("empresa_config/alterar")]
        public HttpResponseMessage Alterar(String Id, String Nome, String Endereco, String Telefone, String Cnpj, String Nome_link, string Taxa_entrega)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "update empresa set nome = '" + Nome + "', endereco = '" + Endereco + "', telefone = '" + Telefone + "', cnpj = '" + Cnpj + "', nome_link= '" + Nome_link + "', " + "taxa_entrega= " + Taxa_entrega + " where id = " + Id;

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

        [HttpPost]
        [Route("empresa_config/altera_foto")]
        public HttpResponseMessage Altera_foto(string fk_empresa, string imagem)
        {
            try
            {
                List<Pedidos> lstPedidos = new List<Pedidos>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        if (imagem != "" && imagem != null)
                        {
                            imagem = imagem.Replace("**maisx", "+");
                        }

                        command.Connection = connection;
                        command.CommandText = "UPDATE empresa SET logo = logo + '" + imagem + "' WHERE id = " + fk_empresa;
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

        [HttpDelete]
        [Route("empresa_config/deletar_foto")]
        public HttpResponseMessage Deletar(string fk_empresa)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " UPDATE empresa SET logo = '' where id = " + fk_empresa;

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
