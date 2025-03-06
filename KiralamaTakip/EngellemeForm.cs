using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace KiralamaTakip
{
    public partial class EngellemeForm : Form
    {
        private string connectionString = DatabaseConfig.GetConnectionString();
        public event Action EngellemeTamamlandi; // Event tanımladık

        public EngellemeForm()
        {
            InitializeComponent();
        }

        private void btnEngelle_Click(object sender, EventArgs e)
        {
            // Kullanıcının girdiği kimlik numarası ve engelleme nedeni
            string kimlikNumarasi = txtKimlikNumarasi.Text.Trim();
            string engellemeNedeni = txtEngellemeNedeni.Text.Trim();

            // Alanlar boşsa uyarı verelim
            if (string.IsNullOrWhiteSpace(kimlikNumarasi) || string.IsNullOrWhiteSpace(engellemeNedeni))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Önce kimlik numarasına sahip müşteri var mı kontrol edelim
                    string selectQuery = "SELECT ad, soyad, kimlik_numarasi, ehliyet_numarasi, dogum_yeri, dogum_tarihi FROM musteriler WHERE kimlik_numarasi = @KimlikNumarasi";

                    using (var selectCmd = new NpgsqlCommand(selectQuery, connection))
                    {
                        selectCmd.Parameters.AddWithValue("@KimlikNumarasi", kimlikNumarasi);

                        using (var reader = selectCmd.ExecuteReader())
                        {
                            if (!reader.Read()) // Eğer müşteri bulunamazsa
                            {
                                MessageBox.Show("Kimlik numarasına ait müşteri bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            // Müşteri bilgilerini alalım
                            string ad = reader["ad"].ToString();
                            string soyad = reader["soyad"].ToString();
                            string ehliyetNumarasi = reader["ehliyet_numarasi"].ToString();
                            string dogumYeri = reader["dogum_yeri"].ToString();
                            DateTime dogumTarihi = Convert.ToDateTime(reader["dogum_tarihi"]);

                            reader.Close(); // Okuma işlemi bitti, kapatabiliriz.

                            // Müşteriyi "engellenen_musteriler" tablosuna ekleyelim
                            string insertQuery = "INSERT INTO engellenen_musteriler (kimlik_numarasi, ad, soyad, ehliyet_numarasi, dogum_yeri, dogum_tarihi, engelleme_nedeni) " +
                                                 "VALUES (@KimlikNumarasi, @Ad, @Soyad, @EhliyetNumarasi, @DogumYeri, @DogumTarihi, @EngellemeNedeni)";

                            using (var insertCmd = new NpgsqlCommand(insertQuery, connection))
                            {
                                insertCmd.Parameters.AddWithValue("@KimlikNumarasi", kimlikNumarasi);
                                insertCmd.Parameters.AddWithValue("@Ad", ad);
                                insertCmd.Parameters.AddWithValue("@Soyad", soyad);
                                insertCmd.Parameters.AddWithValue("@EhliyetNumarasi", ehliyetNumarasi);
                                insertCmd.Parameters.AddWithValue("@DogumYeri", dogumYeri);
                                insertCmd.Parameters.AddWithValue("@DogumTarihi", dogumTarihi);
                                insertCmd.Parameters.AddWithValue("@EngellemeNedeni", engellemeNedeni);

                                insertCmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Müşteri başarıyla engellendi!", "Kiralama Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Engelleme tamamlandığında Form1'e haber veriyoruz
                            EngellemeTamamlandi?.Invoke();

                            this.Close(); // İşlem tamamlanınca formu kapat
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
