using Serilog;
using Gaos.Dbo;

namespace Gaos.Common
{
    public class Guest
    {
        private static string CLASS_NAME = typeof(Guest).Name;

        private string GuestNamesFilePath = "py/guest_names.txt";

        private Db Db;

        private bool NamesLoaded = false;
        private List<string> NamesList = new List<string>();

        public Guest(Db db, string guestNamesFilePath)
        {
            Db = db;
            GuestNamesFilePath = guestNamesFilePath;
        }

        private void ReadGuestNames()
        {
            const string METHOD_NAME = "ReadGuestNames()";

            if (NamesLoaded)
            {
                return;
            }

            if (!File.Exists(GuestNamesFilePath))
            {
                Log.Error($"{CLASS_NAME}:{METHOD_NAME}: error: file not found: {GuestNamesFilePath}");
                throw new FileNotFoundException($"File not found: {GuestNamesFilePath}");
            }

            string[] lines;

            try 
            { 
                lines = File.ReadAllLines(GuestNamesFilePath); 
            }
            catch (Exception ex)
            {
                Log.Error($"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                throw new Exception(ex.Message);
            }

            foreach (string line in lines)
            {
                line.Trim();
                NamesList.Add(line);
            }

            NamesLoaded = true;

        }

        private string GetRandomName()
        {
            ReadGuestNames();

            int index = new Random().Next(0, NamesList.Count - 1);

            return NamesList[index];
        }

        private bool IsGuestNameAlreadyTaken(string name)
        {
            bool isExists = Db.GuestNames.Any(gn => gn.Name == name);
            return isExists;
        }

        public string GenerateGuestName()
        {
            const string METHOD_NAME = "GenerateGuestName()";
            bool isFound = false;

            string name = $"{GetRandomName()}{GetRandomName()}{new Random().Next(1000, 9999)}";

            int n = 5;
            while (n > 0)
            {
                if (!IsGuestNameAlreadyTaken(name))
                {
                    isFound = true;
                    break;
                } else {
                    name = GetRandomName();
                }
                --n;
            }

            if (!isFound)
            {
                // generate string uuid
                string uuid = Guid.NewGuid().ToString();
                name = $"{GetRandomName()}_{uuid}";

                if (IsGuestNameAlreadyTaken(name))
                {
                    Log.Error($"{CLASS_NAME}:{METHOD_NAME}: error: failed to generate a unique name");
                    throw new Exception("Failed to generate a unique name");
                }
                return name;
            } else {
                return name;
            }
        }

    }
}