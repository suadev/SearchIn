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
                AddCommand(this.SearchInGithub, CommandIds.SerchInGithubCommandId);
                AddCommand(this.SearchInGoogle, CommandIds.SearchInGoogleCommandId);
                AddCommand(this.SearchInSof, CommandIds.SerchInSofCommandId);
                AddCommand(this.SearchInMsdn, CommandIds.SearchInMsdnCommandId);
            }
        }

        private void AddCommand(EventHandler handler, int commandId)
        {
            _commandService.AddCommand(new MenuCommand(handler, new CommandID(CommandSet, commandId)));
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
                System.Diagnostics.Process.Start(string.Format(baseUri, text));
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
            return ts.Text.Trim();
        }
    }
}
