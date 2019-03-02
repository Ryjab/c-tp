using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Formlhm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MyAirport.Pim.Models.Factory.Model.GetBagage(this.comboBox1.Text);
            try
            {
                var bagage2 = MyAirport.Pim.Models.Factory.Model.GetBagage(this.comboBox1.Text);
                //this.tbAlpha.Text = bagage2[0].IdBagage;
                //this.tbAlpha.Enabled = false;
                this.textBox6.Text = bagage2[0].Prioritaire.ToString();
                this.textBox6.Enabled = false;
                this.textBox1.Text = bagage2[0].Compagnie;
                this.textBox1.Enabled = false;
                this.textBox5.Text = bagage2[0].Itineraire;
                this.textBox5.Enabled = false;
                this.textBox4.Text = bagage2[0].Jour_Exploitation.ToString();
                this.textBox4.Enabled = false;
                this.textBox3.Text = bagage2[0].Ligne.ToString();
                this.textBox3.Enabled = false;
                this.Continuation.Checked = bagage2[0].EnContinuation;
                this.Continuation.Enabled = false;
                this.Rush.Checked = bagage2[0].Rush;
                this.Rush.Enabled = false;
            }
            catch (ApplicationException appEx)
            {
                this.textBox6.Text = this.textBox1.Text = this.textBox5.Text = this.textBox4.Text = this.textBox3.Text = "";
                this.Continuation.Checked = this.Rush.Checked = false;
                this.textBox6.Enabled = this.textBox1.Enabled = this.textBox5.Enabled = this.textBox4.Enabled =
                    this.textBox3.Enabled = this.Continuation.Enabled = this.Rush.Enabled = true;
            }
            catch
            {
                MessageBox.Show("Une erreur s’est produite dans le traitement de votre demande.\nMerci de bien vouloir re tester ultérieurement ou de contacter votre administrateur.", "Erreur dans le traitement de votre demande", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void réinitialliserToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
