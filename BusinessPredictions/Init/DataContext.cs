using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BusinessPredictions
{
    public class DataContext : DbContext
    {
        public static readonly string BusinessPredictionsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BusinessPredictions");

        public static string dbConnectionString = $"Data Source={Path.Combine(BusinessPredictionsFolder, "Frases.db")}";

        private static bool _created = File.Exists(Path.Combine(BusinessPredictionsFolder, "Frases.db"));
        public DataContext()
        {
            DbInit();
        }
        private async void DbInit()
        {
            if (!_created)
            {
                _created = true;
                try
                {
                    Database.EnsureDeleted();
                    Database.EnsureCreated();
                    await AddDemoData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        {
            optionbuilder.UseSqlite(dbConnectionString);
        }
        public DbSet<Frase> Frases { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public async static Task AddDemoData()
        {
            await Task.Run(() =>
            {

                using (var dataContext = new DataContext())
                {
                    var greetings = new Subject() { SubjectName = "Greetings" };
                    dataContext.Subjects.Add(greetings);
                    dataContext.Frases.Add(new Frase() { Text = "Hello my dear", Subject = greetings, IsLeft = true });
                    dataContext.Frases.Add(new Frase() { Text = "friend ! ", Subject = greetings, IsLeft = false });

                    var badWords = new Subject() { SubjectName = "Funny stuff" };
                    dataContext.Subjects.Add(badWords);
                    dataContext.Frases.Add(new Frase() { Text = "You are a", Subject = badWords, IsLeft = true });
                    dataContext.Frases.Add(new Frase() { Text = "giant elefant", Subject = badWords, IsLeft = false });
                    dataContext.SaveChanges();
                }
            }).ConfigureAwait(true);

            if (PanelsWrapper.GetUserControlWrapper().TryGetPanelByType(typeof(PlayPanel), out UserControl playPanel) &&
                        playPanel is PlayPanel ourPlayPanel)
                ourPlayPanel.GameWrapperVM.LoadData();
        }
    }
}
