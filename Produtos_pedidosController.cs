using exemplo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace exemplo.Controllers
{
    [RoutePrefix("api/produtos_pedidos")]
    public class Produtos_pedidosController : ApiController
    {
        private string ConnectionString = "Data Source=den1.mssql8.gear.host;User Id=softenterdb;Password=gustavo456@;Initial Catalog=softenterdb";

        [HttpGet]
        [Route("produtos/pedido")]
        public HttpResponseMessage Produtos_pedido(string fk_pedido, string fk_empresa)
        {
            try
            {
                List<produtos_pedido> lstprodutos_pedido = new List<produtos_pedido>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "select * from produtos_pedidos a left join produtos b on b.id = a.cod_prod where fk_pedido = " + fk_pedido + " and b.fk_empresa = " + fk_empresa;

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            produtos_pedido produtos_pedido = new produtos_pedido()
                            {
                                Id = (int)(reader["id"] == DBNull.Value ? 0 : Convert.ToInt64(reader["id"])),
                                Cod_prod = reader["cod_prod"] == DBNull.Value ? string.Empty : reader["cod_prod"].ToString(),
                                Nome = reader["nome"] == DBNull.Value ? string.Empty : reader["nome"].ToString(),
                                Qtd = reader["qtd"] == DBNull.Value ? 0 : float.Parse(reader["qtd"].ToString()),
                                Valor_unitario = reader["valor_unitario"] == DBNull.Value ? 0 : float.Parse(reader["valor_unitario"].ToString()),
                                Valor_total = reader["valor_total"] == DBNull.Value ? 0 : float.Parse(reader["valor_total"].ToString()),
                                Fk_pedido = reader["fk_pedido"] == DBNull.Value ? 0 : Convert.ToInt32(reader["fk_pedido"]),
                                Tipo = reader["tipo"] == DBNull.Value ? string.Empty : reader["tipo"].ToString(),
                                Custo = reader["custo"] == DBNull.Value ? string.Empty : reader["custo"].ToString(),
                            };

                            lstprodutos_pedido.Add(produtos_pedido);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstprodutos_pedido.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("produtos/pedido_id")]
        public HttpResponseMessage Produtos_pedido_id(string id, string fk_empresa)
        {
            try
            {
                List<produtos_pedido> lstprodutos_pedido = new List<produtos_pedido>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "select *  from produtos_pedidos a left join produtos b on b.id = a.cod_prod where a.id = " + id + " a.fk_empresa = " + fk_empresa;

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            produtos_pedido produtos_pedido = new produtos_pedido()
                            {
                                Id = (int)(reader["id"] == DBNull.Value ? 0 : Convert.ToInt64(reader["id"])),
                                Cod_prod = reader["cod_prod"] == DBNull.Value ? string.Empty : reader["cod_prod"].ToString(),
                                Nome = reader["nome"] == DBNull.Value ? string.Empty : reader["nome"].ToString(),
                                Qtd = reader["qtd"] == DBNull.Value ? 0 : float.Parse(reader["qtd"].ToString()),
                                Valor_unitario = reader["valor_unitario"] == DBNull.Value ? 0 : float.Parse(reader["valor_unitario"].ToString()),
                                Valor_total = reader["valor_total"] == DBNull.Value ? 0 : float.Parse(reader["valor_total"].ToString()),
                                Fk_pedido = reader["fk_pedido"] == DBNull.Value ? 0 : Convert.ToInt32(reader["fk_pedido"]),
                                Tipo = reader["tipo"] == DBNull.Value ? string.Empty : reader["tipo"].ToString(),
                                Custo = reader["custo"] == DBNull.Value ? string.Empty : reader["custo"].ToString(),
                            };

                            lstprodutos_pedido.Add(produtos_pedido);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstprodutos_pedido.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        [Route("produtos_pedidos/salvar")]
        public HttpResponseMessage Salvar(string nome, string qtd, string valor_unitario,
            string valor_total, string fk_pedido, string cod_prod, string fk_empresa, string custo)
        {
            try
            {
                List<Pedidos> lstPedidos = new List<Pedidos>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "insert into produtos_pedidos (fk_empresa, nome,qtd,valor_unitario,valor_total,fk_pedido, cod_prod, custo)values(" + fk_empresa + ",'" + nome + "' , " + qtd + ", " + valor_unitario + ", " + valor_total + ", " + fk_pedido + ", " + cod_prod + ", " + custo + ")";

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
        [Route("produtos_pedidos/alterar")]
        public HttpResponseMessage Alterar(string id, string nome, string qtd, string valor_unitario,
            string valor_total, string cod_prod, string fk_empresa, string custo)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "update produtos_pedidos set nome = '" + nome + "', qtd = " + qtd + ", valor_unitario = " + valor_unitario + ", valor_total = " + valor_total + ", cod_prod = " + cod_prod + ", custo = " + custo +  " where id = " + id + " and fk_empresa = " + fk_empresa;

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
        [Route("produtos_pedidos/delete")]
        public HttpResponseMessage Delete(string id, string fk_empresa)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "delete from produtos_pedidos where id = " + id + " and fk_empresa = " + fk_empresa;

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

        [HttpDelete]
        [Route("produtos_pedidos/delete_fk_pedido")]
        public HttpResponseMessage Delete_fk_pedido(string fk_pedido, string fk_empresa)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "delete from produtos_pedidos where fk_pedido = " + fk_pedido + " and fk_empresa = " + fk_empresa;

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
