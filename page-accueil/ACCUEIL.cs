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
    public partial class ACCUEIL : Form
    {

        public ACCUEIL()
        {
            InitializeComponent();
        }

        MySqlConnection cnx;
        bool Connecté = false;
        Socket clientSocket;
        byte[] data = new byte[256];
        byte[] Sdepart = new byte[256];
        byte[] Sdestination = new byte[256];
        int auto = 0;
        int i = 0;
        private static ManualResetEvent mre = new ManualResetEvent(false);
        public static int openediter = 0;
        public static int openediter2 = 0;
        public static int opennouveau = 0;
        public static int opensupprimer = 0;
        public static int openposition = 0;
        public static int openenvoi = 0;
        private void ThreadTask()
        {
            //connexion bd
            cnx = new MySqlConnection("SERVER=127.0.0.1;PORT=3306;DATABASE=projet;SslMode=none;UID=root;PWD=;");
            if (cnx.State == ConnectionState.Closed)//ouverture bd
            {
                cnx.Open();
            }
            while (true)
            {
                mre.WaitOne();//met le thread en pause
                string heure = DateTime.Now.ToString("HH:mm:ss");//déclaration heure 
                MySqlCommand AUTOcmd = new MySqlCommand("SELECT * FROM trajets", cnx);//requete sql
                using (MySqlDataReader Liretab = AUTOcmd.ExecuteReader())//lecture dans bd
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

                        if (heure == heuredep)//si on trouve une heure identique à la réelle
                        {
                            //envoie de la ligne correspodant a la base de donnée
                            int pointinterog = 1;
                            string envo = "?a" + Convert.ToString(pointinterog);
                            Sdepart = Encoding.UTF8.GetBytes(envo + "|" + ID + "|" + heuredep + "|" + départ + "|" + destination + "|" + charg + "|" + dechar + "|" + dir + vit + "|");
                            clientSocket.Send(Sdepart, 0, Sdepart.Length, SocketFlags.None);//envoie du message                                                                                          //
                        }
                        
                        Thread.Sleep(100);//la boucle s'execute tout les 10ms
                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)//se connecter
        {
            if (button1.Text == "Se connecter")
            {
                try//eviter contient le code protégé susceptible de provoquer l'exception.
                {
                    //connexion bd
                    cnx = new MySqlConnection("SERVER=127.0.0.1;PORT=3306;DATABASE=projet;SslMode=none;UID=root;PWD=;");
                    //ouverture bd
                    if (cnx.State == ConnectionState.Closed)
                    {
                        cnx.Open();
                    }
                    //afficher des la connexion les infos de la table
                    listView1.Items.Clear();//efface les items de la listView
                    MySqlCommand viewcmd = new MySqlCommand("SELECT * FROM trajets", cnx);//selection de toute la base test
                    using (MySqlDataReader Liretab = viewcmd.ExecuteReader())//lectcure dans table
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
                            if (vit == "1")
                            {
                                vit = "lente";
                            }
                            if (vit == "2")
                            {
                                vit = "moyenne";
                            }
                            if (vit == "3")
                            {
                                vit = "rapide";
                            }
                            if (dir == "2")
                            {
                                dir = "gauche";
                            }
                            if (dir == "1")
                            {
                                dir = "droite";
                            }
                            listView1.Items.Add(new ListViewItem(new[] { ID, heuredep, départ, destination, charg, dechar, vit, dir }));/*ajout
                            des info dans la listView*/
                        }
                    }

                    button1.Text = "Se déconnecter";//afficher se deconnecter car deja conneté
                    label1.Text = "de la base de données";
                    Connecté = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);//si exception afficher le une fenetre avec le message
                }


            }
            else//Se deconnecter
            {
                if (auto == 0)
                {
                    cnx.Close();
                    button1.Text = "Se connecter";
                    label1.Text = "à la base de données";
                    Connecté = false;
                }
                else
                    MessageBox.Show("Veuillez enlever le mode automatique.");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (Connecté)//ouvrir nouveau si connecté
            {
                if(opennouveau == 0)
                {
                    Nouveau open = new Nouveau();
                    open.Show();
                }
                else
                {
                    MessageBox.Show("une fenetre est déjà ouverte");
                }
            }
            else
            {
                MessageBox.Show("Vous n'êtes pas connecté.");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (Connecté)//ouvrir editer si connecté
            {
                if (openediter == 0)
                {
                    editer open = new editer();
                    open.Show();
                }
                else
                {
                    MessageBox.Show("une fenetre est déjà ouverte");
                }          
            }

            else
            {
                MessageBox.Show("Vous n'êtes pas connecté.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Connecté)//Si on est connecté on peut faire le code suivant
            {
                if (auto == 0)
                {
                    listView1.Items.Clear();
                    //selection de toute la base trajets
                    MySqlCommand viewcmd = new MySqlCommand("SELECT * FROM trajets", cnx);
                    using (MySqlDataReader Liretab = viewcmd.ExecuteReader())//lectcure dans la table
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
                            if(vit == "1")
                            {
                                vit = "lente";
                            }
                            if (vit == "2")
                            {
                                vit = "moyenne";
                            }
                            if (vit == "3")
                            {
                                vit = "rapide";
                            }
                            if (dir == "2")
                            {
                                dir = "gauche";
                            }
                            if (dir == "1")
                            {
                                dir = "droite";
                            }
                            listView1.Items.Add(new ListViewItem(new[] { ID, heuredep, départ, destination, charg, dechar, vit, dir }));/*ajout
                        des info dans la listView*/
                        }
                    }
                }
                else
                    MessageBox.Show("Veuillez enlever le mode automatique.");
            }
            else//Sinon ce message sera affiché
            {
                MessageBox.Show("Vous n'êtes pas connecté.");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cnx = new MySqlConnection("SERVER=127.0.0.1;PORT=3306;DATABASE=projet;SslMode=none;UID=root;PWD=;");//connexion bd
            if (cnx.State == ConnectionState.Closed)//ouverture bd
            {
                cnx.Open();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (Connecté)//ouvrir suprimmer si connecté
            {
                if (opensupprimer == 0)
                {
                    Supprimer open = new Supprimer();
                    open.Show();
                }
                else
                {
                    MessageBox.Show("une fenetre est déjà ouverte");
                }
            }

            else
            {
                MessageBox.Show("Vous n'êtes pas connecté.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Connecté)//ouvrir envoi si connecté
            {
                if(auto == 0)
                {
                    if (openenvoi == 0)
                    {
                        envoi open = new envoi();
                        open.Show();
                    }
                    else
                    {
                        MessageBox.Show("une fenetre est déjà ouverte");
                    }
                }
                else
                MessageBox.Show("Veuillez enlever le mode automatique.");
            }

            else
            {
                MessageBox.Show("Vous n'êtes pas connecté.");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Connecté)//ouvrir position si connecté
            {
                if(auto == 1 || openenvoi == 1)
                {
                    if (openposition == 0)
                    {
                        Position open = new Position();
                        open.Show();
                    }
                    else
                    {
                        MessageBox.Show("une fenetre est déjà ouverte");
                    }
                }
                else
                {
                    MessageBox.Show("envoyer un trajet pour pouvoir voir la position");
                }
            }
            else
            {
                MessageBox.Show("Vous n'êtes pas connecté.");
            }
        }

        private void button9_Click(object sender, EventArgs e)//mode automatique
        {
            if (Connecté)
            {
                if(openenvoi == 0)
                {
                    if (i == 2)//apres la desactivation du mode automatique seulement se if fonctionnera lors de la mise en marche
                    {
                        auto = 1;
                        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//ajout du protocole TCP
                        clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000);//option du soclet, definissions du temps pour émettre
                        IPEndPoint serveurEP = new IPEndPoint(IPAddress.Parse("192.168.1.62"), 1500);//Mettre en parametre l'adresse IP et le port pour la connexion
                        clientSocket.Connect(serveurEP);//connexion au socket
                        string pc = "?IHM";
                        data = Encoding.UTF8.GetBytes(pc);//conversion du string en Byte
                        clientSocket.Send(data, 0, data.Length, SocketFlags.None);//envoie du message
                        button9.Text = "MODE AUTOMATIQUE : ON";
                        mre.Set();//On relance le thread
                        i--;//i = 2 donc il devient i = 1
                    }
                    else
                    {
                        if (button9.Text == "MODE AUTOMATIQUE : ON")//si allumé on éteind
                        {
                            button9.Text = "MODE AUTOMATIQUE : OFF";
                            if (i == 1)
                            {
                                //on remet le droit au differents boutons qui ne peuvent pas fonctionner avec en marche
                                auto = 0;
                                mre.Reset();//On met pause au thread
                                string pcoff = "?IHMOFF";
                                data = Encoding.UTF8.GetBytes(pcoff);//conversion du string en Byte
                                clientSocket.Send(data, 0, data.Length, SocketFlags.None);//envoi du message
                                clientSocket.Close();//deconnexion socket
                                i++;//i=1 il devient i = 2
                            }

                        }
                    }
                    if (i == 0)
                    {
                        //un attribut i declarer pour ne pas faire le start avec plusieurs clic mais qu'avec le premier
                        button9.Text = "MODE AUTOMATIQUE : ON";
                        try//connexion wifi
                        {
                            auto = 1;
                            //ajout du protocole TCP
                            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            //option du socket, definissions du temps pour émettre
                            clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000);
                            //Mettre en parametre l'adresse IP et le port pour la connexion
                            IPEndPoint serveurEP = new IPEndPoint(IPAddress.Parse("192.168.1.61"), 1500);
                            clientSocket.Connect(serveurEP);//connexion au socket
                            string pc = "?IHM";
                            data = Encoding.UTF8.GetBytes(pc);//conversion du string en Byte
                            clientSocket.Send(data, 0, data.Length, SocketFlags.None);//envoie du message
                            Thread trd = new Thread(new ThreadStart(this.ThreadTask));//on declare un objet de thread
                            mre.Set();//On enleve le thread de pause 
                            trd.Start();//on le lance (start peut être fais qu'une fois)
                            i++;//i = 0 donc il devient i = 1

                        }
                        catch (Exception)
                        {
                            MessageBox.Show("La connexion n'a pas pu se faire");
                        }
                    }
                     
                }
                else
                {
                    MessageBox.Show("l'envoi manuel est déjà activé");
                } 
            }
            else
            {
                MessageBox.Show("Vous n'êtes pas connecté");
            }
        }

        private void testMove(object sender, MouseEventArgs e)
        {
           
        }

        private void test2(object sender, MouseEventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 open = new Form2();
            open.Show();
        }
    }
}
