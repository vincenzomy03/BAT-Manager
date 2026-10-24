using System;
using System.IO;
using System.Linq;
using BatManager.Models;
using BatManager.Services;

namespace BatManager.Tests
{
    public class ScriptRepositoryTests
    {
        public static void RunAllTests()
        {
            Test_ValidateFileName_Valid();
            Test_ValidateFileName_InvalidChars();
            Test_ValidateFileName_ReservedNames();
            Test_SlugGeneration();
            Test_SaveLoadDelete_RoundTrip();
            Test_GetAvailableFileName();
            Console.WriteLine("  [PASS] All ScriptRepository tests passed!");
        }

        private static void Test_GetAvailableFileName()
        {
            string testDir = GetTestDir();
            var repo = new ScriptRepository(testDir);

            // Se il file non esiste, ritorna il nome originale
            string name1 = repo.GetAvailableFileName("mio_script.bat");
            Assert(name1 == "mio_script.bat", "Nome non occupato doveva rimanere invariato: " + name1);

            // Creiamo il file
            File.WriteAllText(Path.Combine(testDir, "mio_script.bat"), "@echo off");

            // Ora deve proporre mio_script-1.bat
            string name2 = repo.GetAvailableFileName("mio_script.bat");
            Assert(name2 == "mio_script-1.bat", "Collisione doveva aggiungere -1: " + name2);

            if (Directory.Exists(testDir))
            {
                Directory.Delete(testDir, true);
            }
        }

        private static void Test_ValidateFileName_Valid()
        {
            var repo = new ScriptRepository(GetTestDir());
            var res = repo.ValidateFileName("test_script.bat");
            Assert(res.IsValid, "Dovrebbe essere valido: " + res.ErrorMessage);
        }

        private static void Test_ValidateFileName_InvalidChars()
        {
            var repo = new ScriptRepository(GetTestDir());
            string[] invalid = { "test:script.bat", "script?.bat", "test*name.bat", "script<1>.bat" };

            foreach (var name in invalid)
            {
                var res = repo.ValidateFileName(name);
                Assert(!res.IsValid, "Non doveva essere valido il nome: " + name);
            }
        }

        private static void Test_ValidateFileName_ReservedNames()
        {
            var repo = new ScriptRepository(GetTestDir());
            string[] reserved = { "CON.bat", "con.cmd", "NUL.bat", "prn.bat", "aux.bat", "COM1.bat", "LPT2.bat" };

            foreach (var name in reserved)
            {
                var res = repo.ValidateFileName(name);
                Assert(!res.IsValid, "Il nome riservato doveva essere bloccato: " + name);
            }
        }

        private static void Test_SlugGeneration()
        {
            string slug1 = ScriptRepository.GenerateSlugFileName("Backup Database");
            Assert(slug1 == "Backup-Database.bat", "Slug errato: " + slug1);

            string slug2 = ScriptRepository.GenerateSlugFileName("Pulisci / Elimina Cache!");
            Assert(slug2 == "Pulisci-Elimina-Cache.bat", "Slug errato con caratteri speciali: " + slug2);

            string slug3 = ScriptRepository.GenerateSlugFileName("CON");
            Assert(slug3 == "_CON.bat", "Slug riservato doveva essere protetto con prefisso: " + slug3);
        }

        private static void Test_SaveLoadDelete_RoundTrip()
        {
            string testDir = GetTestDir();
            if (Directory.Exists(testDir))
            {
                Directory.Delete(testDir, true);
            }

            var repo = new ScriptRepository(testDir);

            // 1. Save
            var script = new BatScript("Script di Test", "Descrizione di Test", "@echo off\r\necho Funziona!", "test_save.bat", null, true);
            repo.Save(script);

            string expectedPath = Path.Combine(testDir, "test_save.bat");
            Assert(File.Exists(expectedPath), "Il file non è stato creato su disco: " + expectedPath);

            // 2. LoadAll
            var list = repo.LoadAll();
            Assert(list.Count == 1, "LoadAll doveva caricare 1 script, trovati: " + list.Count);
            Assert(list[0].Name == "Script di Test", "Nome errato dopo LoadAll: " + list[0].Name);
            Assert(list[0].Description == "Descrizione di Test", "Descrizione errata");
            Assert(list[0].Content.Trim() == "@echo off\r\necho Funziona!", "Contenuto errato");
            Assert(list[0].RunAsAdmin, "RunAsAdmin doveva essere true dopo LoadAll");

            // 3. Rename
            repo.Rename(script, "test_renamed.bat");
            Assert(!File.Exists(expectedPath), "Il vecchio file doveva essere rimosso dopo rename");
            Assert(File.Exists(Path.Combine(testDir, "test_renamed.bat")), "Il nuovo file rinominato doveva esistere");

            // 4. Delete
            repo.Delete(script);
            Assert(!File.Exists(Path.Combine(testDir, "test_renamed.bat")), "Il file non è stato eliminato");

            // Pulizia
            if (Directory.Exists(testDir))
            {
                Directory.Delete(testDir, true);
            }
        }

        private static string GetTestDir()
        {
            return Path.Combine(Path.GetTempPath(), "BatManagerTests_" + Guid.NewGuid().ToString("N"));
        }

        private static void Assert(bool condition, string message)
        {
            if (!condition)
            {
                throw new Exception("TEST FAILED: " + message);
            }
        }
    }
}
