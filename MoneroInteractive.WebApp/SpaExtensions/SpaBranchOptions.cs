namespace MoneroInteractive.WebApp.SpaExtensions
{
    public class SpaBranchOptions
    {

        public SpaBranchOptions(string lang)
        {
            MapPath = $"/{lang}";
            SourcePath = "ClientApp";
            DistPath = $"/dist/{lang}";
            DevServerScript = $"start-{lang}";
        }

        public string MapPath { get; set; }
        public string SourcePath { get; set; }
        public string DistPath { get; set; } = "/dist";
        public string DefaultPage { get; set; } = "/index.html";
        public string DevServerScript { get; set; } = "start";
        public bool EnableServerSideRendering { get; set; } = false;
        public string ServerRenderBundlePath { get; set; } = "/dist-server/main.bundle.js";
        public string ServerRenderBuildScript { get; set; } = "build:ssr";
    }
}
