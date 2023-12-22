namespace CrazyAboutPi.Server
{
    using System;
    using System.Collections.Generic;

    using Nancy;
    using Nancy.TinyIoc;
    using Nancy.Security;
    using Nancy.Bootstrapper;
    using Nancy.Authentication.Forms;

    using CoreLib;

    public class AuthenticatedUser : IUserIdentity
    {
        public string UserName { get; set; }
        public IEnumerable<string> Claims { get; set; }
    }

    public class DBUser : IUserMapper
    {
        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            User user = User.Get(identifier);

            if (user == null)
            {
                throw new ArgumentNullException();
            }

            return new AuthenticatedUser { UserName = user.Email, Claims = new string[] { } };
        }
    }

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
            container.Register<IUserMapper, DBUser>();
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            var formsAuthConfiguration = new FormsAuthenticationConfiguration
            {
                RedirectUrl = "~/login",
                UserMapper = container.Resolve<IUserMapper>(),
            };

            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
        }

        protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            pipelines.AfterRequest += (ctx) =>
            {
                ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            };
        }
    }
}