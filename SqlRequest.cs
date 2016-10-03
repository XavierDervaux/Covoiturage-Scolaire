using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Windows;

using covoiturage.main.user;
using covoiturage.main.singleton;

namespace covoiturage.main
{
    static class SqlRequest
    {
        private static SqlConnection Connexion { get; set; }

        private static Boolean Statut = false;

        public static Boolean _Statut 
        {
            get{ return Statut; }
        }

        //fonctionnel
        public static void openSqlRequest()
        {
            string Path = Environment.CurrentDirectory;
            string[] appPath = Path.Split(new string[] { "bin" }, StringSplitOptions.None);
            AppDomain.CurrentDomain.SetData("DataDirectory", appPath[0]);
            String informationConnexion = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|Database.mdf;Integrated Security=True";
            Connexion = new SqlConnection(informationConnexion);
            Connexion.Open();

            SqlRequest.Statut = true;        
        }

        public static void closeSqlRequest()
        {
            SqlRequest.Statut = false;
            Connexion.Close();
        }


        public static Utilisateur connexionUtilisateur(String identifiant, String mdp, int statut)
        {
            Utilisateur retour = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM users WHERE identifiant=@identifiant AND mdp=@mdp", Connexion))
                {
                    cmd.Parameters.AddWithValue("@identifiant", identifiant);
                    cmd.Parameters.AddWithValue("@mdp", GetSHA512(mdp));
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (reader["admin"].ToString() == "True")
                        {
                            retour = new Administrateur(int.Parse(reader["id_user"].ToString()), reader["identifiant"].ToString(), reader["nom"].ToString(), reader["prenom"].ToString());
                        }
                        else if (statut == 0)
                        {
                            retour = new Conducteur(int.Parse(reader["id_user"].ToString()), reader["identifiant"].ToString(), reader["nom"].ToString(), reader["prenom"].ToString());
                        }
                        else
                        {
                            retour = new Passager(int.Parse(reader["id_user"].ToString()), reader["identifiant"].ToString(), reader["nom"].ToString(), reader["prenom"].ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show("La combinaison Identifiant/Mot de passe est incorrecte.");
                    }
                    reader.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Erreur à la connection");
            }
            return retour;
        }


        //fonctionnel
        public static void AjouterUtilisateur(String nom, String prenom, string numNational, String identifiant, String mdp, Boolean admin, int idTmp)
        {
            try
            {
                ListRequest list = ListRequest.Instance();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO users VALUES(" + "@nom, @prenom, @numNational,@identifiant, @mdp, @admin)", Connexion))
                {
                    cmd.Parameters.AddWithValue("@nom", nom);
                    cmd.Parameters.AddWithValue("@prenom", prenom);
                    cmd.Parameters.AddWithValue("@numNational", numNational);
                    cmd.Parameters.AddWithValue("@identifiant", identifiant);
                    cmd.Parameters.AddWithValue("@mdp", GetSHA512(mdp));
                    cmd.Parameters.AddWithValue("@admin", admin);
                    cmd.Parameters.AddWithValue("@idTmp", idTmp);
                   
                    list.Add(cmd);
                }
            }
            catch (SqlException)
            {
                //Log exception
                //Display Error message
            }
        }

        //Fonctionnel
        public static void supprimerUtilisateur(Utilisateur util)
        {
            try
            {
                ListRequest list = ListRequest.Instance();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM users " + "WHERE identifiant=@Id", Connexion))
                {
                    cmd.Parameters.AddWithValue("@id", util._Identifiant);
                    list.Add(cmd);
                }
            }
            catch (SqlException)
            {
                //Log exception
                //Display Error message
            }
        }

        //Théorique
        public static void chargerUtilisateur()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM users", Connexion))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        ListUtilisateur utilisateur = ListUtilisateur.Instance();
                        while (reader.Read())
                        {
                            utilisateur.Add(new Conducteur(int.Parse(reader["id_user"].ToString()), reader["identifiant"].ToString(), reader["nom"].ToString(), reader["prenom"].ToString()));
                        }
                    }

