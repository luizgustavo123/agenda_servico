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
    [RoutePrefix("api/Contas_receber")]
    public class Contas_receberController : ApiController
    {
        private string ConnectionString = "Data Source=den1.mssql8.gear.host;User Id=softenterdb;Password=gustavo456@;Initial Catalog=softenterdb";
        private string Where;

        [HttpGet]
        [Route("Contas_receber/todos")]
        public HttpResponseMessage GetAll(string Fk_empresa)
        {
            try
            {
                List<Contas_receber> lstContas_receber = new List<Contas_receber>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " select a.id, a.parcela, CONVERT(VARCHAR(25), a.vencimento, 103) vencimento, b.cliente, format(a.valor, 'N', 'pt-br') as valor, a.fk_empresa, a.fk_pedido, CONVERT(VARCHAR(25), a.data_pagamento, 103) data_pagamento, a.pago,custo, b.forma_pagamento from contas_receber a " +
                                              " inner join pedidos b on b.id = a.fk_pedido " +
                                              " where a.fk_empresa = " + Fk_empresa + " and (a.pago <> 1 or a.pago is null)";

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Contas_receber contas_receber = new Contas_receber()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Parcela = reader["parcela"] == DBNull.Value ? string.Empty : reader["parcela"].ToString(),
                                Vencimento = reader["vencimento"] == DBNull.Value ? string.Empty : reader["vencimento"].ToString(),
                                Cliente = reader["cliente"] == DBNull.Value ? string.Empty : reader["cliente"].ToString(),
                                Valor = reader["valor"] == DBNull.Value ? string.Empty : reader["valor"].ToString(),
                                Fk_empresa = reader["fk_empresa"] == DBNull.Value ? string.Empty : reader["fk_empresa"].ToString(),
                                Fk_pedido = reader["fk_pedido"] == DBNull.Value ? string.Empty : reader["fk_pedido"].ToString(),
                                Data_pagamento = reader["data_pagamento"] == DBNull.Value ? string.Empty : reader["data_pagamento"].ToString(),
                                Pago = reader["pago"] == DBNull.Value ? string.Empty : reader["pago"].ToString(),
                                Custo = reader["custo"] == DBNull.Value ? string.Empty : reader["custo"].ToString(),
                                Forma_pagamento = reader["forma_pagamento"] == DBNull.Value ? string.Empty : reader["forma_pagamento"].ToString(),
                            };

                            lstContas_receber.Add(contas_receber);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstContas_receber.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }




        [HttpGet]
        [Route("Contas_receber/pesq")]
        public HttpResponseMessage GetAll(string Fk_empresa, string datavenci_ini, string datavenci_fim, string datapag_ini,
            string datapag_fim, string status, string cliente)
        {
            try
            {
                List<Contas_receber> lstContas_receber = new List<Contas_receber>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        if(datavenci_ini != null)
                        {
                            Where = Where + " and convert(date, CONVERT(VARCHAR(25), a.vencimento, 105), 103) >= convert(date, '" + datavenci_ini + "', 103) ";
                        }
                        if (datavenci_fim != null)
                        {
                            Where = Where + " and convert(date,CONVERT(VARCHAR(25), a.vencimento, 105),103) <= convert(date, '" + datavenci_fim + "',103) ";
                        }

                        if (datapag_ini != null)
                        {
                            Where = Where + " and convert(date, CONVERT(VARCHAR(25), a.data_pagamento, 105), 103) >= convert(date, '" + datapag_ini + "', 103) ";
                        }
                        if (datapag_fim != null)
                        {
                            Where = Where + " and convert(date,CONVERT(VARCHAR(25), a.data_pagamento, 105),103) <= convert(date, '" + datapag_fim + "',103) ";
                        }

                        if(status == "Pago")
                        {
                            Where = Where + " and a.pago = 1 ";
                        }
                        if (status == "A receber")
                        {
                            Where = Where + " and (a.pago <> 1 or a.pago is null) ";
                        }

                        if (status == "Vencidos")
                        {
                            Where = Where + " and convert(date,CONVERT(VARCHAR(25), a.vencimento, 105),103) <= convert(date,GETDATE(),103) and (a.pago <> 1 or a.pago is null) ";
                        }

                        if(cliente != null)
                        {
                            Where = Where + " and b.cliente like '%" + cliente + "%' ";
                        }

                        command.Connection = connection;
                        command.CommandText = " select a.id, a.parcela, CONVERT(VARCHAR(25), a.vencimento, 103) vencimento, b.cliente, format(a.valor, 'N', 'pt-br') as valor, a.fk_empresa, a.fk_pedido, CONVERT(VARCHAR(25), a.data_pagamento, 103) data_pagamento, a.pago,custo, b.forma_pagamento from contas_receber a " +
                                              " inner join pedidos b on b.id = a.fk_pedido " +
                                              " where a.fk_empresa = " + Fk_empresa + Where;

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Contas_receber contas_receber = new Contas_receber()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Parcela = reader["parcela"] == DBNull.Value ? string.Empty : reader["parcela"].ToString(),
                                Vencimento = reader["vencimento"] == DBNull.Value ? string.Empty : reader["vencimento"].ToString(),
                                Cliente = reader["cliente"] == DBNull.Value ? string.Empty : reader["cliente"].ToString(),
                                Valor = reader["valor"] == DBNull.Value ? string.Empty : reader["valor"].ToString(),
                                Fk_empresa = reader["fk_empresa"] == DBNull.Value ? string.Empty : reader["fk_empresa"].ToString(),
                                Fk_pedido = reader["fk_pedido"] == DBNull.Value ? string.Empty : reader["fk_pedido"].ToString(),
                                Data_pagamento = reader["data_pagamento"] == DBNull.Value ? string.Empty : reader["data_pagamento"].ToString(),
                                Pago = reader["pago"] == DBNull.Value ? string.Empty : reader["pago"].ToString(),
                                Custo = reader["custo"] == DBNull.Value ? string.Empty : reader["custo"].ToString(),
                                Forma_pagamento = reader["forma_pagamento"] == DBNull.Value ? string.Empty : reader["forma_pagamento"].ToString(),
                            };

                            lstContas_receber.Add(contas_receber);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstContas_receber.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        [Route("Contas_receber/salvar")]
        public HttpResponseMessage Salvar(string vencimento, string parcela, string valor, string fk_empresa, string fk_pedido, string custo)
        {
            try
            {
              

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {

                        command.Connection = connection;
                        command.CommandText = " insert into contas_receber (vencimento, parcela, valor, fk_empresa, fk_pedido, custo) values ('" + vencimento + "', '" + parcela + "', "  + valor + ", " + fk_empresa + ", " + fk_pedido + ", " + custo + ") ";

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
        [Route("Contas_receber/alterar")]
        public HttpResponseMessage Alterar(string id, string fk_empresa, string valor, string vencimento, string pago, string data_pagamento)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        if (data_pagamento == "")
                        {
                            data_pagamento = null;
                        }
                        else
                        {
                            data_pagamento = "'" + data_pagamento + "'";
                        }

                        command.Connection = connection;
                        command.CommandText = " update contas_receber set valor = " + valor + ", vencimento = '" + vencimento + "', pago = " + pago + ", data_pagamento = " + data_pagamento + " where id = " + id + " and fk_empresa = " + fk_empresa;

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
        [Route("Contas_receber/deletar")]
        public HttpResponseMessage Deletar(string Id, string fk_empresa)
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
                        command.CommandText = "delete from contas_receber where id = " + Id + " and fk_empresa = " + fk_empresa;

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
        [Route("Contas_receber/deletar_id_pedido")]
        public HttpResponseMessage Deletar_id_pedido(string Id_pedido, string fk_empresa)
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
                        command.CommandText = "delete from contas_receber where fk_pedido = " + Id_pedido + " and fk_empresa = " + fk_empresa;

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
