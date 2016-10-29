using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SatrancAt
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Skorumuz
        /// </summary>
        private int skor = 0;

        /// <summary>
        /// Bir satırdaki hücre sayısı.
        /// </summary>
        private int hucreSayisi = 0;

        /// <summary>
        /// Bir önceki seçim.
        /// </summary>
        private Label oncekiSecim = null;
        /// <summary>
        /// Bir önceki seçimin arkaplan rengi.
        /// </summary>
        private Color oncekiSecimArkaplanRengi;
        /// <summary>
        /// Bir önceki seçimin metin rengi.
        /// </summary>
        private Color oncekiSecimMetinRengi;

        /// <summary>
        /// Daha önce ziyaret edilmiş yerler.
        /// </summary>
        private List<Label> secilmisYerler;

        /// <summary>
        /// Anlık seçilebilecek yerler.
        /// </summary>
        private List<Label> secilebilirYerler;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Açılışta skoru yazdırıyoruz ve listelerizi oluşturuyoruz.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            lblSkor.Text = "Skor: " + skor.ToString();
            secilmisYerler = new List<Label>();
            secilebilirYerler = new List<Label>();
        }

        /// <summary>
        /// Oluşturma tuşuna tıklandığında grid boyutuna karar verip ilgili metodu çağırıyoruz.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            hucreSayisi = -1;

            #region Grid Boyut Seçimi
            if (radioButton1.Checked)
            {
                hucreSayisi = 5;
            }
            else if (radioButton2.Checked)
            {
                hucreSayisi = 6;
            }
            else if (radioButton3.Checked)
            {
                hucreSayisi = 7;
            }
            else if (radioButton4.Checked)
            {
                hucreSayisi = 8;
            }
            else if (radioButton5.Checked)
            {
                hucreSayisi = 9;
            }
            else
            {
                MessageBox.Show("Grid boyutunu seçmediniz!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion

            if (hucreSayisi != -1)
            {
                button1.Enabled = false;
                GridOlustur(hucreSayisi);
            }
        }

        /// <summary>
        /// Hücre sayısına göre gridi oluşturuyoruz.
        /// </summary>
        /// <param name="hucre">Oluşturulacak gridin hücre sayısı.</param>
        private void GridOlustur(int hucre)
        {
            for (int i = 0; i < hucre; i++)
            {
                for (int j = 0; j < hucre; j++)
                {
                    Label l = new Label();

                    #region Pozisyon İşlemleri
                    l.Width = 50;
                    l.Height = 50;

                    l.Left = 30 + (i * 50);
                    l.Top = 75 + (j * 50);

                    l.Text = (j + 1) + (i + 1).ToString();
                    l.TextAlign = ContentAlignment.MiddleCenter;
                    #endregion

                    #region Görsel İşlemler
                    l.BorderStyle = BorderStyle.FixedSingle;

                    if ((i + j) % 2 == 0)
                    {
                        l.BackColor = Color.Beige;
                    }
                    else
                    {
                        l.BackColor = Color.Wheat;
                    }
                    #endregion

                    l.Click += HamleleriGoster;

                    this.Controls.Add(l);

                    // İlk hamle için her yeri seçilebilir yapıyoruz.
                    secilebilirYerler.Add(l);
                }
            }
        }

        /// <summary>
        /// Bir pozisyon seçildiğinde, seçilebilir yerleri gösteriyoruz.
        /// </summary>
        /// <param name="sender">Seçilen pozisyon.</param>
        /// <param name="e">Olay argümanları.</param>
        private void HamleleriGoster(object sender, EventArgs e)
        {
            // Skoru arttırıyoruz.
            skor++;
            lblSkor.Text = "Skor: " + skor.ToString();

            // oyunu kazanma mesajı.
            if (skor >= hucreSayisi * hucreSayisi)
            {
                MessageBox.Show("Oyunu Kazandınız!", "Tebrikler!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // TODO: Seçilebilir mi kontrol et. Skor 1 üzerindeyse. (En az 1 seçim yaptıysa).

            if (oncekiSecim == ((Label)sender))
            {
                MessageBox.Show("Aynı elemanı tekrar seçemezsiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (skor >= 1 && secilebilirYerler.Contains(((Label)sender)))
                {
                    // Önceki seçimi yok ediyoruz.
                    Controls.Remove(oncekiSecim);
                    
                    // Seçilebilir yerleri sıfırlıyoruz.
                    secilebilirYerler = new List<Label>();

                    // Görünümü sıfırla.
                    RenkleriSifirla((Label)sender);

                    #region Renk İşlemleri
                    if (oncekiSecim != null)
                    {
                        oncekiSecim.BackColor = oncekiSecimArkaplanRengi;
                        oncekiSecim.ForeColor = oncekiSecimMetinRengi;
                    }

                    oncekiSecim = ((Label)sender);
                    oncekiSecimArkaplanRengi = ((Label)sender).BackColor;
                    oncekiSecimMetinRengi = ((Label)sender).ForeColor;

                    ((Label)sender).BackColor = Color.DarkBlue;
                    ((Label)sender).ForeColor = Color.White;
                    #endregion

                    lblSecim.Text = "Aktif Seçim: " + ((Label)sender).Text;

                    int secilenKod = Convert.ToInt16(((Label)sender).Text);

                    #region Seçilebilir Kodlar
                    // Seçilebilir kodları seçilen koda göre belirle.
                    int[] secilebilirKodlar = new int[8];
                    secilebilirKodlar[0] = secilenKod + 19;
                    secilebilirKodlar[1] = secilenKod - 19;
                    secilebilirKodlar[2] = secilenKod + 21;
                    secilebilirKodlar[3] = secilenKod - 21;
                    secilebilirKodlar[4] = secilenKod + 8;
                    secilebilirKodlar[5] = secilenKod - 8;
                    secilebilirKodlar[6] = secilenKod + 12;
                    secilebilirKodlar[7] = secilenKod - 12;
                    #endregion

                    secilmisYerler.Add((Label)sender);

                    // Her ihtimale karşı tıklama olayını kaldırıyoruz.
                    ((Label)sender).Click -= HamleleriGoster;

                    // Aynı pozisyon artık seçilemez.
                    secilebilirYerler.Remove((Label)sender);

                    lblSecilebilir.Text = "Seçilebilir Yerler: \n";

                    #region Hamle mantığı
                    foreach (var item in Controls)
                    {
                        if (item is Label)
                        {
                            foreach (var kod in secilebilirKodlar)
                            {
                                if (((Label)item).Text == kod.ToString())
                                {
                                    lblSecilebilir.Text += ((Label)item).Text + "\n";

                                    secilebilirYerler.Add((Label)item);

                                    #region 9x9
                                    // 9x9 da köşeler seçildiyse hatalı işlem yapma.
                                    if (hucreSayisi == 9)
                                    {
                                        // (+8 ve -12) | (-8 ve +12)
                                        if (((secilenKod % 10) == 1 && (
                                            ((Label)item).Text == secilebilirKodlar[4].ToString() ||
                                            ((Label)item).Text == secilebilirKodlar[7].ToString())) ||
                                            ((secilenKod % 10) == 9 && (
                                            ((Label)item).Text == secilebilirKodlar[5].ToString() ||
                                            ((Label)item).Text == secilebilirKodlar[6].ToString()))
                                            )
                                        {
                                            secilebilirYerler.Remove((Label)item);
                                        }
                                        else
                                        {
                                            if (!(secilmisYerler.Contains(((Label)item))))
                                            {
                                                ((Label)item).BackColor = Color.Red;
                                                ((Label)item).ForeColor = Color.White;
                                            }
                                        }
                                    }
                                    #endregion
                                    else
                                    {
                                        if (!(secilmisYerler.Contains(((Label)item))))
                                        {
                                            ((Label)item).BackColor = Color.Red;
                                            ((Label)item).ForeColor = Color.White;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (secilebilirYerler.Count < 1)
                    {
                        MessageBox.Show("Oyunu Kaybettiniz!", "Geçmiş Olsun!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    #endregion

                }
                else
                {
                    MessageBox.Show("Kırmızı ile belirtilen yerler dışında bir yer seçemezsiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Pozisyon haricindeki her hücrenin renklerini sıfırla.
        /// </summary>
        /// <param name="pozisyon">Anlık pozisyonumuz.</param>
        private void RenkleriSifirla(Label pozisyon)
        {
            foreach (var item in Controls)
            {
                if (item is Label && ((Label)item).Text.Length < 3 && ((Label)item) != pozisyon)
                {
                    int kod = Convert.ToInt16(((Label)item).Text);
                    int i = kod / 10;
                    int j = kod % 10;

                    if ((i + j) % 2 == 0)
                    {
                        ((Label)item).BackColor = Color.Beige;
                    }
                    else
                    {
                        ((Label)item).BackColor = Color.Wheat;
                    }

                    ((Label)item).ForeColor = Color.Black;
                }
            }
        }

        /// <summary>
        /// Seçilebilir pozisyonları renklendirir.
        /// </summary>
        private void SecilebilirYerleriRenklendir()
        {
        }
    }
}
