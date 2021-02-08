using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AppBAL.Sevices.Authentication;
using AppBAL.Sevices.Login;
using AppDAL.DBModels;
using AppDAL.DBRepository;
using AppModel;
using AppUtility.AppEncription;
using AppUtility.AppModels;
using AutoMapper;
using BTWebAppFrameWorkCore.AppSecurity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using GroupChat.SignalrHub;
using Microsoft.AspNetCore.Authorization;
using AppBAL.Sevices.Master;
using AppUtility.AppIO;
using BTWebAppFrameWorkCore.Services;
using AppBAL.Sevices.AppCore;
using AppUtility.AppMessage;
using AppBAL.Sevices;
using AppBAL.CoreJobService;

namespace BTWebAppFrameWorkCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;            
        }

        public IConfiguration Configuration { get; }
        private IJobManager _objJobMgr;        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            RegisterAppModel(services);
            RegisterAppServices(services);

            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddDbContext<AppDBContext>(options => options
                .UseMySql(Configuration["database:connection"],
                    mysqlOptions =>
                        mysqlOptions.ServerVersion(new ServerVersion(new Version(10, 4, 13), ServerType.MariaDb))));

            
            services.AddAuthentication(options =>
            {
                // the scheme name has to match the value we're going to use in AuthenticationBuilder.AddScheme(...)
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = "LMSAuthCookies";
                options.LoginPath = "/Account/Login/";
                options.AccessDeniedPath = "/AppError/UnAuthorised401/";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            })
            //.AddCustomCookieAuth(o => { })
            .AddCustomJWTAuth(o => { });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "AllowAuthUsers", policy =>
                    policy.Requirements.Add(
                          new ManageUserPermissionRequirement()));
            });


            services.AddControllersWithViews();
            // register the automapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSignalR();
            //services.AddMvc()
            //.AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.WriteIndented = true;
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IJobManager objJobMgr)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/AppError/UnhandleErrors");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStatusCodePagesWithReExecute("/AppError/HandleStatusCodeErrors/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Add authentication to request pipeline
            app.UseAuthentication();

            app.UseAuthorization();

            //app.UseAppRedirectMiddleware();
            //app.UseStatusCodePages(context =>
            //{
            //    if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            //    {                    
            //        context.HttpContext.Response.Redirect("/Account/Login");
            //    }
            //    return Task.CompletedTask;
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
                endpoints.MapHub<ChatHub>("/chathub");
            });

            _objJobMgr = objJobMgr;
            _objJobMgr.InitialiseJobs(); // initialise all schedule jobs
        }

        private void RegisterAppServices(IServiceCollection services)
        {

            #region Core Injection
            services.AddScoped<ISiteMapService, SiteMapService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAppCookiesAuthService, AppCookiesAuthService>();
            services.AddScoped<IEncriptionService, EncriptionService>();
            services.AddScoped<IAuthorizationHandler, CanAllowOnlyAuthUsersHandler>();
            services.AddScoped<IBaseControllerService, BaseControllerService>();            
            services.AddScoped<IAppJobService, AppJobService>();
            services.AddScoped<IMailTemplateService, MailTemplateService>();
            services.AddScoped<IJobManager, JobManager>();
            #endregion

            #region Register services
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IAppLogService, AppLogService>();
            services.AddScoped<IAppSettingService, AppSettingService>();
            services.AddScoped<IStudentService,StudentService>();
            services.AddScoped<IStandardMasterService, StandardMasterService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<IClassroomService, ClassroomService>();
            #endregion


            #region register DB repository
            services.AddScoped(typeof(ICommonRepository<>), typeof(CommonRepository<>));
            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<IAppActivityLogRepository, AppActivityLogRepository>();
            services.AddScoped<IAppSettingRepository, AppSettingRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IAppJobRepository, AppJobRepository>();
            services.AddScoped<IStandardMasterRepository, StandardMasterRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IClassroomRepository, ClassroomRepository>();
            #endregion

            #region Other Services
            services.AddScoped<IDirectoryFileService, DirectoryFileService>();
            
            #endregion
            //************************************
        }

        private void RegisterAppModel(IServiceCollection services)
        {
            var emailConfig = Configuration
            .GetSection("EmailConfiguration")
            .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            var appSettingConfig = Configuration
            .GetSection("AppSettings")
            .Get<AppSettingsConfiguration>();
            services.AddSingleton(appSettingConfig);

            //AppConfigSingletonRepository.UpdateConfiguration(appSettingConfig);
        }
    }
}
