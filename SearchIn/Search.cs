using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;

namespace SearchIn
{
    internal sealed class Search
    {
        public const int SearchInGithubId = 0x0100;
        public const int SearchInGoogleId = 0x0150;

        public static readonly Guid CommandSet = new Guid("26a7481b-530a-4440-a240-fdf008d4c61b");

        private readonly Package package;
        OleMenuCommandService _commandService;

        private Search(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;
            _commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (_commandService != null)
            {
                _commandService.AddCommand(new MenuCommand(this.SearchInGithub, new CommandID(CommandSet, SearchInGithubId)));
                _commandService.AddCommand(new MenuCommand(this.SearchInGoogle, new CommandID(CommandSet, SearchInGoogleId)));
            }
        }

        public static Search Instance
        {
            get;
            private set;
        }

        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        public static void Initialize(Package package)
        {
            Instance = new Search(package);
        }

        private void SearchInGithub(object sender, EventArgs e)
        {
            var url = string.Format("{0}{1}", "https://github.com/search?utf8=✓&q=", "github ara");
            System.Diagnostics.Process.Start(url); // open browser
        }

        private void SearchInGoogle(object sender, EventArgs e)
        {
            var url = string.Format("{0}{1}", "https://www.google.com.tr/#q=", "google ara");
            System.Diagnostics.Process.Start(url); // open browser
        }
    }
}
