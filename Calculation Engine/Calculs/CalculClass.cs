using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculation_Engine.Calculs
{
    public class CalculClass
    {
        public void Calculation()
        {
            string uid = "root";
            string password = "";
            string sqlDataSource = "SERVER=localhost;PORT=3306;" +
                 "DATABASE=algeco_metrics_calculation;" +
                 "UID=" + uid + ";PASSWORD=" + password;

            Moyenne(sqlDataSource, "presenceSensor");
            Moyenne(sqlDataSource, "temperatureSensor");
            Moyenne(sqlDataSource, "brightnessSensor");
            Moyenne(sqlDataSource, "atmosphericPressureSensor");
            Moyenne(sqlDataSource, "humiditySensor");
            Moyenne(sqlDataSource, "soundLevelSensor");
            Moyenne(sqlDataSource, "co2Sensor");

            Pourcentage(sqlDataSource, "presenceSensor");
        }

        public void Moyenne(string sqlDataSource, string type_device)
        {
            string query = "select id_device,AVG(valeur_metrique) as moy from metriques where type_device='"+type_device+"' group by id_device";

            MySqlDataReader myReader;

            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();

                    while(myReader.Read())
                    {
                        string query_insert = "insert into metriques_calculees(id_device, moyenne) values('" + (string)myReader["id_device"] + "','" + (double)myReader["moy"] + "')";
                        InsertCalculs(sqlDataSource, query_insert);
                    }
                    
                    myReader.Close();
                    mycon.Close();
                }
            }
        }

        public void Pourcentage(string sqlDataSource, string type_device)
        {
            string query = "select id_device,AVG(valeur_metrique) as moy from metriques where type_device='" + type_device + "' group by id_device";

            MySqlDataReader myReader;

            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();

                    while (myReader.Read())
                    {
                        string percent = ((double)myReader["moy"] * 100).ToString();
                        string percent2 = percent + "%";

                        string query_insert = "update metriques_calculees set pourcentage='"+ percent2 + "' where id_device='"+ (string)myReader["id_device"] + "'";
                        InsertCalculs(sqlDataSource, query_insert);
                    }

                    myReader.Close();
                    mycon.Close();
                }
            }
        }

        public void InsertCalculs(string sqlDataSource, string query)
        {
            MySqlDataReader myReader;

            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();

                    myReader.Close();
                    mycon.Close();
                }
            }
        }
    }
}
