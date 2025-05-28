using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using Rditil.Models;

namespace Rditil.Services
{
    public class ExcelService : IExcelService
    {
        private readonly string _questionsFilePath;
        private readonly string _usersFilePath;
        private readonly Random _random;

        public ExcelService()
        {
            _questionsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Excel", "Questions.xlsx");
            _usersFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Excel", "Users.xlsx");
            _random = new Random();
            InitializeExcelFiles();
        }

        private void InitializeExcelFiles()
        {
            if (!Directory.Exists(Path.GetDirectoryName(_questionsFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_questionsFilePath));
            }

            if (!File.Exists(_questionsFilePath))
            {
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Questions");
                    worksheet.Cells[1, 1].Value = "ID";
                    worksheet.Cells[1, 2].Value = "Question";
                    worksheet.Cells[1, 3].Value = "Reponse1";
                    worksheet.Cells[1, 4].Value = "Reponse2";
                    worksheet.Cells[1, 5].Value = "Reponse3";
                    worksheet.Cells[1, 6].Value = "Reponse4";
                    worksheet.Cells[1, 7].Value = "ReponseCorrecte";
                    package.SaveAs(new FileInfo(_questionsFilePath));
                }
            }

            if (!File.Exists(_usersFilePath))
            {
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Users");
                    worksheet.Cells[1, 1].Value = "Email";
                    worksheet.Cells[1, 2].Value = "Nom";
                    worksheet.Cells[1, 3].Value = "MotDePasse";
                    worksheet.Cells[1, 4].Value = "Score";
                    worksheet.Cells[1, 5].Value = "DernierExamen";
                    package.SaveAs(new FileInfo(_usersFilePath));
                }
            }
        }

        public List<Question> GetRandomQuestions(int count)
        {
            var questions = new List<Question>();
            using (var package = new ExcelPackage(new FileInfo(_questionsFilePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                // Sélectionner aléatoirement les questions
                var selectedRows = Enumerable.Range(2, rowCount - 1)
                    .OrderBy(x => _random.Next())
                    .Take(count);

                foreach (var row in selectedRows)
                {
                    questions.Add(new Question
                    {
                        Id = int.Parse(worksheet.Cells[row, 1].Value.ToString()),
                        Enonce = worksheet.Cells[row, 2].Value.ToString(),
                        Reponses = new List<Reponse>
                        {
                            new Reponse { TextReponse = worksheet.Cells[row, 3].Value.ToString(), EstCorrect = worksheet.Cells[row, 7].Value.ToString() == "1" },
                            new Reponse { TextReponse = worksheet.Cells[row, 4].Value.ToString(), EstCorrect = worksheet.Cells[row, 7].Value.ToString() == "2" },
                            new Reponse { TextReponse = worksheet.Cells[row, 5].Value.ToString(), EstCorrect = worksheet.Cells[row, 7].Value.ToString() == "3" },
                            new Reponse { TextReponse = worksheet.Cells[row, 6].Value.ToString(), EstCorrect = worksheet.Cells[row, 7].Value.ToString() == "4" }
                        }
                    });
                }
            }
            return questions;
        }

        public void SaveUserResult(string email, int score)
        {
            using (var package = new ExcelPackage(new FileInfo(_usersFilePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    if (worksheet.Cells[row, 1].Value.ToString() == email)
                    {
                        worksheet.Cells[row, 4].Value = score;
                        worksheet.Cells[row, 5].Value = DateTime.Now;
                        break;
                    }
                }
                package.Save();
            }
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            using (var package = new ExcelPackage(new FileInfo(_usersFilePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    users.Add(new User
                    {
                        Email = worksheet.Cells[row, 1].Value.ToString(),
                        Nom = worksheet.Cells[row, 2].Value.ToString(),
                        MotDePasse = worksheet.Cells[row, 3].Value.ToString(),
                        Score = int.Parse(worksheet.Cells[row, 4].Value?.ToString() ?? "0"),
                        DernierExamen = DateTime.Parse(worksheet.Cells[row, 5].Value?.ToString() ?? DateTime.MinValue.ToString())
                    });
                }
            }
            return users;
        }

        public void AddUser(User user)
        {
            using (var package = new ExcelPackage(new FileInfo(_usersFilePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;
                var newRow = rowCount + 1;

                worksheet.Cells[newRow, 1].Value = user.Email;
                worksheet.Cells[newRow, 2].Value = user.Nom;
                worksheet.Cells[newRow, 3].Value = user.MotDePasse;
                worksheet.Cells[newRow, 4].Value = user.Score;
                worksheet.Cells[newRow, 5].Value = user.DernierExamen;

                package.Save();
            }
        }

        public void UpdateUser(User user)
        {
            using (var package = new ExcelPackage(new FileInfo(_usersFilePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    if (worksheet.Cells[row, 1].Value.ToString() == user.Email)
                    {
                        worksheet.Cells[row, 2].Value = user.Nom;
                        worksheet.Cells[row, 3].Value = user.MotDePasse;
                        worksheet.Cells[row, 4].Value = user.Score;
                        worksheet.Cells[row, 5].Value = user.DernierExamen;
                        break;
                    }
                }
                package.Save();
            }
        }

        public void DeleteUser(string email)
        {
            using (var package = new ExcelPackage(new FileInfo(_usersFilePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    if (worksheet.Cells[row, 1].Value.ToString() == email)
                    {
                        worksheet.DeleteRow(row);
                        break;
                    }
                }
                package.Save();
            }
        }
    }
} 