using EC07_CMS_Umbraco_vs.Contexts;
using EC07_CMS_Umbraco_vs.Models;
using EC07_CMS_Umbraco_vs.ViewModels;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class SubscriptionController : SurfaceController
    {
        private readonly IUmbracoContextFactory _contextFactory;
        private readonly DataContext _dataContext;
        public SubscriptionController(DataContext dataContext, IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider, IUmbracoContextFactory contextFactory) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _dataContext = dataContext;
            _contextFactory = contextFactory;
        }

        /// <summary>
        /// Checks if input is already in database. If not -> Add and save DB. and also add to Umbraco backoffice.
        /// </summary>
        /// <param name="subscribeForm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Subscribe(SubscribeViewModel subscribeForm)
        {
            var currentUrl = Request.GetDisplayUrl();

            if (ModelState.IsValid)
            {
                // Check if already saved.
                var existingSubscriber = await _dataContext.Subscribers.FirstOrDefaultAsync(s => s.Email == subscribeForm.Email);
                if (existingSubscriber == null)
                {
                    var subscriber = new SubscribersEntity { Email = subscribeForm.Email };
                    _dataContext.Subscribers.Add(subscriber);
                    await _dataContext.SaveChangesAsync();

                    // Getting the Umbraco models
                    IPublishedContent? subscriberForms = null;
                    using (var context = _contextFactory.EnsureUmbracoContext())
                    {
                        subscriberForms = context.UmbracoContext.Content.GetAtRoot().DescendantsOrSelfOfType("subscribeForms").FirstOrDefault();
                    }
                    // Null check and save to Umbraco
                    if (subscriberForms != null)
                    {
                        var newSubscriber = Services.ContentService.Create("Subscriber", subscriberForms.Id, "subscribeForm");
                        newSubscriber.SetValue("subscriberEmail", subscribeForm.Email);

                        Services.ContentService.Save(newSubscriber);
                    }

                    TempData["SubscribeSuccess"] = "Thank you for subscribing! (see umbraco backoffice Content section data/subscribe forms)";
                }
                else
                {
                    TempData["SubscribeError"] = "You are already subscribed!";
                }

                return Redirect($"{currentUrl}#subscription-form");
            }
            TempData["SubscribeError"] = "Invalid email address!";
            return Redirect($"{currentUrl}#subscription-form");
        }
    }
}
