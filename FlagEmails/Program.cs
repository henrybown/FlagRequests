using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.IO;

namespace FlagEmails
{
    class Program
    {
        public static void Main(string[] args)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo directory = new DirectoryInfo(currentDirectory);

            var fileName = Path.Combine(directory.FullName, "Embassies.csv");
            var fileContents = ReadEmbassyCSV(fileName);

            foreach (var embassy in fileContents)
            {
                Console.WriteLine("Country: " + embassy.Country);
                Console.WriteLine("Email: " + embassy.Email);

                sendTestEmail(embassy.Country, embassy.Email);
            }

        }

        public static string ReadFile(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                return reader.ReadToEnd();
            }
        }

        public static List<Embassy> ReadEmbassyCSV(string fileName)
        {
            var embassies = new List<Embassy>();

            using (var reader = new StreamReader(fileName))
            {
                reader.ReadLine();

                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    var embassy = new Embassy();
                    string[] values = line.Split(',');

                    embassy.Country = values[0];
                    embassy.Email = values[1];
                    
                    embassies.Add(embassy);
                }
            }

            return embassies;
        }


        public static void sendTestEmail(string country, string emailAddress)
        {
            string redacted = "";

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(redacted);
                mail.To.Add(emailAddress);
                mail.Subject = "The Flag of " + country;
                mail.Body = "Dear Sir/Madam,\r\n\r\nMy name is Henry Bown, and I have recently started collecting flags. I particularly like the look of the flag of " + country + " but unfortunately I do not have one yet. \r\nI would truly appreciate it if you could send me a small flag so that I can expand my collection. My address is\r\n\r\n" + redacted + "\r\n\r\nThank you for your time,\r\nHenry Bown";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(redacted, redacted);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR");
            }
        }
    }
}
