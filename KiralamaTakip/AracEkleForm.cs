using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KiralamaTakip
{
    public partial class AracEkleForm : Form
    {
        //Veritabanına bağlanma işlemi
        private string connectionString = DatabaseConfig.GetConnectionString();
        public AracEkleForm()
        {
            InitializeComponent(); // 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Textbox'lardan verileri alma
            string plaka = textBox1.Text;
            string model = textBox2.Text;
            string marka = textBox3.Text;


            Form1 anaForm = (Form1)Application.OpenForms["Form1"];
            anaForm.LoadAraclar(); // Ana formdaki araç tablosunu güncelle
            this.Close();

            //Veri kontrolu Doldurulması gereken zorunlu alanlar
            if (string.IsNullOrWhiteSpace(plaka) || string.IsNullOrWhiteSpace(marka) || string.IsNullOrWhiteSpace(model))
            {
                MessageBox.Show("Lütfen tüm alanları doğru doldurunuz!", "Kiralama Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            } 

            //Veritabanına veri kaydetme işlemi
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO araclar (plaka, model, marka) " +
                                         "VALUES (@Plaka, @Model, @Marka)";

                    using (var command = new NpgsqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Plaka", plaka);
                        command.Parameters.AddWithValue("@Model", model);
                        command.Parameters.AddWithValue("@Marka", marka);

                        int rowsAffected = command.ExecuteNonQuery();
                        if(rowsAffected > 0)
                        {
                            MessageBox.Show("Araç Başarıyla Eklendi!", "Kiralama Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            Form1 mainForm = (Form1)Application.OpenForms["Form1"];
                            mainForm.LoadAraclar(); // Ana formdaki DataGridView'i güncelle
                            this.Close();

                        }
                        else
                        {
                            MessageBox.Show("Araç Eklenemedi!", "Kiralama Takip Sistemin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Kiralama Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }   
        }
    }
}
