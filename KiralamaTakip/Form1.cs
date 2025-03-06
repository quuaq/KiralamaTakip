using Npgsql;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace KiralamaTakip
{
    public partial class Form1 : Form
    {
        //Veritabanına bağlanma
        private string connectionString = DatabaseConfig.GetConnectionString();

        public Form1()
        {
            InitializeComponent();
            LoadCustomer();
            SetupDataGridView();
            loadCustomer1();
            LoadComboBox();
            LoadAraclar();
            //LoadServis();
            if (cmbPlaka.Items.Count > 0)
            {
                LoadServisByPlaka(cmbPlaka.Items[0].ToString());
            }
            LoadEngellenenMusteriler();
            SetEngellenenMusteriRenkleri();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

        }





        //Program açıldığında müşterilerin yüklenmesi
        private void LoadCustomer()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT * FROM musteriler ORDER BY musteri_id DESC";

                    using (var command = new NpgsqlCommand(selectQuery, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            DataTable customersTable = new DataTable();
                            customersTable.Load(reader);

                            dataGridView1.DataSource = customersTable;

                            if (dataGridView1.Columns["musteri_id"] != null)
                                dataGridView1.Columns["musteri_id"].Visible = false;

                            //DataGridView'in OK ve * işaretini kaldırma
                            dataGridView1.AllowUserToAddRows = false;
                            dataGridView1.RowHeadersVisible = false;

                            //Sütun başlıklarını değiştirme
                            dataGridView1.Columns["kimlik_numarasi"].HeaderText = "Kimlik No";
                            dataGridView1.Columns["ad"].HeaderText = "Ad";
                            dataGridView1.Columns["soyad"].HeaderText = "Soyad";
                            dataGridView1.Columns["dogum_yeri"].HeaderText = "Doğum Yeri";
                            dataGridView1.Columns["dogum_tarihi"].HeaderText = "Doğum Tarihi";
                            dataGridView1.Columns["ehliyet_numarasi"].HeaderText = "Ehliyet No";
                            dataGridView1.Columns["telefon_numarasi"].HeaderText = "Telefon No";

                            //DateTimePicker Varsayılan Formatı
                            dateTimePicker1.Format = DateTimePickerFormat.Custom;
                            dateTimePicker1.CustomFormat = "dd.MM.yyyy";

                            //Sütun genişliklerini otomatik olarak DataGridView'in genişliğine yayar.
                            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                            // **Engellenen müşterileri kırmızı yap**
                            SetEngellenenMusteriRenkleri();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Müşteriler yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK);
            }
        }

        private void SetEngellenenMusteriRenkleri()
        {
            try
            {
                List<string> engellenenMusteriler = new List<string>();

                // Engellenen müşterileri veritabanından çek
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT kimlik_numarasi FROM engellenen_musteriler";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                engellenenMusteriler.Add(reader["kimlik_numarasi"].ToString());
                            }
                        }
                    }
                }

                // DataGridView sütun kontrolü
                if (!dataGridView1.Columns.Contains("kimlik_numarasi"))
                {
                    MessageBox.Show("Hata: DataGridView'de 'kimlik_numarasi' sütunu bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // **Tüm satırları kontrol edip, engellenenleri kırmızı yap**
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["kimlik_numarasi"].Value != null)
                    {
                        string kimlikNumarasi = row.Cells["kimlik_numarasi"].Value.ToString();

                        if (engellenenMusteriler.Contains(kimlikNumarasi))
                        {
                            row.DefaultCellStyle.BackColor = Color.Red;
                            row.DefaultCellStyle.ForeColor = Color.White;
                        }
                        else
                        {
                            row.DefaultCellStyle.BackColor = Color.White;
                            row.DefaultCellStyle.ForeColor = Color.Black;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Engellenen müşteriler belirlenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void loadCustomer1()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT * FROM musteriler";

                    using (var command = new NpgsqlCommand(selectQuery, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            DataTable customersTable = new DataTable();
                            customersTable.Load(reader);

                            dataGridView2.DataSource = customersTable;

                            if (dataGridView2.Columns["musteri_id"] != null)
                                dataGridView2.Columns["musteri_id"].Visible = false;
                            if (dataGridView2.Columns["dogum_yeri"] != null && dataGridView2.Columns["dogum_tarihi"] != null && dataGridView2.Columns["ehliyet_numarasi"] != null)
                            {
                                dataGridView2.Columns["dogum_yeri"].Visible = false;
                                dataGridView2.Columns["dogum_tarihi"].Visible = false;
                                dataGridView2.Columns["ehliyet_numarasi"].Visible = false;
                                dataGridView2.Columns["telefon_numarasi"].Visible = false;
                            }

                            //Sütun genişliklerini otomatik olarak DataGridView'in genişliğine yayar.
                            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                            //DataGridView'in OK ve * işaretlerini kaldırma
                            dataGridView2.AllowUserToAddRows = false;
                            dataGridView2.RowHeadersVisible = false;

                            //DataGridViewin sutun başlıklarını değiştirme
                            dataGridView2.Columns["kimlik_numarasi"].HeaderText = "Kimlik No";
                            dataGridView2.Columns["ad"].HeaderText = "Ad";
                            dataGridView2.Columns["soyad"].HeaderText = "Soyad";

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Müşteriler yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK);
            }
        }
        

        



        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {


        }

        private void btnMusteriEkle_Click(object sender, EventArgs e)
        {
            if (VerileriKontrolEt()) // Eğer hata varsa işlemi durdur
            {
                MessageBox.Show("Lütfen eksik veya hatalı bilgileri düzeltin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // İşlemi durduruyoruz
            }

            // Kimlik numarasını al
            string kimlikNo = textBox1.Text.Trim();

            // Eğer kimlik numarası boşsa hata ver
            if (string.IsNullOrEmpty(kimlikNo))
            {
                MessageBox.Show("Lütfen kimlik numarasını girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // **1. Önce kimlik numarasının var olup olmadığını kontrol et**
                    string checkQuery = "SELECT COUNT(*) FROM musteriler WHERE kimlik_numarasi = @KimlikNo";

                    using (var checkCommand = new NpgsqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@KimlikNo", kimlikNo);
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count > 0)  // Eğer aynı kimlik numarasına sahip müşteri varsa
                        {
                            MessageBox.Show("Bu kimlik numarasına kayıtlı bir müşteri bulunmaktadır!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return; // İşlemi durdur
                        }
                    }

                    // **2. Eğer müşteri yoksa eklemeye devam et**
                    string ad = textBox2.Text.Trim();
                    string soyad = textBox3.Text.Trim();
                    string dogumYeri = textBox4.Text.Trim();
                    string ehliyetNumarasi = textBox5.Text.Trim();
                    DateTime dogumTarihi = dateTimePicker1.Value;
                    string telefonNumarasi = textBox9.Text.Trim();

                    string insertQuery = "INSERT INTO musteriler (kimlik_numarasi, ad, soyad, dogum_yeri, dogum_tarihi, ehliyet_numarasi, telefon_numarasi) " +
                                         "VALUES (@KimlikNo, @Ad, @Soyad, @DogumYeri, @DogumTarihi, @EhliyetNumarasi, @TelefonNumarasi)";

                    using (var command = new NpgsqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@KimlikNo", kimlikNo);
                        command.Parameters.AddWithValue("@Ad", ad);
                        command.Parameters.AddWithValue("@Soyad", soyad);
                        command.Parameters.AddWithValue("@DogumYeri", dogumYeri);
                        command.Parameters.AddWithValue("@DogumTarihi", dogumTarihi);
                        command.Parameters.AddWithValue("@EhliyetNumarasi", ehliyetNumarasi);
                        command.Parameters.AddWithValue("@TelefonNumarasi", telefonNumarasi);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Müşteri başarıyla eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // **3. Müşteri eklendikten sonra tüm giriş alanlarını temizle**
                            ClearCustomerFields();

                            // **4. DataGridView güncelle**
                            LoadCustomer();
                        }
                        else
                        {
                            MessageBox.Show("Müşteri eklenemedi!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Müşteri eklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void ClearCustomerFields()
        {
            textBox1.Text = ""; // Kimlik No
            textBox2.Text = ""; // Ad
            textBox3.Text = ""; // Soyad
            textBox4.Text = ""; // Doğum Yeri
            textBox5.Text = ""; // Ehliyet Numarası
            textBox9.Text = ""; // Telefon Numarası
            dateTimePicker1.Value = DateTime.Now; // Doğum Tarihini sıfırla

            textBox1.Focus(); // Kimlik numarasına odaklan
        }


        private bool VerileriKontrolEt()
        {
            bool hataVarmi = false;

            // Kimlik Numarası Kontrolü (5 hane ve sadece rakam içermeli)
            if (string.IsNullOrWhiteSpace(textBox1.Text) || !textBox1.Text.All(char.IsDigit) || textBox1.Text.Length < 5)
            {
                label2.ForeColor = Color.Red;
                hataVarmi = true;
            }
            else
            {
                label2.ForeColor = Color.Black;
            }

            // Ad Kontrolü
            if (textBox2.Text.Length < 2 || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                label3.ForeColor = Color.Red;
                hataVarmi = true;
            }
            else
            {
                label3.ForeColor = Color.Black;
            }

            // Soyad Kontrolü
            if (textBox3.Text.Length < 2 || string.IsNullOrWhiteSpace(textBox3.Text))
            {
                label4.ForeColor = Color.Red;
                hataVarmi = true;
            }
            else
            {
                label4.ForeColor = Color.Black;
            }

            // Doğum Yeri Kontrolü
            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                label5.ForeColor = Color.Red;
                hataVarmi = true;
            }
            else
            {
                label5.ForeColor = Color.Black;
            }

            // Ehliyet Numarası Kontrolü
            if (string.IsNullOrEmpty(textBox5.Text))
            {
                label6.ForeColor = Color.Red;
                hataVarmi = true;
            }
            else
            {
                label6.ForeColor = Color.Black;
            }

            // Telefon Numarası Kontrolü (En az 10 haneli ve sadece rakam içermeli)
            if (string.IsNullOrEmpty(textBox9.Text) || textBox9.Text.Length < 10 || !textBox9.Text.All(char.IsDigit))
            {
                label19.ForeColor = Color.Red;
                hataVarmi = true;
            }
            else
            {
                label19.ForeColor = Color.Black;
            }

            return hataVarmi;
        }






        private void txtArama_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // TextBox içindeki metni al
                    string aramaMetni = txtArama.Text;

                    // Eğer placeholder varsa, tüm müşterileri yükle
                    if (string.IsNullOrWhiteSpace(aramaMetni) || aramaMetni == "Müşteri Arama")
                    {
                        LoadCustomer(); // Tüm müşterileri yükle
                        return;
                    }

                    // SQL sorgusu: Ad veya Soyad içinde arama metnini filtrele
                    string query = "SELECT * FROM musteriler WHERE ad ILIKE @AramaMetni OR soyad ILIKE @AramaMetni";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AramaMetni", "%" + aramaMetni + "%");

                        using (var reader = command.ExecuteReader())
                        {
                            DataTable customersTable = new DataTable();
                            customersTable.Load(reader);

                            // DataGridView'e verileri bağla
                            dataGridView1.DataSource = customersTable;
                        }
                        SetEngellenenMusteriRenkleri();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Arama sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        

        private void txtArama_Enter(object sender, EventArgs e)
        {
            // Kullanıcı TextBox'a odaklandığında placeholder'ı sil
            if (txtArama.Text == "Müşteri Arama")
            {
                txtArama.Text = ""; // TextBox'ı boşalt
                txtArama.ForeColor = Color.Black; // Yazı rengini siyah yap
            }
        }

        private void txtArama_Leave(object sender, EventArgs e)
        {
            //Kullanıcı Textboxtan çıktığında içerik boşsa placeholderı geri yükle
            if (string.IsNullOrEmpty(txtArama.Text))
            {
                txtArama.Text = "Müşteri Arama";
                txtArama.ForeColor= Color.Gray;
            }
        }

        private void txtKiraci1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string aramaMetni = txtKiraci1.Text;

                    if (string.IsNullOrWhiteSpace(aramaMetni) || aramaMetni == "     1. Kiracı")
                    {
                        loadCustomer1();
                        return;
                    }

                    string query = "SELECT * FROM musteriler WHERE ad ILIKE @AramaMetni OR soyad ILIKE @AramaMetni";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AramaMetni", "%" + aramaMetni + "%");

                        using (var reader = command.ExecuteReader())
                        {
                            DataTable customersTable = new DataTable();
                            customersTable.Load(reader);

                            dataGridView2.DataSource = customersTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Arama Sırasında Bir Hata Oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtKiraci1_Enter(object sender, EventArgs e)
        {
            if (txtKiraci1.Text == "     1. Kiracı")
            {
                txtKiraci1.Text = "";
                txtKiraci1.ForeColor = Color.Black;
            }
        }

        private void txtKiraci1_Leave_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtKiraci1.Text))
            {
                txtKiraci1.Text = "     1. Kiracı";
                txtKiraci1.ForeColor = Color.Gray;
            }
        }

        private void chkKiraci2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkKiraci2.Checked)
            {
                txtKiraci2.Enabled = true; //2. Kiracı textBoxı aktif 
                txtKiraci2.Text = "      2. Kiracı"; //Varsayılan metni temizleme
                txtKiraci2.ForeColor = Color.Black;
            }
            else
            {
                txtKiraci2.Enabled = false; //2. Kiracı textBoxı devre dışı bıraktık
                txtKiraci2.Text = "      2. Kiracı";
                txtKiraci2.ForeColor= Color.Gray;
                dataGridView3.DataSource = null;
            }
        }

        private void txtKiraci2_TextChanged(object sender, EventArgs e)
        {
            // Eğer CheckBox işaretli değilse, işlem yapılmaz
            if (!chkKiraci2.Checked)
            {
                txtKiraci2.Enabled = false;
                txtKiraci2.Text = "      2. Kiracı";
                txtKiraci2.ForeColor = Color.Gray;
                return;
            }

            if (chkKiraci2.Checked)
            {
                try
                {
                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();

                        string aramaMetni = txtKiraci2.Text;

                        //Textbox boşsa hiç bir iş yapma
                        if(string.IsNullOrEmpty(aramaMetni) || aramaMetni == "      2. Kiracı")
                        {
                            dataGridView3.DataSource = null; // 3. Datagridview'i temizle
                            return;
                        }

                        // SQL sorgusu ile arama yap
                        string query = "SELECT * FROM musteriler WHERE ad ILIKE @AramaMetni OR soyad ILIKE @AramaMetni";

                        using (var command = new NpgsqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@AramaMetni", "%" + aramaMetni + "%");
                            using (var reader = command.ExecuteReader())
                            {
                                DataTable customersTable = new DataTable();
                                customersTable.Load(reader);

                                dataGridView3.DataSource = customersTable;

                                if (dataGridView3.Columns["musteri_id"] != null)
                                    dataGridView3.Columns["musteri_id"].Visible = false;
                                if (dataGridView3.Columns["dogum_yeri"] != null && dataGridView3.Columns["dogum_tarihi"] != null && dataGridView3.Columns["ehliyet_numarasi"] != null && dataGridView3.Columns["telefon_numarasi"] != null)
                                {
                                    dataGridView3.Columns["dogum_yeri"].Visible = false;
                                    dataGridView3.Columns["dogum_tarihi"].Visible = false;
                                    dataGridView3.Columns["ehliyet_numarasi"].Visible = false;
                                    dataGridView3.Columns["telefon_numarasi"].Visible = false;

                                    dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                                    //DataGridView'in OK ve * işaretlerini kaldırma
                                    dataGridView3.AllowUserToAddRows = false;
                                    dataGridView3.RowHeadersVisible = false;

                                    //DataGridViewin sutun başlıklarını değiştirme
                                    dataGridView3.Columns["kimlik_numarasi"].HeaderText = "Kimlik No";
                                    dataGridView3.Columns["ad"].HeaderText = "Ad";
                                    dataGridView3.Columns["soyad"].HeaderText = "Soyad";
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"2. Kiracı için arama sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                dataGridView3.DataSource = null; // CheckBox işaretli değilse 3. DataGridView'i temizle
            }
        }

        private void txtKiraci2_Enter_1(object sender, EventArgs e)
        {
            if (txtKiraci2.Text == "      2. Kiracı")
            {
                txtKiraci2.Text = ""; // Placeholder'ı temizle
                txtKiraci2.ForeColor = Color.Black;
            }
        }

        private void txtKiraci2_Leave_1(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtKiraci2.Text))
    {
                txtKiraci2.Text = "      2. Kiracı"; // Placeholder'ı geri yükle
                txtKiraci2.ForeColor = Color.Gray;
            }
        }

        //Araç Plakası Combobox bilgilerini veritabanından çekme
        private void LoadComboBox()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT plaka FROM araclar";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read()) // Tüm satırları okur
                            {
                                cmbPlaka.Items.Add(reader["plaka"].ToString()); // Plakayı ComboBox'a ekler
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Plakalar yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnBilgileriKaydet_Click(object sender, EventArgs e)
        {
            // Kullanıcıdan alınan değerleri alıyoruz
            string aracPlaka = cmbPlaka.SelectedItem?.ToString();
            DateTime cikisTarihi = dateTimePicker2.Value;
            string cikisSaati = textBox6.Text;

            // Kiracı 1 kimlik numarasını alıyoruz
            string kimlikNo = txtKiraci1.Tag?.ToString();
            if (string.IsNullOrEmpty(kimlikNo))
            {
                MessageBox.Show("Lütfen 1. Kiracıyı seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ad, Soyad ve Telefon No'yu al
            string[] isimSoyisim = txtKiraci1.Text.Split(' ');
            string ad = isimSoyisim.Length > 0 ? isimSoyisim[0] : "";
            string soyad = isimSoyisim.Length > 1 ? isimSoyisim[1] : "";
            string telefonNo = GetTelefonNumarasi(kimlikNo); // Telefon numarasını getir

            // DataGridView6'ya yeni satır ekleyelim
            dataGridView6.Rows.Add(
                aracPlaka, // Plaka
                cikisTarihi.ToString("yyyy-MM-dd"), // Teslim Tarihi
                cikisSaati, // Teslim Saati
                ad, // Ad
                soyad, // Soyad
                telefonNo // Telefon No
            );

            MessageBox.Show("Kiralama bilgileri başarıyla kaydedildi.", "Kiralama Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dataGridView2.Rows[e.RowIndex];
                if (selectedRow != null)
                {
                    // Kimlik numarasını kaydet
                    txtKiraci1.Tag = selectedRow.Cells["kimlik_numarasi"].Value;

                    // Ad ve soyadın birleşik olup olmadığını kontrol et
                    string ad = selectedRow.Cells["ad"].Value?.ToString();
                    string soyad = selectedRow.Cells["soyad"].Value?.ToString();

                    // Eğer soyad boşsa ikinci adı buraya yanlış çekiyor olabilir
                    if (string.IsNullOrEmpty(soyad) && ad.Contains(" "))
                    {
                        var adSoyadDizisi = ad.Split(' ');
                        if (adSoyadDizisi.Length > 1)
                        {
                            ad = adSoyadDizisi[0]; // İlk kelime adı
                            soyad = adSoyadDizisi[1]; // İkinci kelime soyadı
                        }
                    }

                    txtKiraci1.Text = $"{ad} {soyad}"; // Ad ve Soyad birleştirilerek yazdırılır
                }
            }


        }


        private string GetTelefonNumarasi(string kimlikNo)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT telefon_numarasi FROM musteriler WHERE kimlik_numarasi = @KimlikNo";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@KimlikNo", kimlikNo);
                        return command.ExecuteScalar()?.ToString() ?? "Bilinmiyor";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Telefon numarası alınırken hata oluştu: {ex.Message}");
                return "Hata";
            }
        }



        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dataGridView3.Rows[e.RowIndex];
                if (selectedRow != null)
                {
                    txtKiraci2.Tag = selectedRow.Cells["musteri_id"].Value; // 2. Kiracının kimlik numarasını atıyoruz
                    txtKiraci2.Text = $"{selectedRow.Cells["ad"].Value} {selectedRow.Cells["soyad"].Value}"; // Ad ve Soyad
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AracEkleForm aracEkleForm = new AracEkleForm();
            aracEkleForm.ShowDialog(); 
        }

        public void  LoadAraclar()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM araclar";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            DataTable araclarTable = new DataTable();
                            araclarTable.Load(reader);

                            //DataGridView'e bağlanma
                            dataGridView4.DataSource = araclarTable;

                            //Sütun başlıklarını düzenleme
                            dataGridView4.Columns["plaka"].HeaderText = "Plaka";
                            dataGridView4.Columns["model"].HeaderText = "Model";
                            dataGridView4.Columns["marka"].HeaderText = "Marka";

                            //OK ve * kısmını kaldırma
                            dataGridView4.AllowUserToAddRows = false;
                            dataGridView4.RowHeadersVisible = false;

                            //arac_id sütununu gizleme
                            if (dataGridView4.Columns["arac_id"] != null)
                                dataGridView4.Columns["arac_id"].Visible = false;

                            

                            //Otomatik Sütun Genişliği
                            dataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Araçlar yüklenirken bir hata oluştu: {ex.Message}", "Kiralama Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PlakaIleAracSil(string plaka)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM araclar WHERE plaka = @Plaka";
                    using (var command = new NpgsqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Plaka", plaka);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Araç başarıyla silindi!", "Kiralama Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //Araç silindikten sonra DataGridView'i güncelleme
                            LoadAraclar();
                        }
                        else
                        {
                            MessageBox.Show("Araç silinemedi. Lütfen plakayı kontrol edin!", "Kiralama Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Kiralama Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView4.SelectedRows.Count > 0)
            {
                //Seçilen satırdaki plakayı alma
                string plaka = dataGridView4.SelectedRows[0].Cells["plaka"].Value.ToString();

                DialogResult result = MessageBox.Show($"Seçilen aracı (Plaka: {plaka}) silmek istediğinize emin misiniz?", "Kiralama Takip Sistemi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    PlakaIleAracSil(plaka);
                }
                else
                {
                    MessageBox.Show("Lütfen silmek istediğiniz aracı seçin!", "Kiralama Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView4.SelectedRows.Count > 0)
            {
                int aracId = Convert.ToInt32(dataGridView4.SelectedRows[0].Cells["arac_id"].Value);
                AracDuzenleForm aracDuzenleForm = new AracDuzenleForm(aracId);
                aracDuzenleForm.ShowDialog();

                // Düzenleme sonrası DataGridView’i güncelle
                LoadAraclar();
            }
            else
            {
                MessageBox.Show("Lütfen düzenlemek istediğiniz aracı seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //Araç Geçmişi DataGridView'ine verileri getirme
        public void LoadServis()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM servis";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using(var reader = command.ExecuteReader())
                        {
                            DataTable servisTable = new DataTable();
                            servisTable.Load(reader);

                            dataGridView5.DataSource = servisTable;

                            dataGridView5.Columns["id"].Visible = false;
                            dataGridView5.Columns["plaka"].HeaderText = "Plaka";
                            dataGridView5.Columns["km"].HeaderText = "KM";
                            dataGridView5.Columns["tarih"].HeaderText = "Tarih";
                            dataGridView5.Columns["yapilan_islem"].HeaderText = "Yapılan İşlem";

                            dataGridView5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                            dataGridView5.AllowUserToAddRows = false;
                            dataGridView5.RowHeadersVisible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Servis verileri yüklenirken bir hata oluştu: {ex.Message}", "Kiralama Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(dataGridView5.SelectedRows.Count > 0)
            {
                int selectedId = Convert.ToInt32(dataGridView5.SelectedRows[0].Cells["id"].Value);

                try
                {
                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();
                        string updateQuery = "UPDATE servis SET km=@KM, tarih=@Tarih, yapilan_islem=@YapilanIslem WHERE id = @Id";

                        using(var command = new NpgsqlCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@KM", Convert.ToInt32(textBox7.Text));
                            command.Parameters.AddWithValue("@Tarih", dateTimePicker4.Value);
                            command.Parameters.AddWithValue("@YapilanIslem", textBox8.Text);
                            command.Parameters.AddWithValue("@Id", selectedId);

                            int rowsAffected = command.ExecuteNonQuery();
                            if(rowsAffected > 0)
                            {
                                MessageBox.Show("Servis bilgileri başarıyla kaydedildi.", "Kiralama Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadServis();
                            }
                            else
                            {
                                MessageBox.Show("Güncelleme sırasında bir hata oluştu", "Kiralama Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Kiralama Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir servis kaydı seçin!", "Kiralama Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) 
            {
                var selectedRow = dataGridView5.Rows[e.RowIndex]; 

                // TextBox ve DateTimePicker'a verileri aktar
                textBox7.Text = selectedRow.Cells["km"].Value?.ToString(); 
                dateTimePicker4.Value = Convert.ToDateTime(selectedRow.Cells["tarih"].Value); 
                textBox8.Text = selectedRow.Cells["yapilan_islem"].Value?.ToString(); 
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView4.SelectedRows.Count > 0)
            {
                string secilenPlaka = dataGridView4.SelectedRows[0].Cells["plaka"].Value.ToString();

                ServisKayitForm servisKayitForm = new ServisKayitForm(secilenPlaka);
                servisKayitForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Lütfem bir araç seçin!", "Kiralama Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMusteriEngelle_Click(object sender, EventArgs e)
        {
            EngellemeForm engellemeForm = new EngellemeForm();

            // Engelleme tamamlandığında DataGridView'i güncelle
            engellemeForm.EngellemeTamamlandi += LoadEngellenenMusteriler;
            engellemeForm.EngellemeTamamlandi += () =>
            {
                LoadCustomer();  // Güncellenmiş müşteri listesini yükle
                SetEngellenenMusteriRenkleri(); // Engellenen müşterileri kırmızı yap
            };

            engellemeForm.ShowDialog();
        }

        private void LoadEngellenenMusteriler()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT kimlik_numarasi, ad, soyad, ehliyet_numarasi, dogum_yeri, dogum_tarihi, engelleme_nedeni FROM engellenen_musteriler";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            dataGridViewEngellenenler.DataSource = dt;

                            dataGridViewEngellenenler.AllowUserToAddRows = false;
                            dataGridViewEngellenenler.RowHeadersVisible = false;

                            // Sütun başlıklarını düzenleme
                            dataGridViewEngellenenler.Columns["kimlik_numarasi"].HeaderText = "Kimlik No";
                            dataGridViewEngellenenler.Columns["ad"].HeaderText = "Ad";
                            dataGridViewEngellenenler.Columns["soyad"].HeaderText = "Soyad";
                            dataGridViewEngellenenler.Columns["ehliyet_numarasi"].HeaderText = "Ehliyet No";
                            dataGridViewEngellenenler.Columns["dogum_yeri"].HeaderText = "Doğum Yeri";
                            dataGridViewEngellenenler.Columns["dogum_tarihi"].HeaderText = "Doğum Tarihi";
                            dataGridViewEngellenenler.Columns["engelleme_nedeni"].HeaderText = "Engelleme Nedeni";

                            dataGridViewEngellenenler.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Listeleme sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       

        private void btnEngelleKaldir_Click_1(object sender, EventArgs e)
        {
            EngelKaldirForm engelKaldirForm = new EngelKaldirForm();

            // Engel kaldırılınca DataGridView'leri güncelle
            engelKaldirForm.EngelKaldirildi += () => {
                LoadCustomer(); // Müşteri listesini yeniden yükle
                LoadEngellenenMusteriler(); // Engellenenler listesini yeniden yükle
                SetEngellenenMusteriRenkleri(); // Güncel engellenenleri tekrar ayarla
            };

            engelKaldirForm.ShowDialog();
        }

        private void SetupDataGridView()
        {
            //DataGridView'in özelliklerini belirleme
            dataGridView6.AllowUserToAddRows = false; //Kullanıcının yeni satır eklemesini önledik
            dataGridView6.RowHeadersVisible = false; //Satır başlıklarını gizle
            dataGridView6.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; //Eşit kolonlar


            //DataGridViewin kolonları tanımlama işlemleri
            dataGridView6.Columns.Clear();
            dataGridView6.Columns.Add("Plaka", "Plaka");
            dataGridView6.Columns.Add("TeslimTarihi", "Teslim Tarihi");
            dataGridView6.Columns.Add("TeslimSaati", "Teslim Saati");
            dataGridView6.Columns.Add("Ad", "Ad");
            dataGridView6.Columns.Add("Soyad", "Soyad");
            dataGridView6.Columns.Add("TelefonNo", "Tel No");
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dataGridView1.Rows[e.RowIndex];
                if (selectedRow != null)
                {
                    // Kimlik Numarasını kaydet
                    textBox1.Tag = selectedRow.Cells["kimlik_numarasi"].Value;
                    textBox1.Text = selectedRow.Cells["kimlik_numarasi"].Value?.ToString(); // Kimlik numarasını da göster

                    // Ad ve soyad birleşik mi kontrol et
                    string ad = selectedRow.Cells["ad"].Value?.ToString();
                    string soyad = selectedRow.Cells["soyad"].Value?.ToString();

                    // Eğer soyad boşsa ikinci adı buraya yanlış çekiyor olabilir
                    if (string.IsNullOrEmpty(soyad) && ad.Contains(" "))
                    {
                        var adSoyadDizisi = ad.Split(' ');
                        if (adSoyadDizisi.Length > 1)
                        {
                            ad = adSoyadDizisi[0]; // İlk kelime adı
                            soyad = adSoyadDizisi[1]; // İkinci kelime soyadı
                        }
                    }

                    
                    textBox2.Text = ad; // Adı ayrı TextBox'a ekle (Ad alanı)
                    textBox3.Text = soyad; // Soyadı ayrı TextBox'a ekle (Soyad alanı)

                    // 📌 **Diğer Bilgileri Doldurma**
                    textBox4.Text = selectedRow.Cells["dogum_yeri"].Value?.ToString(); // Doğum Yeri
                    textBox5.Text = selectedRow.Cells["ehliyet_numarasi"].Value?.ToString(); // Ehliyet No
                    textBox9.Text = selectedRow.Cells["telefon_numarasi"].Value?.ToString(); // Telefon No
                    dateTimePicker1.Value = Convert.ToDateTime(selectedRow.Cells["dogum_tarihi"].Value); // Doğum Tarihi
                }
            }
        }


        //Ad için baş harf büyütme
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(textBox2.Text.ToLower());
                textBox2.SelectionStart = textBox2.Text.Length;
            }
        }

        //Soyad için baş harf büyütme
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox3.Text))
            {
                textBox3.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(textBox3.Text.ToLower());
                textBox3.SelectionStart = textBox3.Text.Length; // İmleci sona al
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            SetEngellenenMusteriRenkleri();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];

                string kimlikNo = selectedRow.Cells["kimlik_numarasi"].Value.ToString();
                string ad = selectedRow.Cells["ad"].Value.ToString();
                string soyad = selectedRow.Cells["soyad"].Value.ToString();
                string dogumYeri = selectedRow.Cells["dogum_yeri"].Value.ToString();
                DateTime dogumTarihi = Convert.ToDateTime(selectedRow.Cells["dogum_tarihi"].Value);
                string ehliyetNo = selectedRow.Cells["ehliyet_numarasi"].Value.ToString();
                string telefonNo = selectedRow.Cells["telefon_numarasi"].Value.ToString();

                // Yeni formu aç ve bilgileri gönder
                MusteriDuzenleForm duzenleForm = new MusteriDuzenleForm(kimlikNo, ad, soyad, dogumYeri, dogumTarihi, ehliyetNo, telefonNo);
                duzenleForm.ShowDialog();

                // Güncellemeden sonra DataGridView'i yenile
                LoadCustomer();
            }
            else
            {
                MessageBox.Show("Lütfen düzenlemek istediğiniz müşteriyi seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Geçerli bir satıra tıklandı mı?
            {
                string secilenPlaka = dataGridView4.Rows[e.RowIndex].Cells["plaka"].Value.ToString();
                LoadServisByPlaka(secilenPlaka); // Araç geçmişini güncelle
            }
        }

        private void LoadServisByPlaka(string plaka)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM servis WHERE plaka = @Plaka"; // SADECE SEÇİLEN PLAKANIN SERVİSLERİ

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Plaka", plaka);

                        using (var reader = command.ExecuteReader())
                        {
                            DataTable servisTable = new DataTable();
                            servisTable.Load(reader);

                            dataGridView5.DataSource = servisTable; // Araç geçmişi tablosunu güncelle

                            // Eğer sütunları tekrar tekrar eklememek için kontrol ekleyelim
                            if (dataGridView5.Columns.Contains("id"))
                                dataGridView5.Columns["id"].Visible = false;

                            dataGridView5.Columns["plaka"].HeaderText = "Plaka";
                            dataGridView5.Columns["km"].HeaderText = "KM";
                            dataGridView5.Columns["tarih"].HeaderText = "Tarih";
                            dataGridView5.Columns["yapilan_islem"].HeaderText = "Yapılan İşlem";

                            dataGridView5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                            dataGridView5.AllowUserToAddRows = false;
                            dataGridView5.RowHeadersVisible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Araç geçmişi yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}


