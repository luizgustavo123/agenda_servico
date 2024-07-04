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
    [RoutePrefix("api/Contas_pagar")]
    public class Contas_pagarController : ApiController
    {
        private string ConnectionString = "Data Source=den1.mssql8.gear.host;User Id=softenterdb;Password=gustavo456@;Initial Catalog=softenterdb";
        private string Where;

        [HttpGet]
        [Route("Contas_pagar/todos")]
        public HttpResponseMessage GetAll(string Fk_empresa)
        {
            try
            {
                List<Contas_pagar> lstContas_pagar = new List<Contas_pagar>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " select pago, fk_empresa, id, descricao, parcela, CONVERT(VARCHAR(25), data_pagamento, 103) data_pagamento, CONVERT(VARCHAR(25), vencimento, 103) vencimento , format(valor, 'N', 'pt-br') as valor_conta from contas_pagar a where a.fk_empresa = " + Fk_empresa + " and (a.pago <> 1 or a.pago is null)";

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Contas_pagar Contas_pagar = new Contas_pagar()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Parcela = reader["parcela"] == DBNull.Value ? string.Empty : reader["parcela"].ToString(),
                                Vencimento = reader["vencimento"] == DBNull.Value ? string.Empty : reader["vencimento"].ToString(),
                                Descricao = reader["descricao"] == DBNull.Value ? string.Empty : reader["descricao"].ToString(),
                                Valor = reader["valor_conta"] == DBNull.Value ? string.Empty : reader["valor_conta"].ToString(),
                                Fk_empresa = reader["fk_empresa"] == DBNull.Value ? string.Empty : reader["fk_empresa"].ToString(),
                                Data_pagamento = reader["data_pagamento"] == DBNull.Value ? string.Empty : reader["data_pagamento"].ToString(),
                                Pago = reader["pago"] == DBNull.Value ? string.Empty : reader["pago"].ToString(),
                            };

                            lstContas_pagar.Add(Contas_pagar);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstContas_pagar.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }




        [HttpGet]
        [Route("Contas_pagar/pesq")]
        public HttpResponseMessage GetAll(string Fk_empresa, string datavenci_ini, string datavenci_fim, string datapag_ini,
            string datapag_fim, string status, string descricao)
        {
            try
            {
                List<Contas_pagar> lstContas_pagar = new List<Contas_pagar>();

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
                        if (status == "A pagar")
                        {
                            Where = Where + " and (a.pago <> 1 or a.pago is null) ";
                        }

                        if (status == "Vencidos")
                        {
                            Where = Where + " and convert(date,CONVERT(VARCHAR(25), a.vencimento, 105),103) <= convert(date,GETDATE(),103) and (a.pago <> 1 or a.pago is null) ";
                        }

                        if(descricao != null)
                        {
                            Where = Where + " and a.descricao like '%" + descricao + "%' ";
                        }

                        command.Connection = connection;
                        command.CommandText = " select pago, fk_empresa, id, descricao, parcela, CONVERT(VARCHAR(25), data_pagamento, 103) data_pagamento, CONVERT(VARCHAR(25), vencimento, 103) vencimento , format(valor, 'N', 'pt-br') as valor_conta from contas_pagar a " +
                                              " where a.fk_empresa = " + Fk_empresa + Where;

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Contas_pagar Contas_pagar = new Contas_pagar()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Parcela = reader["parcela"] == DBNull.Value ? string.Empty : reader["parcela"].ToString(),
                                Vencimento = reader["vencimento"] == DBNull.Value ? string.Empty : reader["vencimento"].ToString(),
                                Descricao = reader["descricao"] == DBNull.Value ? string.Empty : reader["descricao"].ToString(),
                                Valor = reader["valor_conta"] == DBNull.Value ? string.Empty : reader["valor_conta"].ToString(),
                                Fk_empresa = reader["fk_empresa"] == DBNull.Value ? string.Empty : reader["fk_empresa"].ToString(),
                                Data_pagamento = reader["data_pagamento"] == DBNull.Value ? string.Empty : reader["data_pagamento"].ToString(),
                                Pago = reader["pago"] == DBNull.Value ? string.Empty : reader["pago"].ToString(),
                            };

                            lstContas_pagar.Add(Contas_pagar);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstContas_pagar.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        [Route("Contas_pagar/salvar")]
        public HttpResponseMessage Salvar(string pago, string data_pagamento, string descricao, string vencimento, string parcela, string valor, string fk_empresa)
        {
            try
            {
              

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        if (data_pagamento != "null")
                        {
                            data_pagamento = "'" + data_pagamento + "'";
                        }

                        command.Connection = connection;
                        command.CommandText = " insert into contas_pagar (descricao, parcela, valor, vencimento, fk_empresa, data_pagamento, pago) values ('" + descricao + "', '" + parcela + "', " + valor + ", '" + vencimento + "', " + fk_empresa + ", " + data_pagamento + ", " + pago + ") ";

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
        [Route("Contas_pagar/alterar")]
        public HttpResponseMessage Alterar(string id, string pago, string data_pagamento, string descricao, string vencimento, string valor, string fk_empresa)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        if (data_pagamento != "null")
                        {
                            data_pagamento = "'" + data_pagamento + "'";
                        }

                        command.Connection = connection;
                        command.CommandText = " update contas_pagar set descricao = '" + descricao + "', valor = " + valor + ", vencimento = '" + vencimento + "', data_pagamento = " + data_pagamento + ", pago = " + pago + " where id = " + id + " and fk_empresa = " + fk_empresa;

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
        [Route("Contas_pagar/deletar")]
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
                        command.CommandText = "delete from contas_pagar where id = " + Id + " and fk_empresa = " + fk_empresa;

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
