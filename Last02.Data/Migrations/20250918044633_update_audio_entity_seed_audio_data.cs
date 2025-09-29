using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Last02.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_audio_entity_seed_audio_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.AddColumn<string>(
                name: "Script",
                table: "Audios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScriptEn",
                table: "Audios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScriptVi",
                table: "Audios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Audios",
                columns: new[] { "Id", "AudioCode", "AudioType", "CourseId", "FileUrl", "IsFree", "Script", "ScriptEn", "ScriptVi", "Section", "SortOrder", "Title" },
                values: new object[,]
                {
                    { 1, "A001", 1, 1, "E:\\Tốt nghiệp\\Thực tập tốt nghiệp\\audio\\sample1.mp3", true, "おはようございます。今日はとてもいい天気ですね。朝ごはんはもう食べましたか。私はパンとコーヒーをいただきました。これから学校へ行く予定です。", "Good morning. The weather is really nice today. Have you had breakfast yet? I had bread and coffee. I’m planning to go to school now.", "Chào buổi sáng. Hôm nay trời rất đẹp. Bạn đã ăn sáng chưa? Tôi vừa ăn bánh mì và uống cà phê. Sắp tới tôi sẽ đi đến trường.", "Chapter 1", 1, "Sample 1" },
                    { 2, "A002", 1, 1, "E:\\Tốt nghiệp\\Thực tập tốt nghiệp\\audio\\sample2.mp3", true, "こんにちは。今日は授業が早く終わりました。午後は友達と図書館で勉強します。終わったら一緒にカフェに行く予定です。あなたも来ませんか。", "Hello. Today’s class finished early. In the afternoon I’ll study at the library with friends. After that we plan to go to a café. Would you like to come too?", "Xin chào. Hôm nay buổi học kết thúc sớm. Buổi chiều tôi sẽ học ở thư viện với bạn bè. Sau khi xong chúng tôi sẽ đi quán cà phê. Bạn có muốn đi cùng không?", "Chapter 1", 2, "Sample 2" },
                    { 3, "A003", 1, 1, "E:\\Tốt nghiệp\\Thực tập tốt nghiệp\\audio\\sample3.mp3", false, "こんばんは。今日は本当に忙しい一日でした。たくさんの会議に参加して疲れました。でも同僚たちと楽しく話せて良かったです。これからお風呂に入って早く寝ます。", "Good evening. Today was really busy. I joined many meetings and got tired. But I enjoyed talking with my colleagues. Now I’ll take a bath and sleep early.", "Chào buổi tối. Hôm nay thật sự là một ngày bận rộn. Tôi tham dự rất nhiều cuộc họp nên khá mệt. Nhưng tôi đã trò chuyện vui vẻ với đồng nghiệp, điều đó thật tuyệt. Giờ tôi sẽ tắm và ngủ sớm.", "Chapter 1", 3, "Sample 3" },
                    { 4, "A004", 1, 1, "E:\\Tốt nghiệp\\Thực tập tốt nghiệp\\audio\\sample4.mp3", false, "ありがとうございます。あなたの助けがなければ終わらなかったでしょう。おかげで時間通りに提出できました。次回は私が手伝います。これからもよろしくお願いします。", "Thank you very much. Without you I couldn’t have finished. Thanks to you I submitted on time. Next time I will help you. I look forward to working together again.", "Cảm ơn bạn rất nhiều. Nếu không có bạn tôi đã không hoàn thành. Nhờ bạn mà tôi kịp nộp đúng giờ. Lần tới tôi sẽ giúp bạn. Mong được hợp tác tiếp.", "Chapter 2", 4, "Sample 4" },
                    { 5, "A005", 1, 1, "E:\\Tốt nghiệp\\Thực tập tốt nghiệp\\audio\\sample5.mp3", true, "すみません、道に迷いました。駅までの行き方を教えていただけますか。地図を持っていますが少し分かりにくいです。バスと電車どちらが早いでしょうか。助けてくださってありがとうございます。", "Excuse me, I’m lost. Could you tell me how to get to the station? I have a map but it’s a bit confusing. Which is faster, bus or train? Thank you for helping me.", "Xin lỗi, tôi bị lạc đường. Bạn có thể chỉ giúp tôi đường đến ga tàu không? Tôi có bản đồ nhưng hơi khó hiểu. Đi xe buýt hay tàu sẽ nhanh hơn? Cảm ơn bạn đã giúp đỡ.", "Chapter 2", 5, "Sample 5" },
                    { 6, "A006", 1, 1, "E:\\Tốt nghiệp\\Thực tập tốt nghiệp\\audio\\sample6.mp3", false, "初めまして。私は田中と申します。東京から来ました。日本語を勉強して三年になります。これから一緒に楽しく学びたいと思います。どうぞよろしくお願いします。", "Nice to meet you. My name is Tanaka and I’m from Tokyo. I’ve been studying Japanese for three years. I hope to enjoy learning together with you. Please take care of me.", "Rất vui được gặp bạn. Tôi tên là Tanaka và đến từ Tokyo. Tôi đã học tiếng Nhật được ba năm. Tôi mong muốn cùng bạn học tập vui vẻ từ bây giờ. Rất mong được giúp đỡ.", "Chapter 2", 6, "Sample 6" },
                    { 7, "A007", 1, 1, "E:\\Tốt nghiệp\\Thực tập tốt nghiệp\\audio\\sample7.mp3", true, "昨日は雨が降りましたが、今日は晴れてとても気持ちがいいです。朝から公園を散歩しました。花がたくさん咲いていました。鳥の声も聞こえてとても楽しかったです。写真もたくさん撮りました。", "It rained yesterday, but today it’s sunny and very pleasant. I took a walk in the park this morning. Many flowers were blooming. I heard birds singing and enjoyed it. I also took many photos.", "Hôm qua trời mưa nhưng hôm nay trời nắng thật dễ chịu. Sáng nay tôi đi dạo trong công viên. Có rất nhiều hoa nở. Tiếng chim hót nghe rất vui. Tôi cũng chụp nhiều bức ảnh.", "Chapter 3", 7, "Sample 7" },
                    { 8, "A008", 1, 1, "E:\\Tốt nghiệp\\Thực tập tốt nghiệp\\audio\\sample8.mp3", false, "明日は友達と一緒に映画を見に行く予定です。とても楽しみにしています。映画の後はレストランで夕食を食べます。新しいメニューがあるそうです。写真も撮ってSNSにアップしたいです。", "Tomorrow I plan to watch a movie with friends. I’m really looking forward to it. After the movie we’ll have dinner at a restaurant. I heard there’s a new menu. I want to take photos and post them online.", "Ngày mai tôi dự định đi xem phim cùng bạn. Tôi rất mong chờ. Sau khi xem phim, chúng tôi sẽ ăn tối ở nhà hàng. Nghe nói có thực đơn mới. Tôi cũng muốn chụp ảnh và đăng lên mạng xã hội.", "Chapter 3", 8, "Sample 8" },
                    { 9, "A009", 1, 1, "E:\\Tốt nghiệp\\Thực tập tốt nghiệp\\audio\\sample9.mp3", true, "このレストランの料理はとても美味しいです。特にラーメンがおすすめです。スープが濃くて香りが良いです。デザートのケーキも絶品でした。友達にもぜひ紹介したいです。", "The food at this restaurant is very delicious, especially the ramen. The broth is rich and fragrant. The dessert cake was excellent too. I definitely want to recommend it to my friends.", "Món ăn ở nhà hàng này rất ngon, đặc biệt là mì ramen. Nước dùng đậm đà và thơm. Bánh ngọt tráng miệng cũng tuyệt vời. Tôi nhất định sẽ giới thiệu cho bạn bè.", "Chapter 3", 9, "Sample 9" },
                    { 10, "A010", 1, 1, "E:\\Tốt nghiệp\\Thực tập tốt nghiệp\\audio\\sample10.mp3", false, "日本へ行ったら、ぜひ京都を訪れてください。美しい寺や神社がたくさんあります。四季折々の景色が楽しめます。歴史的な町並みも魅力的です。ゆっくり散歩すると心が落ち着きます。", "If you visit Japan, be sure to go to Kyoto. There are many beautiful temples and shrines. You can enjoy the scenery of all four seasons. The historical streets are charming. A slow walk there will calm your heart.", "Nếu bạn đến Nhật, hãy ghé thăm Kyoto. Nơi đây có rất nhiều đền chùa xinh đẹp. Bạn có thể thưởng thức phong cảnh bốn mùa. Phố cổ mang đậm dấu ấn lịch sử cũng rất hấp dẫn. Dạo bộ chậm rãi khiến tâm hồn bình yên.", "Chapter 4", 10, "Sample 10" },
                    { 21, "G001", 0, 1, "E:\\Tốt nghiệp\\Thực tập tốt nghiệp\\audio\\grammar1.mp3", true, "田中さんは音楽を聞きながら勉強します。勉強してから友達と公園へ行きます。公園で写真を撮ったことがありますか。私は一度も撮ったことがありません。でも、今日は写真を撮りたいです。", "Tanaka studies while listening to music. After studying, he goes to the park with friends. Have you ever taken pictures in the park? I never have. But today I want to take some photos.", "Anh Tanaka vừa nghe nhạc vừa học. Sau khi học xong anh ấy đi công viên với bạn. Bạn đã từng chụp ảnh ở công viên chưa? Tôi thì chưa từng. Nhưng hôm nay tôi muốn chụp ảnh.", "Grammar N5 - 1", 1, "Grammar Sample 1" },
                    { 22, "G002", 0, 1, "E:\\Tốt nghiệp\\Thực tập tốt nghiệp\\audio\\grammar2.mp3", false, "朝ごはんを食べてから会社へ行きます。会社に着いたらまずメールをチェックします。会議が終わった後、上司と話さなければなりません。午後はお客様に電話をかけたいです。でも忙しいかもしれません。", "I go to the office after eating breakfast. Upon arrival, I first check emails. After the meeting, I must talk with my boss. In the afternoon I’d like to call customers, but I might be busy.", "Sau khi ăn sáng tôi đi làm. Đến công ty tôi kiểm tra email trước. Sau khi họp xong, tôi phải nói chuyện với sếp. Buổi chiều tôi muốn gọi điện cho khách hàng nhưng có thể sẽ bận.", "Grammar N5 - 1", 2, "Grammar Sample 2" },
                    { 23, "G003", 0, 1, "E:\\Tốt nghiệp\\Thực tập tốt nghiệp\\audio\\grammar3.mp3", true, "昨日は雨が降っていましたが、今日は晴れそうです。散歩したいけれど、仕事をしなければなりません。仕事が終わったら一緒に映画を見に行きませんか。映画を見たことがありますか。", "It rained yesterday, but it looks sunny today. I want to take a walk, but I must work. After finishing work, shall we go to watch a movie? Have you seen that movie before?", "Hôm qua trời mưa nhưng hôm nay có vẻ nắng. Tôi muốn đi dạo nhưng phải làm việc. Sau khi làm xong chúng ta đi xem phim nhé. Bạn đã từng xem phim đó chưa?", "Grammar N5 - 2", 3, "Grammar Sample 3" },
                    { 24, "G004", 0, 1, "E:\\Tốt nghiệp\\Thực tập tốt nghiệp\\audio\\grammar4.mp3", false, "母は料理を作りながら歌を歌っています。私は宿題を終えてから手伝います。夜ご飯を食べたあと、みんなでテレビを見ます。楽しい時間を過ごしたいです。", "Mother sings while cooking. After finishing homework, I will help her. After dinner, we all watch TV together. I want to spend a fun time.", "Mẹ vừa nấu ăn vừa hát. Tôi làm xong bài tập rồi sẽ giúp mẹ. Sau khi ăn tối, cả nhà cùng xem TV. Tôi muốn có thời gian vui vẻ.", "Grammar N5 - 2", 4, "Grammar Sample 4" },
                    { 25, "G005", 0, 1, "E:\\Tốt nghiệp\\Thực tập tốt nghiệp\\audio\\grammar5.mp3", true, "駅に着いたら田中さんに電話します。田中さんはもう駅に来たことがありますか。来なければなりませんが、少し遅れるかもしれません。私は本を読みながら待っています。", "I will call Tanaka when I arrive at the station. Has he been to this station before? He must come, but he might be late. I’ll wait while reading a book.", "Khi đến ga tôi sẽ gọi cho Tanaka. Tanaka đã từng đến ga này chưa? Anh ấy phải đến nhưng có thể sẽ trễ. Tôi sẽ vừa đọc sách vừa chờ.", "Grammar N5 - 3", 5, "Grammar Sample 5" },
                    { 26, "G006", 0, 1, "E:\\Tốt nghiệp\\Thực tập tốt nghiệp\\audio\\grammar6.mp3", false, "週末は友達と買い物をしたいです。買い物してからカフェで休みます。コーヒーを飲みながら話すのが好きです。帰る前に駅でお土産を買わなければなりません。", "I want to shop with friends on the weekend. After shopping, we will rest at a café. I like talking while drinking coffee. Before going home, I must buy souvenirs at the station.", "Cuối tuần tôi muốn đi mua sắm với bạn. Sau khi mua sắm chúng tôi sẽ nghỉ ở quán cà phê. Tôi thích vừa uống cà phê vừa trò chuyện. Trước khi về phải mua quà lưu niệm ở ga.", "Grammar N5 - 3", 6, "Grammar Sample 6" },
                    { 27, "G007", 0, 1, "E:\\Tốt nghiệp\\Thực tập tốt nghiệp\\audio\\grammar7.mp3", true, "朝ジョギングをしてから朝ごはんを食べます。走りながら音楽を聞くのが楽しいです。その後、学校へ行かなければなりません。授業が終わったあと、図書館で勉強したいです。", "I jog in the morning and then eat breakfast. Listening to music while running is fun. After that I must go to school. After class, I want to study at the library.", "Buổi sáng tôi chạy bộ rồi ăn sáng. Nghe nhạc khi chạy rất vui. Sau đó tôi phải đến trường. Sau giờ học tôi muốn học thêm ở thư viện.", "Grammar N5 - 4", 7, "Grammar Sample 7" },
                    { 28, "G008", 0, 1, "E:\\Tốt nghiệp\\Thực tập tốt nghiệp\\audio\\grammar8.mp3", false, "友達は旅行したことがありますか。私は一度も海外へ行ったことがありません。でも、来年は行きたいです。パスポートを取らなければなりません。", "Has my friend ever traveled? I have never been abroad. But next year I want to go. I must get a passport.", "Bạn tôi đã từng đi du lịch chưa? Tôi chưa từng ra nước ngoài. Nhưng năm sau tôi muốn đi. Tôi phải làm hộ chiếu.", "Grammar N5 - 4", 8, "Grammar Sample 8" },
                    { 29, "G009", 0, 1, "E:\\Tốt nghiệp\\Thực tập tốt nghiệp\\audio\\grammar9.mp3", true, "授業が始まる前に教室を掃除します。掃除しながら友達と話します。終わってからみんなで朝ごはんを食べに行きます。食べた後は図書館に行きたいです。", "Before class starts we clean the room. We talk while cleaning. After finishing we all go for breakfast. After eating I want to go to the library.", "Trước khi giờ học bắt đầu chúng tôi dọn lớp. Vừa dọn vừa trò chuyện. Dọn xong mọi người cùng đi ăn sáng. Ăn xong tôi muốn đến thư viện.", "Grammar N5 - 5", 9, "Grammar Sample 9" },
                    { 30, "G010", 0, 1, "E:\\Tốt nghiệp\\Thực tập tốt nghiệp\\audio\\grammar10.mp3", false, "昨日スーパーで買い物してから映画を見ました。映画を見ながらポップコーンを食べました。とても楽しかったです。帰る前に駅で友達に会わなければなりませんでした。", "Yesterday I went shopping at the supermarket and then watched a movie. I ate popcorn while watching. It was very fun. Before going home, I had to meet a friend at the station.", "Hôm qua tôi đi siêu thị mua sắm rồi xem phim. Vừa xem phim vừa ăn bỏng ngô. Rất vui. Trước khi về tôi phải gặp bạn ở ga.", "Grammar N5 - 5", 10, "Grammar Sample 10" }
                });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 9, 18, 11, 46, 32, 543, DateTimeKind.Local).AddTicks(2579));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 9, 18, 11, 46, 32, 543, DateTimeKind.Local).AddTicks(2926));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 9, 18, 11, 46, 32, 543, DateTimeKind.Local).AddTicks(2928));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 9, 18, 11, 46, 32, 543, DateTimeKind.Local).AddTicks(2929));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 9, 18, 11, 46, 32, 543, DateTimeKind.Local).AddTicks(2931));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Audios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Audios",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Audios",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Audios",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Audios",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Audios",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Audios",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Audios",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Audios",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Audios",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Audios",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Audios",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Audios",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Audios",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Audios",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Audios",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Audios",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Audios",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Audios",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Audios",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DropColumn(
                name: "Script",
                table: "Audios");

            migrationBuilder.DropColumn(
                name: "ScriptEn",
                table: "Audios");

            migrationBuilder.DropColumn(
                name: "ScriptVi",
                table: "Audios");

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 9, 16, 22, 33, 3, 141, DateTimeKind.Local).AddTicks(4088));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 9, 16, 22, 33, 3, 141, DateTimeKind.Local).AddTicks(4406));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 9, 16, 22, 33, 3, 141, DateTimeKind.Local).AddTicks(4407));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 9, 16, 22, 33, 3, 141, DateTimeKind.Local).AddTicks(4409));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 9, 16, 22, 33, 3, 141, DateTimeKind.Local).AddTicks(4410));
        }
    }
}
