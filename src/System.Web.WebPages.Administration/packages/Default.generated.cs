﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace System.Web.WebPages.Administration.PackageManager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using System.Web.WebPages.Html;
    using System.Globalization;
    using NuGet;
    
    [System.Web.WebPages.PageVirtualPathAttribute("~/packages/Default.cshtml")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorSingleFileGenerator", "0.6.0.0")]
    public class packages_Default_cshtml : System.Web.WebPages.WebPage
    {
#line hidden

    private enum View { Installed, Updates, Online };
    private static readonly IDictionary<string, View> _viewMapping = new Dictionary<string, View>(StringComparer.OrdinalIgnoreCase) { 
        { "Installed", View.Installed }, { "Updates", View.Updates }, { "Online", View.Online } 
    };

    private static View GetView(string viewName) {
        View view;
        if (String.IsNullOrEmpty(viewName) || !_viewMapping.TryGetValue(viewName, out view)) {
            view = View.Installed;
        }
        return view;
    }

    private static string GetViewName(View view) {
        switch (view) {
            case View.Online:
                return PackageManagerResources.OnlineLabel;
            case View.Updates:
                return PackageManagerResources.UpdatesLabel;
            default:
                return PackageManagerResources.InstalledLabel;
        }
    }

    private static string GetSubmitText(View view, IPackage package, WebProjectManager projectManager) {
        switch (view) {
            case View.Online:
                return projectManager.IsPackageInstalled(package) ? PackageManagerResources.UninstallPackage : PackageManagerResources.InstallPackage;
            case View.Updates:
                return PackageManagerResources.UpdatePackage;
            default:
                return PackageManagerResources.UninstallPackage;
        }
    }

    private static string GetPostUrl(View view, IPackage package, WebProjectManager projectManager) {
        switch (view) {
            case View.Online:
                return projectManager.IsPackageInstalled(package) ? "Uninstall" : "Install";
            case View.Updates:
                return "Update";
            default:
                return "Uninstall";
        }
    }

                    // Resolve package relative syntax
                    // Also, if it comes from a static embedded resource, change the path accordingly
                    public override string Href(string virtualPath, params object[] pathParts) {
                        virtualPath = ApplicationPart.ProcessVirtualPath(GetType().Assembly, VirtualPath, virtualPath);
                        return base.Href(virtualPath, pathParts);
                    }
        public packages_Default_cshtml()
        {
        }
        protected System.Web.HttpApplication ApplicationInstance
        {
            get
            {
                return ((System.Web.HttpApplication)(Context.ApplicationInstance));
            }
        }
        public override void Execute()
        {


WriteLiteral("\r\n\r\n");



WriteLiteral("\r\n");


DefineSection("PackageHead", () => {

WriteLiteral("\r\n    <script type=\"text/javascript\" src=\"");


                                   Write(Href("scripts/Default.js"));

WriteLiteral("\"></script>\r\n    <noscript>");


         Write(PackageManagerResources.JavascriptRequired);

WriteLiteral("</noscript>\r\n");


});

WriteLiteral("\r\n");


   
    Page.SectionTitle = PackageManagerResources.SectionTitle;
    // Page Constants
    const int PackagesPerPage = 10;
    const string FilterCookieName = "packagesFilter";

    // Request parameters
    var view = GetView(PageUtils.GetFilterValue(Request, FilterCookieName, "view"));
    var searchTerm = Request.QueryString["search"];
    var packageSourceName = PageUtils.GetFilterValue(Request, FilterCookieName, "source");

    var packageSource = PageUtils.GetPackageSource(packageSourceName);

    PageUtils.PersistFilter(Response, FilterCookieName, new Dictionary<string, string> { 
                { "view", view.ToString() }, 
                { "source", packageSourceName },
    });

    // Add values to ModelState
    ModelState.SetModelValue("view", view.ToString());

    WebProjectManager projectManager;
    WebGrid grid;
    int totalPackages = 0;

    try {
        // This entire block needs ot be inside a try catch block. This is necessary because exceptions could be fired at two places 
        // 1. When trying to connect to the remote repository. 2. When executing the Linq expression 

        IEnumerable<IPackage> packages;

        projectManager = new WebProjectManager(packageSource.Source, PackageManagerModule.SiteRoot);
        // Read packages
        switch (view) {
            case View.Updates:
                packages = projectManager.GetPackagesWithUpdates(searchTerm, packageSource.FilterPreferredPackages);
                break;
            case View.Online:
                IQueryable<IPackage> remotePackages = projectManager.GetRemotePackages(searchTerm, packageSource.FilterPreferredPackages);
                totalPackages = remotePackages.Count();
                packages = WebProjectManager.CollapseVersions(remotePackages);
                break;
            default:
                packages = projectManager.GetInstalledPackages(searchTerm);
                break;
        }
        if (view != View.Online) {
            totalPackages = packages.Count();
        }
        
        grid = new WebGrid(rowsPerPage: PackagesPerPage, pageFieldName: "page");

        packages = packages.Skip(grid.PageIndex * PackagesPerPage)
                           .Take(PackagesPerPage);

        grid.Bind(packages.ToList(), columnNames: Enumerable.Empty<string>(), autoSortAndPage: false, rowCount: totalPackages);

    }
    catch (Exception exception) {

WriteLiteral("        <div class=\"error message\">");


                              Write(exception.Message);

WriteLiteral("</div>\r\n");


        return;
    }


WriteLiteral("\r\n");


  
    var completedAction = Request.QueryString["action-completed"];
    if (!completedAction.IsEmpty()) {
        var packageName = Html.Encode(Request.QueryString["packageName"]);
        string message = null;
        if (completedAction.Equals("Install", StringComparison.OrdinalIgnoreCase)) {
            message = String.Format(CultureInfo.CurrentCulture, PackageManagerResources.InstallSuccess, packageName);
        }
        else if (completedAction.Equals("Uninstall", StringComparison.OrdinalIgnoreCase)) {
            message = String.Format(CultureInfo.CurrentCulture, PackageManagerResources.UninstallSuccess, packageName);
        }
        else if (completedAction.Equals("Update", StringComparison.OrdinalIgnoreCase)) {
            message = String.Format(CultureInfo.CurrentCulture, PackageManagerResources.UpdateSuccess, packageName);
        }

        if (message != null) { 

WriteLiteral("            <div class=\"success message\">\r\n                <img class=\"inline\" sr" +
"c=\"");


                                    Write(Href(SiteAdmin.GetVirtualPath("~/images/ok.png")));

WriteLiteral("\" alt=\"");


                                                                                             Write(Html.Raw(message));

WriteLiteral("\" />&nbsp;");


                                                                                                                         Write(Html.Raw(message));

WriteLiteral("\r\n            </div>\r\n");



WriteLiteral("            <br />\r\n");


        }
    }


WriteLiteral("<form method=\"get\" action=\"\" class=\"group\">\r\n    <div class=\"left form-actions\">\r" +
"\n        <label>");


          Write(PackageManagerResources.ShowLabel);

WriteLiteral(":\r\n        ");


   Write(Html.DropDownList("view", from v in new[] { View.Installed, View.Online, View.Updates }
                                   select new SelectListItem { Text = GetViewName(v), Value = v.ToString() }));

WriteLiteral("\r\n        </label>                           \r\n   \r\n");


         if (PackageManagerModule.PackageSources.Count() > 1) {


WriteLiteral("            <span ");

                   if (view == View.Installed) { 
                                            Write(Html.Raw("style=\"display: none\""));

                                                                                     ;
                  }
WriteLiteral(">\r\n                &nbsp;\r\n                <label>");


                  Write(PackageManagerResources.PackageSourceLabel);

WriteLiteral(":</label>\r\n                ");


           Write(Html.DropDownList("source", from f in PackageManagerModule.PackageSources.OrderBy(p => p.Name)
                                             select new SelectListItem {
                                                 Value = f.Name,
                                                 Text = f.Name,
                                                 Selected = f.Name.Equals(packageSourceName, StringComparison.OrdinalIgnoreCase)
                                             }
                                   ));

WriteLiteral("\r\n            </span>\r\n");


        }

WriteLiteral("    </div>\r\n\r\n    <div class=\"right\">\r\n        <fieldset class=\"no-border\">\r\n    " +
"        <input type=\"text\" id=\"search\" name=\"search\" size=\"30\" value=\"");


                                                                     Write(searchTerm);

WriteLiteral("\" />\r\n            <input type=\"submit\" value=\"");


                                   Write(PackageManagerResources.SearchLabel);

WriteLiteral("\" />\r\n            <input type=\"reset\" id=\"searchReset\" value=\"");


                                                   Write(PackageManagerResources.ClearLabel);

WriteLiteral("\" />\r\n        </fieldset>\r\n    </div>\r\n</form>\r\n\r\n");


 if (view != View.Online && !projectManager.LocalRepository.GetPackages().Any()) {
    var onlineLink = Href(PageUtils.GetPackagesHome()) + "?view=" + View.Online;
    
Write(Html.Raw(String.Format(CultureInfo.CurrentCulture, PackageManagerResources.NoPackagesInstalled, Html.Encode(onlineLink))));

                                                                                                                              
    return;
}

WriteLiteral("\r\n");


 if (!grid.Rows.Any()) {

WriteLiteral("    <h3>");


   Write(PackageManagerResources.NoPackagesFound);

WriteLiteral("</h3>\r\n");


}
else {

WriteLiteral("    <ul id=\"package-list\">\r\n");


       var dataDictionary = new Dictionary<string, object>(1); 


     foreach (var item in grid.Rows) {
        IPackage package = item.Value;
        dataDictionary["package"] = package;

WriteLiteral("        <li>\r\n            <div class=\"column-left\">\r\n                ");


           Write(RenderPage("_Package.cshtml", dataDictionary));

WriteLiteral("\r\n            </div>\r\n            <div class=\"right\">\r\n                <form meth" +
"od=\"get\" action=\"");


                                      Write(Href(GetPostUrl(view, package, projectManager)));

WriteLiteral("\">\r\n                    <input type=\"hidden\" name=\"page\" value=\"");


                                                        Write(grid.PageIndex + 1);

WriteLiteral("\" />\r\n                    <input type=\"hidden\" name=\"package\" value=\"");


                                                          Write(package.Id);

WriteLiteral("\" />\r\n                    <input type=\"hidden\" name=\"version\" value=\"");


                                                          Write(package.Version);

WriteLiteral("\" />\r\n                    <input type=\"hidden\" name=\"packageName\" value=\"");


                                                              Write(package.GetDisplayName());

WriteLiteral("\" />\r\n                    <input class=\"formatted\" type=\"submit\" value=\"");


                                                             Write(GetSubmitText(view, package, projectManager));

WriteLiteral("\" />\r\n                </form>\r\n            </div>\r\n            <div class=\"clear\"" +
"></div>\r\n        </li>\r\n");


    }

WriteLiteral("    </ul>\r\n");


}


 if (totalPackages > PackagesPerPage) {

WriteLiteral("    <div class=\"pager\">\r\n        <strong>");


           Write(PackageManagerResources.PageLabel);

WriteLiteral(": </strong>\r\n        ");


   Write(grid.Pager(WebGridPagerModes.FirstLast | WebGridPagerModes.NextPrevious, 
            nextText: PackageManagerResources.NextText,
            previousText: PackageManagerResources.PreviousText));

WriteLiteral("\r\n    </div>\r\n");


}

WriteLiteral("\r\n\r\n");



        }
    }
}
#pragma warning restore 1591