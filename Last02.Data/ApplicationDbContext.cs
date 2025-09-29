using Last02.Commons;
using Last02.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection.Emit;

namespace Last02.Data
{
    public class ApplicationDbContext : IdentityDbContext<Users, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Member> Members { get; set; } = default!;
        public DbSet<Claims> Claims { get; set; } = default!;
        public DbSet<Course> Course { get; set; }
        public DbSet<MemberCourse> MemberCourses { get; set; }
        public DbSet<LocalizedMessage> LocalizedMessages { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Audio> Audios { get; set; }
        public DbSet<Flashcard> Flashcards { get; set; }
        public DbSet<MemberFlashcard> MemberFlashcards { get; set; }
        public DbSet<UserDeviceToken> UserDeviceTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
            SeedCourse(builder);
            SeedTopic(builder);
            SeedFlashcardByCourseId(builder);
            SeedKaiwaByCourseId(builder);
            SeedGrammarByCourseId(builder);

            // Configure UserCourse relationship
            builder.Entity<MemberCourse>()
                .HasOne(mc => mc.Member)
                .WithMany(m => m.MemberCourses)
                .HasForeignKey(mc => mc.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MemberCourse>()
                .HasOne(uc => uc.Course)
                .WithMany(c => c.MemberCourses)
                .HasForeignKey(uc => uc.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure LocalizedMessage composite key and default value
            builder.Entity<LocalizedMessage>(entity => { entity.HasKey(e => new { e.Code, e.LanguageCode }); });

            // Remove prefix name of table
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (!string.IsNullOrEmpty(tableName) && tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName[6..]);
                }
            }
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int>() { Id = 1, Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "ADMIN" },
                new IdentityRole<int>() { Id = 2, Name = "User", ConcurrencyStamp = "2", NormalizedName = "USER" }
            );
        }

        private void SeedCourse(ModelBuilder builder)
        {
            builder.Entity<Course>().HasData(
                new Course
                {
                    Id = 1,
                    Title = "N5",
                    CreatedDate = DateTime.Now,
                    Deleted = false
                },
                new Course
                {
                    Id = 2,
                    Title = "N4",
                    CreatedDate = DateTime.Now,
                    Deleted = false
                },
                new Course
                {
                    Id = 3,
                    Title = "N3",
                    CreatedDate = DateTime.Now,
                    Deleted = false
                },
                new Course
                {
                    Id = 4,
                    Title = "N2",
                    CreatedDate = DateTime.Now,
                    Deleted = false
                },
                new Course
                {
                    Id = 5,
                    Title = "N1",
                    CreatedDate = DateTime.Now,
                    Deleted = false
                }
            );
        }

        private void SeedTopic(ModelBuilder builder)
        {
            builder.Entity<Topic>().HasData(
                new Topic
                {
                    Id = 1,
                    TopicCode = "N5-001",
                    Title = "Chào hỏi cơ bản",
                    Description = "Học các câu chào hỏi và giao tiếp cơ bản trong tiếng Nhật N5.",
                    HexColorCode = "#FFB74D",
                    IsFree = true,
                    CourseId = 1
                },
                new Topic
                {
                    Id = 2,
                    TopicCode = "N5-002",
                    Title = "Số đếm & Thời gian",
                    Description = "Học cách đếm số, nói giờ, ngày tháng trong tiếng Nhật N5.",
                    HexColorCode = "#4DB6AC",
                    IsFree = true,
                    CourseId = 1
                },
                new Topic
                {
                    Id = 3,
                    TopicCode = "N5-003",
                    Title = "Gia đình",
                    Description = "Từ vựng và mẫu câu liên quan đến gia đình trong tiếng Nhật N5.",
                    HexColorCode = "#64B5F6",
                    IsFree = false,
                    CourseId = 1
                },
                new Topic
                {
                    Id = 4,
                    TopicCode = "N5-004",
                    Title = "Trường học",
                    Description = "Từ vựng và mẫu câu sử dụng trong môi trường học tập.",
                    HexColorCode = "#BA68C8",
                    IsFree = false,
                    CourseId = 1
                },
                new Topic
                {
                    Id = 5,
                    TopicCode = "N5-005",
                    Title = "Mua sắm",
                    Description = "Từ vựng và mẫu câu giao tiếp khi đi mua sắm.",
                    HexColorCode = "#E57373",
                    IsFree = false,
                    CourseId = 1
                }
            );
        }

        private void SeedFlashcardByCourseId(ModelBuilder builder)
        {
            builder.Entity<Flashcard>().HasData(
                new Flashcard
                {
                    Id = 1,
                    Front = "こんにちは",
                    Furigana = "こんにちわ",
                    FlashcardCode = "N5-001-001",
                    MeaningVi = "Xin chào",
                    MeaningEn = "Hello",
                    Example = "こんにちは、はじめまして。",
                    ExampleVi = "Xin chào, rất vui được gặp bạn.",
                    ExampleEn = "Hello, nice to meet you.",
                    ImageUrl = "/images/flashcards/konnichiwa.jpg",
                    TopicId = 1
                },
                new Flashcard
                {
                    Id = 2,
                    Front = "おはよう",
                    Furigana = "おはよう",
                    FlashcardCode = "N5-001-002",
                    MeaningVi = "Chào buổi sáng",
                    MeaningEn = "Good morning",
                    Example = "おはよう、きょうもがんばってね。",
                    ExampleVi = "Chào buổi sáng, hôm nay cùng cố gắng nhé.",
                    ExampleEn = "Good morning, do your best today too.",
                    ImageUrl = "/images/flashcards/ohayou.jpg",
                    TopicId = 1
                },
                new Flashcard
                {
                    Id = 3,
                    Front = "こんばんは",
                    Furigana = "こんばんは",
                    FlashcardCode = "N5-001-003",
                    MeaningVi = "Chào buổi tối",
                    MeaningEn = "Good evening",
                    Example = "こんばんは、いい天気ですね。",
                    ExampleVi = "Chào buổi tối, thời tiết thật đẹp nhỉ.",
                    ExampleEn = "Good evening, the weather is nice, isn’t it?",
                    ImageUrl = "/images/flashcards/konbanwa.jpg",
                    TopicId = 1
                },
                new Flashcard
                {
                    Id = 4,
                    Front = "さようなら",
                    Furigana = "さようなら",
                    FlashcardCode = "N5-001-004",
                    MeaningVi = "Tạm biệt",
                    MeaningEn = "Goodbye",
                    Example = "さようなら、またあした。",
                    ExampleVi = "Tạm biệt, hẹn gặp lại ngày mai.",
                    ExampleEn = "Goodbye, see you tomorrow.",
                    ImageUrl = "/images/flashcards/sayounara.jpg",
                    TopicId = 1
                },
                new Flashcard
                {
                    Id = 5,
                    Front = "ありがとう",
                    Furigana = "ありがとう",
                    FlashcardCode = "N5-001-005",
                    MeaningVi = "Cảm ơn",
                    MeaningEn = "Thank you",
                    Example = "プレゼントをありがとう。",
                    ExampleVi = "Cảm ơn vì món quà.",
                    ExampleEn = "Thank you for the present.",
                    ImageUrl = "/images/flashcards/arigatou.jpg",
                    TopicId = 1
                },
                new Flashcard
                {
                    Id = 6,
                    Front = "すみません",
                    Furigana = "すみません",
                    FlashcardCode = "N5-001-006",
                    MeaningVi = "Xin lỗi / Làm phiền",
                    MeaningEn = "Excuse me / Sorry",
                    Example = "すみません、もういちどおねがいします。",
                    ExampleVi = "Xin lỗi, vui lòng nhắc lại một lần nữa.",
                    ExampleEn = "Excuse me, please say it once again.",
                    ImageUrl = "/images/flashcards/sumimasen.jpg",
                    TopicId = 1
                },
                new Flashcard
                {
                    Id = 7,
                    Front = "はい",
                    Furigana = "はい",
                    FlashcardCode = "N5-001-007",
                    MeaningVi = "Vâng / Đúng",
                    MeaningEn = "Yes / Correct",
                    Example = "はい、わかりました。",
                    ExampleVi = "Vâng, tôi đã hiểu.",
                    ExampleEn = "Yes, I understand.",
                    ImageUrl = "/images/flashcards/hai.jpg",
                    TopicId = 1
                },
                new Flashcard
                {
                    Id = 8,
                    Front = "いいえ",
                    Furigana = "いいえ",
                    FlashcardCode = "N5-001-008",
                    MeaningVi = "Không",
                    MeaningEn = "No",
                    Example = "いいえ、ちがいます。",
                    ExampleVi = "Không, không phải vậy.",
                    ExampleEn = "No, that’s not right.",
                    ImageUrl = "/images/flashcards/iie.jpg",
                    TopicId = 1
                },
                new Flashcard
                {
                    Id = 9,
                    Front = "はじめまして",
                    Furigana = "はじめまして",
                    FlashcardCode = "N5-001-009",
                    MeaningVi = "Rất vui được gặp bạn",
                    MeaningEn = "Nice to meet you",
                    Example = "はじめまして、やまだです。",
                    ExampleVi = "Rất vui được gặp bạn, tôi là Yamada.",
                    ExampleEn = "Nice to meet you, I am Yamada.",
                    ImageUrl = "/images/flashcards/hajimemashite.jpg",
                    TopicId = 1
                },
                new Flashcard
                {
                    Id = 10,
                    Front = "おやすみなさい",
                    Furigana = "おやすみなさい",
                    FlashcardCode = "N5-001-010",
                    MeaningVi = "Chúc ngủ ngon",
                    MeaningEn = "Good night",
                    Example = "おやすみなさい、またあした。",
                    ExampleVi = "Chúc ngủ ngon, hẹn gặp lại ngày mai.",
                    ExampleEn = "Good night, see you tomorrow.",
                    ImageUrl = "/images/flashcards/oyasuminasai.jpg",
                    TopicId = 1
                },
                new Flashcard
                {
                    Id = 11,
                    Front = "いち",
                    Furigana = "いち",
                    FlashcardCode = "N5-002-001",
                    MeaningVi = "Số một",
                    MeaningEn = "One",
                    Example = "りんごをいちつください。",
                    ExampleVi = "Xin cho tôi một quả táo.",
                    ExampleEn = "Please give me one apple.",
                    ImageUrl = "/images/flashcards/ichi.jpg",
                    TopicId = 2
                },
                new Flashcard
                {
                    Id = 12,
                    Front = "に",
                    Furigana = "に",
                    FlashcardCode = "N5-002-002",
                    MeaningVi = "Số hai",
                    MeaningEn = "Two",
                    Example = "にほんごをべんきょうしています。",
                    ExampleVi = "Tôi đang học tiếng Nhật.",
                    ExampleEn = "I am studying Japanese.",
                    ImageUrl = "/images/flashcards/ni.jpg",
                    TopicId = 2
                },
                new Flashcard
                {
                    Id = 13,
                    Front = "さん",
                    Furigana = "さん",
                    FlashcardCode = "N5-002-003",
                    MeaningVi = "Số ba",
                    MeaningEn = "Three",
                    Example = "さんにんのともだちがいます。",
                    ExampleVi = "Tôi có ba người bạn.",
                    ExampleEn = "I have three friends.",
                    ImageUrl = "/images/flashcards/san.jpg",
                    TopicId = 2
                },
                new Flashcard
                {
                    Id = 14,
                    Front = "よん / し",
                    Furigana = "よん / し",
                    FlashcardCode = "N5-002-004",
                    MeaningVi = "Số bốn",
                    MeaningEn = "Four",
                    Example = "よんじにあいましょう。",
                    ExampleVi = "Hãy gặp nhau lúc bốn giờ.",
                    ExampleEn = "Let’s meet at four o’clock.",
                    ImageUrl = "/images/flashcards/yon.jpg",
                    TopicId = 2
                },
                new Flashcard
                {
                    Id = 15,
                    Front = "ご",
                    Furigana = "ご",
                    FlashcardCode = "N5-002-005",
                    MeaningVi = "Số năm",
                    MeaningEn = "Five",
                    Example = "ごじにおきます。",
                    ExampleVi = "Tôi thức dậy lúc năm giờ.",
                    ExampleEn = "I wake up at five o’clock.",
                    ImageUrl = "/images/flashcards/go.jpg",
                    TopicId = 2
                },
                new Flashcard
                {
                    Id = 16,
                    Front = "ろく",
                    Furigana = "ろく",
                    FlashcardCode = "N5-002-006",
                    MeaningVi = "Số sáu",
                    MeaningEn = "Six",
                    Example = "ろっぷんまってください。",
                    ExampleVi = "Xin hãy chờ sáu phút.",
                    ExampleEn = "Please wait for six minutes.",
                    ImageUrl = "/images/flashcards/roku.jpg",
                    TopicId = 2
                },
                new Flashcard
                {
                    Id = 17,
                    Front = "しち / なな",
                    Furigana = "しち / なな",
                    FlashcardCode = "N5-002-007",
                    MeaningVi = "Số bảy",
                    MeaningEn = "Seven",
                    Example = "ななじはんにおきます。",
                    ExampleVi = "Tôi dậy lúc bảy giờ rưỡi.",
                    ExampleEn = "I get up at half past seven.",
                    ImageUrl = "/images/flashcards/nana.jpg",
                    TopicId = 2
                },
                new Flashcard
                {
                    Id = 18,
                    Front = "はち",
                    Furigana = "はち",
                    FlashcardCode = "N5-002-008",
                    MeaningVi = "Số tám",
                    MeaningEn = "Eight",
                    Example = "はちじにでかけます。",
                    ExampleVi = "Tôi ra ngoài lúc tám giờ.",
                    ExampleEn = "I go out at eight o’clock.",
                    ImageUrl = "/images/flashcards/hachi.jpg",
                    TopicId = 2
                },
                new Flashcard
                {
                    Id = 19,
                    Front = "きゅう / く",
                    Furigana = "きゅう / く",
                    FlashcardCode = "N5-002-009",
                    MeaningVi = "Số chín",
                    MeaningEn = "Nine",
                    Example = "きゅうじまでにきてください。",
                    ExampleVi = "Hãy đến trước chín giờ.",
                    ExampleEn = "Please come by nine o’clock.",
                    ImageUrl = "/images/flashcards/kyuu.jpg",
                    TopicId = 2
                },
                new Flashcard
                {
                    Id = 20,
                    Front = "じゅう",
                    Furigana = "じゅう",
                    FlashcardCode = "N5-002-010",
                    MeaningVi = "Số mười",
                    MeaningEn = "Ten",
                    Example = "じゅうにんのせいとがいます。",
                    ExampleVi = "Có mười học sinh.",
                    ExampleEn = "There are ten students.",
                    ImageUrl = "/images/flashcards/juu.jpg",
                    TopicId = 2
                },
                new Flashcard
                {
                    Id = 21,
                    Front = "かぞく",
                    Furigana = "かぞく",
                    FlashcardCode = "N5-003-001",
                    MeaningVi = "Gia đình",
                    MeaningEn = "Family",
                    Example = "わたしのかぞくはごにんです。",
                    ExampleVi = "Gia đình tôi có năm người.",
                    ExampleEn = "My family has five people.",
                    ImageUrl = "/images/flashcards/kazoku.jpg",
                    TopicId = 3
                },
                new Flashcard
                {
                    Id = 22,
                    Front = "ちち",
                    Furigana = "ちち",
                    FlashcardCode = "N5-003-002",
                    MeaningVi = "Cha (nói về cha mình)",
                    MeaningEn = "Father (talking about one's own)",
                    Example = "ちちはぎんこうではたらいています。",
                    ExampleVi = "Cha tôi làm việc ở ngân hàng.",
                    ExampleEn = "My father works at a bank.",
                    ImageUrl = "/images/flashcards/chichi.jpg",
                    TopicId = 3
                },
                new Flashcard
                {
                    Id = 23,
                    Front = "はは",
                    Furigana = "はは",
                    FlashcardCode = "N5-003-003",
                    MeaningVi = "Mẹ (nói về mẹ mình)",
                    MeaningEn = "Mother (talking about one's own)",
                    Example = "はははりょうりがじょうずです。",
                    ExampleVi = "Mẹ tôi nấu ăn rất giỏi.",
                    ExampleEn = "My mother is good at cooking.",
                    ImageUrl = "/images/flashcards/haha.jpg",
                    TopicId = 3
                },
                new Flashcard
                {
                    Id = 24,
                    Front = "おとうさん",
                    Furigana = "おとうさん",
                    FlashcardCode = "N5-003-004",
                    MeaningVi = "Bố (cách gọi lịch sự)",
                    MeaningEn = "Father (polite)",
                    Example = "おとうさんはどこですか。",
                    ExampleVi = "Bố bạn ở đâu?",
                    ExampleEn = "Where is your father?",
                    ImageUrl = "/images/flashcards/otousan.jpg",
                    TopicId = 3
                },
                new Flashcard
                {
                    Id = 25,
                    Front = "おかあさん",
                    Furigana = "おかあさん",
                    FlashcardCode = "N5-003-005",
                    MeaningVi = "Mẹ (cách gọi lịch sự)",
                    MeaningEn = "Mother (polite)",
                    Example = "おかあさんはげんきですか。",
                    ExampleVi = "Mẹ bạn có khỏe không?",
                    ExampleEn = "How is your mother?",
                    ImageUrl = "/images/flashcards/okaasan.jpg",
                    TopicId = 3
                },
                new Flashcard
                {
                    Id = 26,
                    Front = "あに",
                    Furigana = "あに",
                    FlashcardCode = "N5-003-006",
                    MeaningVi = "Anh trai (nói về anh mình)",
                    MeaningEn = "Older brother (talking about one's own)",
                    Example = "あにはだいがくせいです。",
                    ExampleVi = "Anh trai tôi là sinh viên đại học.",
                    ExampleEn = "My older brother is a university student.",
                    ImageUrl = "/images/flashcards/ani.jpg",
                    TopicId = 3
                },
                new Flashcard
                {
                    Id = 27,
                    Front = "あね",
                    Furigana = "あね",
                    FlashcardCode = "N5-003-007",
                    MeaningVi = "Chị gái (nói về chị mình)",
                    MeaningEn = "Older sister (talking about one's own)",
                    Example = "あねはせんせいです。",
                    ExampleVi = "Chị gái tôi là giáo viên.",
                    ExampleEn = "My older sister is a teacher.",
                    ImageUrl = "/images/flashcards/ane.jpg",
                    TopicId = 3
                },
                new Flashcard
                {
                    Id = 28,
                    Front = "おにいさん",
                    Furigana = "おにいさん",
                    FlashcardCode = "N5-003-008",
                    MeaningVi = "Anh trai (lịch sự)",
                    MeaningEn = "Older brother (polite)",
                    Example = "おにいさんはなんさいですか。",
                    ExampleVi = "Anh trai bạn bao nhiêu tuổi?",
                    ExampleEn = "How old is your older brother?",
                    ImageUrl = "/images/flashcards/oniisan.jpg",
                    TopicId = 3
                },
                new Flashcard
                {
                    Id = 29,
                    Front = "おねえさん",
                    Furigana = "おねえさん",
                    FlashcardCode = "N5-003-009",
                    MeaningVi = "Chị gái (lịch sự)",
                    MeaningEn = "Older sister (polite)",
                    Example = "おねえさんはべんきょうしていますか。",
                    ExampleVi = "Chị gái bạn có đang học không?",
                    ExampleEn = "Is your older sister studying?",
                    ImageUrl = "/images/flashcards/oneesan.jpg",
                    TopicId = 3
                },
                new Flashcard
                {
                    Id = 30,
                    Front = "きょうだい",
                    Furigana = "きょうだい",
                    FlashcardCode = "N5-003-010",
                    MeaningVi = "Anh chị em",
                    MeaningEn = "Siblings",
                    Example = "きょうだいはなんにんいますか。",
                    ExampleVi = "Bạn có mấy anh chị em?",
                    ExampleEn = "How many siblings do you have?",
                    ImageUrl = "/images/flashcards/kyoudai.jpg",
                    TopicId = 3
                },
                new Flashcard
                {
                    Id = 31,
                    Front = "がっこう",
                    Furigana = "がっこう",
                    FlashcardCode = "N5-004-001",
                    MeaningVi = "Trường học",
                    MeaningEn = "School",
                    Example = "がっこうへいきます。",
                    ExampleVi = "Tôi đi đến trường.",
                    ExampleEn = "I go to school.",
                    ImageUrl = "/images/flashcards/gakkou.jpg",
                    TopicId = 4
                },
                new Flashcard
                {
                    Id = 32,
                    Front = "せんせい",
                    Furigana = "せんせい",
                    FlashcardCode = "N5-004-002",
                    MeaningVi = "Giáo viên",
                    MeaningEn = "Teacher",
                    Example = "せんせいはにほんじんです。",
                    ExampleVi = "Giáo viên là người Nhật.",
                    ExampleEn = "The teacher is Japanese.",
                    ImageUrl = "/images/flashcards/sensei.jpg",
                    TopicId = 4
                },
                new Flashcard
                {
                    Id = 33,
                    Front = "がくせい",
                    Furigana = "がくせい",
                    FlashcardCode = "N5-004-003",
                    MeaningVi = "Học sinh / Sinh viên",
                    MeaningEn = "Student",
                    Example = "わたしはがくせいです。",
                    ExampleVi = "Tôi là sinh viên.",
                    ExampleEn = "I am a student.",
                    ImageUrl = "/images/flashcards/gakusei.jpg",
                    TopicId = 4
                },
                new Flashcard
                {
                    Id = 34,
                    Front = "きょうしつ",
                    Furigana = "きょうしつ",
                    FlashcardCode = "N5-004-004",
                    MeaningVi = "Phòng học",
                    MeaningEn = "Classroom",
                    Example = "きょうしつはにかいにあります。",
                    ExampleVi = "Phòng học ở tầng hai.",
                    ExampleEn = "The classroom is on the second floor.",
                    ImageUrl = "/images/flashcards/kyoushitsu.jpg",
                    TopicId = 4
                },
                new Flashcard
                {
                    Id = 35,
                    Front = "ほん",
                    Furigana = "ほん",
                    FlashcardCode = "N5-004-005",
                    MeaningVi = "Sách",
                    MeaningEn = "Book",
                    Example = "ほんをよみます。",
                    ExampleVi = "Tôi đọc sách.",
                    ExampleEn = "I read books.",
                    ImageUrl = "/images/flashcards/hon.jpg",
                    TopicId = 4
                },
                new Flashcard
                {
                    Id = 36,
                    Front = "えんぴつ",
                    Furigana = "えんぴつ",
                    FlashcardCode = "N5-004-006",
                    MeaningVi = "Bút chì",
                    MeaningEn = "Pencil",
                    Example = "えんぴつでかきます。",
                    ExampleVi = "Tôi viết bằng bút chì.",
                    ExampleEn = "I write with a pencil.",
                    ImageUrl = "/images/flashcards/enpitsu.jpg",
                    TopicId = 4
                },
                new Flashcard
                {
                    Id = 37,
                    Front = "じしょ",
                    Furigana = "じしょ",
                    FlashcardCode = "N5-004-007",
                    MeaningVi = "Từ điển",
                    MeaningEn = "Dictionary",
                    Example = "じしょをつかいます。",
                    ExampleVi = "Tôi dùng từ điển.",
                    ExampleEn = "I use a dictionary.",
                    ImageUrl = "/images/flashcards/jisho.jpg",
                    TopicId = 4
                },
                new Flashcard
                {
                    Id = 38,
                    Front = "しけん",
                    Furigana = "しけん",
                    FlashcardCode = "N5-004-008",
                    MeaningVi = "Kỳ thi",
                    MeaningEn = "Exam / Test",
                    Example = "らいしゅうしけんがあります。",
                    ExampleVi = "Tuần sau có kỳ thi.",
                    ExampleEn = "There is an exam next week.",
                    ImageUrl = "/images/flashcards/shiken.jpg",
                    TopicId = 4
                },
                new Flashcard
                {
                    Id = 39,
                    Front = "べんきょう",
                    Furigana = "べんきょう",
                    FlashcardCode = "N5-004-009",
                    MeaningVi = "Học tập",
                    MeaningEn = "Study / To study",
                    Example = "まいにちべんきょうします。",
                    ExampleVi = "Tôi học mỗi ngày.",
                    ExampleEn = "I study every day.",
                    ImageUrl = "/images/flashcards/benkyou.jpg",
                    TopicId = 4
                },
                new Flashcard
                {
                    Id = 40,
                    Front = "やすみじかん",
                    Furigana = "やすみじかん",
                    FlashcardCode = "N5-004-010",
                    MeaningVi = "Giờ nghỉ",
                    MeaningEn = "Break time / Recess",
                    Example = "やすみじかんにともだちとあそびます。",
                    ExampleVi = "Trong giờ nghỉ tôi chơi với bạn.",
                    ExampleEn = "I play with friends during break time.",
                    ImageUrl = "/images/flashcards/yasumijikan.jpg",
                    TopicId = 4
                },

                // --- TopicId = 5: Mua sắm ---
                new Flashcard
                {
                    Id = 41,
                    Front = "みせ",
                    Furigana = "みせ",
                    FlashcardCode = "N5-005-001",
                    MeaningVi = "Cửa hàng",
                    MeaningEn = "Shop / Store",
                    Example = "みせでくだものをかいます。",
                    ExampleVi = "Tôi mua hoa quả ở cửa hàng.",
                    ExampleEn = "I buy fruit at the shop.",
                    ImageUrl = "/images/flashcards/mise.jpg",
                    TopicId = 5
                },
                new Flashcard
                {
                    Id = 42,
                    Front = "かいもの",
                    Furigana = "かいもの",
                    FlashcardCode = "N5-005-002",
                    MeaningVi = "Mua sắm",
                    MeaningEn = "Shopping",
                    Example = "かいものにいきます。",
                    ExampleVi = "Tôi đi mua sắm.",
                    ExampleEn = "I go shopping.",
                    ImageUrl = "/images/flashcards/kaimono.jpg",
                    TopicId = 5
                },
                new Flashcard
                {
                    Id = 43,
                    Front = "かう",
                    Furigana = "かう",
                    FlashcardCode = "N5-005-003",
                    MeaningVi = "Mua",
                    MeaningEn = "To buy",
                    Example = "ほんをかいました。",
                    ExampleVi = "Tôi đã mua một quyển sách.",
                    ExampleEn = "I bought a book.",
                    ImageUrl = "/images/flashcards/kau.jpg",
                    TopicId = 5
                },
                new Flashcard
                {
                    Id = 44,
                    Front = "うる",
                    Furigana = "うる",
                    FlashcardCode = "N5-005-004",
                    MeaningVi = "Bán",
                    MeaningEn = "To sell",
                    Example = "このみせはやさいをうっています。",
                    ExampleVi = "Cửa hàng này bán rau.",
                    ExampleEn = "This shop sells vegetables.",
                    ImageUrl = "/images/flashcards/uru.jpg",
                    TopicId = 5
                },
                new Flashcard
                {
                    Id = 45,
                    Front = "やすい",
                    Furigana = "やすい",
                    FlashcardCode = "N5-005-005",
                    MeaningVi = "Rẻ",
                    MeaningEn = "Cheap / Inexpensive",
                    Example = "このシャツはやすいです。",
                    ExampleVi = "Chiếc áo này rẻ.",
                    ExampleEn = "This shirt is cheap.",
                    ImageUrl = "/images/flashcards/yasui.jpg",
                    TopicId = 5
                },
                new Flashcard
                {
                    Id = 46,
                    Front = "たかい",
                    Furigana = "たかい",
                    FlashcardCode = "N5-005-006",
                    MeaningVi = "Đắt / Cao",
                    MeaningEn = "Expensive / High",
                    Example = "このくつはたかいです。",
                    ExampleVi = "Đôi giày này đắt.",
                    ExampleEn = "These shoes are expensive.",
                    ImageUrl = "/images/flashcards/takai.jpg",
                    TopicId = 5
                },
                new Flashcard
                {
                    Id = 47,
                    Front = "かばん",
                    Furigana = "かばん",
                    FlashcardCode = "N5-005-007",
                    MeaningVi = "Cặp sách / Túi",
                    MeaningEn = "Bag / Backpack",
                    Example = "あたらしいかばんをかいました。",
                    ExampleVi = "Tôi đã mua một chiếc túi mới.",
                    ExampleEn = "I bought a new bag.",
                    ImageUrl = "/images/flashcards/kaban.jpg",
                    TopicId = 5
                },
                new Flashcard
                {
                    Id = 48,
                    Front = "さいふ",
                    Furigana = "さいふ",
                    FlashcardCode = "N5-005-008",
                    MeaningVi = "Ví tiền",
                    MeaningEn = "Wallet",
                    Example = "さいふをなくしました。",
                    ExampleVi = "Tôi làm mất ví.",
                    ExampleEn = "I lost my wallet.",
                    ImageUrl = "/images/flashcards/saifu.jpg",
                    TopicId = 5
                },
                new Flashcard
                {
                    Id = 49,
                    Front = "きる",
                    Furigana = "きる",
                    FlashcardCode = "N5-005-009",
                    MeaningVi = "Mặc (quần áo)",
                    MeaningEn = "To wear/put on (clothes, upper body)",
                    Example = "シャツをきます。",
                    ExampleVi = "Tôi mặc áo sơ mi.",
                    ExampleEn = "I wear a shirt.",
                    ImageUrl = "/images/flashcards/kiru.jpg",
                    TopicId = 5
                },
                new Flashcard
                {
                    Id = 50,
                    Front = "はく",
                    Furigana = "はく",
                    FlashcardCode = "N5-005-010",
                    MeaningVi = "Mang (giày, quần)",
                    MeaningEn = "To wear/put on (shoes, pants, skirt)",
                    Example = "くつをはきます。",
                    ExampleVi = "Tôi mang giày.",
                    ExampleEn = "I put on shoes.",
                    ImageUrl = "/images/flashcards/haku.jpg",
                    TopicId = 5
                }
            );
        }

        private void SeedKaiwaByCourseId(ModelBuilder builder)
        {
            builder.Entity<Audio>().HasData(
                new Audio
                {
                    Id = 1,
                    AudioCode = "A001",
                    AudioType = AudioType.Kaiwa,
                    Title = "Sample 1",
                    Section = "Chapter 1",
                    FileUrl = @"E:\Tốt nghiệp\Thực tập tốt nghiệp\audio\sample1.mp3",
                    SortOrder = 1,
                    IsFree = true,
                    Script = "おはようございます。今日はとてもいい天気ですね。朝ごはんはもう食べましたか。私はパンとコーヒーをいただきました。これから学校へ行く予定です。",
                    ScriptVi =
                        "Chào buổi sáng. Hôm nay trời rất đẹp. Bạn đã ăn sáng chưa? Tôi vừa ăn bánh mì và uống cà phê. Sắp tới tôi sẽ đi đến trường.",
                    ScriptEn =
                        "Good morning. The weather is really nice today. Have you had breakfast yet? I had bread and coffee. I’m planning to go to school now.",
                    CourseId = 1
                },
                new Audio
                {
                    Id = 2,
                    AudioCode = "A002",
                    AudioType = AudioType.Kaiwa,
                    Title = "Sample 2",
                    Section = "Chapter 1",
                    FileUrl = @"E:\Tốt nghiệp\Thực tập tốt nghiệp\audio\sample2.mp3",
                    SortOrder = 2,
                    IsFree = true,
                    Script = "こんにちは。今日は授業が早く終わりました。午後は友達と図書館で勉強します。終わったら一緒にカフェに行く予定です。あなたも来ませんか。",
                    ScriptVi =
                        "Xin chào. Hôm nay buổi học kết thúc sớm. Buổi chiều tôi sẽ học ở thư viện với bạn bè. Sau khi xong chúng tôi sẽ đi quán cà phê. Bạn có muốn đi cùng không?",
                    ScriptEn =
                        "Hello. Today’s class finished early. In the afternoon I’ll study at the library with friends. After that we plan to go to a café. Would you like to come too?",
                    CourseId = 1
                },
                new Audio
                {
                    Id = 3,
                    AudioCode = "A003",
                    AudioType = AudioType.Kaiwa,
                    Title = "Sample 3",
                    Section = "Chapter 1",
                    FileUrl = @"E:\Tốt nghiệp\Thực tập tốt nghiệp\audio\sample3.mp3",
                    SortOrder = 3,
                    IsFree = false,
                    Script = "こんばんは。今日は本当に忙しい一日でした。たくさんの会議に参加して疲れました。でも同僚たちと楽しく話せて良かったです。これからお風呂に入って早く寝ます。",
                    ScriptVi =
                        "Chào buổi tối. Hôm nay thật sự là một ngày bận rộn. Tôi tham dự rất nhiều cuộc họp nên khá mệt. Nhưng tôi đã trò chuyện vui vẻ với đồng nghiệp, điều đó thật tuyệt. Giờ tôi sẽ tắm và ngủ sớm.",
                    ScriptEn =
                        "Good evening. Today was really busy. I joined many meetings and got tired. But I enjoyed talking with my colleagues. Now I’ll take a bath and sleep early.",
                    CourseId = 1
                },
                new Audio
                {
                    Id = 4,
                    AudioCode = "A004",
                    AudioType = AudioType.Kaiwa,
                    Title = "Sample 4",
                    Section = "Chapter 2",
                    FileUrl = @"E:\Tốt nghiệp\Thực tập tốt nghiệp\audio\sample4.mp3",
                    SortOrder = 4,
                    IsFree = false,
                    Script = "ありがとうございます。あなたの助けがなければ終わらなかったでしょう。おかげで時間通りに提出できました。次回は私が手伝います。これからもよろしくお願いします。",
                    ScriptVi =
                        "Cảm ơn bạn rất nhiều. Nếu không có bạn tôi đã không hoàn thành. Nhờ bạn mà tôi kịp nộp đúng giờ. Lần tới tôi sẽ giúp bạn. Mong được hợp tác tiếp.",
                    ScriptEn =
                        "Thank you very much. Without you I couldn’t have finished. Thanks to you I submitted on time. Next time I will help you. I look forward to working together again.",
                    CourseId = 1
                },
                new Audio
                {
                    Id = 5,
                    AudioCode = "A005",
                    AudioType = AudioType.Kaiwa,
                    Title = "Sample 5",
                    Section = "Chapter 2",
                    FileUrl = @"E:\Tốt nghiệp\Thực tập tốt nghiệp\audio\sample5.mp3",
                    SortOrder = 5,
                    IsFree = true,
                    Script =
                        "すみません、道に迷いました。駅までの行き方を教えていただけますか。地図を持っていますが少し分かりにくいです。バスと電車どちらが早いでしょうか。助けてくださってありがとうございます。",
                    ScriptVi =
                        "Xin lỗi, tôi bị lạc đường. Bạn có thể chỉ giúp tôi đường đến ga tàu không? Tôi có bản đồ nhưng hơi khó hiểu. Đi xe buýt hay tàu sẽ nhanh hơn? Cảm ơn bạn đã giúp đỡ.",
                    ScriptEn =
                        "Excuse me, I’m lost. Could you tell me how to get to the station? I have a map but it’s a bit confusing. Which is faster, bus or train? Thank you for helping me.",
                    CourseId = 1
                },
                new Audio
                {
                    Id = 6,
                    AudioCode = "A006",
                    AudioType = AudioType.Kaiwa,
                    Title = "Sample 6",
                    Section = "Chapter 2",
                    FileUrl = @"E:\Tốt nghiệp\Thực tập tốt nghiệp\audio\sample6.mp3",
                    SortOrder = 6,
                    IsFree = false,
                    Script = "初めまして。私は田中と申します。東京から来ました。日本語を勉強して三年になります。これから一緒に楽しく学びたいと思います。どうぞよろしくお願いします。",
                    ScriptVi =
                        "Rất vui được gặp bạn. Tôi tên là Tanaka và đến từ Tokyo. Tôi đã học tiếng Nhật được ba năm. Tôi mong muốn cùng bạn học tập vui vẻ từ bây giờ. Rất mong được giúp đỡ.",
                    ScriptEn =
                        "Nice to meet you. My name is Tanaka and I’m from Tokyo. I’ve been studying Japanese for three years. I hope to enjoy learning together with you. Please take care of me.",
                    CourseId = 1
                },
                new Audio
                {
                    Id = 7,
                    AudioCode = "A007",
                    AudioType = AudioType.Kaiwa,
                    Title = "Sample 7",
                    Section = "Chapter 3",
                    FileUrl = @"E:\Tốt nghiệp\Thực tập tốt nghiệp\audio\sample7.mp3",
                    SortOrder = 7,
                    IsFree = true,
                    Script =
                        "昨日は雨が降りましたが、今日は晴れてとても気持ちがいいです。朝から公園を散歩しました。花がたくさん咲いていました。鳥の声も聞こえてとても楽しかったです。写真もたくさん撮りました。",
                    ScriptVi =
                        "Hôm qua trời mưa nhưng hôm nay trời nắng thật dễ chịu. Sáng nay tôi đi dạo trong công viên. Có rất nhiều hoa nở. Tiếng chim hót nghe rất vui. Tôi cũng chụp nhiều bức ảnh.",
                    ScriptEn =
                        "It rained yesterday, but today it’s sunny and very pleasant. I took a walk in the park this morning. Many flowers were blooming. I heard birds singing and enjoyed it. I also took many photos.",
                    CourseId = 1
                },
                new Audio
                {
                    Id = 8,
                    AudioCode = "A008",
                    AudioType = AudioType.Kaiwa,
                    Title = "Sample 8",
                    Section = "Chapter 3",
                    FileUrl = @"E:\Tốt nghiệp\Thực tập tốt nghiệp\audio\sample8.mp3",
                    SortOrder = 8,
                    IsFree = false,
                    Script = "明日は友達と一緒に映画を見に行く予定です。とても楽しみにしています。映画の後はレストランで夕食を食べます。新しいメニューがあるそうです。写真も撮ってSNSにアップしたいです。",
                    ScriptVi =
                        "Ngày mai tôi dự định đi xem phim cùng bạn. Tôi rất mong chờ. Sau khi xem phim, chúng tôi sẽ ăn tối ở nhà hàng. Nghe nói có thực đơn mới. Tôi cũng muốn chụp ảnh và đăng lên mạng xã hội.",
                    ScriptEn =
                        "Tomorrow I plan to watch a movie with friends. I’m really looking forward to it. After the movie we’ll have dinner at a restaurant. I heard there’s a new menu. I want to take photos and post them online.",
                    CourseId = 1
                },
                new Audio
                {
                    Id = 9,
                    AudioCode = "A009",
                    AudioType = AudioType.Kaiwa,
                    Title = "Sample 9",
                    Section = "Chapter 3",
                    FileUrl = @"E:\Tốt nghiệp\Thực tập tốt nghiệp\audio\sample9.mp3",
                    SortOrder = 9,
                    IsFree = true,
                    Script = "このレストランの料理はとても美味しいです。特にラーメンがおすすめです。スープが濃くて香りが良いです。デザートのケーキも絶品でした。友達にもぜひ紹介したいです。",
                    ScriptVi =
                        "Món ăn ở nhà hàng này rất ngon, đặc biệt là mì ramen. Nước dùng đậm đà và thơm. Bánh ngọt tráng miệng cũng tuyệt vời. Tôi nhất định sẽ giới thiệu cho bạn bè.",
                    ScriptEn =
                        "The food at this restaurant is very delicious, especially the ramen. The broth is rich and fragrant. The dessert cake was excellent too. I definitely want to recommend it to my friends.",
                    CourseId = 1
                },
                new Audio
                {
                    Id = 10,
                    AudioCode = "A010",
                    AudioType = AudioType.Kaiwa,
                    Title = "Sample 10",
                    Section = "Chapter 4",
                    FileUrl = @"E:\Tốt nghiệp\Thực tập tốt nghiệp\audio\sample10.mp3",
                    SortOrder = 10,
                    IsFree = false,
                    Script = "日本へ行ったら、ぜひ京都を訪れてください。美しい寺や神社がたくさんあります。四季折々の景色が楽しめます。歴史的な町並みも魅力的です。ゆっくり散歩すると心が落ち着きます。",
                    ScriptVi =
                        "Nếu bạn đến Nhật, hãy ghé thăm Kyoto. Nơi đây có rất nhiều đền chùa xinh đẹp. Bạn có thể thưởng thức phong cảnh bốn mùa. Phố cổ mang đậm dấu ấn lịch sử cũng rất hấp dẫn. Dạo bộ chậm rãi khiến tâm hồn bình yên.",
                    ScriptEn =
                        "If you visit Japan, be sure to go to Kyoto. There are many beautiful temples and shrines. You can enjoy the scenery of all four seasons. The historical streets are charming. A slow walk there will calm your heart.",
                    CourseId = 1
                }
            );
        }

        private void SeedGrammarByCourseId(ModelBuilder builder)
        {
            builder.Entity<Audio>().HasData(
                new Audio
                {
                    Id = 21,
                    AudioCode = "G001",
                    AudioType = AudioType.Grammar,
                    Title = "Grammar Sample 1",
                    Section = "Grammar N5 - 1",
                    FileUrl = @"E:\Tốt nghiệp\Thực tập tốt nghiệp\audio\grammar1.mp3",
                    SortOrder = 1,
                    IsFree = true,
                    Script = "田中さんは音楽を聞きながら勉強します。勉強してから友達と公園へ行きます。公園で写真を撮ったことがありますか。私は一度も撮ったことがありません。でも、今日は写真を撮りたいです。",
                    ScriptVi =
                        "Anh Tanaka vừa nghe nhạc vừa học. Sau khi học xong anh ấy đi công viên với bạn. Bạn đã từng chụp ảnh ở công viên chưa? Tôi thì chưa từng. Nhưng hôm nay tôi muốn chụp ảnh.",
                    ScriptEn =
                        "Tanaka studies while listening to music. After studying, he goes to the park with friends. Have you ever taken pictures in the park? I never have. But today I want to take some photos.",
                    CourseId = 1
                },
                new Audio
                {
                    Id = 22,
                    AudioCode = "G002",
                    AudioType = AudioType.Grammar,
                    Title = "Grammar Sample 2",
                    Section = "Grammar N5 - 1",
                    FileUrl = @"E:\Tốt nghiệp\Thực tập tốt nghiệp\audio\grammar2.mp3",
                    SortOrder = 2,
                    IsFree = false,
                    Script =
                        "朝ごはんを食べてから会社へ行きます。会社に着いたらまずメールをチェックします。会議が終わった後、上司と話さなければなりません。午後はお客様に電話をかけたいです。でも忙しいかもしれません。",
                    ScriptVi =
                        "Sau khi ăn sáng tôi đi làm. Đến công ty tôi kiểm tra email trước. Sau khi họp xong, tôi phải nói chuyện với sếp. Buổi chiều tôi muốn gọi điện cho khách hàng nhưng có thể sẽ bận.",
                    ScriptEn =
                        "I go to the office after eating breakfast. Upon arrival, I first check emails. After the meeting, I must talk with my boss. In the afternoon I’d like to call customers, but I might be busy.",
                    CourseId = 1
                },
                new Audio
                {
                    Id = 23,
                    AudioCode = "G003",
                    AudioType = AudioType.Grammar,
                    Title = "Grammar Sample 3",
                    Section = "Grammar N5 - 2",
                    FileUrl = @"E:\Tốt nghiệp\Thực tập tốt nghiệp\audio\grammar3.mp3",
                    SortOrder = 3,
                    IsFree = true,
                    Script = "昨日は雨が降っていましたが、今日は晴れそうです。散歩したいけれど、仕事をしなければなりません。仕事が終わったら一緒に映画を見に行きませんか。映画を見たことがありますか。",
                    ScriptVi =
                        "Hôm qua trời mưa nhưng hôm nay có vẻ nắng. Tôi muốn đi dạo nhưng phải làm việc. Sau khi làm xong chúng ta đi xem phim nhé. Bạn đã từng xem phim đó chưa?",
                    ScriptEn =
                        "It rained yesterday, but it looks sunny today. I want to take a walk, but I must work. After finishing work, shall we go to watch a movie? Have you seen that movie before?",
                    CourseId = 1
                },
                new Audio
                {
                    Id = 24,
                    AudioCode = "G004",
                    AudioType = AudioType.Grammar,
                    Title = "Grammar Sample 4",
                    Section = "Grammar N5 - 2",
                    FileUrl = @"E:\Tốt nghiệp\Thực tập tốt nghiệp\audio\grammar4.mp3",
                    SortOrder = 4,
                    IsFree = false,
                    Script = "母は料理を作りながら歌を歌っています。私は宿題を終えてから手伝います。夜ご飯を食べたあと、みんなでテレビを見ます。楽しい時間を過ごしたいです。",
                    ScriptVi =
                        "Mẹ vừa nấu ăn vừa hát. Tôi làm xong bài tập rồi sẽ giúp mẹ. Sau khi ăn tối, cả nhà cùng xem TV. Tôi muốn có thời gian vui vẻ.",
                    ScriptEn =
                        "Mother sings while cooking. After finishing homework, I will help her. After dinner, we all watch TV together. I want to spend a fun time.",
                    CourseId = 1
                },
                new Audio
                {
                    Id = 25,
                    AudioCode = "G005",
                    AudioType = AudioType.Grammar,
                    Title = "Grammar Sample 5",
                    Section = "Grammar N5 - 3",
                    FileUrl = @"E:\Tốt nghiệp\Thực tập tốt nghiệp\audio\grammar5.mp3",
                    SortOrder = 5,
                    IsFree = true,
                    Script = "駅に着いたら田中さんに電話します。田中さんはもう駅に来たことがありますか。来なければなりませんが、少し遅れるかもしれません。私は本を読みながら待っています。",
                    ScriptVi =
                        "Khi đến ga tôi sẽ gọi cho Tanaka. Tanaka đã từng đến ga này chưa? Anh ấy phải đến nhưng có thể sẽ trễ. Tôi sẽ vừa đọc sách vừa chờ.",
                    ScriptEn =
                        "I will call Tanaka when I arrive at the station. Has he been to this station before? He must come, but he might be late. I’ll wait while reading a book.",
                    CourseId = 1
                },
                new Audio
                {
                    Id = 26,
                    AudioCode = "G006",
                    AudioType = AudioType.Grammar,
                    Title = "Grammar Sample 6",
                    Section = "Grammar N5 - 3",
                    FileUrl = @"E:\Tốt nghiệp\Thực tập tốt nghiệp\audio\grammar6.mp3",
                    SortOrder = 6,
                    IsFree = false,
                    Script = "週末は友達と買い物をしたいです。買い物してからカフェで休みます。コーヒーを飲みながら話すのが好きです。帰る前に駅でお土産を買わなければなりません。",
                    ScriptVi =
                        "Cuối tuần tôi muốn đi mua sắm với bạn. Sau khi mua sắm chúng tôi sẽ nghỉ ở quán cà phê. Tôi thích vừa uống cà phê vừa trò chuyện. Trước khi về phải mua quà lưu niệm ở ga.",
                    ScriptEn =
                        "I want to shop with friends on the weekend. After shopping, we will rest at a café. I like talking while drinking coffee. Before going home, I must buy souvenirs at the station.",
                    CourseId = 1
                },
                new Audio
                {
                    Id = 27,
                    AudioCode = "G007",
                    AudioType = AudioType.Grammar,
                    Title = "Grammar Sample 7",
                    Section = "Grammar N5 - 4",
                    FileUrl = @"E:\Tốt nghiệp\Thực tập tốt nghiệp\audio\grammar7.mp3",
                    SortOrder = 7,
                    IsFree = true,
                    Script = "朝ジョギングをしてから朝ごはんを食べます。走りながら音楽を聞くのが楽しいです。その後、学校へ行かなければなりません。授業が終わったあと、図書館で勉強したいです。",
                    ScriptVi =
                        "Buổi sáng tôi chạy bộ rồi ăn sáng. Nghe nhạc khi chạy rất vui. Sau đó tôi phải đến trường. Sau giờ học tôi muốn học thêm ở thư viện.",
                    ScriptEn =
                        "I jog in the morning and then eat breakfast. Listening to music while running is fun. After that I must go to school. After class, I want to study at the library.",
                    CourseId = 1
                },
                new Audio
                {
                    Id = 28,
                    AudioCode = "G008",
                    AudioType = AudioType.Grammar,
                    Title = "Grammar Sample 8",
                    Section = "Grammar N5 - 4",
                    FileUrl = @"E:\Tốt nghiệp\Thực tập tốt nghiệp\audio\grammar8.mp3",
                    SortOrder = 8,
                    IsFree = false,
                    Script = "友達は旅行したことがありますか。私は一度も海外へ行ったことがありません。でも、来年は行きたいです。パスポートを取らなければなりません。",
                    ScriptVi =
                        "Bạn tôi đã từng đi du lịch chưa? Tôi chưa từng ra nước ngoài. Nhưng năm sau tôi muốn đi. Tôi phải làm hộ chiếu.",
                    ScriptEn =
                        "Has my friend ever traveled? I have never been abroad. But next year I want to go. I must get a passport.",
                    CourseId = 1
                },
                new Audio
                {
                    Id = 29,
                    AudioCode = "G009",
                    AudioType = AudioType.Grammar,
                    Title = "Grammar Sample 9",
                    Section = "Grammar N5 - 5",
                    FileUrl = @"E:\Tốt nghiệp\Thực tập tốt nghiệp\audio\grammar9.mp3",
                    SortOrder = 9,
                    IsFree = true,
                    Script = "授業が始まる前に教室を掃除します。掃除しながら友達と話します。終わってからみんなで朝ごはんを食べに行きます。食べた後は図書館に行きたいです。",
                    ScriptVi =
                        "Trước khi giờ học bắt đầu chúng tôi dọn lớp. Vừa dọn vừa trò chuyện. Dọn xong mọi người cùng đi ăn sáng. Ăn xong tôi muốn đến thư viện.",
                    ScriptEn =
                        "Before class starts we clean the room. We talk while cleaning. After finishing we all go for breakfast. After eating I want to go to the library.",
                    CourseId = 1
                },
                new Audio
                {
                    Id = 30,
                    AudioCode = "G010",
                    AudioType = AudioType.Grammar,
                    Title = "Grammar Sample 10",
                    Section = "Grammar N5 - 5",
                    FileUrl = @"E:\Tốt nghiệp\Thực tập tốt nghiệp\audio\grammar10.mp3",
                    SortOrder = 10,
                    IsFree = false,
                    Script = "昨日スーパーで買い物してから映画を見ました。映画を見ながらポップコーンを食べました。とても楽しかったです。帰る前に駅で友達に会わなければなりませんでした。",
                    ScriptVi =
                        "Hôm qua tôi đi siêu thị mua sắm rồi xem phim. Vừa xem phim vừa ăn bỏng ngô. Rất vui. Trước khi về tôi phải gặp bạn ở ga.",
                    ScriptEn =
                        "Yesterday I went shopping at the supermarket and then watched a movie. I ate popcorn while watching. It was very fun. Before going home, I had to meet a friend at the station.",
                    CourseId = 1
                }
            );
        }
    }
}