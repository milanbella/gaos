using Scriban;
using Scriban.Runtime;
using Gaos.Templates;

namespace Gaos.Templates.Email
{
    public class VerifyEmailTemplate
    {
        public static string CLASS_NAME = typeof(VerifyEmailTemplate).Name;

        public static string GenerateVerifyEmailTemplate(TemplateService templateSerivice, string verifyEmailUrl)
        {
            var scriptObject1 = new ScriptObject();
            scriptObject1.Add("verifyEmailUrl", verifyEmailUrl);

            var context = new TemplateContext();
            context.PushGlobal(scriptObject1);

            var template = templateSerivice.ParseFile("Email/VerifyEmailTemplate.sbn", true);

            var result = template.Render(context);


            return result;
        }
    }
}
