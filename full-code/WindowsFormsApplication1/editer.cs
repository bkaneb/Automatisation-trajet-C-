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

    public partial class editer : Form
    {
        public editer()
        {
            InitializeComponent();
        }
        MySqlConnection cnx;
        public
            int envoie;
        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e){   
        }

        private void button2_Click(object sender, EventArgs e){
        }

        private void editer_Load(object sender, EventArgs e)
        {
            ACCUEIL.openediter = 1;
            cnx = new MySqlConnection("SERVER=127.0.0.1;PORT=3306;DATABASE=projet;SslMode=none;UID=root;PWD=;");//connexion bd
            if (cnx.State == ConnectionState.Closed)//ouverture bd
            {
                cnx.Open();
            }
            comboBox1.Items.Clear();//effacer les Items de la comboBox
            //selection dans la table de la collone ID
            MySqlCommand combocmd = new MySqlCommand("SELECT ID FROM trajets", cnx);
            using (MySqlDataReader Liretab = combocmd.ExecuteReader())
            {
                while (Liretab.Read())
                {
                    string ID = Liretab["ID"].ToString();
                    comboBox1.Items.Add(ID);//ajout de des ID dans l'items de la combobox
                }
            }
        }
        public static int Valid;//passage de l'id dans une autre form grâce a cette methode
        private void button1_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text != "")
            {
                string recup = comboBox1.Text;//recupération de l'id selectionner
                 envoie = Int32.Parse(recup);//conversion de l'id recupéré de string en int
                Valid = envoie;//donner la valeur de l'id à l'attribut qui enverra l'id
                Editer2 open = new Editer2();//ouvrir Editer2                              
                open.Show();                  
            }
            else
            {
                MessageBox.Show("Selectionner un trajet");
            }
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void editer_FormClosing(object sender, FormClosingEventArgs e)
        {
            ACCUEIL.openediter = 0;
        }
    }
    }

