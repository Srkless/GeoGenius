using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using GeoGenius.Model;
using static System.Formats.Asn1.AsnWriter;

namespace GeoGenius.Service
{
    public class GameService
    {

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static bool RegisterUser(string username, string password)
        {
            var users = GetAllUsers();
            if (users.Any(u => u.Username == username))
            {
                return false; // korisnik već postoji
            }

            if(password.Length < 8)
            {
                return false; // lozinka je prekratka
            }

            string hashedPassword = HashPassword(password);
            string line = $"{username}#{hashedPassword}#0";
            File.AppendAllText("users.txt", line + Environment.NewLine);
            return true;
        }
        public static List<User> GetAllUsers()
        {
            var users = new List<User>();

            if (!File.Exists("users.txt"))
                return users;

            var lines = File.ReadAllLines("users.txt");

            foreach (var line in lines)
            {
                var parts = line.Split('#');
                if (parts.Length == 3)
                {
                    users.Add(new User(parts[0], parts[1], int.Parse(parts[2])));
                }
            }

            return users;
        }

        public static List<User> GetTop3Users()
        {
            return GetAllUsers()
                   .OrderByDescending(u => u.Score)
                   .Take(3)
                   .ToList();
        }

        public static User GetUserByUsername(string username)
        {
            string filePath = "users.txt"; // Putanja do fajla sa korisnicima

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Fajl sa korisnicima nije pronađen.");
            }

            // Pročitaj sve linije iz fajla
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var parts = line.Split('#'); // Pretpostavljamo da je delimiter "#" između korisničkog imena, lozinke i bodova

                if (parts.Length == 3 && parts[0] == username) // Ako je korisničko ime u fajlu isto kao onaj koji tražimo
                {
                    string userName = parts[0];
                    string hashedPassword = parts[1];
                    int score = int.Parse(parts[2]);

                    return new User(userName, hashedPassword, score); // Vraćamo korisnika
                }
            }

            return null; // Ako korisnik nije pronađen
        }
        public static void UpdateUserPoints(string username, int newScore)
        {
            string path = "users.txt";
            if (!File.Exists(path)) return;

            var lines = File.ReadAllLines(path).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                var parts = lines[i].Split('#');
                if (parts.Length == 3 && parts[0] == username)
                {
                    int currentScore = int.Parse(parts[2]);
                    if (newScore > currentScore)
                    {
                        lines[i] = $"{parts[0]}#{parts[1]}#{newScore}";
                    }
                    break;
                }
            }

            File.WriteAllLines(path, lines);
        }

        public static bool AuthLogin(string username, string password)
        {
            string hashedPassword = HashPassword(password);
            var lines = File.ReadAllLines("users.txt");

            foreach (string line in lines)
            {
                string[] parts = line.Split('#');
                if (parts.Length == 3 && parts[0] == username && parts[1] == hashedPassword)
                {
                    return true;
                }
            }

            return false;
        }
    }

}
