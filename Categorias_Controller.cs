using exemplo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace exemplo.Controllers
{
    [RoutePrefix("api/categorias")]
    public class CategoriasController : ApiController
    {
        private string ConnectionString = "Data Source=den1.mssql8.gear.host;User Id=softenterdb;Password=gustavo456@;Initial Catalog=softenterdb";
        private string Where;

        [HttpGet]
        [Route("listar/categorias")]
        public HttpResponseMessage Produtos(string Fk_empresa)
        {
            try
            {
                List<categorias> lstcategorias = new List<categorias>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " select * from categoria where fk_empresa = " + Fk_empresa + " ORDER BY nome ";
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            categorias categorias = new categorias()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Nome = reader["nome"] == DBNull.Value ? string.Empty : reader["nome"].ToString(),
                                Imagem = reader["imagem"] == DBNull.Value ? string.Empty : reader["imagem"].ToString(),
                            };

                            lstcategorias.Add(categorias);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstcategorias.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("listar/categorias_pesq")]
        public HttpResponseMessage Categorias_pesq(string Nome, string Fk_empresa)
        {
            try
            {
                List<categorias> lstcategorias = new List<categorias>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " select * from categoria where nome like '%" + Nome + "%' and fk_empresa = " + Fk_empresa + "  ORDER BY nome ";
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            categorias categorias = new categorias()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Nome = reader["nome"] == DBNull.Value ? string.Empty : reader["nome"].ToString(),
                                Imagem = reader["imagem"] == DBNull.Value ? string.Empty : reader["imagem"].ToString(),
                            };

                            lstcategorias.Add(categorias);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstcategorias.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        [Route("categorias/salvar")]
        public HttpResponseMessage Salvar(String Nome, string Fk_empresa)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "  insert into categoria(nome, fk_empresa) values ('" + Nome + "', " + Fk_empresa + ")";

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
        [Route("categorias/alterar")]
        public HttpResponseMessage Alterar(String id, String nome, string fk_empresa)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " update categoria set nome = '" + nome + "' where id = " + id + " and fk_empresa = " + fk_empresa;

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
        [Route("categorias/deletar")]
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
                        command.CommandText = " update produtos set fk_categoria = null where fk_categoria = " + Id + " and fk_empresa = " + fk_empresa + " delete from categoria where id = " + Id + " and fk_empresa = " + fk_empresa;
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

        [HttpPost]
        [Route("categorias/altera_foto")]
        public HttpResponseMessage Altera_foto(string fk_empresa, string imagem, string id)
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
                        command.CommandText = "UPDATE categoria SET imagem = COALESCE(imagem, '') + '" + imagem + "' WHERE id = " + id + " AND fk_empresa = " + fk_empresa;
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
        [Route("categorias/deletar_foto")]
        public HttpResponseMessage Deletar_foto(string fk_empresa, string id)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " UPDATE categoria SET imagem = '' where fk_empresa = " + fk_empresa + " and id = " + id;

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
