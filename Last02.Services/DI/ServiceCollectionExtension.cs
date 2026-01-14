using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Azure.Storage;
using Azure.Storage.Blobs;
using Last02.Data.UnitOfWork;
using Last02.Services.Implement;
using Last02.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                builder.RegisterType<S3StorageService>().As<IStorageService>();
                builder.RegisterType<PasswordService>().As<IPasswordService>().InstancePerLifetimeScope();
                builder.RegisterType<FlashcardService>().As<IFlashcardService>();
                builder.RegisterType<TopicService>().As<ITopicService>();
                builder.RegisterType<MemberService>().As<IMemberService>();
                builder.RegisterType<CloudinaryService>().AsSelf().SingleInstance();
                builder.RegisterType<MailService>().As<IMailService>();
                builder.RegisterType<GrammarService>().As<IGrammarService>();
                builder.RegisterType<ConversationService>().As<IConversationService>();
                builder.RegisterType<AudioService>().As<IAudioService>();
                builder.RegisterType<FlashcardUpdateHistoryService>().As<IFlashcardUpdateHistoryService>().InstancePerLifetimeScope();
                builder.RegisterType<AudioGenHistoryService>().As<IAudioGenHistoryService>().InstancePerLifetimeScope();
                builder.Register(ctx =>
                {
                    var cfg = ctx.Resolve<IConfiguration>();

                    var regionName = cfg["AWS:Region"] ?? "ap-southeast-1";
                    var region = RegionEndpoint.GetBySystemName(regionName);

                    var accessKey = cfg["AWS:AccessKey"];
                    var secretKey = cfg["AWS:SecretKey"];

                    if (!string.IsNullOrWhiteSpace(accessKey) && !string.IsNullOrWhiteSpace(secretKey))
                    {
                        var creds = new BasicAWSCredentials(accessKey, secretKey);
                        return new AmazonS3Client(creds, region);
                    }

                    // Fallback: Default Credential Chain (IAM role, env, profile...)
                    return new AmazonS3Client(region);
                })
                .As<IAmazonS3>()
                .SingleInstance();

                // 2) Register storage service mới
                builder.RegisterType<S3StorageService>()
                       .As<IStorageService>()
                       .InstancePerLifetimeScope();
            });
        }
    }
}
