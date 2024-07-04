using exemplo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace exemplo.Controllers
{
    [RoutePrefix("api/produtos")]
    public class ProdutosController : ApiController
    {
        private string ConnectionString = "Data Source=den1.mssql8.gear.host;User Id=softenterdb;Password=gustavo456@;Initial Catalog=softenterdb";
        private string Where;

        [HttpGet]
        [Route("listar/produtos")]
        public HttpResponseMessage Produtos(string Fk_empresa)
        {
            try
            {
                List<produtos> lstprodutos = new List<produtos>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "select fk_adicionais, fk_categoria, categoria, data_validade, fk_empresa, tipo, descricao, imagem_link, id, produto, FORMAT(custo, 'N', 'pt-br') AS 'custo', FORMAT(valor_pessoa_fisica, 'N', 'pt-br') AS 'valor_pessoa_fisica', FORMAT(valor_pessoa_juridica, 'N', 'pt-br') AS 'valor_pessoa_juridica', qtd, estoque_minimo from produtos where fk_empresa = " + Fk_empresa + " ORDER BY produto ";
                        //(valor_pessoa_fisica <> 0 or valor_pessoa_juridica <> 0)
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            produtos produtos = new produtos()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Produto = reader["produto"] == DBNull.Value ? string.Empty : reader["produto"].ToString(),
                                Custo = reader["custo"] == DBNull.Value ? string.Empty : reader["custo"].ToString(),
                                Valor_pessoa_fisica = reader["valor_pessoa_fisica"] == DBNull.Value ? string.Empty : reader["valor_pessoa_fisica"].ToString(),
                                Valor_pessoa_juridica = reader["valor_pessoa_juridica"] == DBNull.Value ? string.Empty : reader["valor_pessoa_juridica"].ToString(),
                                Qtd = reader["qtd"] == DBNull.Value ? string.Empty : reader["qtd"].ToString(),
                                Estoque_minimo = reader["qtd"] == DBNull.Value ? string.Empty : reader["estoque_minimo"].ToString(),
                                Imagem_link = reader["imagem_link"] == DBNull.Value ? string.Empty : reader["imagem_link"].ToString(),
                                Descricao = reader["descricao"] == DBNull.Value ? string.Empty : reader["descricao"].ToString(),
                                Tipo = reader["tipo"] == DBNull.Value ? string.Empty : reader["tipo"].ToString(),
                                Fk_empresa = reader["fk_empresa"] == DBNull.Value ? string.Empty : reader["fk_empresa"].ToString(),
                                Data_validade = reader["data_validade"] == DBNull.Value ? string.Empty : reader["data_validade"].ToString(),
                                Categoria = reader["fk_categoria"] == DBNull.Value ? string.Empty : reader["fk_categoria"].ToString(),
                                Adicionais = reader["fk_adicionais"] == DBNull.Value ? string.Empty : reader["fk_adicionais"].ToString(),
                            };

                            lstprodutos.Add(produtos);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstprodutos.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("listar/produtos_filtro")]
        public HttpResponseMessage Produtos_filtro(String Id, String Nome, string fk_empresa, string filtrar_vencidos, string categoria)
        {
            try
            {
                List<produtos> lstprodutos = new List<produtos>();

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
                            Where = Where + " and produto like '%" + Nome + "%' ";
                        }
                        if(categoria != "null")
                        {
                            Where = Where + " and fk_categoria = " + categoria;
                        }
                        if (filtrar_vencidos == "true")
                        {
                            Where = Where + " and data_validade < (GETDATE() + 60) ";
                            Where = Where + " ORDER BY data_validade ";
                        }
                        else
                        {
                            Where = Where + " ORDER BY produto ";
                        }


                        command.Connection = connection;
                        command.CommandText = "select fk_adicionais, fk_categoria, categoria, data_validade, fk_empresa, tipo, descricao, imagem_link, id, produto, FORMAT(custo, 'N', 'pt-br') AS 'custo', FORMAT(valor_pessoa_fisica, 'N', 'pt-br') AS 'valor_pessoa_fisica', FORMAT(valor_pessoa_juridica, 'N', 'pt-br') AS 'valor_pessoa_juridica', qtd, estoque_minimo from produtos where fk_empresa = " + fk_empresa + Where;
                        //(valor_pessoa_fisica <> 0 or valor_pessoa_juridica<> 0) and
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            produtos produtos = new produtos()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Produto = reader["produto"] == DBNull.Value ? string.Empty : reader["produto"].ToString(),
                                Custo = reader["custo"] == DBNull.Value ? string.Empty : reader["custo"].ToString(),
                                Valor_pessoa_fisica = reader["valor_pessoa_fisica"] == DBNull.Value ? string.Empty : reader["valor_pessoa_fisica"].ToString(),
                                Valor_pessoa_juridica = reader["valor_pessoa_juridica"] == DBNull.Value ? string.Empty : reader["valor_pessoa_juridica"].ToString(),
                                Qtd = reader["qtd"] == DBNull.Value ? string.Empty : reader["qtd"].ToString(),
                                Estoque_minimo = reader["qtd"] == DBNull.Value ? string.Empty : reader["estoque_minimo"].ToString(),
                                Imagem_link = reader["imagem_link"] == DBNull.Value ? string.Empty : reader["imagem_link"].ToString(),
                                Descricao = reader["descricao"] == DBNull.Value ? string.Empty : reader["descricao"].ToString(),
                                Tipo = reader["tipo"] == DBNull.Value ? string.Empty : reader["tipo"].ToString(),
                                Data_validade = reader["data_validade"] == DBNull.Value ? string.Empty : reader["data_validade"].ToString(),
                                Categoria = reader["fk_categoria"] == DBNull.Value ? string.Empty : reader["fk_categoria"].ToString(),
                                Adicionais = reader["fk_adicionais"] == DBNull.Value ? string.Empty : reader["fk_adicionais"].ToString(),
                            };

                            lstprodutos.Add(produtos);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstprodutos.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("listar/produtos_max")]
        public HttpResponseMessage Produtos_max(string fk_empresa)
        {
            try
            {
                List<produtos> lstprodutos = new List<produtos>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "select max(id) as max from produtos where fk_empresa = " + fk_empresa;

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            produtos produtos = new produtos()
                            {
                                Id = reader["max"] == DBNull.Value ? string.Empty : reader["max"].ToString(),
                            };

                            lstprodutos.Add(produtos);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstprodutos.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }


        [HttpGet]
        [Route("listar/produtos_catalogo")]
        public HttpResponseMessage Produtos_catalogo(string primeiro_acesso, string fk_empresa)
        {
            try
            {
                List<produtos> lstprodutos = new List<produtos>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        if (primeiro_acesso == "true")
                        {
                            command.CommandText = "select top 21 fk_adicionais, fk_categoria, tipo, imagem_link, id, produto, FORMAT(custo, 'C', 'pt-br') AS 'custo', FORMAT(valor_pessoa_fisica, 'C', 'pt-br') AS 'valor_pessoa_fisica', FORMAT(valor_pessoa_juridica, 'C', 'pt-br') AS 'valor_pessoa_juridica', qtd, estoque_minimo from produtos where qtd > 0 and fk_empresa = " + fk_empresa + " ORDER BY id";
                        }
                        else
                        {
                            command.CommandText = "select tipo, fk_adicionais, fk_categoria, imagem_link, id, produto, FORMAT(custo, 'C', 'pt-br') AS 'custo', FORMAT(valor_pessoa_fisica, 'C', 'pt-br') AS 'valor_pessoa_fisica', FORMAT(valor_pessoa_juridica, 'C', 'pt-br') AS 'valor_pessoa_juridica', qtd, estoque_minimo from produtos where qtd > 0 and fk_empresa = " + fk_empresa + " ORDER BY id";
                        }
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            produtos produtos_catalogo = new produtos()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Produto = reader["produto"] == DBNull.Value ? string.Empty : reader["produto"].ToString(),
                                Custo = reader["custo"] == DBNull.Value ? string.Empty : reader["custo"].ToString(),
                                Valor_pessoa_fisica = reader["valor_pessoa_fisica"] == DBNull.Value ? string.Empty : reader["valor_pessoa_fisica"].ToString(),
                                Valor_pessoa_juridica = reader["valor_pessoa_juridica"] == DBNull.Value ? string.Empty : reader["valor_pessoa_juridica"].ToString(),
                                Qtd = reader["qtd"] == DBNull.Value ? string.Empty : reader["qtd"].ToString(),
                                Estoque_minimo = reader["qtd"] == DBNull.Value ? string.Empty : reader["estoque_minimo"].ToString(),
                                Imagem_link = reader["imagem_link"] == DBNull.Value ? string.Empty : reader["imagem_link"].ToString(),
                                Tipo = reader["tipo"] == DBNull.Value ? string.Empty : reader["tipo"].ToString(),
                                Adicionais = reader["fk_adicionais"] == DBNull.Value ? string.Empty : reader["fk_adicionais"].ToString(),
                            };

                            lstprodutos.Add(produtos_catalogo);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstprodutos.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("listar/produtos_catalogo_categoria")]
        public HttpResponseMessage produtos_catalogo_categoria(string primeiro_acesso, string categoria, string fk_empresa)
        {
            try
            {
                List<produtos> lstprodutos = new List<produtos>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        if (primeiro_acesso == "true")
                        {
                            command.CommandText = "select top 21 fk_adicionais, fk_categoria, tipo, imagem_link, id, produto, FORMAT(custo, 'C', 'pt-br') AS 'custo', FORMAT(valor_pessoa_fisica, 'C', 'pt-br') AS 'valor_pessoa_fisica', FORMAT(valor_pessoa_juridica, 'C', 'pt-br') AS 'valor_pessoa_juridica', qtd, estoque_minimo from produtos where qtd > 0 and fk_categoria = " + categoria + " and fk_empresa = " + fk_empresa + " ORDER BY id";
                        }
                        else
                        {
                            command.CommandText = "select tipo, fk_adicionais, fk_categoria, imagem_link, id, produto, FORMAT(custo, 'C', 'pt-br') AS 'custo', FORMAT(valor_pessoa_fisica, 'C', 'pt-br') AS 'valor_pessoa_fisica', FORMAT(valor_pessoa_juridica, 'C', 'pt-br') AS 'valor_pessoa_juridica', qtd, estoque_minimo from produtos where qtd > 0 and fk_categoria = " + categoria + " and fk_empresa = " + fk_empresa + " ORDER BY id";
                        }



                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            produtos produtos_catalogo_categoria = new produtos()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Produto = reader["produto"] == DBNull.Value ? string.Empty : reader["produto"].ToString(),
                                Custo = reader["custo"] == DBNull.Value ? string.Empty : reader["custo"].ToString(),
                                Valor_pessoa_fisica = reader["valor_pessoa_fisica"] == DBNull.Value ? string.Empty : reader["valor_pessoa_fisica"].ToString(),
                                Valor_pessoa_juridica = reader["valor_pessoa_juridica"] == DBNull.Value ? string.Empty : reader["valor_pessoa_juridica"].ToString(),
                                Qtd = reader["qtd"] == DBNull.Value ? string.Empty : reader["qtd"].ToString(),
                                Estoque_minimo = reader["qtd"] == DBNull.Value ? string.Empty : reader["estoque_minimo"].ToString(),
                                Tipo = reader["tipo"] == DBNull.Value ? string.Empty : reader["tipo"].ToString(),
                                Imagem_link = reader["imagem_link"] == DBNull.Value ? string.Empty : reader["imagem_link"].ToString(),
                                Adicionais = reader["fk_adicionais"] == DBNull.Value ? string.Empty : reader["fk_adicionais"].ToString(),
                            };

                            lstprodutos.Add(produtos_catalogo_categoria);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstprodutos.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("listar/produtos_nome")]
        public HttpResponseMessage Produtos_nome(string Nome, int Check_estoque_minimo, string fk_empresa)
        {
            try
            {
                List<produtos> lstprodutos = new List<produtos>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;

                        if (Check_estoque_minimo == 1)
                        {
                            command.CommandText = "select fk_adicionais, fk_categoria, categoria, data_validade, tipo, imagem_link, id, produto, FORMAT(custo, 'C', 'pt-br') AS 'custo', FORMAT(valor_pessoa_fisica, 'C', 'pt-br') AS 'valor_pessoa_fisica', FORMAT(valor_pessoa_juridica, 'C', 'pt-br') AS 'valor_pessoa_juridica', qtd, estoque_minimo from produtos where produto like '%" + Nome + "%' and (qtd < estoque_minimo or qtd = 0 ) and fk_empresa = " + fk_empresa + " ORDER BY produto ";
                        }
                        if (Check_estoque_minimo == 2)
                        {
                            command.CommandText = "select fk_adicionais, fk_categoria, categoria, data_validade, tipo, imagem_link, id, produto, FORMAT(custo, 'C', 'pt-br') AS 'custo', FORMAT(valor_pessoa_fisica, 'C', 'pt-br') AS 'valor_pessoa_fisica', FORMAT(valor_pessoa_juridica, 'C', 'pt-br') AS 'valor_pessoa_juridica', qtd, estoque_minimo from produtos where produto like '%" + Nome + "%' and (qtd > 0) and fk_empresa = " + fk_empresa + " ORDER BY produto ";
                        }
                        else
                        {
                            command.CommandText = "select fk_adicionais, fk_categoria, categoria, data_validade, tipo, imagem_link, id, produto, FORMAT(custo, 'C', 'pt-br') AS 'custo', FORMAT(valor_pessoa_fisica, 'C', 'pt-br') AS 'valor_pessoa_fisica', FORMAT(valor_pessoa_juridica, 'C', 'pt-br') AS 'valor_pessoa_juridica', qtd, estoque_minimo from produtos where produto like '%" + Nome + "%' and fk_empresa = " + fk_empresa + " ORDER BY produto ";
                        }

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            produtos produtos = new produtos()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Produto = reader["produto"] == DBNull.Value ? string.Empty : reader["produto"].ToString(),
                                Custo = reader["custo"] == DBNull.Value ? string.Empty : reader["custo"].ToString(),
                                Valor_pessoa_fisica = reader["valor_pessoa_fisica"] == DBNull.Value ? string.Empty : reader["valor_pessoa_fisica"].ToString(),
                                Valor_pessoa_juridica = reader["valor_pessoa_juridica"] == DBNull.Value ? string.Empty : reader["valor_pessoa_juridica"].ToString(),
                                Qtd = reader["qtd"] == DBNull.Value ? string.Empty : reader["qtd"].ToString(),
                                Estoque_minimo = reader["qtd"] == DBNull.Value ? string.Empty : reader["estoque_minimo"].ToString(),
                                Imagem_link = reader["imagem_link"] == DBNull.Value ? string.Empty : reader["imagem_link"].ToString(),
                                Tipo = reader["tipo"] == DBNull.Value ? string.Empty : reader["tipo"].ToString(),
                                Data_validade = reader["data_validade"] == DBNull.Value ? string.Empty : reader["data_validade"].ToString(),
                                Categoria = reader["fk_categoria"] == DBNull.Value ? string.Empty : reader["fk_categoria"].ToString(),
                                Adicionais = reader["fk_adicionais"] == DBNull.Value ? string.Empty : reader["fk_adicionais"].ToString(),
                            };

                            lstprodutos.Add(produtos);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstprodutos.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("listar/produtos_nome_igual")]
        public HttpResponseMessage Produtos_nome_igual(string Nome, string fk_empresa)
        {
            try
            {
                List<produtos> lstprodutos = new List<produtos>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;

                        command.CommandText = "SELECT * FROM PRODUTOS where produto = '" + Nome + "' and fk_empresa = " + fk_empresa;


                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            produtos produtos = new produtos()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Produto = reader["produto"] == DBNull.Value ? string.Empty : reader["produto"].ToString(),
                                Custo = reader["custo"] == DBNull.Value ? string.Empty : reader["custo"].ToString(),
                                Valor_pessoa_fisica = reader["valor_pessoa_fisica"] == DBNull.Value ? string.Empty : reader["valor_pessoa_fisica"].ToString(),
                                Valor_pessoa_juridica = reader["valor_pessoa_juridica"] == DBNull.Value ? string.Empty : reader["valor_pessoa_juridica"].ToString(),
                                Qtd = reader["qtd"] == DBNull.Value ? string.Empty : reader["qtd"].ToString(),
                                Estoque_minimo = reader["qtd"] == DBNull.Value ? string.Empty : reader["estoque_minimo"].ToString(),
                                Tipo = reader["tipo"] == DBNull.Value ? string.Empty : reader["tipo"].ToString(),
                                Data_validade = reader["data_validade"] == DBNull.Value ? string.Empty : reader["data_validade"].ToString(),
                                Categoria = reader["fk_categoria"] == DBNull.Value ? string.Empty : reader["fk_categoria"].ToString(),
                                Adicionais = reader["fk_adicionais"] == DBNull.Value ? string.Empty : reader["fk_adicionais"].ToString(),
                            };

                            lstprodutos.Add(produtos);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstprodutos.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("listar/produtos_id")]
        public HttpResponseMessage Produtos_id(string Id, string fk_empresa)
        {
            try
            {
                List<produtos> lstprodutos = new List<produtos>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;

                        command.CommandText = "SELECT * FROM PRODUTOS where id = " + Id + " and fk_empresa = " + fk_empresa;


                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            produtos produtos = new produtos()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Produto = reader["produto"] == DBNull.Value ? string.Empty : reader["produto"].ToString(),
                                Custo = reader["custo"] == DBNull.Value ? string.Empty : reader["custo"].ToString(),
                                Valor_pessoa_fisica = reader["valor_pessoa_fisica"] == DBNull.Value ? string.Empty : reader["valor_pessoa_fisica"].ToString(),
                                Valor_pessoa_juridica = reader["valor_pessoa_juridica"] == DBNull.Value ? string.Empty : reader["valor_pessoa_juridica"].ToString(),
                                Qtd = reader["qtd"] == DBNull.Value ? string.Empty : reader["qtd"].ToString(),
                                Estoque_minimo = reader["qtd"] == DBNull.Value ? string.Empty : reader["estoque_minimo"].ToString(),
                                Tipo = reader["tipo"] == DBNull.Value ? string.Empty : reader["tipo"].ToString(),
                                Data_validade = reader["data_validade"] == DBNull.Value ? string.Empty : reader["data_validade"].ToString(),
                                Categoria = reader["fk_categoria"] == DBNull.Value ? string.Empty : reader["fk_categoria"].ToString(),
                                Adicionais = reader["fk_adicionais"] == DBNull.Value ? string.Empty : reader["fk_adicionais"].ToString(),
                            };

                            lstprodutos.Add(produtos);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstprodutos.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("listar/valor_estoque")]
        public HttpResponseMessage Valor_estoque(string fk_empresa)
        {
            try
            {
                List<produtos> lstprodutos = new List<produtos>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;

                        command.CommandText = " select FORMAT(sum(qtd * custo), 'N', 'pt-br') AS 'valor_estoque' from produtos where tipo = 'Produto' and fk_empresa = " + fk_empresa;


                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            produtos produtos = new produtos()
                            {
                                Valor_estoque = reader["valor_estoque"] == DBNull.Value ? string.Empty : reader["valor_estoque"].ToString(),
                            };

                            lstprodutos.Add(produtos);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstprodutos.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        [Route("produtos/salvar")]
        public HttpResponseMessage Salvar(string fk_empresa, string Categoria, string Id, string Produto, string Custo,
            string Valor_pessoa_fisica, string Valor_pessoa_juridica, string Qtd, string Estoque_minimo, string Tipo, string Descricao, string data_validade, string Adicionais)
        {
            try
            {
                List<Pedidos> lstPedidos = new List<Pedidos>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        if (data_validade != "null")
                        {
                            data_validade = "'" + data_validade + "'";
                        }

                        command.Connection = connection;
                        command.CommandText = "insert into produtos (fk_adicionais, imagem_link, data_validade, fk_empresa, fk_categoria, id, produto, custo, valor_pessoa_fisica, valor_pessoa_juridica, qtd, estoque_minimo,descricao, tipo )values(" + Adicionais  + ", '', " + data_validade + "," + fk_empresa + "," + Categoria + ", " + Id + ", '" + Produto + "', " + Custo + ", " + Valor_pessoa_fisica + ", " + Valor_pessoa_juridica + "," + Qtd + ", " + Estoque_minimo + ",'" + Descricao + "','" + Tipo + "')";
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
        [Route("produtos/altera_foto")]
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
                        command.CommandText = "UPDATE PRODUTOS SET imagem_link = imagem_link + '" + imagem + "' WHERE id = " + id + " AND fk_empresa = " + fk_empresa;
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
        [Route("produtos/deletar_foto")]
        public HttpResponseMessage Deletar(string fk_empresa, string id)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " UPDATE PRODUTOS SET imagem_link = '' where fk_empresa = " + fk_empresa + " and id = " + id;

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
        [Route("produtos/alterar")]
        public HttpResponseMessage Alterar(string Id_novo, string Categoria, string Id, string Produto, string Custo, string Valor_pessoa_fisica, string Valor_pessoa_juridica, string Qtd, string Estoque_minimo, string Tipo, string Descricao, string fk_empresa, string data_validade, string Adicionais)
        {
            try
            {
                List<Pedidos> lstPedidos = new List<Pedidos>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        if (data_validade != "null")
                        {
                            data_validade = "'" + data_validade + "'";
                        }

                        command.Connection = connection;
                        command.CommandText = "update produtos set fk_adicionais = " + Adicionais +  ", data_validade = " + data_validade + ", id = " + Id_novo + ", fk_categoria = " + Categoria + ", produto = '" + Produto + "', custo = " + Custo + ", valor_pessoa_fisica = " + Valor_pessoa_fisica + ", valor_pessoa_juridica = " + Valor_pessoa_juridica + ", qtd = " + Qtd + ", estoque_minimo = " + Estoque_minimo + ", tipo = '" + Tipo + "', descricao = '" + Descricao + "' where id = " + Id + " and fk_empresa = " + fk_empresa;

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
        [Route("produtos/alterar_qtd")]
        public HttpResponseMessage Alterar_qtd(string Qtd, string Id, string fk_empresa)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " update produtos set qtd = qtd - " + Qtd + " where id = " + Id + " and fk_empresa = " + fk_empresa;

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
        [Route("produtos/deleta_adicionais")]
        public HttpResponseMessage Deleta_adicionais(string Fk_adicionais, string Fk_empresa)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " update produtos set fk_adicionais = null where fk_adicionais = " + Fk_adicionais + " and fk_empresa = " + Fk_empresa;

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
        [Route("produtos/altera_qtd_custo")]
        public HttpResponseMessage Aumenta_qtd(string Custo, string Qtd, string Id, string fk_empresa)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " update produtos set custo = " + Custo + ", qtd = qtd + " + Qtd + " where id = " + Id + " and fk_empresa = " + fk_empresa;

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
        [Route("produtos/aumenta_qtd")]
        public HttpResponseMessage Aumenta_qtd(string Qtd, string Id, string fk_empresa)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " update produtos set qtd = qtd + " + Qtd + " where id = " + Id + " and fk_empresa = " + fk_empresa;

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
        [Route("produtos/deletar")]
        public HttpResponseMessage Deletar()
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " em desuso delete from produtos where  ";

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
        [Route("produtos/deletar_id")]
        public HttpResponseMessage Deletar(String Id)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " delete from produtos where id = " + Id;

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
