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
using System.Collections;

namespace Sinema_Otomasyonu_Şeysi
{
    public partial class Bilet : Form
    {
        public Bilet()
        {
            InitializeComponent();
        }
        ArrayList secilenkoltuk = new ArrayList();
        void baglan()
        {
            if (baglanti.State == ConnectionState.Open)
            {
                baglanti.Close();
            }
            baglanti.Open();
        }
        public void dolukoltuklar()
        {
            try
            {
                baglan();
                OleDbCommand kom = new OleDbCommand("select koltuk_no from biletler where film_adi = '" + comboBox1.SelectedItem + "' and seans_sa = '" + comboBox2.SelectedItem + "' and seans = '" + comboBox3.SelectedItem + "' ", baglanti);
                OleDbDataReader ko = kom.ExecuteReader();
                while (ko.Read())
                {
                    foreach (Control item in groupBox1.Controls)
                    {
                        if (item is Button)
                        {
                            if (ko["koltuk_no"].ToString() == item.Text)
                            {
                                item.BackColor = Color.Red;
                            }
                        }
                    }
                }
            }
            catch (Exception hata1)
            {
                MessageBox.Show("Hata !\n" + hata1);
            }
        }
        void yeniden_beyaz()
        {
            try
            {
                foreach (Control item in groupBox1.Controls)
                {
                    if (item is Button)
                    {
                        item.BackColor = Color.Silver;
                    }
                }
            }
            catch (Exception hata2)
            {
                MessageBox.Show("Hata !\n" + hata2);
            }
        }
        public void koltuk_olusturma()
        {
            int sayac = 0;
            try
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int k = 0; k < 6; k++)
                    {
                        baglan();
                        Button btn = new Button();
                        btn.Size = new Size(40, 40);
                        btn.BackColor = Color.Silver;
                        btn.ForeColor = Color.Black;
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.Location = new Point(k * 69 + 30, i * 49 + 30);
                        btn.Name = sayac.ToString();
                        btn.Text = (sayac + 1).ToString();
                        if (k == 3)
                        {
                            continue;
                        }
                        sayac++;
                        this.groupBox1.Controls.Add(btn);
                        baglanti.Close();
                        btn.Click += btn_Click;
                    }
                }
            }
            catch (Exception hata3)
            {
                MessageBox.Show("Hata !\n" + hata3);
            }
        }
        void combo_dolu_koltuklar()
        {
            try
            {
                comboBox4.Items.Clear();
                baglan();
                foreach (Control item in groupBox1.Controls)
                {
                    if (item is Button)
                    {
                        if (item.BackColor == Color.Red)
                        {
                            comboBox4.Items.Add(item.Text);
                        }
                    }
                }
            }
            catch (Exception hata4)
            {
                MessageBox.Show("Hata !\n" + hata4);
            }
        }
        public void film_doldurma()
        {
            try
            {
                baglan();
                OleDbCommand komu = new OleDbCommand("select film_adi from filmler", baglanti);
                OleDbDataReader ok = komu.ExecuteReader();
                while (ok.Read())
                {
                    comboBox1.Items.Add(ok["film_adi"]);
                }
                ok.Close();
            }
            catch (Exception hata5)
            {
                MessageBox.Show("Hata !\n" + hata5);
            }
        }
        void seans_doldur()
        {
            try
            {
                comboBox3.Items.Clear();
                baglan();
                OleDbCommand kom = new OleDbCommand("select seans1,film_adi from seanslar where film_adi='" + comboBox1.SelectedItem + "' and seans1_sa='" + comboBox2.SelectedItem + "' ", baglanti);
                OleDbDataReader oku = kom.ExecuteReader();
                while (oku.Read())
                {
                    comboBox3.Items.Add(oku["seans1"].ToString());
                }
                oku.Close();
                baglanti.Close();
            }
            catch (Exception hata)
            {
                MessageBox.Show("Hata !\n" + hata);
                baglanti.Close();
            }
        }
        void salon_getir()
        {
            try
            {
                comboBox2.Items.Clear();
                baglan();
                OleDbCommand kom = new OleDbCommand("select distinct seans1_sa from seanslar where film_adi='" + comboBox1.SelectedItem + "' ", baglanti);
                OleDbDataReader okuyucu = kom.ExecuteReader();
                while (okuyucu.Read())
                {
                    comboBox2.Items.Add(okuyucu["seans1_sa"].ToString());
                }
                okuyucu.Close();
                baglanti.Close();
            }
            catch (Exception es)
            {
                MessageBox.Show("Hata !\n" + es);
                baglanti.Close();
            }
        }

        void satis_yap()
        {
            try
            {
                baglan();
                for (int i = 0; i < secilenkoltuk.Count; i++)
                {
                    OleDbCommand kom = new OleDbCommand("insert into biletler(film_adi,koltuk_no,seans,seans_sa,ad,soyad,ucreti) values ('" + comboBox1.Text + "','" + Convert.ToInt32(secilenkoltuk[i]) + "','" + comboBox3.Text + "','" + comboBox2.Text + "','" + textBox2.Text + "','" + textBox3.Text + "',@bilet)", baglanti);
                    if (comboBox5.SelectedIndex == 0)
                        kom.Parameters.AddWithValue("@bilet", "8");
                    else if (comboBox5.SelectedIndex == 1)
                        kom.Parameters.AddWithValue("@bilet", "10");
                    kom.ExecuteNonQuery();
                }
                MessageBox.Show("Satıldı!");
                baglanti.Close();
            }
            catch (Exception es)
            {
                MessageBox.Show("Hata !\n" + es);
                baglanti.Close();
            }
        }

        void koltuk_iptal()
        {
            try
            {
                DialogResult dia = new DialogResult();
                dia = MessageBox.Show("Bilet İptal Edilecektir !", "Emin Misiniz?", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (dia == DialogResult.OK)
                {
                    baglan();
                    OleDbCommand kom = new OleDbCommand("delete from biletler where koltuk_no='" + comboBox4.SelectedItem + "' and film_adi='" + comboBox1.SelectedItem + "' and seans = '" + comboBox3.SelectedItem + "' and seans_sa='" + comboBox2.SelectedItem + "' ", baglanti);
                    kom.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Bilet İptal Edilmiştir!");
                }
                else
                {
                    MessageBox.Show("İşlem İptal Edilmiştir!");
                }
            }
            catch (Exception es)
            {
                MessageBox.Show("Hata !\n" + es);
                baglanti.Close();
            }
        }

        void temizle()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            comboBox5.SelectedIndex = -1;
        }

        OleDbConnection baglanti = new OleDbConnection("Provider = Microsoft.JET.OLEDB.4.0; Data Source=kullanicisifre.mdb");
        DataTable dt = new DataTable();
        OleDbDataAdapter adaptor = new OleDbDataAdapter();


        private void Bilet_Load(object sender, EventArgs e)
        {
            comboBox5.Items.Add("Öğrenci: 8 TL");
            comboBox5.Items.Add("Normal: 10 TL");
            film_doldurma();
            koltuk_olusturma();
        }

        private void Bilet_FormClosed(object sender, FormClosedEventArgs e)
        {
            Panel pan = new Panel();
            pan.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Panel pan = new Panel();
            pan.Show();
            this.Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            salon_getir();
            yeniden_beyaz();
            comboBox3.SelectedIndex = -1;
            comboBox3.Items.Clear();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            dolukoltuklar();
            combo_dolu_koltuklar();
        }

        void btn_Click(object sender, EventArgs e)
        {
            string denme = "";
            if (((Button)sender).BackColor == Color.Silver)
            {
                secilenkoltuk.Add(((Button)sender).Text);
                ((Button)sender).BackColor = Color.BlanchedAlmond;
            }
            else if (((Button)sender).BackColor == Color.BlanchedAlmond)
            {
                ((Button)sender).BackColor = Color.Silver;
                secilenkoltuk.Remove(((Button)sender).Text);
            }
            else
            {

            }
            foreach (var item in secilenkoltuk)
            {
                denme += item + ",";
            }
            if (denme.Length>=1)
            {
                denme = denme.Remove(denme.Length - 1, 1);
                textBox1.Text = denme;
            }
            else
            {
                textBox1.Text = "";
            }
        }


        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.SelectedIndex = -1;
            seans_doldur();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Lütfen Film Seçiniz!");
                comboBox1.Focus();
            }
            else if (comboBox2.Text == "")
            {
                MessageBox.Show("Lütfen Salon Seçiniz!");
                comboBox2.Focus();
            }
            else if (comboBox3.Text == "")
            {
                MessageBox.Show("Lütfen Seans Seçiniz!");
                comboBox3.Focus();
            }
            else if (textBox1.Text == "")
            {
                MessageBox.Show("Lütfen Yan Taraftan Koltuk Seçiniz!");
                textBox1.Focus();
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("Lütfen İsminizi Girin!");
                textBox2.Focus();
            }
            else if (textBox3.Text == "")
            {
                MessageBox.Show("Lütfen Soyisim Giriniz!");
                textBox3.Focus();
            }
            else if (comboBox5.Text == "")
            {
                MessageBox.Show("Lütfen Ödenecek Ücreti Seçiniz!");
                comboBox5.Focus();
            }
            else
            {
                satis_yap();
                yeniden_beyaz();
                dolukoltuklar();
                combo_dolu_koltuklar();
                temizle();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox4.SelectedIndex != -1)
            {
                koltuk_iptal();
                yeniden_beyaz();
                dolukoltuklar();
                combo_dolu_koltuklar();
            }
        }
    }
}