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
    [RoutePrefix("api/pedidos")]
    public class PedidosController : ApiController
    {
        private string ConnectionString = "Data Source=den1.mssql8.gear.host;User Id=softenterdb;Password=gustavo456@;Initial Catalog=softenterdb";
        private string Where;

        [HttpGet]
        [Route("pedidos/todos")]
        public HttpResponseMessage GetAll(string Fk_empresa)
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
                        command.CommandText = " select a.prazo, a.parcelas, CONVERT(VARCHAR(25), a.data_vencimento, 121) as data_vencimento , a.forma_pagamento, a.concluido_por, a.fk_funcionario, a.fk_cod_cli, a.desc_ataca, a.bairro, a.cliente, CONVERT(VARCHAR(25), a.data, 121) as data, a.id, a.concluir, a.endereco, a.cidade, a.celular, a.descricao, b.usuario, (select COALESCE(sum(d.valor_total),0) as valor_total from produtos_pedidos d where d.fk_pedido = a.id) as valor_total from " +
                            "pedidos a left join funcionarios b on b.id = a.fk_funcionario left join produtos_pedidos c on c.fk_pedido = a.id where ((a.data >= GETDATE()) or(a.data <= GETDATE() and a.concluir = 0)) and a.fk_empresa = " + Fk_empresa + " group by c.fk_pedido, a.prazo, a.parcelas, a.data_vencimento, a.forma_pagamento, a.concluido_por, a.fk_funcionario, a.fk_cod_cli, a.desc_ataca, a.bairro, a.cliente, a.data, a.id, a.concluir, a.endereco, a.cidade, a.celular, a.descricao, b.usuario order by a.data";

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Pedidos pedidos = new Pedidos()
                            {
                                Id = reader["id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["id"]),
                                Data = reader["data"] == DBNull.Value ? string.Empty : reader["data"].ToString(),
                                Bairro = reader["bairro"] == DBNull.Value ? string.Empty : reader["bairro"].ToString(),
                                Concluir = reader["concluir"] == DBNull.Value ? 0 : Convert.ToInt32(reader["concluir"]),
                                Desc_ataca = reader["desc_ataca"] == DBNull.Value ? 0 : Convert.ToInt32(reader["desc_ataca"]),
                                Cliente = reader["cliente"] == DBNull.Value ? string.Empty : reader["cliente"].ToString(),
                                Endereco = reader["endereco"] == DBNull.Value ? string.Empty : reader["endereco"].ToString(),
                                Cidade = reader["cidade"] == DBNull.Value ? string.Empty : reader["cidade"].ToString(),
                                Celular = reader["celular"] == DBNull.Value ? string.Empty : reader["celular"].ToString(),
                                Descricao = reader["descricao"] == DBNull.Value ? string.Empty : reader["descricao"].ToString(),
                                Fk_cod_cli = reader["fk_cod_cli"] == DBNull.Value ? string.Empty : reader["fk_cod_cli"].ToString(),
                                Fk_funcionario = reader["fk_funcionario"] == DBNull.Value ? string.Empty : reader["fk_funcionario"].ToString(),
                                Usuario = reader["usuario"] == DBNull.Value ? string.Empty : reader["usuario"].ToString(),
                                Concluido_por = reader["concluido_por"] == DBNull.Value ? string.Empty : reader["concluido_por"].ToString(),
                                Valor_total = reader["valor_total"] == DBNull.Value ? 0 : float.Parse(reader["valor_total"].ToString()),
                                Prazo = reader["prazo"] == DBNull.Value ? string.Empty : reader["prazo"].ToString(),
                                Parcelas = reader["parcelas"] == DBNull.Value ? string.Empty : reader["parcelas"].ToString(),
                                Data_vencimento = reader["usuario"] == DBNull.Value ? string.Empty : reader["data_vencimento"].ToString(),
                                Forma_pagamento = reader["forma_pagamento"] == DBNull.Value ? string.Empty : reader["forma_pagamento"].ToString(),
                                //DataNascimento = reader["data_nascimento"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["data_nascimento"]),
                            };

                            lstPedidos.Add(pedidos);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstPedidos.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("pedidos/maior_id")]
        public HttpResponseMessage Maior_id(string Fk_empresa)
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
                        command.CommandText = "select max(id) id from pedidos where fk_empresa = " + Fk_empresa;

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Pedidos maior_id = new Pedidos()
                            {
                                Id = reader["id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["id"]),
                            };

                            lstPedidos.Add(maior_id);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstPedidos.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }


        [HttpGet]
        [Route("pedidos/pesquisar")]
        public HttpResponseMessage Getpesquisar(string data_ini, string data_fim, string Fk_empresa, string usuario, string cliente_pesq, string nome_prod)
        {
            try
            {

                List<Pedidos> lstPedidos = new List<Pedidos>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        if (usuario != null)
                        {
                            Where = Where + " and b.usuario = '" + usuario + "'";
                        }
                        if (cliente_pesq != null)
                        {
                            Where = Where + " and a.cliente like '%" + cliente_pesq + "%' ";
                        }
                        if (nome_prod != null)
                        {
                            Where = Where + " and c.nome = '" + nome_prod + "'";
                        }
                        

                        command.Connection = connection;
                        command.CommandText = " select a.prazo, a.parcelas, CONVERT(VARCHAR(25), a.data_vencimento, 121) as data_vencimento , a.forma_pagamento, a.concluido_por, a.fk_funcionario, a.fk_cod_cli, a.desc_ataca, a.bairro, a.cliente, CONVERT(VARCHAR(25), a.data, 121) as data, a.id, a.concluir, a.endereco, a.cidade, a.celular, a.descricao, b.usuario, (select COALESCE(sum(d.valor_total),0) as valor_total from produtos_pedidos d where d.fk_pedido = a.id) as valor_total from " +
                            "pedidos a left join funcionarios b on b.id = a.fk_funcionario left join produtos_pedidos c on c.fk_pedido = a.id where (convert(date,CONVERT(VARCHAR(25), a.data, 105),103) >= convert(date,'" + data_ini + "',103) and convert(date,CONVERT(VARCHAR(25), a.data, 105),103) <= convert(date,'" + data_fim + "',103)) and a.fk_empresa = " + Fk_empresa + Where + " group by c.fk_pedido, a.prazo, a.parcelas, a.data_vencimento, a.forma_pagamento, a.concluido_por, a.fk_funcionario, a.fk_cod_cli, a.desc_ataca, a.bairro, a.cliente, a.data, a.id, a.concluir, a.endereco, a.cidade, a.celular, a.descricao, b.usuario order by a.data";

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Pedidos cliente = new Pedidos()
                            {
                                Id = reader["id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["id"]),
                                Data = reader["data"] == DBNull.Value ? string.Empty : reader["data"].ToString(),
                                Bairro = reader["bairro"] == DBNull.Value ? string.Empty : reader["bairro"].ToString(),
                                Concluir = reader["concluir"] == DBNull.Value ? 0 : Convert.ToInt32(reader["concluir"]),
                                Desc_ataca = reader["desc_ataca"] == DBNull.Value ? 0 : Convert.ToInt32(reader["desc_ataca"]),
                                Cliente = reader["cliente"] == DBNull.Value ? string.Empty : reader["cliente"].ToString(),
                                Endereco = reader["endereco"] == DBNull.Value ? string.Empty : reader["endereco"].ToString(),
                                Cidade = reader["cidade"] == DBNull.Value ? string.Empty : reader["cidade"].ToString(),
                                Celular = reader["celular"] == DBNull.Value ? string.Empty : reader["celular"].ToString(),
                                Descricao = reader["descricao"] == DBNull.Value ? string.Empty : reader["descricao"].ToString(),
                                Fk_cod_cli = reader["fk_cod_cli"] == DBNull.Value ? string.Empty : reader["fk_cod_cli"].ToString(),
                                Fk_funcionario = reader["fk_funcionario"] == DBNull.Value ? string.Empty : reader["fk_funcionario"].ToString(),
                                Usuario = reader["usuario"] == DBNull.Value ? string.Empty : reader["usuario"].ToString(),
                                Concluido_por = reader["concluido_por"] == DBNull.Value ? string.Empty : reader["concluido_por"].ToString(),
                                Prazo = reader["prazo"] == DBNull.Value ? string.Empty : reader["prazo"].ToString(),
                                Parcelas = reader["parcelas"] == DBNull.Value ? string.Empty : reader["parcelas"].ToString(),
                                Data_vencimento = reader["usuario"] == DBNull.Value ? string.Empty : reader["data_vencimento"].ToString(),
                                Forma_pagamento = reader["forma_pagamento"] == DBNull.Value ? string.Empty : reader["forma_pagamento"].ToString(),
                                Valor_total = reader["valor_total"] == DBNull.Value ? 0 : float.Parse(reader["valor_total"].ToString()),
                                //DataNascimento = reader["data_nascimento"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["data_nascimento"]),
                            };

                            lstPedidos.Add(cliente);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstPedidos.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("pedidos/agendados")]
        public HttpResponseMessage Getagendados(string data_ini, string data_fim, string Fk_empresa)
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
                        command.CommandText = " select COUNT(CONVERT(VARCHAR(25), a.data, 103)) as n_agendados, CONVERT(VARCHAR(25), a.data, 103) as data from pedidos a " + 
                        " left join funcionarios b on b.id = a.fk_funcionario left join produtos_pedidos c on c.fk_pedido = a.id where(convert(date, CONVERT(VARCHAR(25), a.data, 105), 103) >= convert(date, '" + data_ini + "', 103) and convert(date, CONVERT(VARCHAR(25), a.data, 105), 103) <= convert(date, '" + data_fim +"', 103)) and a.fk_empresa = " + Fk_empresa + " group by CONVERT(VARCHAR(25), a.data, 103)";

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Pedidos pedidos = new Pedidos()
                            {
                                N_agendados = reader["n_agendados"] == DBNull.Value ? 0 : Convert.ToInt32(reader["n_agendados"]),
                                Data = reader["data"] == DBNull.Value ? string.Empty : reader["data"].ToString(),

                            };

                            lstPedidos.Add(pedidos);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstPedidos.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        [Route("pedidos/salvar")]
        public HttpResponseMessage Salvar(string desc_ataca, string concluir, string endereco, string cidade,
            string celular, string descricao, string data, string cliente, string bairro, string Fk_cod_cli, 
            string Fk_empresa, string fk_funcionario, string concluido_por, string prazo, string parcelas, string data_vencimento, string forma_pagamento)
        {
            try
            {
                List<Pedidos> lstPedidos = new List<Pedidos>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        if (data_vencimento != "null")
                        {
                            data_vencimento = "'" + data_vencimento + "'";
                        }

                        command.Connection = connection;
                        command.CommandText = "INSERT INTO pedidos (fk_funcionario, fk_empresa, desc_ataca, bairro, cliente, data, concluir, endereco, cidade, celular, descricao, fk_cod_cli, concluido_por, prazo, parcelas, data_vencimento, forma_pagamento) VALUES ( " + fk_funcionario + ", " + Fk_empresa + ", " + desc_ataca + ",'" + bairro + "','" + cliente + "','" + data + "'," + concluir + ",'" + endereco + "','" + cidade + "','" +
                            celular + "','" + descricao + "'," + Fk_cod_cli + ", " + concluido_por + ", " + prazo + ", " + parcelas + ", " + data_vencimento + ", '" + forma_pagamento + "')";

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
        [Route("pedidos/alterar")]
        public HttpResponseMessage Alterar(string desc_ataca, string concluir, string endereco, string cidade,
            string celular, string descricao, string id, string data, string cliente, string bairro, 
            string Fk_cod_cli, string fk_funcionario, string concluido_por, string fk_empresa, string prazo, 
            string parcelas, string data_vencimento, string forma_pagamento)
        {
            try
            {
                List<Pedidos> lstPedidos = new List<Pedidos>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        if (data_vencimento != "null")
                        {
                            data_vencimento = "'" + data_vencimento + "'";
                        }

                        command.Connection = connection;
                        command.CommandText = "UPDATE pedidos SET forma_pagamento = '" + forma_pagamento + "', data_vencimento = " + data_vencimento + ", parcelas = " + parcelas + ", prazo = " + prazo + ", fk_funcionario = " + fk_funcionario + ", desc_ataca = " + desc_ataca + ", bairro = '" + bairro + "', cliente = '" + cliente + "', data = '" + data + "',concluir = " + concluir + ", endereco = " + "'" + endereco + "'" + ", cidade = " + "'" + cidade + "'" +  ", celular = " + "'" + celular + "'" + ", descricao = " + "'" + descricao + "', fk_cod_cli=" + Fk_cod_cli + ", concluido_por = " + concluido_por + " WHERE id = " + id + " and fk_empresa = " + fk_empresa;

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
        [Route("pedidos/deletar")]
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
                        command.CommandText = "delete from pedidos where id = " + Id + " and fk_empresa = " + fk_empresa;

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
