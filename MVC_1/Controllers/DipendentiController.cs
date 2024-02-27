using MVC_1.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_1.Controllers
{
    public class DipendentiController : Controller
    {
        public ActionResult Index()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Lavoro"].ToString();
            SqlConnection conn = new SqlConnection(connectionString);

            List<Dipendenti> listaDipendenti = new List<Dipendenti>();

            try
            {
                conn.Open();

                string query = "SELECT * FROM DIPENDENTI";
                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    Dipendenti dipendente = new Dipendenti();
                    dipendente.Id = Convert.ToInt32(reader["Id"]);
                    dipendente.Nome = reader.GetString(1);
                    dipendente.Cognome = reader.GetString(2);
                    dipendente.Indirizzo = reader.GetString(3);
                    dipendente.CodiceFiscale = reader.GetString(4);
                    dipendente.Coniugato = reader.GetBoolean(5);
                    dipendente.NumeroFigli = reader.GetInt32(6);
                    dipendente.Mansione = reader.GetString(7);

                    listaDipendenti.Add(dipendente);



                }
            }
            catch (Exception ex)
            {
                Response.Write($"Errore durante il recupero dei dati: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
            return View(listaDipendenti);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Dipendenti dipendente)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Lavoro"].ToString();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        string query = "INSERT INTO DIPENDENTI (Nome, Cognome, Indirizzo, CodiceFiscale, Coniugato, NumeroFigli, Mansione) " +
                               "VALUES (@Nome, @Cognome, @Indirizzo, @CodiceFiscale, @Coniugato, @NumeroFigli, @Mansione)";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {

                            cmd.Parameters.AddWithValue("@Nome", dipendente.Nome);
                            cmd.Parameters.AddWithValue("@Cognome", dipendente.Cognome);
                            cmd.Parameters.AddWithValue("@Indirizzo", dipendente.Indirizzo);
                            cmd.Parameters.AddWithValue("@CodiceFiscale", dipendente.CodiceFiscale);
                            cmd.Parameters.AddWithValue("@Coniugato", dipendente.Coniugato);
                            cmd.Parameters.AddWithValue("@NumeroFigli", dipendente.NumeroFigli);
                            cmd.Parameters.AddWithValue("@Mansione", dipendente.Mansione);

                            cmd.ExecuteNonQuery();
                        }

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {

                        ModelState.AddModelError(string.Empty, $"Errore durante il salvataggio: {ex.Message}");
                    }
                }
            }
            return View(dipendente);
        }

        public ActionResult Details(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Lavoro"].ToString();
            SqlConnection conn = new SqlConnection(connectionString);

            DettagliDipendente dettagliDipendente = new DettagliDipendente();

            try
            {
                conn.Open();

                // Query per ottenere i dettagli del dipendente
                string dipendenteQuery = "SELECT * FROM DIPENDENTI WHERE Id = @Id";
                using (SqlCommand dipendenteCmd = new SqlCommand(dipendenteQuery, conn))
                {
                    dipendenteCmd.Parameters.AddWithValue("@Id", id);

                    SqlDataReader dipendenteReader = dipendenteCmd.ExecuteReader();

                    if (dipendenteReader.Read())
                    {
                        dettagliDipendente.Dipendente = new Dipendenti
                        {
                            Id = Convert.ToInt32(dipendenteReader["Id"]),
                            Nome = dipendenteReader.GetString(1),
                            Cognome = dipendenteReader.GetString(2),
                            Indirizzo = dipendenteReader.GetString(3),
                            CodiceFiscale = dipendenteReader.GetString(4),
                            Coniugato = dipendenteReader.GetBoolean(5),
                            NumeroFigli = dipendenteReader.GetInt32(6),
                            Mansione = dipendenteReader.GetString(7)
                        };
                    }

                    dipendenteReader.Close();
                }

                // Query per ottenere i pagamenti associati a questo dipendente
                string pagamentiQuery = "SELECT * FROM PAGAMENTI WHERE DipendenteId = @DipendenteId";
                using (SqlCommand pagamentiCmd = new SqlCommand(pagamentiQuery, conn))
                {
                    pagamentiCmd.Parameters.AddWithValue("@DipendenteId", id);

                    SqlDataReader pagamentiReader = pagamentiCmd.ExecuteReader();

                    while (pagamentiReader.Read())
                    {
                        Pagamenti pagamento = new Pagamenti
                        {
                            Id = Convert.ToInt32(pagamentiReader["Id"]),
                            DipendenteId = Convert.ToInt32(pagamentiReader["DipendenteId"]),
                            PeriodoPagamento = pagamentiReader.GetDateTime(2),
                            Ammontare = pagamentiReader.GetDecimal(3),
                            Stipendio = pagamentiReader.GetBoolean(4)
                        };

                        dettagliDipendente.Pagamenti.Add(pagamento);
                    }

                    pagamentiReader.Close();
                }
            }
            catch (Exception ex)
            {
                Response.Write($"Errore durante il recupero dei dati: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }

            return View(dettagliDipendente);
        }





    }
}