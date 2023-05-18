using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Sinema_Otomasyonu_Şeysi
{
    public partial class Seans : Form
    {
        public Seans()
        {
            InitializeComponent();
        }

        void baglan()
        {
            if (baglanti.State == ConnectionState.Open)
            {
                baglanti.Close();
            }
            baglanti.Open();
        }
        void film_doldur()
        {
            try
            {
                baglan();
                OleDbCommand komut = new OleDbCommand("select film_adi from filmler", baglanti);
                OleDbDataReader oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    comboBox1.Items.Add(oku["film_adi"].ToString());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Hata !" + e);
            }
            baglanti.Close();

        }
        void seans_doldur()
        {
            try
            {
                comboBox2.Items.Clear();
                comboBox4.Items.Clear();
                baglan();
                OleDbCommand komut = new OleDbCommand("select seans_saatleri from seans_saat order by seans_saatleri asc", baglanti);
                OleDbDataReader oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    comboBox2.Items.Add(oku["seans_saatleri"].ToString());
                    comboBox4.Items.Add(oku["seans_saatleri"].ToString());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Hata !" + e);
            }
            baglanti.Close();
        }
        void salon_doldur()
        {
            try
            {
                baglan();
                OleDbCommand komut = new OleDbCommand("select * from salonlar", baglanti);
                OleDbDataReader oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    comboBox3.Items.Add(oku[1].ToString());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Hata !" + e);
            }
            baglanti.Close();
        }

        void seans_ekleme()
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen Bir Film Seçiniz !");
                comboBox1.Focus();
            }
            else if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen Seansı Seçiniz");
                comboBox2.Focus();
            }
            else if (comboBox3.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen Salonu Seçiniz");
                comboBox3.Focus();
            }
            else
            {
                string film = comboBox1.SelectedItem.ToString();
                string seans = comboBox2.SelectedItem.ToString();
                string salon = comboBox3.SelectedItem.ToString();
                try
                {
                    baglan();
                    OleDbCommand kontrol = new OleDbCommand("select * from seanslar where seans1='" + seans + "' and seans1_sa='" + salon + "' ", baglanti);
                    int sonuc = Convert.ToInt32(kontrol.ExecuteScalar());
                    if (sonuc != 0)
                    {
                        MessageBox.Show("Bu Seans Doludur!", "Dolu Seans!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        baglanti.Close();
                        comboBox2.SelectedIndex = -1;
                        comboBox3.SelectedIndex = -1;
                    }
                    else
                    {
                        OleDbCommand komut = new OleDbCommand("insert into seanslar(film_adi,seans1,seans1_sa) values('" + film + "','" + seans + "', '" + salon + "')", baglanti);
                        komut.ExecuteNonQuery();
                        MessageBox.Show("Seans Eklendi!", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        baglanti.Close();
                        comboBox1.SelectedIndex = -1;
                        comboBox2.SelectedIndex = -1;
                        comboBox3.SelectedIndex = -1;
                    }
                }

                catch (Exception a)
                {
                    MessageBox.Show("Hata !" + a);
                }
            }
        }

        void seans_saat_ekle()
        {
            try
            {
                baglan();
                string text = maskedTextBox1.Text.ToString();
                OleDbCommand kontrol = new OleDbCommand("select * from seans_saat where seans_saatleri='" + text + "' ", baglanti);
                int sonuc = Convert.ToInt32(kontrol.ExecuteScalar());
                if (sonuc != 0)
                {
                    MessageBox.Show("Bu Seans Saati Kayıtlıdır !", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    OleDbCommand komut = new OleDbCommand("insert into seans_saat(seans_saatleri) values('" + text + "')", baglanti);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Seans Saati Eklendi!", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                baglanti.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Hata !" + e);
            }
            seans_doldur();
        }

        void seans_silme()
        {
            try
            {
                baglan();
                DialogResult dia = new DialogResult();
                dia = MessageBox.Show("Bu Seans Saatiyle İlgili Olan Biletler İpta Edilecektir.\nEmin Misiniz ?", " ", MessageBoxButtons.YesNo);
                if (dia == DialogResult.Yes)
                {
                    if (comboBox4.SelectedIndex == -1)
                    {
                        MessageBox.Show("Lütfen Seans Saati Seçiniz!");
                    }
                    else
                    {
                        OleDbCommand komut = new OleDbCommand("delete from seans_saat where seans_saatleri = '" + comboBox4.SelectedItem + "' ", baglanti);
                        OleDbCommand komut2 = new OleDbCommand("delete from biletler where seans = '" + comboBox4.SelectedItem + "' ", baglanti);
                        OleDbCommand komut3 = new OleDbCommand("delete from seanslar where seans1 = '" + comboBox4.SelectedItem + "' ", baglanti);
                        komut.ExecuteNonQuery();
                        komut2.ExecuteNonQuery();
                        komut3.ExecuteNonQuery();
                        MessageBox.Show("Seans Saati Silindi!", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        baglanti.Close();
                        seans_doldur();
                        film_silme();
                    }
                }
            }
            catch (Exception ed)
            {
                MessageBox.Show("Hata !" + ed);
            }

        }


        //HHEEYY
        OleDbConnection baglanti = new OleDbConnection("Provider = Microsoft.JET.OLEDB.4.0; Data Source=kullanicisifre.mdb");
        //HHEEYY

        private void Seans_FormClosed(object sender, FormClosedEventArgs e)
        {
            Panel pan = new Panel();
            pan.Show();
            this.Hide();
        }
        void film_silme()
        {
            try
            {
                baglan();
                OleDbCommand komut1 = new OleDbCommand("select  filmler.film_adi,filmler.film_turu,seans1, seans1_sa from filmler ,seanslar where seanslar.film_adi=filmler.film_adi", baglanti);
                OleDbDataAdapter adaptor = new OleDbDataAdapter(komut1);
                DataTable tablo = new DataTable();
                adaptor.Fill(tablo);
                dataGridView1.DataSource = tablo;
                baglanti.Close();
            }
            catch (Exception es)
            {
                MessageBox.Show("Hata !" + es);
            }
            dataGridView1.ColumnHeadersVisible = true;
            dataGridView1.Columns[0].HeaderText = "Film Adı";
            dataGridView1.Columns[1].HeaderText = "Film Türü";
            dataGridView1.Columns[2].HeaderText = "Seans";
            dataGridView1.Columns[3].HeaderText = "Salon No";
        }

        private void Seans_Load(object sender, EventArgs e)
        {
            film_doldur();
            seans_doldur();
            salon_doldur();
            film_silme();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            seans_ekleme();
            film_silme();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            seans_saat_ekle();
            maskedTextBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Panel pan = new Panel();
            pan.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
        }

        void film_seansını_sil()
        {
            try
            {
                baglan();
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
                {
                    MessageBox.Show("Lütfen Tablodan Bir Film Seçiniz!");
                }
                else
                {
                    DialogResult dia = new DialogResult();
                    dia = MessageBox.Show("Silmek İstediğinize Emin Misiniz?", "Emin Misiniz?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dia == DialogResult.Yes)
                    {
                        OleDbCommand komut = new OleDbCommand("delete from seanslar where film_adi='" + textBox1.Text + "' and seans1_sa='" + textBox2.Text + "' and seans1='" + textBox3.Text + "' ", baglanti);
                        komut.ExecuteNonQuery();
                        film_silme();
                    }
                    else
                    {
                        MessageBox.Show("İşlem İptal Edildi");
                    }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Hata !" + hata.Message);
            }
            baglanti.Close();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            film_seansını_sil();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            seans_silme();
        }
    }
}