using apitask.Models;
using Npgsql;
using System;
using System.Data;

namespace apitask.DAO
{
    public class PersonDaoImplementation:IPersonDao
    {
        NpgsqlConnection _connection;

        public PersonDaoImplementation(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> AddPerson(Person person)
        {
            int rowsInserted = 0;
            string insertQuery = @$"insert into info.personal_info (name, date_of_birth, residential_address, permanent_address, phone_number, email_address, marital_status, gender, occupation, aadhar_card_number, pan_number)
                                    values('{person.Name}', '{person.DateOfBirth}', '{person.ResidentialAddress}', '{person.PermanentAddress}', '{person.PhoneNumber}', '{person.EmailAddress}', '{person.MaritalStatus}', '{person.Gender}', '{person.Occupation}', '{person.AadharCardNumber}', '{person.PanNumber}')";

            try
            {
                using (_connection)
                {
                    await _connection.OpenAsync ();
                    NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, _connection);
                    insertCommand.CommandType = CommandType.Text;
                    rowsInserted = await insertCommand.ExecuteNonQueryAsync ();
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }
            return rowsInserted;
        }

        public async Task<List<Person>> GetAllPersons()
        {
            List<Person> personList = new List<Person>();
            string query = "select * from info.personal_info";
            try
            {
                using (_connection)
                {
                    await _connection.OpenAsync();
                    NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Person person = new Person();
                            person.Id = reader.GetInt32(0);
                            person.Name = reader.GetString(1);
                            person.DateOfBirth = reader.GetDateTime(2).ToString("yyyy-MM-dd"); 
                            person.ResidentialAddress = reader.GetString(3);
                            person.PermanentAddress = reader.GetString(4);
                            person.PhoneNumber = reader.GetString(5);
                            person.EmailAddress = reader.GetString(6);
                            person.MaritalStatus = reader.IsDBNull(7) ? null : reader.GetString(7); 
                            person.Gender = reader.IsDBNull(8) ? null : reader.GetString(8);
                            person.Occupation = reader.IsDBNull(9) ? null : reader.GetString(9);
                            person.AadharCardNumber = reader.IsDBNull(10) ? null : reader.GetString(10);
                            person.PanNumber = reader.IsDBNull(11) ? null : reader.GetString(11);
                            personList.Add(person);
                        }
                    }
                    reader?.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return personList;
        }

        public async Task<bool> UpdatePersonNameById(int id, string newName)
        {
            string query = "update info.personal_info set name = @NewName where id = @Id";
            try
            {
                using (_connection)
                {
                    await _connection.OpenAsync();
                    NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                    command.Parameters.AddWithValue("@NewName", newName);
                    command.Parameters.AddWithValue("@Id", id);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<bool> UpdatePersonNameByAadhar(string aadharCardNumber,string newName)
        {
            string query = "UPDATE info.personal_info SET name = @NewName WHERE aadhar_card_number = @AadharCardNumber";
            try
            {
                using(_connection)
                {
                    await _connection.OpenAsync();
                    NpgsqlCommand command = new NpgsqlCommand(query,_connection);
                    command.Parameters.AddWithValue("@AadharCardNumber",aadharCardNumber);
                    command.Parameters.AddWithValue("@NewName",newName);
                    int rowAffected = await command.ExecuteNonQueryAsync();
                    return rowAffected > 0;
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        public async Task<bool> DeletePersonByAadharCardNumber(string aadharCardNumber)
        {
            string query = "delete from info.personal_info where aadhar_card_number = @AadharCardNumber";
            try
            {
                using (_connection)
                {
                    await _connection.OpenAsync();
                    NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                    command.Parameters.AddWithValue("@AadharCardNumber", aadharCardNumber);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        //
    }
}
