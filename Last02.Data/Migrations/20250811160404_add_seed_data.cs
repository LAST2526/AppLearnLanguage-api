using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Last02.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_seed_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IPA",
                table: "Flashcards",
                newName: "Furigana");

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 23, 4, 3, 743, DateTimeKind.Local).AddTicks(5887));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 23, 4, 3, 743, DateTimeKind.Local).AddTicks(6237));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 23, 4, 3, 743, DateTimeKind.Local).AddTicks(6240));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 23, 4, 3, 743, DateTimeKind.Local).AddTicks(6241));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 23, 4, 3, 743, DateTimeKind.Local).AddTicks(6243));

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "CourseId", "Description", "HexColorCode", "IsFree", "Title", "TopicCode" },
                values: new object[,]
                {
                    { 1, 1, "Học các câu chào hỏi và giao tiếp cơ bản trong tiếng Nhật N5.", "#FFB74D", true, "Chào hỏi cơ bản", "N5-001" },
                    { 2, 1, "Học cách đếm số, nói giờ, ngày tháng trong tiếng Nhật N5.", "#4DB6AC", true, "Số đếm & Thời gian", "N5-002" },
                    { 3, 1, "Từ vựng và mẫu câu liên quan đến gia đình trong tiếng Nhật N5.", "#64B5F6", false, "Gia đình", "N5-003" },
                    { 4, 1, "Từ vựng và mẫu câu sử dụng trong môi trường học tập.", "#BA68C8", false, "Trường học", "N5-004" },
                    { 5, 1, "Từ vựng và mẫu câu giao tiếp khi đi mua sắm.", "#E57373", false, "Mua sắm", "N5-005" }
                });

            migrationBuilder.InsertData(
                table: "Flashcards",
                columns: new[] { "Id", "Example", "FlashcardCode", "Front", "Furigana", "ImageUrl", "Meaning", "TopicId" },
                values: new object[,]
                {
                    { 1, "こんにちは、はじめまして。", "N5-001-001", "こんにちは", "こんにちわ", "/images/flashcards/konnichiwa.jpg", "Xin chào", 1 },
                    { 2, "おはよう、きょうもがんばってね。", "N5-001-002", "おはよう", "おはよう", "/images/flashcards/ohayou.jpg", "Chào buổi sáng", 1 },
                    { 3, "こんばんは、いい天気ですね。", "N5-001-003", "こんばんは", "こんばんは", "/images/flashcards/konbanwa.jpg", "Chào buổi tối", 1 },
                    { 4, "さようなら、またあした。", "N5-001-004", "さようなら", "さようなら", "/images/flashcards/sayounara.jpg", "Tạm biệt", 1 },
                    { 5, "プレゼントをありがとう。", "N5-001-005", "ありがとう", "ありがとう", "/images/flashcards/arigatou.jpg", "Cảm ơn", 1 },
                    { 6, "すみません、もういちどおねがいします。", "N5-001-006", "すみません", "すみません", "/images/flashcards/sumimasen.jpg", "Xin lỗi / Làm phiền", 1 },
                    { 7, "はい、わかりました。", "N5-001-007", "はい", "はい", "/images/flashcards/hai.jpg", "Vâng / Đúng", 1 },
                    { 8, "いいえ、ちがいます。", "N5-001-008", "いいえ", "いいえ", "/images/flashcards/iie.jpg", "Không", 1 },
                    { 9, "はじめまして、やまだです。", "N5-001-009", "はじめまして", "はじめまして", "/images/flashcards/hajimemashite.jpg", "Rất vui được gặp bạn", 1 },
                    { 10, "おやすみなさい、またあした。", "N5-001-010", "おやすみなさい", "おやすみなさい", "/images/flashcards/oyasuminasai.jpg", "Chúc ngủ ngon", 1 },
                    { 11, "りんごをいちつください。", "N5-002-001", "いち", "いち", "/images/flashcards/ichi.jpg", "Số một", 2 },
                    { 12, "にほんごをべんきょうしています。", "N5-002-002", "に", "に", "/images/flashcards/ni.jpg", "Số hai", 2 },
                    { 13, "さんにんのともだちがいます。", "N5-002-003", "さん", "さん", "/images/flashcards/san.jpg", "Số ba", 2 },
                    { 14, "よんじにあいましょう。", "N5-002-004", "よん / し", "よん / し", "/images/flashcards/yon.jpg", "Số bốn", 2 },
                    { 15, "ごじにおきます。", "N5-002-005", "ご", "ご", "/images/flashcards/go.jpg", "Số năm", 2 },
                    { 16, "ろっぷんまってください。", "N5-002-006", "ろく", "ろく", "/images/flashcards/roku.jpg", "Số sáu", 2 },
                    { 17, "ななじはんにおきます。", "N5-002-007", "しち / なな", "しち / なな", "/images/flashcards/nana.jpg", "Số bảy", 2 },
                    { 18, "はちじにでかけます。", "N5-002-008", "はち", "はち", "/images/flashcards/hachi.jpg", "Số tám", 2 },
                    { 19, "きゅうじまでにきてください。", "N5-002-009", "きゅう / く", "きゅう / く", "/images/flashcards/kyuu.jpg", "Số chín", 2 },
                    { 20, "じゅうにんのせいとがいます。", "N5-002-010", "じゅう", "じゅう", "/images/flashcards/juu.jpg", "Số mười", 2 },
                    { 21, "わたしのかぞくはごにんです。", "N5-003-001", "かぞく", "かぞく", "/images/flashcards/kazoku.jpg", "Gia đình", 3 },
                    { 22, "ちちはぎんこうではたらいています。", "N5-003-002", "ちち", "ちち", "/images/flashcards/chichi.jpg", "Cha (nói về cha mình)", 3 },
                    { 23, "はははりょうりがじょうずです。", "N5-003-003", "はは", "はは", "/images/flashcards/haha.jpg", "Mẹ (nói về mẹ mình)", 3 },
                    { 24, "おとうさんはどこですか。", "N5-003-004", "おとうさん", "おとうさん", "/images/flashcards/otousan.jpg", "Bố (cách gọi lịch sự)", 3 },
                    { 25, "おかあさんはげんきですか。", "N5-003-005", "おかあさん", "おかあさん", "/images/flashcards/okaasan.jpg", "Mẹ (cách gọi lịch sự)", 3 },
                    { 26, "あにはだいがくせいです。", "N5-003-006", "あに", "あに", "/images/flashcards/ani.jpg", "Anh trai (nói về anh mình)", 3 },
                    { 27, "あねはせんせいです。", "N5-003-007", "あね", "あね", "/images/flashcards/ane.jpg", "Chị gái (nói về chị mình)", 3 },
                    { 28, "おにいさんはなんさいですか。", "N5-003-008", "おにいさん", "おにいさん", "/images/flashcards/oniisan.jpg", "Anh trai (lịch sự)", 3 },
                    { 29, "おねえさんはべんきょうしていますか。", "N5-003-009", "おねえさん", "おねえさん", "/images/flashcards/oneesan.jpg", "Chị gái (lịch sự)", 3 },
                    { 30, "きょうだいはなんにんいますか。", "N5-003-010", "きょうだい", "きょうだい", "/images/flashcards/kyoudai.jpg", "Anh chị em", 3 },
                    { 31, "がっこうへいきます。", "N5-004-001", "がっこう", "がっこう", "/images/flashcards/gakkou.jpg", "Trường học", 4 },
                    { 32, "せんせいはにほんじんです。", "N5-004-002", "せんせい", "せんせい", "/images/flashcards/sensei.jpg", "Giáo viên", 4 },
                    { 33, "わたしはがくせいです。", "N5-004-003", "がくせい", "がくせい", "/images/flashcards/gakusei.jpg", "Học sinh / Sinh viên", 4 },
                    { 34, "きょうしつはにかいにあります。", "N5-004-004", "きょうしつ", "きょうしつ", "/images/flashcards/kyoushitsu.jpg", "Phòng học", 4 },
                    { 35, "ほんをよみます。", "N5-004-005", "ほん", "ほん", "/images/flashcards/hon.jpg", "Sách", 4 },
                    { 36, "えんぴつでかきます。", "N5-004-006", "えんぴつ", "えんぴつ", "/images/flashcards/enpitsu.jpg", "Bút chì", 4 },
                    { 37, "じしょをつかいます。", "N5-004-007", "じしょ", "じしょ", "/images/flashcards/jisho.jpg", "Từ điển", 4 },
                    { 38, "らいしゅうしけんがあります。", "N5-004-008", "しけん", "しけん", "/images/flashcards/shiken.jpg", "Kỳ thi", 4 },
                    { 39, "まいにちべんきょうします。", "N5-004-009", "べんきょう", "べんきょう", "/images/flashcards/benkyou.jpg", "Học tập", 4 },
                    { 40, "やすみじかんにともだちとあそびます。", "N5-004-010", "やすみじかん", "やすみじかん", "/images/flashcards/yasumijikan.jpg", "Giờ nghỉ", 4 },
                    { 41, "みせでくだものをかいます。", "N5-005-001", "みせ", "みせ", "/images/flashcards/mise.jpg", "Cửa hàng", 5 },
                    { 42, "かいものにいきます。", "N5-005-002", "かいもの", "かいもの", "/images/flashcards/kaimono.jpg", "Mua sắm", 5 },
                    { 43, "ほんをかいました。", "N5-005-003", "かう", "かう", "/images/flashcards/kau.jpg", "Mua", 5 },
                    { 44, "このみせはやさいをうっています。", "N5-005-004", "うる", "うる", "/images/flashcards/uru.jpg", "Bán", 5 },
                    { 45, "このシャツはやすいです。", "N5-005-005", "やすい", "やすい", "/images/flashcards/yasui.jpg", "Rẻ", 5 },
                    { 46, "このくつはたかいです。", "N5-005-006", "たかい", "たかい", "/images/flashcards/takai.jpg", "Đắt / Cao", 5 },
                    { 47, "あたらしいかばんをかいました。", "N5-005-007", "かばん", "かばん", "/images/flashcards/kaban.jpg", "Cặp sách / Túi", 5 },
                    { 48, "さいふをなくしました。", "N5-005-008", "さいふ", "さいふ", "/images/flashcards/saifu.jpg", "Ví tiền", 5 },
                    { 49, "シャツをきます。", "N5-005-009", "きる", "きる", "/images/flashcards/kiru.jpg", "Mặc (quần áo)", 5 },
                    { 50, "くつをはきます。", "N5-005-010", "はく", "はく", "/images/flashcards/haku.jpg", "Mang (giày, quần)", 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.RenameColumn(
                name: "Furigana",
                table: "Flashcards",
                newName: "IPA");

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 3, 22, 24, 53, 585, DateTimeKind.Local).AddTicks(6561));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 3, 22, 24, 53, 585, DateTimeKind.Local).AddTicks(6936));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 3, 22, 24, 53, 585, DateTimeKind.Local).AddTicks(6938));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 3, 22, 24, 53, 585, DateTimeKind.Local).AddTicks(6939));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 3, 22, 24, 53, 585, DateTimeKind.Local).AddTicks(6941));
        }
    }
}
