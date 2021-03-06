using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using AutoMapper;
using Chino.AutoMapper;
using Chino.EntityFramework.Shared.Data;
using Chino.EntityFramework.Shared.Entities.User;
using Chino.IdentityServer.Configures;
using Chino.IdentityServer.Const;
using Chino.IdentityServer.Resources.DataAnnotation;
using Chino.IdentityServer.Services;
using Chino.IdentityServer.Services.Account;
using Chino.IdentityServer.Services.ApiResources;
using Chino.IdentityServer.Services.ApiScopes;
using Chino.IdentityServer.Services.Clients;
using Chino.IdentityServer.Services.IdentityResources;
using Chino.IdentityServer.Services.Localization;
using Chino.IdentityServer.Services.Oidc;
using Chino.IdentityServer.Services.Roles;
using Chino.IdentityServer.Services.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Chino.IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ChinoAccountConfiguration chinoAccountConfiguration;
            services.AddChinoConfigurations(Configuration, out chinoAccountConfiguration);


            #region I18N
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("ja"),
                    new CultureInfo("en"),
                    new CultureInfo("zh"),
                    new CultureInfo("zh-CN"),
                    new CultureInfo("en-US"),
                    new CultureInfo("ja-JP"),

                };

                var supportedUICultures = new List<CultureInfo>
                {
                    //new CultureInfo("ja"),
                    new CultureInfo("en"),
                    new CultureInfo("zh"),
                    new CultureInfo("zh-CN"),
                    new CultureInfo("en-US"),
                    new CultureInfo("ja-JP"),
                };

                options.DefaultRequestCulture = new RequestCulture("zh-CN");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedUICultures;
            });

            #endregion

            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/Dashboard", ChinoConst.PolicyName_Dashboard);
                options.Conventions.AuthorizeFolder("/User");
            })
                .AddViewLocalization()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName = new AssemblyName(typeof(DataAnnotationResources).GetTypeInfo().Assembly.FullName);
                        return factory.Create($"DataAnnotation.{nameof(DataAnnotationResources)}", assemblyName.Name);
                    };

                });
                //.AddRazorRuntimeCompilation();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Chino.IdentityServer", Version = "v1" });
            });


            services.AddChinoDatabase<ChinoApplicationDbContext>(this.Configuration);

            services.AddIdentity<ChinoUser, IdentityRole>(options =>
            {
                options.ApplyIdentityOptionsByChiniAccountConfiguration(chinoAccountConfiguration);
                Configuration.GetSection("Chino:IdentityOptions").Bind(options);
            })
                .AddEntityFrameworkStores<ChinoApplicationDbContext>()
                .AddDefaultTokenProviders();

            //services.AddSession(options =>
            //{
            //});

            #region IdentityServer
            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                options.EmitStaticAudienceClaim = true;

                options.UserInteraction = new IdentityServer4.Configuration.UserInteractionOptions
                {
                    LoginUrl = "/Account/Login",
                    LogoutUrl = "/Account/Logout",
                    LoginReturnUrlParameter = "returnUrl",
                    ErrorUrl = "~/Error/IdentityService",
                    ErrorIdParameter = "Id"
                };
            })
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.AddIdentityServerConfigurationDatabase(this.Configuration);
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.AddIdentityServerOperationalDatabase(this.Configuration);

                    options.EnableTokenCleanup = true;
                })
                .AddProfileService<ChinoProfileService>() //这个服务用来给IdentityServer获取Chino的用户Claims
                .AddAspNetIdentity<ChinoUser>();

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }


            #endregion
            services.AddAuthentication()
                .AddExternalAuthProviders(this.Configuration);
            services.AddAuthorization(options =>
            {
                options.AddPolicy(ChinoConst.PolicyName_Dashboard, policy =>
                {
                    policy.RequireRole(Configuration["Chino:AdminRoleName"]);
                });
            });


            //AutoMapper
            services.AddAutoMapper(typeof(AutoMapperProfile));

            //内存缓存
            services.AddMemoryCache();

            //IUrlHelper
            //services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            //services.AddScoped<IUrlHelper>(factory =>
            //{
            //    var actionContext = factory.GetService<IActionContextAccessor>().ActionContext;
            //    return new UrlHelper(actionContext);
            //});


            //Chino Services
            services.AddSingleton<CommonLocalizationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IApiResourceService, ApiResourceService>();
            services.AddScoped<IApiScopeService, ApiScopeService>();
            services.AddScoped<IIdentityResouceService, IdentityResourceService>();
            services.AddScoped<IUserAvatarService, UserAvatarService>();
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<IJsonLocalizationService, JsonLocalizationService>();

            services.AddSMSService(Configuration); //短信服务

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Chino.IdentityServer v1"));
            }

            if (Configuration.GetValue<bool>("Chino:EnableForwardedHeaders"))
            {
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor| ForwardedHeaders.XForwardedProto
                });
            }

            app.UseHttpsRedirection();

            app.UseRequestLocalization();

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseStaticFiles();

            app.UseRequestLocalization();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
 