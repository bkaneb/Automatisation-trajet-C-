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
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class envoie : Form
    {
        public envoie()
        {
            InitializeComponent();
        }
        MySqlConnection cnx;
        bool Connecté = false;
        Socket clientSocket;
       
        byte[] data = new byte[256];
        byte[] Sdepart = new byte[256];
        byte[] Sdestination = new byte[256];
        int valeurid;

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Se connecter")
            {
                try
                {
                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//ajout du protocole TCP
                    clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000);//option du soclet, 
                    //definition du temps pour émettre
                    IPEndPoint serveurEP = new IPEndPoint(IPAddress.Parse("192.168.1.61"), 1500);//Mettre en parametre l'adresse IP 
                    //et le port pour la connexion
                    clientSocket.Connect(serveurEP);//connexion au socket
                    string pc = "!IHM";
                    data = Encoding.UTF8.GetBytes(pc);//conversion du string en Byte
                    clientSocket.Send(data, 0, data.Length, SocketFlags.None);//envoie du message
                    button1.Text = "Se déconnecter";
                    label2.Text = "de l'application";
                    Connecté = true;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
               
            }
            else
            {
                string pcoff = "!IHMOFF";
                data = Encoding.UTF8.GetBytes(pcoff);//conversion du string en Byte
                clientSocket.Send(data, 0, data.Length, SocketFlags.None);//envoie du message
                clientSocket.Close();//deconnexion socket
                button1.Text = "Se connecter";
                label2.Text = "à l'application";
                Connecté = false;
            }
        }

        private void envoie_Load(object sender, EventArgs e)
        {
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

        private void button2_Click(object sender, EventArgs e)
        {
            if(Connecté)
            {      
                if (comboBox1.Text != "")
                {
                    string recup = comboBox1.Text;//recuperation de l'id selectionner
                    valeurid = Int32.Parse(recup);
                    MySqlCommand lirecmd = new MySqlCommand("SELECT * FROM trajets", cnx);//requete sql
                    using (MySqlDataReader Liretab = lirecmd.ExecuteReader())//lecture dans bd
                    {
                        while (Liretab.Read())//tant qu'on lit dans la bd
                        {
                            //transfere valeur de bd au programme
                            string ID = Liretab["ID"].ToString();
                            string heuredep = Liretab["heuredep"].ToString();
                            string départ = Liretab["depart"].ToString();
                            string destination = Liretab["destination"].ToString();
                            string charg = Liretab["charg"].ToString();
                            string dechar = Liretab["dechar"].ToString();
                            string vit = Liretab["vitesse"].ToString();
                            string dir = Liretab["direction"].ToString();
                            //
                            int ID2 = Int32.Parse(ID);//conversion ID en int
                            if (ID2 == valeurid)//la lecture est en boucle donc en faisant ca on attend que la boucle                                
                            {//arrive au bon ID dans la lecture

                                int pointinterog = 1;
                                string envo = "?a" + Convert.ToString(pointinterog);
                                Sdepart = Encoding.UTF8.GetBytes(envo + "|" + ID + "|" + heuredep + "|" + départ + "|" + destination + "|" + charg + "|" + dechar + "|" + dir + vit + "|");
                                clientSocket.Send(Sdepart, 0, Sdepart.Length, SocketFlags.None);//envoie du message                                                 
                                MessageBox.Show("Envoyer.");
                            }
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Selectionner un trajet");
                }

            }
            else 
            {
                MessageBox.Show("Vous n'etes pas connecté");
            }

        }
        private void label2_Click(object sender, EventArgs e)
        {
        }
    }
}
