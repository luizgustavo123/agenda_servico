using exemplo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace exemplo.Controllers
{
    [RoutePrefix("api/agenda_horarios")]
    public class Agenda_horariosController : ApiController
    {
        private string ConnectionString = "Data Source=den1.mssql8.gear.host;User Id=softenterdb;Password=gustavo456@;Initial Catalog=softenterdb";
        private string Where;

        [HttpGet]
        [Route("listar/agenda_horarios")]
        public HttpResponseMessage Agenda_horarios(string Fk_empresa)
        {
            try
            {
                List<Agenda_horarios> lstAgenda_horarios = new List<Agenda_horarios>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "select * from agenda_horarios where fk_empresa = " + Fk_empresa;
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Agenda_horarios Agenda_horarios = new Agenda_horarios()
                            {
                                Id = reader["id"] == DBNull.Value ? string.Empty : reader["id"].ToString(),
                                Aberto = reader["aberto"] == DBNull.Value ? string.Empty : reader["aberto"].ToString(),
                                Dia_semana = reader["dia_semana"] == DBNull.Value ? string.Empty : reader["dia_semana"].ToString(),
                                Horario = reader["horario"] == DBNull.Value ? string.Empty : reader["horario"].ToString(),
                                Fk_empresa = reader["fk_empresa"] == DBNull.Value ? string.Empty : reader["fk_empresa"].ToString(),
                            };

                            lstAgenda_horarios.Add(Agenda_horarios);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstAgenda_horarios.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("listar/qtd_h")]
        public HttpResponseMessage qtd_h(string Fk_empresa)
        {
            try
            {
                List<Agenda_horarios> lstAgenda_horarios = new List<Agenda_horarios>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = " select count(dia_semana) qtd_h, dia_semana from agenda_horarios where fk_empresa = " + Fk_empresa + " group by dia_semana";
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Agenda_horarios Agenda_horarios = new Agenda_horarios()
                            {
                                Qtd_h = reader["qtd_h"] == DBNull.Value ? string.Empty : reader["qtd_h"].ToString(),
                                Dia_semana = reader["dia_semana"] == DBNull.Value ? string.Empty : reader["dia_semana"].ToString(),

                            };

                            lstAgenda_horarios.Add(Agenda_horarios);
                        }
                    }

                    connection.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstAgenda_horarios.ToArray());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        [Route("Agenda_horarios/salvar")]
        public HttpResponseMessage Salvar(string aberto, string dia_semana, string horario, string fk_empresa)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "insert into agenda_horarios (aberto, dia_semana, horario, fk_empresa) values (" + aberto + ", '" + dia_semana + "', '" + horario + "', " + fk_empresa + ")";
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
        [Route("Agenda_horarios/deletar")]
        public HttpResponseMessage Deletar(string dia_semana, string fk_empresa)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "delete from agenda_horarios where dia_semana in (" + dia_semana + ") and  fk_empresa = " + fk_empresa;

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
