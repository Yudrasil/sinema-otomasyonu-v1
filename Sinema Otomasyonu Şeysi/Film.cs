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
    public partial class Film : Form
    {
        public Film()
        {
            InitializeComponent();
        }

        string sonuc = "";

        void baglan()
        {
            if (baglanti.State == ConnectionState.Open)
            {
                baglanti.Close();
            }
            baglanti.Open();
        }
        void film_turu_doldur()
        {
            try
            {
                baglan();
                OleDbCommand komut = new OleDbCommand("select film_turu from film_turleri", baglanti);
                OleDbDataReader oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    checkedListBox1.Items.Add(oku[0].ToString());
                }
            }
            catch (Exception a)
            {
                MessageBox.Show("Hata !" + a);
            }
            baglanti.Close();
        }
        void film_ekle()
        {
            try
            {
                baglan();
                OleDbCommand kom = new OleDbCommand("insert into filmler(film_adi,film_turu,yonetmen_adi,yapim_yili) values('" + textBox1.Text + "',@turleri,'" + textBox2.Text + "','" + textBox3.Text + "' )", baglanti);
                foreach (object a in checkedListBox1.CheckedItems)
                {
                    sonuc += a.ToString() + "/";
                }
                sonuc = sonuc.Remove(sonuc.Length - 1, 1);
                kom.Parameters.AddWithValue("@turleri", sonuc);
                kom.ExecuteNonQuery();
                baglanti.Close();
            }
            catch (Exception a)
            {
                MessageBox.Show("Hata !" + a);
            }
            MessageBox.Show("Film Eklendi!");
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
        }
        void filmleri_doldur()
        {
            try
            {
                comboBox1.Items.Clear();
                baglan();
                OleDbCommand kom = new OleDbCommand("select * from filmler", baglanti);
                OleDbDataReader oku = kom.ExecuteReader();
                while (oku.Read())
                {
                    comboBox1.Items.Add(oku["film_adi"].ToString());
                }
            }
            catch (Exception a)
            {
                MessageBox.Show("Hata !" + a);
            }
            baglanti.Close();
        }
        void combo_secilen_doldur()
        {
            try
            {
                baglan();
                OleDbCommand kom = new OleDbCommand("select * from filmler where film_adi='" + comboBox1.SelectedItem + "' ", baglanti);
                OleDbDataReader ok = kom.ExecuteReader();
                while (ok.Read())
                {
                    textBox4.Text = ok[1].ToString();
                    textBox5.Text = ok[2].ToString();
                    textBox6.Text = ok[3].ToString();
                    textBox7.Text = ok[4].ToString();
                }
            }
            catch (Exception a)
            {
                MessageBox.Show("Hata !" + a);
            }
            baglanti.Close();
        }
        //HHEEYY
        OleDbConnection baglanti = new OleDbConnection("Provider = Microsoft.JET.OLEDB.4.0; Data Source=kullanicisifre.mdb");
        //HHEEYY

        private void Film_Load(object sender, EventArgs e)
        {
            film_turu_doldur();
            filmleri_doldur();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            checkedListBox1.Visible = true;
            button2.Visible = true;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            button3.Enabled = false;
        }

        private void Film_FormClosed(object sender, FormClosedEventArgs e)
        {
            Panel pan = new Panel();
            pan.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkedListBox1.Visible = false;
            button2.Visible = false;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Lütfen Film Adını Giriniz !");
                textBox1.Focus();
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("Yönetmen Adını Giriniz !");
                textBox2.Focus();
            }
            else if (checkedListBox1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen Film Türü Seçiniz !");
                button2.Focus();
            }
            else if (textBox3.Text == "")
            {
                MessageBox.Show("Lütfen Yapım Yılını Giriniz !");
                textBox3.Focus();
            }
            else
            {
                film_ekle();
                filmleri_doldur();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Panel pan = new Panel();
            pan.Show();
            this.Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            combo_secilen_doldur();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                baglan();
                OleDbCommand kom = new OleDbCommand("delete from filmler where film_adi='" + textBox4.Text + "' ", baglanti);
                OleDbCommand kom2 = new OleDbCommand("delete from seanslar where film_adi='" + textBox4.Text + "' ", baglanti);
                OleDbCommand kom3 = new OleDbCommand("delete from biletler where film_adi='" + textBox4.Text + "' ", baglanti);
                kom.ExecuteNonQuery();
                kom2.ExecuteNonQuery();
                kom3.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Film Silindi!");
                filmleri_doldur();
            }
            catch (Exception a)
            {
                MessageBox.Show("Hata !" + a);
            }
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
        }
    }
}
