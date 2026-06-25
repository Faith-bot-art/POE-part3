using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace CyberbotPart3
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class TaskManager
    {
        private string connectionString;

        public TaskManager()
        {
            connectionString = "Server=localhost;Database=cyberbot_db;Uid=root;Pwd=;";
        }

        public void CreateDatabase()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Uid=root;Pwd=;"))
                {
                    conn.Open();
                    string createDb = "CREATE DATABASE IF NOT EXISTS cyberbot_db";
                    new MySqlCommand(createDb, conn).ExecuteNonQuery();
                }

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string createTable = @"CREATE TABLE IF NOT EXISTS tasks (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        title VARCHAR(255) NOT NULL,
                        description TEXT,
                        reminder_date VARCHAR(100),
                        is_completed BOOLEAN DEFAULT FALSE,
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    )";
                    new MySqlCommand(createTable, conn).ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB Error: " + ex.Message);
            }
        }

        public void AddTask(string title, string description)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO tasks (title, description) VALUES (@title, @description)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add error: " + ex.Message);
            }
        }

        public void SetReminder(string taskId, int days)
        {
            string reminderDate = DateTime.Now.AddDays(days).ToString("yyyy-MM-dd");
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE tasks SET reminder_date = @reminder WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@reminder", reminderDate);
                    cmd.Parameters.AddWithValue("@id", taskId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Reminder error: " + ex.Message);
            }
        }

        public void MarkComplete(string taskId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE tasks SET is_completed = TRUE WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", taskId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Complete error: " + ex.Message);
            }
        }

        public void DeleteTask(string taskId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM tasks WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", taskId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete error: " + ex.Message);
            }
        }

        public List<Task> GetAllTasks()
        {
            var tasks = new List<Task>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM tasks ORDER BY is_completed ASC, created_at DESC";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new Task
                            {
                                Id = reader.GetInt32("id"),
                                Title = reader.GetString("title"),
                                Description = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description"),
                                ReminderDate = reader.IsDBNull(reader.GetOrdinal("reminder_date")) ? "" : reader.GetString("reminder_date"),
                                IsCompleted = reader.GetBoolean("is_completed"),
                                CreatedAt = reader.GetDateTime("created_at")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get tasks error: " + ex.Message);
            }
            return tasks;
        }

        public string GetLatestTaskId()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    var result = new MySqlCommand("SELECT id FROM tasks ORDER BY id DESC LIMIT 1", conn).ExecuteScalar();
                    return result?.ToString();
                }
            }
            catch { return null; }
        }
    }
}
