using System;
using System.IO;
using BatManager.Models;
using BatManager.Services;

namespace BatManager.Tests
{
    public class BatParserTests
    {
        public static void RunAllTests()
        {
            Test_StandardHeader_ExtractsMetadata();
            Test_ColonCommentHeader_ExtractsMetadata();
            Test_MissingHeader_FallbacksToFileName();
            Test_HeaderSeparation_LeavesCommandsInContent();
            Test_BuildFileContent_FormatsCorrectly();
            Test_RunAsAdminHeader_ParsesCorrectly();
            Console.WriteLine("  [PASS] All BatParser tests passed!");
        }

        private static void Test_RunAsAdminHeader_ParsesCorrectly()
        {
            string raw = "REM @name: Test Admin\r\nREM @description: Desc\r\nREM @runAsAdmin: true\r\n\r\necho Admin";
            var script = BatParser.Parse(raw, "admin.bat");

            Assert(script.RunAsAdmin, "RunAsAdmin doveva essere true per @runAsAdmin: true");

            string built = BatParser.BuildFileContent(script.Name, script.Description, script.Content, script.RunAsAdmin);
            Assert(built.Contains("REM @runAsAdmin: true"), "BuildFileContent non ha incluso @runAsAdmin: true");

            // Test con false
            string rawFalse = "REM @name: Test Non Admin\r\nREM @runAsAdmin: false\r\n\r\necho Normal";
            var scriptFalse = BatParser.Parse(rawFalse, "non_admin.bat");
            Assert(!scriptFalse.RunAsAdmin, "RunAsAdmin doveva essere false per @runAsAdmin: false");
        }

        private static void Test_StandardHeader_ExtractsMetadata()
        {
            string raw = "REM @name: Backup Giornaliero\r\nREM @description: Salva i file importanti su disco esterno\r\n\r\necho Inizio backup...";
            var script = BatParser.Parse(raw, "backup.bat", @"C:\scripts\backup.bat");

            Assert(script.Name == "Backup Giornaliero", "Name non estratto correttamente: " + script.Name);
            Assert(script.Description == "Salva i file importanti su disco esterno", "Description non estratta correttamente: " + script.Description);
            Assert(script.Content.Trim() == "echo Inizio backup...", "Content non separato correttamente: " + script.Content);
            Assert(script.FileName == "backup.bat", "FileName non corretto: " + script.FileName);
        }

        private static void Test_ColonCommentHeader_ExtractsMetadata()
        {
            string raw = ":: @name: Pulizia File Temp\r\n:: @description: Elimina file temporanei\r\n\r\ndel /q %temp%\\*";
            var script = BatParser.Parse(raw, "clean.bat");

            Assert(script.Name == "Pulizia File Temp", "Name con sintassi :: fallito: " + script.Name);
            Assert(script.Description == "Elimina file temporanei", "Description con sintassi :: fallita");
            Assert(script.Content.Trim() == "del /q %temp%\\*", "Content fallito");
        }

        private static void Test_MissingHeader_FallbacksToFileName()
        {
            string raw = "@echo off\r\necho Hello World\r\npause";
            var script = BatParser.Parse(raw, "mio-script.bat");

            Assert(script.Name == "mio-script", "Il fallback del nome doveva essere mio-script, ottenuto: " + script.Name);
            Assert(script.Description == string.Empty, "La descrizione doveva essere vuota");
            Assert(script.Content.Contains("echo Hello World"), "Il contenuto è stato perso");
        }

        private static void Test_HeaderSeparation_LeavesCommandsInContent()
        {
            string raw = "REM @name: Test\r\nREM @description: Desc\r\n\r\nREM Commento normale interno allo script\r\necho 123";
            var script = BatParser.Parse(raw, "test.bat");

            Assert(script.Content.Contains("REM Commento normale interno allo script"), "I commenti normali nello script devono essere preservati");
            Assert(!script.Content.Contains("@name:"), "Il tag @name non deve essere presente in Content");
            Assert(!script.Content.Contains("@description:"), "Il tag @description non deve essere presente in Content");
        }

        private static void Test_BuildFileContent_FormatsCorrectly()
        {
            string full = BatParser.BuildFileContent("Nome", "Descrizione", "echo Ciao");
            Assert(full.Contains("REM @name: Nome"), "Manca header @name");
            Assert(full.Contains("REM @description: Descrizione"), "Manca header @description");
            Assert(full.Contains("echo Ciao"), "Manca contenuto");
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
