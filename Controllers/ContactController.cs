using EC07_CMS_Umbraco_vs.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace EC07_CMS_Umbraco_vs.Controllers
{
    public class ContactController : SurfaceController
    {

        private readonly IUmbracoContextFactory _contextFactory;

        public ContactController(IUmbracoContextFactory contextFactory, IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _contextFactory = contextFactory;
        }

        [HttpPost]
        public IActionResult HandleContactForm(ContactFormViewModel contactForm)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            IPublishedContent? contactForms = null;

            using (var context = _contextFactory.EnsureUmbracoContext())
            {
                contactForms = context.UmbracoContext.Content.GetAtRoot().DescendantsOrSelfOfType("contactForms").FirstOrDefault();
            }

            if (contactForms != null)
            {
                var newContact = Services.ContentService.Create("Contact", contactForms.Id, "contactForm");
                newContact.SetValue("contactName", contactForm.Name);
                newContact.SetValue("contactEmail", contactForm.EmailAddress);
                newContact.SetValue("contactComment", contactForm.Comment);

                Services.ContentService.Save(newContact);
            }

            TempData["FormSuccess"] = "Message sent successfully (see umbraco backoffice Content section data/contact forms)";
            return RedirectToCurrentUmbracoPage();
        }
    }
}
