using Last02.Data.Entities;
using Last02.Models;
using Last02.Models.Members;
using Last02.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Last02.Web.Controllers
{
    [Route("[controller]")]
    public class MemberController(IMemberService memberService, ILogger<MemberController> logger) : BaseController
    {
        private readonly IMemberService _memberService = memberService;
        private readonly ILogger<MemberController> _logger = logger;

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(MemberSearchView memberSearch, string keyword, string orders, int page = 1, int size = 100)
        {
            if (size <= 0)
                size = 100;
            if (page <= 0)
                page = 1;

            List<OrderModel> sortOrders = new();

            if (!string.IsNullOrEmpty(orders))
            {
                try
                {
                    sortOrders = JsonSerializer.Deserialize<List<OrderModel>>(orders) ?? new List<OrderModel>();
                }
                catch
                {
                    _logger.LogError("Failed to deserialize orders: {Orders}", orders);
                }
            }

            var members = await _memberService.AdminSearch(memberSearch, keyword, sortOrders, page, size);

            return Json(members);
        }

        [HttpGet("ExportToExcel")]
        public IActionResult ExportToExcel(string ids, string keyword, string field, string sort)
        {
            List<Member> members = [];
            if (!string.IsNullOrEmpty(ids))
            {
                List<int> selectedIds = [.. ids.Split(',').Select(int.Parse)];
                members = _memberService.GetByIds(selectedIds);
            }
            else
            {
                members = _memberService.Search(keyword);
            }

            StringBuilder csvContent = new();
            csvContent.AppendLine("ID, FullName, Email, Gender, DOB, Nationaity,SelectedCourse, CreatedAt");

            if (members == null || members.Count == 0)
            {
                return File(GetBytesWithBOM(csvContent.ToString()), "text/csv", "ListUser.csv");
            }

            foreach (var member in members)
            {
                var activeCourse = member.MemberCourses?
                    .FirstOrDefault(mc => mc.IsActive)?.Course;

                csvContent.AppendLine(string.Join(",",
                    EscapeCsvValue(member.Id.ToString()),
                    EscapeCsvValue(member.FullName ?? ""),
                    EscapeCsvValue(member.User != null && member.User.Email != null ? member.User.Email : ""),
                    EscapeCsvValue(member.Gender.ToString() ?? ""),
                    EscapeCsvValue(member.DOB.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)),
                    EscapeCsvValue(member.Nationality ?? ""),
                    EscapeCsvValue(activeCourse?.Title ?? ""),
                    EscapeCsvValue(member.User != null ? member.User.CreatedAt.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture) : "")
                ));
            }

            // Convert to byte array with UTF-8 BOM (use BOM for Excel compatibility)
            var csvBytes = GetBytesWithBOM(csvContent.ToString());

            // Return the file
            return File(csvBytes, "text/csv", "ListUser.csv");
        }

        [HttpPost("ActivateMember")]
        public IActionResult ActivateMember(IFormCollection formCollection)
        {
            try
            {
                var idValues = formCollection["id"];
                if (idValues.Count == 0)
                {
                    return BadRequest();
                }

                int id = int.Parse(idValues.First() ?? "");
                var member = _memberService.GetById(id);
                if (member == null)
                {
                    return NotFound();
                }
                member.IsActive = true;
                _memberService.Update(member);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("DeactivateMember")]
        public IActionResult DeactivateMember(IFormCollection formCollection)
        {
            try
            {
                var idValues = formCollection["id"];
                if (idValues.Count == 0)
                {
                    return BadRequest();
                }

                int id = int.Parse(idValues.First() ?? "");
                _memberService.Deactivate(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DeleteMember")]
        public IActionResult DeleteMember(IFormCollection formCollection)
        {
            try
            {
                var idValues = formCollection["id"];
                if (idValues.Count == 0)
                {
                    return BadRequest();
                }

                int id = int.Parse(idValues.First() ?? "");
                _memberService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private static byte[] GetBytesWithBOM(string content)
        {
            // UTF-8 BOM header
            byte[] bom = [0xEF, 0xBB, 0xBF];
            byte[] contentBytes = Encoding.UTF8.GetBytes(content);

            // Combine BOM and content
            byte[] result = new byte[bom.Length + contentBytes.Length];
            bom.CopyTo(result, 0);
            contentBytes.CopyTo(result, bom.Length);

            return result;
        }

        private static string EscapeCsvValue(string value)
        {
            // Escape CSV special characters (like commas, quotes, etc.)
            if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
            {
                value = "\"" + value.Replace("\"", "\"\"") + "\"";
            }
            return value;
        }
    }
}
