﻿using CWCTMA.Helpers;
using CWCTMA.Model.XMD;
using Markdig;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace CWCTMA.Model
{
    public class Globals
    {
        #region Properties

        public static MarkdownPipeline MarkdownPipeline { get; internal set; }

        public static string CurrentPage { get; set; }

        public static ApplicationMetadata AppMeta { get; internal set; }

        public static JsonSerializerOptions JsonSerializerOptions => new()
        {
            AllowTrailingCommas = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReadCommentHandling = JsonCommentHandling.Skip,
            WriteIndented = true
        };
        #endregion

        #region Directories
        public static string DataDirectory
        {
            get
            {
                var dir = Path.GetFullPath(Path.Combine("..", "data"));
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                return dir;
            }
        }
        public static string ContentDirectory
        {
            get
            {
                var dir = Path.GetFullPath(Path.Combine(DataDirectory, "content"));
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                return dir;
            }
        }
        #endregion

        #region Configs
        public static string ConfigFile => Path.Combine(DataDirectory, "config.json");
        public static Config Config { get; internal set; }

        public static string GroupFile => Path.Combine(DataDirectory, "group.json");
        public static GroupConfig Group { get; internal set; }

        public static string PagesFile => Path.Combine(ContentDirectory, "pages.xml");
        public static PagesConfig Pages { get; internal set; }
        #endregion

        #region Methods
        public static Metadata GetMetadata(string pageId) => Pages?.Page.FirstOrDefault(x => x.Id.Equals(pageId, StringComparison.OrdinalIgnoreCase)) ?? new() { };

        internal static void LoadConfigs()
        {
            if (!File.Exists(Globals.ConfigFile))
            {
                File.WriteAllText(Globals.ConfigFile, JsonSerializer.Serialize(new Config
                {
                    Id = "CWCTMADE".ToLower(),
                    ShortName = "CWCT/MA DE",
                    FullName = "Czompi WebAPP Common Template for Microsoft ASP.NET - Development Environment",

#if RELEASE
                    CdnUrl = "https://cdn.czompisoftware.hu/",
#else
                    CdnUrl = "https://cdn.czompisoftware.dev/",
#endif
                    SiteURL = "./",
                    Meta = new()
                    {
                        Title = "CWCT/MA DE",
                        Description = "Czompi WebAPP Common Template for Microsoft ASP.NET - Development Environment",
                        Image = null,
                        PrimaryColor = "#EAEAEA"
                    }
                }, Globals.JsonSerializerOptions));
            }
            if (!File.Exists(Globals.GroupFile))
            {
                File.WriteAllText(Globals.GroupFile, JsonSerializer.Serialize(new GroupConfig
                {
                    Current = "czd",
                    Groups = new()
                    {
                        new() { Id = "czd", Name = "Czompi", Url = "https://czompi.hu" },
                        new() { Id = "czs", Name = "Czompi Software", Url = "https://czompisoftware.hu" },
                        new() { Id = "hls", Name = "HunLuxSCHOOL", Url = "https://hunluxschool.hu" },
                        new() { Id = "hll", Name = "HunLux Launcher", Url = "https://hunluxlauncher.hu" }
                    }
                }, Globals.JsonSerializerOptions));
            }
            if (!File.Exists(Globals.PagesFile))
            {
                new PagesConfig
                {
                    Page = new List<Metadata>()
                    {
                        new() { Id = "index", Title = "Main page" }
                    }
                }.ToFile(Globals.PagesFile);
            }
            Globals.Config = JsonSerializer.Deserialize<Config>(File.ReadAllText(Globals.ConfigFile), Globals.JsonSerializerOptions);
            Globals.Group = JsonSerializer.Deserialize<GroupConfig>(File.ReadAllText(Globals.GroupFile), Globals.JsonSerializerOptions);
            Globals.Pages = File.ReadAllText(Globals.PagesFile).ParseXml<PagesConfig>();
            Globals.AppMeta = new()
            {
                Name = "CWCT/MA",
                FullName = "Czompi WebAPP Common Template for Microsoft ASP.NET",
                Version = Assembly.GetExecutingAssembly().GetName().Version,
                CompileTime = CWCTMA.Builtin.CompileTime,
                BuildId = CWCTMA.Builtin.BuildId
            };
        }
        #endregion

    }
}