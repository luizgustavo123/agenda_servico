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
    [RoutePrefix("api/financeiro")]
    public class FinanceiroController : ApiController
    {
        private string ConnectionString = "Data Source=den1.mssql8.gear.host;User Id=softenterdb;Password=gustavo456@;Initial Catalog=softenterdb";
        private string Where;

        [HttpGet]
        [Route("financeiro/mes")]
        public HttpResponseMessage GetAll(string empresa)
        {
            try
            {
                List<Financeiro> lstFinanceiro = new List<Financeiro>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " select sum(valor_total) as vendas,  (sum(a.valor_total) - sum(a.custo)) as lucro, (sum(a.valor_total*d.comissao/100)) as valor_comissao from produtos_pedidos a "
                                              + " inner join pedidos b on a.fk_pedido = b.id "
                                           //   + " inner join produtos c on a.cod_prod = c.id "
                                              + " left join funcionarios d on b.fk_funcionario = d.id "
                                              + " where MONTH(b.data ) = month(getdate()) and b.concluir = 1 and b.prazo = 0 and a.fk_empresa = " + empresa;

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Financeiro Financeiro = new Financeiro()
                            {
                                Lucro = reader["lucro"] == DBNull.Value ? string.Empty : reader["lucro"].ToString(),
                                Vendas = reader["vendas"] == DBNull.Value ? string.Empty : reader["vendas"].ToString(),
                                Valor_comissao = reader["valor_comissao"] == DBNull.Value ? string.Empty : reader["valor_comissao"].ToString(),
                                /*  Lucro = reader["lucro"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["lucro"]),
                                  Vendas = reader["vendas"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["vendas"]),*/

                            };

                            lstFinanceiro.Add(Financeiro);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstFinanceiro.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("financeiro/mes_pesq")]
        public HttpResponseMessage GetAll(string Inicio, string Fim, string usuario, string empresa)
        {
            try
            {
                List<Financeiro> lstFinanceiro = new List<Financeiro>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        if (usuario != null)
                        {
                            Where = Where + " and d.usuario = '" + usuario + "'";
                        }
                        command.Connection = connection;
                        command.CommandText = " select sum(valor_total) as vendas,  (sum(a.valor_total) - sum(a.custo)) as lucro, (sum(a.valor_total*d.comissao/100)) as valor_comissao from produtos_pedidos a "
                                              + " inner join pedidos b on a.fk_pedido = b.id "
                                           //   + " inner join produtos c on a.cod_prod = c.id "  
                                              + " left join funcionarios d on b.fk_funcionario = d.id "
                                              + " where CONVERT(DATE, b.data, 103) between CONVERT(DATE, '" + Inicio + "', 103) AND CONVERT(DATE, '" + Fim + "', 103)  and b.concluir = 1 and b.prazo = 0 and a.fk_empresa = " + empresa + Where;
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Financeiro Financeiro = new Financeiro()
                            {
                                Lucro = reader["lucro"] == DBNull.Value ? string.Empty : reader["lucro"].ToString(),
                                Vendas = reader["vendas"] == DBNull.Value ? string.Empty : reader["vendas"].ToString(),
                                Valor_comissao = reader["valor_comissao"] == DBNull.Value ? string.Empty : reader["valor_comissao"].ToString(),
                                /*
                                Lucro = reader["lucro"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["lucro"]),
                                Vendas = reader["vendas"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["vendas"]), */

                            };

                            lstFinanceiro.Add(Financeiro);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstFinanceiro.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("conta_receber/mes")]
        public HttpResponseMessage GetAll2(String empresa)
        {
            try
            {
                List<Financeiro> lstFinanceiro = new List<Financeiro>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " select sum(a.valor) as valor_conta, (sum(a.valor) - sum(a.custo)) as lucro_conta, sum(a.valor*c.comissao/100) as valor_comissao from contas_receber a " +
                             " inner join pedidos b on a.fk_pedido = b.id " +
                             " left join funcionarios c on b.fk_funcionario = c.id " +
                             " where a.pago = 1 and MONTH(a.data_pagamento) = month(getdate()) and a.fk_empresa = " + empresa;

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Financeiro Financeiro = new Financeiro()
                            {
                                Valor_conta = reader["valor_conta"] == DBNull.Value ? string.Empty : reader["valor_conta"].ToString(),
                                Lucro_conta = reader["lucro_conta"] == DBNull.Value ? string.Empty : reader["lucro_conta"].ToString(),
                                Valor_comissao = reader["valor_comissao"] == DBNull.Value ? string.Empty : reader["valor_comissao"].ToString(),
                            };

                            lstFinanceiro.Add(Financeiro);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstFinanceiro.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("conta_pagar/mes")]
        public HttpResponseMessage GetAll3(String empresa)
        {
            try
            {
                List<Financeiro> lstFinanceiro = new List<Financeiro>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " select sum(valor) as valor_conta from contas_pagar where pago = 1 and MONTH(data_pagamento) = month(getdate()) and fk_empresa = " + empresa;

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Financeiro Financeiro = new Financeiro()
                            {
                                Valor_conta = reader["valor_conta"] == DBNull.Value ? string.Empty : reader["valor_conta"].ToString(),
                            };

                            lstFinanceiro.Add(Financeiro);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstFinanceiro.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("conta_receber/mes_pesq")]
        public HttpResponseMessage GetAll2(string Inicio, string Fim, string usuario, string empresa)
        {
            try
            {
                List<Financeiro> lstFinanceiro = new List<Financeiro>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        if (usuario != null)
                        {
                            Where = Where + " and c.usuario = '" + usuario + "'";
                        }
                        command.Connection = connection;
                        command.CommandText = " select sum(a.valor) as valor_conta, (sum(a.valor) - sum(a.custo)) as lucro_conta, sum(a.valor*c.comissao/100) as valor_comissao from contas_receber a " +
                            " inner join pedidos b on a.fk_pedido = b.id " +
                            " left join funcionarios c on b.fk_funcionario = c.id " +
                            " where a.pago = 1 and CONVERT(DATE, a.data_pagamento, 103) between CONVERT(DATE, '" + Inicio + "', 103) AND CONVERT(DATE, '" + Fim + "', 103) and a.fk_empresa = " + empresa + Where;

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Financeiro Financeiro = new Financeiro()
                            {
                                Valor_conta = reader["valor_conta"] == DBNull.Value ? string.Empty : reader["valor_conta"].ToString(),
                                Lucro_conta = reader["lucro_conta"] == DBNull.Value ? string.Empty : reader["lucro_conta"].ToString(),
                                Valor_comissao = reader["valor_comissao"] == DBNull.Value ? string.Empty : reader["valor_comissao"].ToString(),
                            };

                            lstFinanceiro.Add(Financeiro);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstFinanceiro.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("conta_pagar/mes_pesq")]
        public HttpResponseMessage GetAll3(string Inicio, string Fim, string empresa)
        {
            try
            {
                List<Financeiro> lstFinanceiro = new List<Financeiro>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " select sum(valor) as valor_conta from contas_pagar " +
                            " where pago = 1 and CONVERT(DATE, data_pagamento, 103) between CONVERT(DATE, '" + Inicio + "', 103) AND CONVERT(DATE, '" + Fim + "', 103) and fk_empresa = " + empresa;

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Financeiro Financeiro = new Financeiro()
                            {
                                Valor_conta = reader["valor_conta"] == DBNull.Value ? string.Empty : reader["valor_conta"].ToString(),
                            };

                            lstFinanceiro.Add(Financeiro);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstFinanceiro.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }




    }
}
