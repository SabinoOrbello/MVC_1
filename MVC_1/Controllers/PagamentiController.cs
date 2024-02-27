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
    public class PagamentiController : Controller
    {
        // GET: Pagamenti
        public ActionResult Index()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Lavoro"].ToString();
            SqlConnection conn = new SqlConnection(connectionString);

            List<Pagamenti> listaPagamenti = new List<Pagamenti>();

            try
            {
                conn.Open();

                string query = "SELECT * FROM PAGAMENTI";
                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    Pagamenti pagamento = new Pagamenti();
                    pagamento.Id = Convert.ToInt32(reader["Id"]);
                    pagamento.DipendenteId = reader.GetInt32(1);
                    pagamento.PeriodoPagamento = reader.GetDateTime(2);
                    pagamento.Ammontare = reader.GetDecimal(3);
                    pagamento.Stipendio = reader.GetBoolean(4);


                    listaPagamenti.Add(pagamento);



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
            return View(listaPagamenti);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Pagamenti pagamento)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Lavoro"].ToString();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        string query = "INSERT INTO PAGAMENTI (DipendenteId, PeriodoPagamento, Ammontare, Stipendio) " +
                               "VALUES (@DipendenteId, @PeriodoPagamento, @Ammontare, @Stipendio)";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {

                            cmd.Parameters.AddWithValue("@DipendenteId", pagamento.DipendenteId);
                            cmd.Parameters.AddWithValue("@PeriodoPagamento", pagamento.PeriodoPagamento);
                            cmd.Parameters.AddWithValue("@Ammontare", pagamento.Ammontare);
                            cmd.Parameters.AddWithValue("@Stipendio", pagamento.Stipendio);

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
            return View(pagamento);
        }
    }
}