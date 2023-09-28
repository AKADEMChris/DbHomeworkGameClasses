using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbHomeworkGameClasses
{
    /* Задание:
     Для таблицы однотабличной БД из прошлого ДЗ реализовать набор базовых 
    CRUD-операций в виде набора процедур. Протестировать работу процедур 
    (UI реализовывать не надо). 
Требуемые процедуры:
1.	Получить все записи
2.	Получить запись по id
3.	Добавить новую запись
4.	Удалить запись по id
5.	Изменить запись по id

     */
    internal class Program
    {
        //1 процедура, создающая и открывающая подключение к БД
        static SqlConnection OpenDbConnection()
        {
            //обработка исключений бдует выполняться выше по стеку
            string connectionString = @"Data Source=DESKTOP-9A7DMMO\SQLEXPRESS1;Initial Catalog=game_classes_db;Integrated Security=SSPI";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
        //2 вспомогательная процедура читающая и выводящая таблчный результат запроса
        static void ReadQueryResult(SqlDataReader queryResult)
        {
            for (int i = 0; i < queryResult.FieldCount - 1; i++)
            {
                Console.Write($"{queryResult.GetName(i)} - ");
            }
            Console.WriteLine(queryResult.GetName(queryResult.FieldCount - 1));
            
            bool noRows = true;
            while (queryResult.Read())
            {
                noRows = false;
                for (int i = 0; i < queryResult.FieldCount - 1; i++)
                {
                    Console.Write($"{queryResult[i]} - ");
                }
                Console.WriteLine(queryResult.GetName(queryResult.FieldCount - 1));
            }
            if (noRows)
            {
                Console.WriteLine("no rows in result");
            }
        }
        //3 процедура получения всех записей в таблице
        static void SelectAllRows()
        {
            SqlConnection connection = null;
            SqlDataReader queryResult = null;
            try
            {
                //1 открыть соедние
                connection = OpenDbConnection();
                //2 подготовить запрос
                SqlCommand query = new SqlCommand("SELECT * FROM classes_t", connection);
                //3 выполнить запрос с табличным результатом
                queryResult = query.ExecuteReader();
                //4 считать запрос универсальный способ
                ReadQueryResult(queryResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong:{ex.Message}");
            }
            finally
            {
                connection?.Close();
                queryResult?.Close();
            }
        }
        //4 процедура получения записи по айди
        static void SelectRowById(int id)
        {
            SqlConnection connection = null;
            SqlDataReader queryResult = null;
            try
            {
                //1 открыть соедние
                connection = OpenDbConnection();
                //2 подготовить запрос
                SqlCommand query = new SqlCommand($"SELECT * FROM classes_t WHERE id={id}", connection);
                //3 выполнить запрос с табличным результатом
                queryResult = query.ExecuteReader();
                //4 считать запрос универсальный способ
                ReadQueryResult(queryResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong:{ex.Message}");
            }
            finally
            {
                connection?.Close();
                queryResult?.Close();
            }
        }
        //5 добавить новую запись в таблицу
        static void InsertRow(string name, int hit_die, string primary_ability)
        {
            SqlConnection connection = null;
            try
            {
                connection = OpenDbConnection();
                string cmdString = $"INSERT INTO classes_t (name_f, hit_die_f, primary_ability_f) VALUES ('{name}', {hit_die}, '{primary_ability}');";
                SqlCommand cmd = new SqlCommand(cmdString, connection);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected != 1)
                {
                    Console.WriteLine($"insert failed, rowsAffected!=1 ({rowsAffected})");
                }
                else
                {
                    Console.WriteLine("Successfully inserted");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong:{ex.Message}");
            }
            finally
            {
                connection?.Close();
            }
        }
        //удаление
        static void DeleteRow(int id)
        {
            SqlConnection connection = null;
            try
            {
                connection = OpenDbConnection();
                string cmdString = $"DELETE FROM classes_t WHERE Id = {id};";
                SqlCommand cmd = new SqlCommand(cmdString, connection);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected != 1)
                {
                    Console.WriteLine($"delete failed, rowsAffected!=1 ({rowsAffected})");
                }
                else
                {
                    Console.WriteLine("Successfully deleted");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong:{ex.Message}");
            }
            finally
            {
                connection?.Close();
            }
        }
        //изменение
        static void UpdateRow(int id, string name, int hit_die, string primary_ability)
        {
            SqlConnection connection = null;
            try
            {
                connection = OpenDbConnection();
                string cmdString = $"UPDATE classes_t SET name_f = '{name}',hit_die_f = {hit_die},primary_ability_f = '{primary_ability}' WHERE id = {id};";
                //string cmdString = $"UPDATE game_t SET name_f = '{name}',released_in_f = {released},price_f = {price} WHERE id = {id};";
                SqlCommand cmd = new SqlCommand(cmdString, connection);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected != 1)
                {
                    Console.WriteLine($"update failed, rowsAffected!=1 ({rowsAffected})");
                }
                else
                {
                    Console.WriteLine("Successfully updated");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong:{ex.Message}");
            }
            finally
            {
                connection?.Close();
            }
        }
        static void Main(string[] args)
        {
            //OpenDbConnection().Close();
            //SelectRowById(3);

            //InsertRow("Blood Hunter", 10, "Dexterity");
            //SelectAllRows();

            //DeleteRow(1);
            //SelectAllRows();

            UpdateRow(2, "Bard", 2, "Charisma");
            SelectAllRows();


        }
    }
}
