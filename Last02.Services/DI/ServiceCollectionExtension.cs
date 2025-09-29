using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Last02.Data.UnitOfWork;
using Last02.Services.Implement;
using Last02.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Last02.Services.DI
{
    public static class ServiceCollectionExtension
    {
        public static void AddServiceCollection(this IServiceCollection services, IConfiguration configuration, IHostBuilder host)
        {
            host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            host.ConfigureContainer<ContainerBuilder>((context, builder) =>
            {
                builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
                builder.RegisterType<AuthService>().As<IAuthService>();
                builder.RegisterType<UserService>().As<IUserService>();
                builder.RegisterType<CourseService>().As<ICourseService>();
                builder.RegisterType<LocalizedMessageService>().As<ILocalizedMessageService>();
                builder.RegisterType<StorageService>().As<IStorageService>();
                builder.RegisterType<PasswordService>().As<IPasswordService>().InstancePerLifetimeScope();
                builder.RegisterType<FlashcardService>().As<IFlashcardService>();
                builder.RegisterType<TopicService>().As<ITopicService>();
                builder.RegisterType<MemberService>().As<IMemberService>();
                builder.RegisterType<CloudinaryService>().AsSelf().SingleInstance();
                builder.RegisterType<MailService>().As<IMailService>();
                builder.RegisterType<GrammarService>().As<IGrammarService>();
                builder.RegisterType<ConversationService>().As<IConversationService>();
                builder.RegisterType<AudioService>().As<IAudioService>();
            });
        }
    }
}
