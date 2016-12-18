using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;

namespace SearchIn
{
    internal sealed class Search
    {
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
                _commandService.AddCommand(new MenuCommand(this.SearchInGithub, new CommandID(CommandSet, CommandIds.SerchInGithubCommandId)));
                _commandService.AddCommand(new MenuCommand(this.SearchInGoogle, new CommandID(CommandSet, CommandIds.SearchInGoogleCommandId)));
                _commandService.AddCommand(new MenuCommand(this.SearchInSof, new CommandID(CommandSet, CommandIds.SerchInSofCommandId)));
                _commandService.AddCommand(new MenuCommand(this.SearchInMsdn, new CommandID(CommandSet, CommandIds.SearchInMsdnCommandId)));
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
            RedirectToPage(BaseUrls.GithubBaseUrl);
        }

        private void SearchInGoogle(object sender, EventArgs e)
        {
            RedirectToPage(BaseUrls.GoogleBaseUrl);
        }

        private void SearchInSof(object sender, EventArgs e)
        {
            RedirectToPage(BaseUrls.SofBaseUrl);
        }

        private void SearchInMsdn(object sender, EventArgs e)
        {
            RedirectToPage(BaseUrls.MsdnBaseUrl);
        }

        private void RedirectToPage(string baseUri)
        {
            var text = GetSelectedText();
            if (!string.IsNullOrEmpty(text))
            {
                var url = string.Format(baseUri, GetSelectedText());
                System.Diagnostics.Process.Start(url);
            }
            else
            {
                VsShellUtilities.ShowMessageBox(this.ServiceProvider, "Select text to search in...", "Oops!",
                         OLEMSGICON.OLEMSGICON_WARNING, OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            }
        }

        private string GetSelectedText()
        {
            DTE dte = Package.GetGlobalService(typeof(DTE)) as DTE;
            TextSelection ts = dte.ActiveDocument.Selection as TextSelection;
            return ts.Text;
        }
    }
}