                    reader.Close();
                }
            }
            catch (Exception)
            {
                //Log exception
                //Display Error message
            }
        }

        //Théorique
        public static void modifierMdp(Utilisateur util, String newMdp)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE users SET mdp=@mdp WHERE identifiant=@id", Connexion))
                {
                    cmd.Parameters.AddWithValue("@mdp", GetSHA512(newMdp));
                    cmd.Parameters.AddWithValue("@id", util._Identifiant);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                //Log exception
                //Display Error message
            }
        }

        public static void ajouterTrajet(Trajet trajet, Utilisateur util)
        {
            try
            {
                ListRequest list = ListRequest.Instance();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO trajet VALUES(" +
                    "@departVille, @departAdress, @departNum,@departCP," +
                    "@arriveeVille, @arriveeAdress, @arriveeNum,@arriveeCP," +
                    "@nbrKm,@nbrPlaceVoit, @nbrLitreVoit,@carburant, @heureDepart, @heureArrivee)"
                    , Connexion))
                {
                    cmd.Parameters.AddWithValue("@departVille", trajet._Depart._Ville);
                    cmd.Parameters.AddWithValue("@departAdress", trajet._Depart._Adress);
                    cmd.Parameters.AddWithValue("@departNum", trajet._Depart._Numero);
                    cmd.Parameters.AddWithValue("@departCP", trajet._Depart._Cp);
                    cmd.Parameters.AddWithValue("@arriveeVille", trajet._Arrivee._Ville);
                    cmd.Parameters.AddWithValue("@arriveeAdress", trajet._Arrivee._Adress);
                    cmd.Parameters.AddWithValue("@arriveeNum", trajet._Arrivee._Numero);
                    cmd.Parameters.AddWithValue("@arriveeCP", trajet._Arrivee._Cp);

                    cmd.Parameters.AddWithValue("@nbrKm", trajet._NbrKm);
                    cmd.Parameters.AddWithValue("@nbrPlaceVoit", trajet._NbrPlaceVoiture);
                    cmd.Parameters.AddWithValue("@nbrLitreVoit", trajet._NbrLitreVoiture);
                    cmd.Parameters.AddWithValue("@carburant", trajet._Carburant._Id);
                    cmd.Parameters.AddWithValue("@heureDepart", trajet._HDepart);
                    cmd.Parameters.AddWithValue("@heureArrivee", trajet._HArrivee);
                    cmd.Parameters.AddWithValue("@idTmp", trajet._Id);
                    
                   // cmd.ExecuteNonQuery();
                    list.Add(cmd);
                }
                using (SqlCommand cmdId = new SqlCommand("SELECT id_trajet FROM trajet WHERE"+
                    " departVille=@departVille AND departAdress=@departAdress AND departNumero=@departNum AND departCP=@departCP AND " +
                     "arriveeVille=@arriveeVille AND arriveeAdress=@arriveeAdress AND arriveeNumero=@arriveeNum AND arriveeCP=@arriveeCP AND " +
                     "nbrKm=@nbrKm AND nbrPlaceVoit=@nbrPlaceVoit AND nbrLitreVoit=@nbrLitreVoit AND typeCarburant=@carburant AND "+
                     "heureDepart=@heureDepart AND heureArrivee=@heureArrivee", Connexion))
                {
                    cmdId.Parameters.AddWithValue("@departVille", trajet._Depart._Ville);
                    cmdId.Parameters.AddWithValue("@departAdress", trajet._Depart._Adress);
                    cmdId.Parameters.AddWithValue("@departNum", trajet._Depart._Numero);
                    cmdId.Parameters.AddWithValue("@departCP", trajet._Depart._Cp);
                    cmdId.Parameters.AddWithValue("@arriveeVille", trajet._Arrivee._Ville);
                    cmdId.Parameters.AddWithValue("@arriveeAdress", trajet._Arrivee._Adress);
                    cmdId.Parameters.AddWithValue("@arriveeNum", trajet._Arrivee._Numero);
                    cmdId.Parameters.AddWithValue("@arriveeCP", trajet._Arrivee._Cp);

                    cmdId.Parameters.AddWithValue("@nbrKm", trajet._NbrKm);
                    cmdId.Parameters.AddWithValue("@nbrPlaceVoit", trajet._NbrPlaceVoiture);
                    cmdId.Parameters.AddWithValue("@nbrLitreVoit", trajet._NbrLitreVoiture);
                    cmdId.Parameters.AddWithValue("@carburant", trajet._Carburant._Id);
                    cmdId.Parameters.AddWithValue("@heureDepart", trajet._HDepart);
                    cmdId.Parameters.AddWithValue("@heureArrivee", trajet._HArrivee);

                    list.Add(cmdId);
                }
                using (SqlCommand cmdVoyager = new SqlCommand("INSERT INTO voyager VALUES(" +
                   "@statut,@id_user,@id_trajt)", Connexion))
                {
                    cmdVoyager.Parameters.AddWithValue("@id_user", util._Id);
                    cmdVoyager.Parameters.AddWithValue("@statut", 0);

                    list.Add(cmdVoyager);
                }
            }
            catch (SqlException)
            {
                //Log exception
                //Display Error message
            }
        }

        public static void updateTrajet(Trajet trajet)
        {
            try
            {
                ListRequest list = ListRequest.Instance();
                using (SqlCommand cmd = new SqlCommand("UPDATE trajet SET" +
                   " departVille=@departVille,"+
                   " departAdress=@departAdress,"+
                   " departNumero=@departNum,"+
                   " departCP=@departCP," +
                   " arriveeVille=@arriveeVille,"+
                   " arriveeAdress=@arriveeAdress," +
                   " arriveeNumero=@arriveeNum,"+
                   " arriveeCP=@arriveeCP," +
                   " nbrKm=@nbrKm," + 
                   " nbrPlaceVoit=@nbrPlaceVoit,"+
                   " nbrLitreVoit=@nbrLitreVoit," + 
                   " typeCarburant=@carburant, "+
                   " heureDepart=@heureDepart," +
                   " heureArrivee=@heureArrivee"+
                   " WHERE id_trajet=@id",
                Connexion))
                {
                    cmd.Parameters.AddWithValue("@departVille", trajet._Depart._Ville);
                    cmd.Parameters.AddWithValue("@departAdress", trajet._Depart._Adress);
                    cmd.Parameters.AddWithValue("@departNum", trajet._Depart._Numero);
                    cmd.Parameters.AddWithValue("@departCP", trajet._Depart._Cp);
                    cmd.Parameters.AddWithValue("@arriveeVille", trajet._Arrivee._Ville);
                    cmd.Parameters.AddWithValue("@arriveeAdress", trajet._Arrivee._Adress);
                    cmd.Parameters.AddWithValue("@arriveeNum", trajet._Arrivee._Numero);
                    cmd.Parameters.AddWithValue("@arriveeCP", trajet._Arrivee._Cp);

                    cmd.Parameters.AddWithValue("@nbrKm", trajet._NbrKm);
                    cmd.Parameters.AddWithValue("@nbrPlaceVoit", trajet._NbrPlaceVoiture);
                    cmd.Parameters.AddWithValue("@nbrLitreVoit", trajet._NbrLitreVoiture);
                    cmd.Parameters.AddWithValue("@carburant", trajet._Carburant._Id);
                    cmd.Parameters.AddWithValue("@heureDepart", trajet._HDepart);
                    cmd.Parameters.AddWithValue("@heureArrivee", trajet._HArrivee);
                    cmd.Parameters.AddWithValue("@id", trajet._Id);

                    list.Add(cmd);
                }
            }
            catch (SqlException)
            {
                //Log exception
                //Display Error message
            }
        }

        public static void supprimerTrajet(Trajet trajet)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM trajet " + "WHERE id_trajet=@Id", Connexion))
                {
                    cmd.Parameters.AddWithValue("@Id", trajet._Id);
                    ListRequest list = ListRequest.Instance();
                    list.Add(cmd);
                    notifierPassagers(trajet, "Suppresion", "Le trajet auquel vous participiez à été supprimé.");
                }
            }
            catch (SqlException)
            {
                //Log exception
                //Display Error message
            }
        }

        public static void chargerTrajet()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM trajet", Connexion))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        ListTrajet trajet = ListTrajet.Instance();
                        ListUtilisateur util = ListUtilisateur.Instance();

                        while (reader.Read())
                        {
                            Adresse depart = new Adresse(reader["departAdress"].ToString(), reader["departNumero"].ToString(), reader["departVille"].ToString(), reader["departCp"].ToString());
                            Adresse arrivee = new Adresse(reader["arriveeAdress"].ToString(), reader["arriveeNumero"].ToString(), reader["arriveeVille"].ToString(), reader["arriveeCp"].ToString());

                            trajet.Add(new Trajet(int.Parse(reader["id_trajet"].ToString()), depart, arrivee,
                                int.Parse(reader["nbrKm"].ToString()), 
                                int.Parse(reader["nbrPlaceVoit"].ToString()), 
                                float.Parse(reader["nbrLitreVoit"].ToString()),
                                ListCarburant.trouverCarburant(int.Parse(reader["typeCarburant"].ToString())),
                                int.Parse(reader["heureDepart"].ToString()),
                                int.Parse(reader["heureArrivee"].ToString())
                            ));
                        }
                    }

                    reader.Close();
                }
            }
            catch (Exception)
            {
                //Log exception
                //Display Error message
            }
        }

        //fonctionnel
        private static string GetSHA512(string text)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(text);

            SHA512Managed hashString = new SHA512Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }

            return hex;
        }

        public static Boolean mdpCorrect(String identifiant, String mdp)
        {
            Boolean correct = false;
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM users WHERE identifiant=@identifiant AND mdp=@mdp", Connexion))
                {
                    cmd.Parameters.AddWithValue("@identifiant", identifiant);
                    cmd.Parameters.AddWithValue("@mdp", GetSHA512(mdp));
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        correct = true;
                    }

                    reader.Close();
                }
            }
            catch (Exception)
            {
                //Log exception
                //Display Error message
            }

            return correct;
        }

        public static Utilisateur regNatCorrect(String identifiant, String regNat)
        {
            Utilisateur retour = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM users WHERE identifiant=@identifiant AND numNational=@numNat", Connexion))
                {
                    cmd.Parameters.AddWithValue("@identifiant", identifiant);
                    cmd.Parameters.AddWithValue("@numNat", regNat);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        retour = new Passager(int.Parse(reader["id_user"].ToString()), reader["identifiant"].ToString(), reader["nom"].ToString(), reader["prenom"].ToString());
                    }

                    reader.Close();
                }
            }
            catch (Exception)
            {
                //Log exception
                //Display Error message
            }

            return retour;
        }

        //Théorique
        public static void chargerVoyager()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM voyager", Connexion))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        ListVoyager voyager = ListVoyager.Instance();
                        while (reader.Read())
                        {
                            voyager.Add(new Voyager(int.Parse(reader["statut"].ToString()), int.Parse(reader["id_user"].ToString()), int.Parse(reader["id_trajet"].ToString())));
                        }
                    }

                    reader.Close();
                }
            }
            catch (Exception)
            {
                //Log exception
                //Display Error message
            }
        }

        public static void ajouterVoyage(Voyager voyage)
        {
            try
            {
                ListRequest list = ListRequest.Instance();
                using (SqlCommand cmdVoyager = new SqlCommand("INSERT INTO voyager VALUES(" +
                   "@statut,@id_user,@id_trajt)", Connexion))
                {
                    cmdVoyager.Parameters.AddWithValue("@statut", 1);
                    cmdVoyager.Parameters.AddWithValue("@id_user", voyage._UserId);
                    cmdVoyager.Parameters.AddWithValue("@id_trajt", voyage._TrajetId);
                    

                    list.Add(cmdVoyager);
                }
            }
            catch (SqlException)
            {
                //Log exception
                //Display Error message
            }
        }
        
        public static void supprimerVoyager(Voyager voyage)
        {
            try
            {
                ListRequest list = ListRequest.Instance();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM voyager WHERE statut=@statut AND id_user=@idUser AND id_trajet=@idTrajet", Connexion))
                {
                    cmd.Parameters.AddWithValue("@statut", voyage._Statut);
                    cmd.Parameters.AddWithValue("@idUser", voyage._UserId);
                    cmd.Parameters.AddWithValue("@idTrajet", voyage._TrajetId);
                             
                    list.Add(cmd);
                }
            }
            catch (SqlException)
            {
                //Log exception
                //Display Error message
            }
        }

        public static void retirerStackRequest(Utilisateur util)
        {
            ListRequest request = ListRequest.Instance();
            SqlCommand result = request.Listl.Find(
               delegate(SqlCommand bk)
               {
                   SqlParameterCollection paramCollection = bk.Parameters;
                     //string parameterList = "";
                     //parameterList += String.Format("{0}", paramCollection[3].Value);
                     //return parameterList == util._Identifiant;
                     int parameterList = int.Parse(String.Format("{0}", paramCollection[6].Value));
                     return parameterList == util._Id;
               }
            );
            request.Remove(result);
        }

        public static void retirerStackRequest(Trajet trajet)
        {
            int position;
            ListRequest request = ListRequest.Instance();
            SqlCommand result = request.Listl.Find(
               delegate(SqlCommand bk)
               {
                   SqlParameterCollection paramCollection = bk.Parameters;
                   int parameterList;
                   if (paramCollection.Count == 15)
                       parameterList = int.Parse(String.Format("{0}", paramCollection[14].Value));
                   else
                       parameterList = 1;
                   return parameterList == trajet._Id;
               }
               );
            position = request.Listl.IndexOf(result);
            request.Remove(result);
            request.Remove(request.Listl[position]);
            request.Remove(request.Listl[position]);
        }

        public static void retirerStackRequest(Voyager voyage, int stat)
        {
            ListRequest request = ListRequest.Instance();
            SqlCommand result = request.Listl.Find(
               delegate(SqlCommand bk)
               {
                   SqlParameterCollection paramCollection = bk.Parameters;
                   int user, traj, statut;
                   statut = int.Parse(String.Format("{0}", paramCollection[0].Value));
                   user = int.Parse(String.Format("{0}", paramCollection[1].Value));
                   traj = int.Parse(String.Format("{0}", paramCollection[2].Value));
                   return (voyage._TrajetId == traj && voyage._UserId == user && voyage._Statut == statut);
               }
            );
            if (result != null)
            {
                request.Remove(result);
            }
            else if(stat == 1)
            {
                SqlRequest.supprimerVoyager(voyage);
            }
        }

        public static void viderStackRequest()
        {
            int i, stop, trouver = -1;
            SqlCommand cmd;

            ListRequest list = ListRequest.Instance();
            stop = list.Listl.Count;

            try
            {
                for (i = 0; i < stop; i++)
                {
                    cmd = list.Listl[0];
                    if (checkSelect(cmd) == true)
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            ListUtilisateur utilisateur = ListUtilisateur.Instance();
                            if (reader.Read())
                            {
                                trouver = int.Parse(reader["id_trajet"].ToString());
                            }
                        }
                        reader.Close();
                    }
                    else
                    {
                        if(trouver > 0)
                        {
                            cmd.Parameters.AddWithValue("@id_trajt", trouver);
                            trouver = -1;
                        }
                        cmd.ExecuteNonQuery();
                    }
                    list.Remove(cmd);
                }
            }
            catch (SqlException)
            {

            }
        }

        private static Boolean checkSelect (SqlCommand cmd)
        {
            int i;
            String text = cmd.CommandText;
            String t = "";
            for (i = 0; i < 6; i++)
            {
                t += cmd.CommandText[i];
            }
            return t == "SELECT";
        }

        public static void chargerNotification(int id)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM notifications WHERE id_user=@id", Connexion))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        ListNotification notifs = ListNotification.Instance();
                        while (reader.Read())
                        {
                            notifs.Add(new Notification(int.Parse(reader["id_notification"].ToString()), int.Parse(reader["id_user"].ToString()), reader["titre"].ToString(), reader["txt"].ToString()));
                        }
                    }

                    reader.Close();
                }
            }
            catch (Exception)
            {
                //Log exception
                //Display Error message
            }
        }
        public static void notifierAll(Trajet trajet, String titre, String message)
        {
            ListVoyager listV = ListVoyager.Instance();
            ListRequest listR = ListRequest.Instance();
            Voyager conducteur = listV.rechercheConducteur(trajet);
            List<Voyager> voyage = listV.rchPassager(trajet);

            try
            {

                using (SqlCommand cmdVoyager = new SqlCommand("INSERT INTO notifications VALUES(" +
                   "@titre,@message,@id_user)", Connexion))
                {
                    cmdVoyager.Parameters.AddWithValue("@titre", titre);
                    cmdVoyager.Parameters.AddWithValue("@message", message);
                    cmdVoyager.Parameters.AddWithValue("@id_user", conducteur._UserId);

                    listR.Add(cmdVoyager);
                } //Conducteur

                foreach (Voyager value in voyage)
                { //Passager
                    using (SqlCommand cmdVoyager = new SqlCommand("INSERT INTO notifications VALUES(" +
                       "@titre,@message,@id_user)", Connexion))
                    {
                        cmdVoyager.Parameters.AddWithValue("@titre", titre);
                        cmdVoyager.Parameters.AddWithValue("@message", message);
                        cmdVoyager.Parameters.AddWithValue("@id_user", value._UserId);

                        listR.Add(cmdVoyager);
                    }
                }
            }
            catch (SqlException)
            {
                //Log exception
                //Display Error message
            }
        }

        public static void notifierPassagers(Trajet trajet, String titre, String message)
        {
            ListVoyager listV = ListVoyager.Instance();
            ListRequest listR = ListRequest.Instance();
            List<Voyager> voyage = listV.rchPassager(trajet);

            try
            {
                foreach (Voyager value in voyage)
                { //Passager
                    using (SqlCommand cmdVoyager = new SqlCommand("INSERT INTO notifications VALUES(" +
                       "@titre,@message,@id_user)", Connexion))
                    {
                        cmdVoyager.Parameters.AddWithValue("@titre", titre);
                        cmdVoyager.Parameters.AddWithValue("@message", message);
                        cmdVoyager.Parameters.AddWithValue("@id_user", value._UserId);

                        listR.Add(cmdVoyager);
                    }
                }
            }
            catch (SqlException)
            {
                //Log exception
                //Display Error message
            }
        }

        public static void supprimerNotif(Notification notif)
        {
            try
            {
                ListNotification listN = ListNotification.Instance();
                ListRequest list = ListRequest.Instance();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM notifications WHERE id_notification=@id", Connexion))
                {
                    cmd.Parameters.AddWithValue("@id", notif._Id);

                    list.Add(cmd);
                    listN.Remove(notif);
                }
            }
            catch (SqlException)
            {
                //Log exception
                //Display Error message
            }
        }

        public static int nbrInscrits(Trajet trajet)
        {
            int i = 1; //Conducteur donc 1
            ListVoyager listV = ListVoyager.Instance();
            List<Voyager> voyage = listV.rchPassager(trajet);

            foreach (Voyager value in voyage) { i = i + 1; }

            return i;
        }
    }
}
