using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GrainElevatorCS
{
    public class User : ISaveToDb
    {
        public string FirstName { get; set; } = string.Empty; 
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; } = DateTime.MinValue;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }


        public User() { }

        public User(string firstName, string lastName, DateTime birthDate, string email, string phone, string city, string country)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Email = email;
            Phone = phone;
            City = city;
            Country = country;
        }


        public async Task SaveAllInfo(string connString, string databaseName, string tableName, params object[] objects)
        {
            string query = @"INSERT INTO" + $"{tableName}" + "(firstName, lastName, birthDate, email, phone, city, country)" +
                                            "VALUES (@firstName, @lastName, @birthDate, @email, @phone, @city, @country)";

            using SqlConnection conn = new SqlConnection(connString);

            try
            {
                await conn.OpenAsync();

                SqlCommand cmd = new SqlCommand(query, conn);

                SqlParameter firstNameParam = new SqlParameter("@firstName", SqlDbType.VarChar, 30)
                {
                    Value = objects[0]
                };
                cmd.Parameters.Add(firstNameParam);

                SqlParameter lastNameParam = new SqlParameter("@lastName", SqlDbType.VarChar, 30)
                {
                    Value = objects[1]
                };
                cmd.Parameters.Add(lastNameParam);

                SqlParameter birthDateParam = new SqlParameter("@birthDate", SqlDbType.Date, 3)
                {
                    Value = objects[2]
                };
                cmd.Parameters.Add(birthDateParam);

                SqlParameter emailParam = new SqlParameter("@email", SqlDbType.VarChar, 30)
                {
                    Value = objects[3]
                };
                cmd.Parameters.Add(emailParam);

                SqlParameter phoneParam = new SqlParameter("@phone", SqlDbType.VarChar, 13)
                {
                    Value = objects[4]
                };
                cmd.Parameters.Add(phoneParam);

                SqlParameter cityParam = new SqlParameter("@city", SqlDbType.VarChar, 30)
                {
                    Value = objects[5]
                };
                cmd.Parameters.Add(cityParam);

                SqlParameter countryParam = new SqlParameter("@country", SqlDbType.VarChar, 30)
                {
                    Value = objects[6]
                };
                cmd.Parameters.Add(countryParam);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");  //  TODO
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
    }
}

