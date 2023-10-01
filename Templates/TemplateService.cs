using Scriban;
using Gaos.Dbo;
using Serilog;

namespace Gaos.Templates
{
    public class TemplateService
    {
        public static string CLASS_NAME = typeof(TemplateService).Name;

        private IConfiguration Configuration;
        private Db db;

        private string TEMPLATES_FDOLDER_PATH;

        private static Dictionary<string, Template> templateCache = new Dictionary<string, Template>();

        public TemplateService(IConfiguration configuration, Db db)
        {
            this.Configuration = configuration;
            this.db = db;

            if (Configuration["templates_fdolder_path"] == null)
            {
                throw new Exception("missing configuration value: templates_fdolder_path");
            }
            TEMPLATES_FDOLDER_PATH = Configuration["templates_fdolder_path"];
        }

        public Template ParseFile(string path, bool isUseCache = false)
        {
            const string METHOD_NAME = "ParseFile";

            if (isUseCache)
            {
                if (templateCache.ContainsKey(path))
                {
                    return templateCache[path];
                }
            }

            string fullPath = TEMPLATES_FDOLDER_PATH + "/" + path;
            Template template = Template.Parse(System.IO.File.ReadAllText(fullPath));

            // Report any errors
            if (template.HasErrors)
            {
                foreach (var message in template.Messages)
                {
                    Console.WriteLine(message);
                }

                Log.Error($"{CLASS_NAME}:{METHOD_NAME}: error parsing tewmplete: {path}");
            }

            if (isUseCache)
            {
                templateCache.Add(path, template);
                
            }

            return template;
        }
    }
}
