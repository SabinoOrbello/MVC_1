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


    }
}