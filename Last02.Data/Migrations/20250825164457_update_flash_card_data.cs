using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Last02.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_flash_card_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Meaning",
                table: "Flashcards",
                newName: "MeaningVi");

            migrationBuilder.AddColumn<string>(
                name: "ExampleEn",
                table: "Flashcards",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExampleVi",
                table: "Flashcards",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeaningEn",
                table: "Flashcards",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 25, 23, 44, 56, 882, DateTimeKind.Local).AddTicks(9759));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 25, 23, 44, 56, 883, DateTimeKind.Local).AddTicks(141));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 25, 23, 44, 56, 883, DateTimeKind.Local).AddTicks(143));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 25, 23, 44, 56, 883, DateTimeKind.Local).AddTicks(144));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 25, 23, 44, 56, 883, DateTimeKind.Local).AddTicks(146));

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "Hello, nice to meet you.", "Xin chào, rất vui được gặp bạn.", "Hello" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "Good morning, do your best today too.", "Chào buổi sáng, hôm nay cùng cố gắng nhé.", "Good morning" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "Good evening, the weather is nice, isn’t it?", "Chào buổi tối, thời tiết thật đẹp nhỉ.", "Good evening" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "Goodbye, see you tomorrow.", "Tạm biệt, hẹn gặp lại ngày mai.", "Goodbye" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "Thank you for the present.", "Cảm ơn vì món quà.", "Thank you" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "Excuse me, please say it once again.", "Xin lỗi, vui lòng nhắc lại một lần nữa.", "Excuse me / Sorry" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "Yes, I understand.", "Vâng, tôi đã hiểu.", "Yes / Correct" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "No, that’s not right.", "Không, không phải vậy.", "No" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "Nice to meet you, I am Yamada.", "Rất vui được gặp bạn, tôi là Yamada.", "Nice to meet you" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "Good night, see you tomorrow.", "Chúc ngủ ngon, hẹn gặp lại ngày mai.", "Good night" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "Please give me one apple.", "Xin cho tôi một quả táo.", "One" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "I am studying Japanese.", "Tôi đang học tiếng Nhật.", "Two" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "I have three friends.", "Tôi có ba người bạn.", "Three" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "Let’s meet at four o’clock.", "Hãy gặp nhau lúc bốn giờ.", "Four" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "I wake up at five o’clock.", "Tôi thức dậy lúc năm giờ.", "Five" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "Please wait for six minutes.", "Xin hãy chờ sáu phút.", "Six" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "I get up at half past seven.", "Tôi dậy lúc bảy giờ rưỡi.", "Seven" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "I go out at eight o’clock.", "Tôi ra ngoài lúc tám giờ.", "Eight" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "Please come by nine o’clock.", "Hãy đến trước chín giờ.", "Nine" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "There are ten students.", "Có mười học sinh.", "Ten" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "My family has five people.", "Gia đình tôi có năm người.", "Family" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "My father works at a bank.", "Cha tôi làm việc ở ngân hàng.", "Father (talking about one's own)" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "My mother is good at cooking.", "Mẹ tôi nấu ăn rất giỏi.", "Mother (talking about one's own)" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "Where is your father?", "Bố bạn ở đâu?", "Father (polite)" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "How is your mother?", "Mẹ bạn có khỏe không?", "Mother (polite)" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "My older brother is a university student.", "Anh trai tôi là sinh viên đại học.", "Older brother (talking about one's own)" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "My older sister is a teacher.", "Chị gái tôi là giáo viên.", "Older sister (talking about one's own)" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "How old is your older brother?", "Anh trai bạn bao nhiêu tuổi?", "Older brother (polite)" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "Is your older sister studying?", "Chị gái bạn có đang học không?", "Older sister (polite)" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "How many siblings do you have?", "Bạn có mấy anh chị em?", "Siblings" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "I go to school.", "Tôi đi đến trường.", "School" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "The teacher is Japanese.", "Giáo viên là người Nhật.", "Teacher" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "I am a student.", "Tôi là sinh viên.", "Student" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "The classroom is on the second floor.", "Phòng học ở tầng hai.", "Classroom" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "I read books.", "Tôi đọc sách.", "Book" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "I write with a pencil.", "Tôi viết bằng bút chì.", "Pencil" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "I use a dictionary.", "Tôi dùng từ điển.", "Dictionary" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "There is an exam next week.", "Tuần sau có kỳ thi.", "Exam / Test" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "I study every day.", "Tôi học mỗi ngày.", "Study / To study" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "I play with friends during break time.", "Trong giờ nghỉ tôi chơi với bạn.", "Break time / Recess" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "I buy fruit at the shop.", "Tôi mua hoa quả ở cửa hàng.", "Shop / Store" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "I go shopping.", "Tôi đi mua sắm.", "Shopping" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "I bought a book.", "Tôi đã mua một quyển sách.", "To buy" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "This shop sells vegetables.", "Cửa hàng này bán rau.", "To sell" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "This shirt is cheap.", "Chiếc áo này rẻ.", "Cheap / Inexpensive" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "These shoes are expensive.", "Đôi giày này đắt.", "Expensive / High" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "I bought a new bag.", "Tôi đã mua một chiếc túi mới.", "Bag / Backpack" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "I lost my wallet.", "Tôi làm mất ví.", "Wallet" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "I wear a shirt.", "Tôi mặc áo sơ mi.", "To wear/put on (clothes, upper body)" });

            migrationBuilder.UpdateData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "ExampleEn", "ExampleVi", "MeaningEn" },
                values: new object[] { "I put on shoes.", "Tôi mang giày.", "To wear/put on (shoes, pants, skirt)" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExampleEn",
                table: "Flashcards");

            migrationBuilder.DropColumn(
                name: "ExampleVi",
                table: "Flashcards");

            migrationBuilder.DropColumn(
                name: "MeaningEn",
                table: "Flashcards");

            migrationBuilder.RenameColumn(
                name: "MeaningVi",
                table: "Flashcards",
                newName: "Meaning");

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 23, 16, 26, 31, DateTimeKind.Local).AddTicks(1614));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 23, 16, 26, 31, DateTimeKind.Local).AddTicks(1980));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 23, 16, 26, 31, DateTimeKind.Local).AddTicks(1983));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 23, 16, 26, 31, DateTimeKind.Local).AddTicks(1984));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 23, 16, 26, 31, DateTimeKind.Local).AddTicks(1986));
        }
    }
}
