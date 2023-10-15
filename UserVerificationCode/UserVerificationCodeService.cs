using Gaos.Dbo;
using System.Text;
using Serilog;

namespace Gaos.UserVerificationCode
{
    public class UserVerificationCodeService
    {
        public static string CLASS_NAME = typeof(UserVerificationCodeService).Name;

        private Db db;

        public UserVerificationCodeService(Db db)
        {
            this.db = db;
        }

        private string GetRandomCode()
        {
            int length = 4;
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            Random random = new Random();

            StringBuilder sb = new StringBuilder();


            for (int i = 0; i < length; i++)
            {
                int index = random.Next(characters.Length);
                sb.Append(characters[index]);
            }

            string randomString = sb.ToString();
            return randomString;
        }

        private string _GetUniqueRandomCode(int cnt)
        {
            const string METHOD_NAME = "_GetUniqueRandomCode()";
            if (cnt < 1)
            {
                Log.Error($"{CLASS_NAME}.{METHOD_NAME} - Unable to generate unique code");
                throw new Exception("Unable to generate unique code");
            }

            string code = GetRandomCode();

            var codes = db.UserVerificationCode.Where(x => x.Code == code);

            if (codes.Count() > 0)
            {
                return _GetUniqueRandomCode(cnt - 1);
            }
            else
            {
                return code;
            }
        }

        private string GetUniqueRandomCode()
        {
            const string METHOD_NAME = "GetUniqueRandomCode()";
            const int maxRetries = 10;
            try
            {
                return _GetUniqueRandomCode(maxRetries);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{CLASS_NAME}.{METHOD_NAME} - Error: unable to generate unique code after {maxRetries} tries");
                throw new Exception("unable to generate unique code");
            }

        }

        private void RemoveAllCodes(int userId)
        {
            var codes = db.UserVerificationCode.Where(x => x.UserId == userId);
            db.UserVerificationCode.RemoveRange(codes);
            db.SaveChanges();
        }

        public string GenerateCode(int userId)
        {
            const string METHOD_NAME = "GenerateCode()";
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    RemoveAllCodes(userId);

                    var verificationCode = new Dbo.Model.UserVerificationCode()
                    {
                        UserId = userId,
                        Code = GetUniqueRandomCode(),
                        ExpiresAt = DateTime.Now.AddMinutes(5)
                    };

                    db.UserVerificationCode.Add(verificationCode);
                    db.SaveChanges();
                    transaction.Commit();

                    return verificationCode.Code;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Log.Error(ex, $"{CLASS_NAME}.{METHOD_NAME} - Error: {ex.Message}");
                    throw new Exception($"{METHOD_NAME} failed");
                }
            }
        }

        public bool VerifyCode(int userId, string code)
        {
            const string METHOD_NAME = "VerifyCode()";
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var userCount = db.UserVerificationCode.Where(x => x.UserId == userId && x.Code == code && x.ExpiresAt > DateTime.Now).Count();

                    if (userCount < 1)
                    {
                        RemoveAllCodes(userId);
                        transaction.Commit();
                        return false;
                    }
                    else
                    {
                        RemoveAllCodes(userId);
                        transaction.Commit();
                        return true;
                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Log.Error(ex, $"{CLASS_NAME}.{METHOD_NAME} - Error: {ex.Message}");
                    throw new Exception($"{METHOD_NAME} failed");
                }
            }
        }
    }
}
