using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApplication1
{
    public partial class Supprimer : Form
    {
        public Supprimer()
        {
            InitializeComponent();
        }
        MySqlConnection cnx;


        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                string id = comboBox1.Text;
                int ID = Int32.Parse(id);
                MySqlCommand suppcmd = new MySqlCommand("DELETE FROM trajets WHERE ID=@valeurid", cnx);
                suppcmd.Parameters.AddWithValue("@valeurid", ID);
                suppcmd.ExecuteNonQuery();
                MessageBox.Show("Supprimer.");
            }
            else
            {
                MessageBox.Show("Selectionner un trajet.");
            }
            comboBox1.Items.Clear();//effacer les Items de la comboBox
            MySqlCommand combocmd = new MySqlCommand("SELECT ID FROM trajets", cnx);//selection dans la table de la collone ID
            using (MySqlDataReader Liretab = combocmd.ExecuteReader())
            {
                while (Liretab.Read())
                {
                    string ID = Liretab["ID"].ToString();
                    comboBox1.Items.Add(ID);//ajout de des ID dans l'items de la combobox
                }
            }
        }        

        private void Supprimer_Load(object sender, EventArgs e)
        {
            ACCUEIL.opensupprimer = 1;
            cnx = new MySqlConnection("SERVER=127.0.0.1;PORT=3306;DATABASE=projet;SslMode=none;UID=root;PWD=;");//connexion bd
            if (cnx.State == ConnectionState.Closed)//ouverture bd
            {
                cnx.Open();
            }
            comboBox1.Items.Clear();//effacer les Items de la comboBox
            MySqlCommand combocmd = new MySqlCommand("SELECT ID FROM trajets", cnx);//selection dans la table de la collone ID
            using (MySqlDataReader Liretab = combocmd.ExecuteReader())
            {
                while (Liretab.Read())
                {
                    string ID = Liretab["ID"].ToString();
                    comboBox1.Items.Add(ID);//ajout de des ID dans l'items de la combobox
                }
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Supprimer_FormClosing(object sender, FormClosingEventArgs e)
        {
            ACCUEIL.opensupprimer = 0;
        }
    }
}
